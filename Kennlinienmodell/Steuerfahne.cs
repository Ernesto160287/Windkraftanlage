using MathematikWerkzeuge;
using MathematikWerkzeuge.Integration;
using System;

namespace Kennlinienmodell
{
    class Steuerfahne : Bauteil
    {
        // Ausmaße der Steuerfahne
        const double a = 0.1;
        const double c = 0.63;
        const double h = 0.95;

        // Korrektursummanden
        const double K4 = Modell.KRest;
        const double K5c = Modell.KRest;
        const double K8 = Modell.KRest;
        const double K11 = -Modell.KRest;
        const double KR1 = Modell.KRotor;

        // Windfaktoren
        const double Gamma4 = 1.0 - 3.0 * Modell.nu;
        const double Gamma5c = 1.0 - Modell.nu;
        const double Gamma8 = 1.0 - 2.0 * Modell.nu;
        const double Gamma11 = 1.0 - Modell.nu;

        // Längen unter Berücksichtigung der Korrektursummanden
        double l4;
        double l5c;
        double l8;
        double l11;
        double lR1;

        internal Steuerfahne(Integration integrator, Func<double, double> cW, Func<double, double> cA) : base(integrator, cW, cA)
        {
        }

        internal override void Aktualisiere(Punkte punkte, double alpha, double beta)
        {
            l4 = BerechneWindschattenLaenge(punkte.P4.y, K4, alpha);
            l5c = BerechneWindschattenLaenge(punkte.P5c.y, K5c, alpha);
            l8 = BerechneWindschattenLaenge(punkte.P8.y, K8, alpha);
            l11 = BerechneWindschattenLaenge(punkte.P11.y, K11, alpha);
            lR1 = BerechneWindschattenLaenge(punkte.PR1.y, KR1, alpha);
        }

        internal override Vektor2 BerechneWiderstandskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(integrator.Integriere(integrand, 0.0, Parameter.l1b), 0.0);
        }

        internal override Vektor2 BerechneAuftriebskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(0.0, -integrator.Integriere(integrand, 0.0, Parameter.l1b));
        }

        internal override double BerechneWiderstandsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * Math.Sin(alpha) * l * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -integrator.Integriere(integrand, 0.0, Parameter.l1b);
        }

        internal override double BerechneAuftriebsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * Math.Cos(alpha) * l * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -integrator.Integriere(integrand, 0.0, Parameter.l1b);
        }

        protected override void SetzeProfil()
        {
            profil = l =>
            {
                if (l < 0.0)
                    throw new ValueOutOfRangeException("Das Argument von f_S liegt außerhalb des erlaubten Bereichs.");
                else if (l <= Parameter.l1a)
                    return a;
                else if (l <= Parameter.l1b)
                    return c / h * (l - Parameter.l1a) + a;
                else
                    throw new ValueOutOfRangeException("Das Argument von f_S liegt außerhalb des erlaubten Bereichs.");
            };
        }

        double BerechneWindschattenLaenge(double y, double K, double alpha)
        {
            return (y + K) / Math.Sin(alpha);
        }

        protected override double cW(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cW_S liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l1b)
                return cW_Basis(alpha);
            else
                throw new ValueOutOfRangeException("Das Argument von cW_S liegt außerhalb des erlaubten Bereichs.");
        }

        protected override double cA(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cA_S liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l1b)
                return cA_Basis(alpha);
            else
                throw new ValueOutOfRangeException("Das Argument von cA_S liegt außerhalb des erlaubten Bereichs.");
        }

        protected override double vW(double l, double v)
        {
            return GammaRotor(l) * GammaRest(l) * v;
        }

        double GammaRotor(double l)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von GammaRotor_S liegt außerhalb des erlaubten Bereichs.");
            else if (l <= lR1)
                return Modell.GammaRotor;
            else
                return 1.0;
        }

        double GammaRest(double l)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von GammaRest_S liegt außerhalb des erlaubten Bereichs.");
            else if (l <= l8)
                return Gamma8;
            else if (l <= l11)
                return Gamma11;
            else if (l <= l4)
                return Gamma4;
            else if (l <= l5c)
                return Gamma5c;
            else
                return 1.0;
        }
    }
}
