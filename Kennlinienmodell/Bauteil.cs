using Mathematik;
using Mathematik.Integration;
using System;

namespace Kennlinienmodell
{
    abstract class Bauteil
    {
        protected readonly Func<double, double> cW_Basis;
        protected readonly Func<double, double> cA_Basis;

        protected Func<double, double> profil;

        protected Bauteil(Func<double, double> cW_Basis, Func<double, double> cA_Basis)
        {
            this.cW_Basis = cW_Basis;
            this.cA_Basis = cA_Basis;
            SetzeProfil();
        }

        internal abstract void Aktualisiere(Punkte modellpunkte, double alpha, double beta);
        internal abstract Vektor2 BerechneWiderstandskraft(double v, double alpha, double beta, Integration integrator);
        internal abstract Vektor2 BerechneAuftriebskraft(double v, double alpha, double beta, Integration integrator);
        internal abstract double BerechneWiderstandsdrehmoment(Punkte punkte, double v, double alpha, double beta, Integration integrator);
        internal abstract double BerechneAuftriebsdrehmoment(Punkte punkte, double v, double alpha, double beta, Integration integrator);


        protected abstract double vW(double l, double v);
        protected abstract double cW(double l, double alpha, double beta);
        protected abstract double cA(double l, double alpha, double beta);
        protected abstract void SetzeProfil();
    }
}
