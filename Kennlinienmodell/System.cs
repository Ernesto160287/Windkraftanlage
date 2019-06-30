using System;
using Windkraftanlage.Mathematikwerkzeuge;

namespace Windkraftanlage.Kennlinienmodell
{
    abstract class System
    {
        private protected Punkte punkte;
        private protected Vektor2 verschiebungUrsprung;

        private protected Func<double, double> cW;
        private protected Func<double, double> cA;

        private protected Vektor2 gesamtkraftBauteile;
        private protected double gesamtdrehmomentBauteile;

        internal Vektor2 FGelenk { get; set; }   // Gelenkraft
        private protected double MSeil = 0.0;    // Seildrehmoment
        internal Vektor2 FSeil { get; set; }   // Seilkraft
        private protected double MGelenk;      // Gelenkdrehmoment


        private protected Bauteil[] bauteile;

        private protected System()
        {
        }

        internal void Aktualisiere(double alpha, double beta)
        {
            AktualisierePunkte(alpha, beta);
            AktualisiereBauteile(punkte, alpha, beta);
        }

        private protected virtual void AktualisierePunkte(double alpha, double beta)
        {
            verschiebungUrsprung = BestimmeVerschiebung(beta);
            punkte.Aktualisiere(alpha, beta, verschiebungUrsprung);
        }

        private protected abstract Vektor2 BestimmeVerschiebung(double beta);

        private protected void AktualisiereBauteile(Punkte punkte, double alpha, double beta)
        {
            foreach (Bauteil bauteil in bauteile)
            {
                bauteil.Aktualisiere(punkte, alpha, beta);
            }
        }
        
        internal abstract void WerteAus(double v, double alpha, double beta);

        private protected abstract void BerechneKraefte(double v, double alpha, double beta);
        private protected abstract void BerechneDrehmomente(double v, double alpha, double beta);


        private protected Vektor2 BerechneGesamtkraftBauteile(double v, double alpha, double beta)
        {
            return BerechneWiderstandskraftBauteile(v, alpha, beta) + BerechneAuftriebskraftBauteile(v, alpha, beta);
        }

        private protected double BerechneGesamtdrehmomentBauteile(double v, double alpha, double beta)
        {
            return BerechneWiderstandsdrehmomentBauteile(v, alpha, beta) + BerechneAuftriebsdrehmomentBauteile(v, alpha, beta);
        }

        private protected Vektor2 BerechneWiderstandskraftBauteile(double v, double alpha, double beta)
        {
            Vektor2 FW = Vektor2.Zero();

            foreach (Bauteil bauteil in bauteile)
            {
                FW += bauteil.BerechneWiderstandskraft(v, alpha, beta);
            }
            return FW;
        }

        private protected Vektor2 BerechneAuftriebskraftBauteile(double v, double alpha, double beta)
        {
            Vektor2 FA = Vektor2.Zero();

            foreach (Bauteil bauteil in bauteile)
            {
                FA += bauteil.BerechneAuftriebskraft(v, alpha, beta);
            }
            return FA;
        }

        private protected double BerechneWiderstandsdrehmomentBauteile(double v, double alpha, double beta)
        {
            double MW = 0.0;

            foreach (Bauteil bauteil in bauteile)
            {
                MW += bauteil.BerechneWiderstandsdrehmoment(punkte, v, alpha, beta);
            }
            return MW;
        }

        private protected double BerechneAuftriebsdrehmomentBauteile(double v, double alpha, double beta)
        {
            double MA = 0.0;

            foreach (Bauteil bauteil in bauteile)
            {
                MA += bauteil.BerechneAuftriebsdrehmoment(punkte, v, alpha, beta);
            }
            return MA;
        }
    }
}
