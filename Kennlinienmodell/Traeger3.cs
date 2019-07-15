using MathematikWerkzeuge;
using MathematikWerkzeuge.Integration;
using System;

namespace Kennlinienmodell
{
    class Traeger3 : Bauteil
    {
        // Ausmaße des Trägers 3
        const double a = 0.04;

        // Korrektursummanden
        const double KR1 = Modell.KRotor;

        // Längen unter Berücksichtigung der Korrektursummanden
        double lR1;


        internal Traeger3(Integration integrator, Func<double, double> cW, Func<double, double> cA) : base(integrator, cW, cA)
        {
        }

        internal override void Aktualisiere(Punkte punkte, double alpha, double beta)
        {
            lR1 = BerechneWindschattenLaenge(punkte.PR1.y, punkte.P5a.y, KR1, beta);
        }

        internal override Vektor2 BerechneWiderstandskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(integrator.Integriere(integrand, 0.0, Parameter.l5d), 0.0);
        }

        internal override Vektor2 BerechneAuftriebskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(0.0, -integrator.Integriere(integrand, 0.0, Parameter.l5d));
        }

        internal override double BerechneWiderstandsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Sin(Punkte.Phi4(beta)) * l * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -punkte.P5a.y * integrator.Integriere(integrand1, 0.0, Parameter.l5d) - integrator.Integriere(integrand2, 0.0, Parameter.l5d);
        }

        internal override double BerechneAuftriebsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Cos(Punkte.Phi4(beta)) * l * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -punkte.P5a.x * integrator.Integriere(integrand1, 0.0, Parameter.l5d) - integrator.Integriere(integrand2, 0.0, Parameter.l5d);
        }

        protected override void SetzeProfil()
        {
            profil = l =>
            {
                if (l < 0.0)
                    throw new ValueOutOfRangeException("Das Argument von f_T3 liegt außerhalb des erlaubten Bereichs.");
                else if (l <= Parameter.l5d)
                    return a;
                else
                    throw new ValueOutOfRangeException("Das Argument von f_T3 liegt außerhalb des erlaubten Bereichs.");
            };
        }

        double BerechneWindschattenLaenge(double y1, double y2, double K, double beta)
        {
            return (y1 + K - y2) / Math.Sin(Punkte.Phi4(beta));
        }

        protected override double cW(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cW_T3 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l5d)
                return cW_Basis(Punkte.Phi1(beta));
            else
                throw new ValueOutOfRangeException("Das Argument von cW_T3 liegt außerhalb des erlaubten Bereichs.");
        }

        protected override double cA(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cA_T3 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l5d)
                return cA_Basis(Punkte.Phi1(beta));
            else
                throw new ValueOutOfRangeException("Das Argument von cA_T3 liegt außerhalb des erlaubten Bereichs.");
        }

        protected override double vW(double l, double v)
        {
            return GammaRotor(l) * v;
        }

        double GammaRotor(double l)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von GammaRotor_T3 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= lR1)
                return Modell.GammaRotor;
            else
                return 1.0;
        }
    }
}
