using System;
using Windkraftanlage.Mathematikwerkzeuge;
using Windkraftanlage.Mathematikwerkzeuge.Integration;

namespace Windkraftanlage.Kennlinienmodell
{
    abstract class Bauteil
    {
        private protected readonly Integration integrator;
        private protected readonly Func<double, double> cW_Basis;
        private protected readonly Func<double, double> cA_Basis;

        private protected Func<double, double> profil;

        private protected Bauteil(Integration integrator, Func<double, double> cW_Basis, Func<double, double> cA_Basis)
        {
            this.integrator = integrator;
            this.cW_Basis = cW_Basis;
            this.cA_Basis = cA_Basis;
            SetzeProfil();
        }

        internal abstract void Aktualisiere(Punkte modellpunkte, double alpha, double beta);
        internal abstract Vektor2 BerechneWiderstandskraft(double v, double alpha, double beta);
        internal abstract Vektor2 BerechneAuftriebskraft(double v, double alpha, double beta);
        internal abstract double BerechneWiderstandsdrehmoment(Punkte punkte, double v, double alpha, double beta);
        internal abstract double BerechneAuftriebsdrehmoment(Punkte punkte, double v, double alpha, double beta);


        private protected abstract double vW(double l, double v);
        private protected abstract double cW(double l, double alpha, double beta);
        private protected abstract double cA(double l, double alpha, double beta);
        private protected abstract void SetzeProfil();
    }

}
