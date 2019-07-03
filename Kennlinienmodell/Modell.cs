using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windkraftanlage.Mathematikwerkzeuge;
using Windkraftanlage.Mathematikwerkzeuge.Interpolation;
using Windkraftanlage.Mathematikwerkzeuge.Nullstelle;

namespace Windkraftanlage.Kennlinienmodell
{
    class Modell
    {
        // charakteristische Verzögerung des Windes hinter einem Hindernis (für GammaRest)
        public const double nu = 0.15;

        // Windfaktor des Rotors
        public const double GammaRotor = 0.7;

        // Korrektursummand für den Rotor
        public const double KRotor = 0.2;

        // Korrektursummand für die übrigen Bauteile
        public const double KRest = 0.1;


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

        double[] v_Werte;
        double[] alpha_Werte;
        double[] beta_Werte;
        double[] seillaenge_Werte;
        double[] seilkraft_Werte;

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
            InitialisiereModellparameter();
            InitialisiereNumerischeHilfsmittel();
            InitialisiereSysteme();

            if (alleKraefte)
            {
                InitialisiereOptionaleKraefte();
            }
        }

        private void InitialisiereModellparameter()
        {
            v_Werte = new double[anzahlSchritte];
            alpha_Werte = new double[anzahlSchritte];
            beta_Werte = new double[anzahlSchritte];
            seillaenge_Werte = new double[anzahlSchritte];
            seilkraft_Werte = new double[anzahlSchritte];
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
            LoeseSystem1(alphaVar);
            UebertrageKraefteVonSystem1AufSystem2();
            LoeseSystem2(alphaVar);

            return system2.BerechneGesamtdrehmoment();
        }

        private void LoeseSystem1(double alphaVar)
        {
            system1.Aktualisiere(alphaVar, beta);
            system1.WerteAus(v, alphaVar, beta);
        }

        private void UebertrageKraefteVonSystem1AufSystem2()
        {
            system2.FSeil = -system1.FSeil;
            system2.FGelenk = -system1.FGelenk;
        }

        private void LoeseSystem2(double alphaVar)
        {
            system2.Aktualisiere(alphaVar, beta);
            system2.WerteAus(v, alphaVar, beta);
        }

        private void SpeichereWerte(int i)
        {
            SpeichereModellparameter(i);

            if (alleKraefte)
            {
                SpeichereOptionaleKraefte(i);
            }
        }

        private void SpeichereModellparameter(int i)
        {
            v_Werte[i] = v;
            alpha_Werte[i] = alpha;
            beta_Werte[i] = beta;

            (double seillaengeFinal, double seilkraftFinal) = BestimmeSeillaengeUndSeilkraft();

            if (i == 0)
                seillaengeStart = seillaengeFinal;

            seillaenge_Werte[i] = seillaengeFinal - seillaengeStart;
            seilkraft_Werte[i] = seilkraftFinal;
        }

        private (double, double) BestimmeSeillaengeUndSeilkraft()
        {
            system1.Aktualisiere(alpha, beta);
            return (system1.BerechneSeillaenge(), system1.BerechneSeilkraft());
        }

        private void SpeichereOptionaleKraefte(int i)
        {
            // TODO
        }
    }

}
