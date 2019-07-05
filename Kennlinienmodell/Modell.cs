using System;

namespace Windkraftanlage.Kennlinienmodell
{
    class Modell
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

        readonly int anzahlSchritte;
        readonly double vSchritt;

        double v;
        double alpha;
        double beta;
        double seillaengeStart;

        Numerikhilfsmittel numerik;
        
        System1 system1;
        System2 system2;

        Modellwerte modellwerte;
        OptionaleKraefte optionaleKraefte;

        internal Modell(double vmin, double vmax, double vSchritt, double genauigkeit, bool alleKraefte = false)
        {
            v = vmin;
            this.vSchritt = vSchritt;
            anzahlSchritte = (int)Math.Ceiling((vmax - vmin) / vSchritt + 1);

            this.genauigkeit = genauigkeit;
            this.alleKraefte = alleKraefte;
        }

        internal void Initialisiere()
        {
            InitialisiereModellwerte();
            InitialisiereNumerischeHilfsmittel();
            InitialisiereSysteme();

            if (alleKraefte)
            {
                InitialisiereOptionaleKraefte();
            }
        }

        private void InitialisiereModellwerte()
        {
            modellwerte = new Modellwerte(anzahlSchritte);
        }

        private void InitialisiereNumerischeHilfsmittel()
        {
            numerik = new Numerikhilfsmittel(genauigkeit);
        }

        private void InitialisiereSysteme()
        {
            system1 = new System1(numerik.integrator, numerik.cW, numerik.cA);
            system2 = new System2(numerik.integrator, numerik.cW, numerik.cA);
        }
        private void InitialisiereOptionaleKraefte()
        {
            optionaleKraefte = new OptionaleKraefte(anzahlSchritte);
        }


        internal void Verarbeite()
        {
            for (int i = 0; i < anzahlSchritte; i++)
            {
                AktualisiereGeschwindigkeit();

                beta = BestimmteBeta();

                try
                {
                    alpha = BestimmeAlpha();
                    SpeichereWerte(i);
                }
                catch (NumericsFailedException)
                {
                    continue;
                }
            }
        }
        
        private void AktualisiereGeschwindigkeit()
        {
            v += vSchritt;
        }

        private double BestimmteBeta()
        {
            double q = (4 * Parameter.PG) 
                        / (3 * Parameter.eta * Parameter.rhoL * Math.Pow(v, 3) * Math.PI * Math.Pow(Parameter.r, 2));

            if (q <= 1)
                return Math.Asin(q);
            else
                return Math.PI / 2;
        }

        private double BestimmeAlpha()
        {
            Func<double, double> gesamtdrehmoment = alphaVar => BerechneGesamtdrehmoment(alphaVar);

            return numerik.approxNst.ErmittleNullstelle(gesamtdrehmoment, genauigkeit, Math.PI / 2 - genauigkeit);
        }

        private double BerechneGesamtdrehmoment(double alphaVar)
        {
            system1.Loese(v, alphaVar, beta);
            UebertrageKraefteVonSystem1AufSystem2();
            system2.Loese(v, alphaVar, beta);

            return system2.BerechneGesamtdrehmoment();
        }

        private void UebertrageKraefteVonSystem1AufSystem2()
        {
            system2.FSeil = -system1.FSeil;
            system2.FGelenk = -system1.FGelenk;
        }

        private void SpeichereWerte(int i)
        {
            SpeichereModellwerte(i);

            if (alleKraefte)
            {
                SpeichereOptionaleKraefte(i);
            }
        }

        private void SpeichereModellwerte(int i)
        {
            (double seillaenge, double seilkraft) = system1.BestimmeSeillaengeUndSeilkraft(alpha, beta);
            seillaenge = VerkuerzeUmSeillaengeStart(i, seillaenge);

            modellwerte.SpeichereAktuelleWerte(i, v, alpha, beta, seillaenge, seilkraft);
        }

        private double VerkuerzeUmSeillaengeStart(int i, double laenge)
        {
            if (i == 0)
                seillaengeStart = laenge;

            return laenge - seillaengeStart;
        }

        private void SpeichereOptionaleKraefte(int i)
        {
            // TODO
        }
    }

}
