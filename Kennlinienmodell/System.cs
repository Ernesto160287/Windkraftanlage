using MathematikWerkzeuge;
using System;

namespace Kennlinienmodell
{
    abstract class System
    {
        protected Punkte punkte;
        protected Vektor2 verschiebungUrsprung;

        protected Func<double, double> cW;
        protected Func<double, double> cA;

        protected Vektor2 gesamtkraftBauteile;
        protected double gesamtdrehmomentBauteile;

        internal Vektor2 FGelenk { get; set; }   // Gelenkraft
        protected double MSeil = 0.0;    // Seildrehmoment
        internal Vektor2 FSeil { get; set; }   // Seilkraft
        protected double MGelenk;      // Gelenkdrehmoment

        protected Bauteil[] bauteile;

        internal void Loese(double v, double alpha, double beta)
        {
            Aktualisiere(alpha, beta);
            WerteAus(v, alpha, beta);
        }

        protected void Aktualisiere(double alpha, double beta)
        {
            AktualisierePunkte(alpha, beta);
            AktualisiereBauteile(punkte, alpha, beta);
        }

        protected virtual void AktualisierePunkte(double alpha, double beta)
        {
            verschiebungUrsprung = BestimmeVerschiebung(beta);
            punkte.Aktualisiere(alpha, beta, verschiebungUrsprung);
        }

        protected abstract Vektor2 BestimmeVerschiebung(double beta);

        protected void AktualisiereBauteile(Punkte punkte, double alpha, double beta)
        {
            foreach (Bauteil bauteil in bauteile)
            {
                bauteil.Aktualisiere(punkte, alpha, beta);
            }
        }

        internal abstract void WerteAus(double v, double alpha, double beta);

        protected abstract void BerechneKraefte(double v, double alpha, double beta);
        protected abstract void BerechneDrehmomente(double v, double alpha, double beta);


        protected Vektor2 BerechneGesamtkraftBauteile(double v, double alpha, double beta)
        {
            return BerechneWiderstandskraftBauteile(v, alpha, beta) + BerechneAuftriebskraftBauteile(v, alpha, beta);
        }

        protected Vektor2 BerechneWiderstandskraftBauteile(double v, double alpha, double beta)
        {
            Vektor2 FW = Vektor2.Zero();

            foreach (Bauteil bauteil in bauteile)
            {
                FW += bauteil.BerechneWiderstandskraft(v, alpha, beta);
            }
            return FW;
        }

        protected Vektor2 BerechneAuftriebskraftBauteile(double v, double alpha, double beta)
        {
            Vektor2 FA = Vektor2.Zero();

            foreach (Bauteil bauteil in bauteile)
            {
                FA += bauteil.BerechneAuftriebskraft(v, alpha, beta);
            }
            return FA;
        }

        protected double BerechneGesamtdrehmomentBauteile(double v, double alpha, double beta)
        {
            return BerechneWiderstandsdrehmomentBauteile(v, alpha, beta) + BerechneAuftriebsdrehmomentBauteile(v, alpha, beta);
        }

        protected double BerechneWiderstandsdrehmomentBauteile(double v, double alpha, double beta)
        {
            double MW = 0.0;

            foreach (Bauteil bauteil in bauteile)
            {
                MW += bauteil.BerechneWiderstandsdrehmoment(punkte, v, alpha, beta);
            }
            return MW;
        }

        protected double BerechneAuftriebsdrehmomentBauteile(double v, double alpha, double beta)
        {
            double MA = 0.0;

            foreach (Bauteil bauteil in bauteile)
            {
                MA += bauteil.BerechneAuftriebsdrehmoment(punkte, v, alpha, beta);
            }
            return MA;
        }

        internal abstract double[] GebeOptionaleKraefteAus(double v, double alpha, double beta);
    }
}
