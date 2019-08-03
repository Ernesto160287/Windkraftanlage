using Mathematik;
using Mathematik.Integration;
using System;

namespace Kennlinienmodell
{
    class System1 : System
    {
        internal double Seillaenge { get; private set; } // Seillänge
        internal double Seilkraft { get; private set; }  // Seilkraft
        Vektor2 P2P5;  // Verbindungsvektor Steuerfahne und Trägerkonstruktion

        internal System1(Func<double, double> cW, Func<double, double> cA) : base()
        {
            bauteile = new Bauteil[1];
            bauteile[0] = new Steuerfahne(cW, cA);
        }

        protected override void AktualisierePunkte(double alpha, double beta)
        {
            base.Aktualisiere(alpha, beta);
            AktualisiereP2P5();
        }

        void AktualisiereP2P5()
        {
            P2P5 = new Vektor2(punkte.P2, punkte.P5);
        }

        protected override Vektor2 BestimmeVerschiebung(double beta)
        {
            return Vektor2.Zero();
        }

        internal override void WerteAus(double v, double alpha, double beta, Integration integrator)
        {
            BerechneDrehmomente(v, alpha, beta, integrator);

            Seillaenge = BerechneSeillaenge();
            Seilkraft = BerechneSeilkraft();

            BerechneKraefte(v, alpha, beta, integrator);
        }

        protected override void BerechneDrehmomente(double v, double alpha, double beta, Integration integrator)
        {
            gesamtdrehmomentBauteile = BerechneGesamtdrehmomentBauteile(v, alpha, beta, integrator);

            MSeil = -gesamtdrehmomentBauteile;
            MGelenk = 0.0;
        }

        double BerechneSeillaenge()
        {
            return P2P5.Norm();
        }

        double BerechneSeilkraft()
        {
            return Seillaenge * gesamtdrehmomentBauteile / (punkte.P2.y * P2P5.x - punkte.P2.x * P2P5.y);
        }

        protected override void BerechneKraefte(double v, double alpha, double beta, Integration integrator)
        {
            gesamtkraftBauteile = BerechneGesamtkraftBauteile(v, alpha, beta, integrator);

            FSeil = Seilkraft / Seillaenge * P2P5;
            FGelenk = -(gesamtkraftBauteile + FSeil);
        }

        internal (double, double) BestimmeSeillaengeUndSeilkraft(double alpha, double beta)
        {
            Aktualisiere(alpha, beta);
            return (BerechneSeillaenge(), BerechneSeilkraft());
        }

        internal override double[] GebeOptionaleKraefteAus(double v, double alpha, double beta, Integration integrator)
        {
            double[] optionaleKraefte = new double[3];

            optionaleKraefte[0] = FGelenk.Norm();
            optionaleKraefte[1] = bauteile[0].BerechneWiderstandskraft(v, alpha, beta, integrator).Norm();
            optionaleKraefte[2] = bauteile[0].BerechneAuftriebskraft(v, alpha, beta, integrator).Norm();

            return optionaleKraefte;
        }
    }
}
