using System;

namespace Kennlinienmodell
{
    public class Modell
    {
        // charakteristische Verzögerung des Windes hinter einem Hindernis (für GammaRest)
        internal const double nu = 0.15;

        // Windfaktor des Rotors
        internal const double GammaRotor = 0.7;

        // Korrektursummand für den Rotor
        internal const double KRotor = 0.2;

        // Korrektursummand für die übrigen Bauteile
        internal const double KRest = 0.1;


        readonly double genauigkeit;
        readonly bool alleKraefte;

        readonly double vSchritt;

        double v;
        double alpha;
        double beta;

        double? seillaengeStart;

        Numerikhilfsmittel numerik;

        System1 system1;
        System2 system2;

        Modellwerte modellwerte;
        OptionaleKraefte optionaleKraefte;

        public Modell(double vmin, double vmax, int anzahlSchritte, double genauigkeit, bool alleKraefte = false)
        {
            v = vmin;
            vSchritt = (vmax - vmin) / anzahlSchritte;

            this.genauigkeit = genauigkeit;
            this.alleKraefte = alleKraefte;
        }

        public void Initialisiere()
        {
            InitialisiereModellwerte();
            InitialisiereNumerischeHilfsmittel();
            InitialisiereSysteme();

            if (alleKraefte)
            {
                InitialisiereOptionaleKraefte();
            }
        }

        void InitialisiereModellwerte()
        {
            modellwerte = new Modellwerte();
        }

        void InitialisiereNumerischeHilfsmittel()
        {
            numerik = new Numerikhilfsmittel(genauigkeit);
        }

        void InitialisiereSysteme()
        {
            system1 = new System1(numerik.cW, numerik.cA);
            system2 = new System2(numerik.cW, numerik.cA);
        }
        void InitialisiereOptionaleKraefte()
        {
            optionaleKraefte = new OptionaleKraefte();
        }

        public void VerarbeiteSchritt()
        {
            AktualisiereGeschwindigkeit();

            beta = BestimmteBeta();
            alpha = BestimmeAlpha();
        }

        void AktualisiereGeschwindigkeit()
        {
            v += vSchritt;
        }

        double BestimmteBeta()
        {
            double q = (4 * Parameter.PG)
                        / (3 * Parameter.eta * Parameter.rhoL * Math.Pow(v, 3) * Math.PI * Math.Pow(Parameter.r, 2));

            if (q <= 1)
                return Math.Asin(q);
            else
                return Math.PI / 2;
        }

        double BestimmeAlpha()
        {
            Func<double, double> gesamtdrehmoment = alphaVar => BerechneGesamtdrehmoment(alphaVar);

            return numerik.approxNst.ErmittleNullstelle(gesamtdrehmoment, genauigkeit, Math.PI / 2 - genauigkeit);
        }

        double BerechneGesamtdrehmoment(double alphaVar)
        {
            system1.Loese(v, alphaVar, beta, numerik.integrator);
            UebertrageKraefteVonSystem1AufSystem2();
            system2.Loese(v, alphaVar, beta, numerik.integrator);

            return system2.BerechneGesamtdrehmoment();
        }

        void UebertrageKraefteVonSystem1AufSystem2()
        {
            system2.FSeil = -system1.FSeil;
            system2.FGelenk = -system1.FGelenk;
        }

        public string GebeAktuelleModellwerteAus()
        {
            (double seillaenge, double seilkraft) = BestimmeAktuelleSeillaengeUndSeilkraft();

            return "v = " + v + ", alpha = " + alpha;
        }

        (double, double) BestimmeAktuelleSeillaengeUndSeilkraft()
        {
            double seillaenge = VerkuerzeUmSeillaengeStart(system1.Seillaenge);
            double seilkraft = system1.Seilkraft;

            return (seillaenge, seilkraft);
        }

        void SpeichereWerte()
        {
            SpeichereModellwerte();

            if (alleKraefte)
            {
                SpeichereOptionaleKraefte();
            }
        }

        void SpeichereModellwerte()
        {
            double seillaenge = VerkuerzeUmSeillaengeStart(system1.Seillaenge);
            double seilkraft = system1.Seilkraft;

            modellwerte.SpeichereAktuelleWerte(v, alpha, beta, seillaenge, seilkraft);
        }

        double VerkuerzeUmSeillaengeStart(double laenge)
        {
            if (!seillaengeStart.HasValue)
                seillaengeStart = laenge;

            return laenge - seillaengeStart.Value;
        }

        void SpeichereOptionaleKraefte()
        {
            SpeichereOptionaleKraefteSystem1();
            SpeichereOptionaleKraefteSystem2();
        }

        void SpeichereOptionaleKraefteSystem1()
        {
            double[] optionaleKraefteSystem1 = system1.GebeOptionaleKraefteAus(v, alpha, beta, numerik.integrator);
            optionaleKraefte.SpeichereAktuelleWerteSystem1(optionaleKraefteSystem1);
        }

        void SpeichereOptionaleKraefteSystem2()
        {
            double[] optionaleKraefteSystem2 = system2.GebeOptionaleKraefteAus(v, alpha, beta, numerik.integrator);
            optionaleKraefte.SpeichereAktuelleWerteSystem2(optionaleKraefteSystem2);
        }
    }
}
