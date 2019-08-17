using Mathematik;
using Mathematik.Integration;

namespace Kennlinienmodell
{
    abstract class System
    {
        protected Punkte punkte;
        protected Vektor2 verschiebungUrsprung;

        protected Vektor2 gesamtkraftBauteile;
        protected double gesamtdrehmomentBauteile;

        internal Vektor2 FGelenk { get; set; }   // Gelenkraft
        protected double MSeil = 0.0;    // Seildrehmoment
        internal Vektor2 FSeil { get; set; }   // Seilkraft
        protected double MGelenk;      // Gelenkdrehmoment

        protected Bauteil[] bauteile;

        internal void Loese(double v, double alpha, double beta, Integration integrator)
        {
            Aktualisiere(alpha, beta);
            WerteAus(v, alpha, beta, integrator);
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

        internal abstract void WerteAus(double v, double alpha, double beta, Integration integrator);

        protected abstract void BerechneKraefte(double v, double alpha, double beta, Integration integrator);
        protected abstract void BerechneDrehmomente(double v, double alpha, double beta, Integration integrator);


        protected Vektor2 BerechneGesamtkraftBauteile(double v, double alpha, double beta, Integration integrator)
        {
            return BerechneWiderstandskraftBauteile(v, alpha, beta, integrator) + BerechneAuftriebskraftBauteile(v, alpha, beta, integrator);
        }

        protected Vektor2 BerechneWiderstandskraftBauteile(double v, double alpha, double beta, Integration integrator)
        {
            Vektor2 FW = Vektor2.Zero();

            foreach (Bauteil bauteil in bauteile)
            {
                FW += bauteil.BerechneWiderstandskraft(v, alpha, beta, integrator);
            }
            return FW;
        }

        protected Vektor2 BerechneAuftriebskraftBauteile(double v, double alpha, double beta, Integration integrator)
        {
            Vektor2 FA = Vektor2.Zero();

            foreach (Bauteil bauteil in bauteile)
            {
                FA += bauteil.BerechneAuftriebskraft(v, alpha, beta, integrator);
            }
            return FA;
        }

        protected double BerechneGesamtdrehmomentBauteile(double v, double alpha, double beta, Integration integrator)
        {
            return BerechneWiderstandsdrehmomentBauteile(v, alpha, beta, integrator) + BerechneAuftriebsdrehmomentBauteile(v, alpha, beta, integrator);
        }

        protected double BerechneWiderstandsdrehmomentBauteile(double v, double alpha, double beta, Integration integrator)
        {
            double MW = 0.0;

            foreach (Bauteil bauteil in bauteile)
            {
                MW += bauteil.BerechneWiderstandsdrehmoment(punkte, v, alpha, beta, integrator);
            }
            return MW;
        }

        protected double BerechneAuftriebsdrehmomentBauteile(double v, double alpha, double beta, Integration integrator)
        {
            double MA = 0.0;

            foreach (Bauteil bauteil in bauteile)
            {
                MA += bauteil.BerechneAuftriebsdrehmoment(punkte, v, alpha, beta, integrator);
            }
            return MA;
        }

        internal abstract double[] GebeOptionaleKraefteAus(double v, double alpha, double beta, Integration integrator);
    }
}
