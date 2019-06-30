using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windkraftanlage.Mathematikwerkzeuge;
using Windkraftanlage.Mathematikwerkzeuge.Integration;

namespace Windkraftanlage.Kennlinienmodell
{
    class Querfahne : Bauteil
    {
        // Ausmaße der Querfahne
        const double a = 0.1;
        const double c = 0.48;
        const double h = 0.63;

        // Korrektursummanden
        const double K8  = Modell.KRest;
        const double K9  = -Modell.KRest;
        const double K11 = -Modell.KRest;
        const double KR1 = Modell.KRotor;
        const double KR2 = -Modell.KRotor;

        // Windfaktoren
        const double Gamma8  = 1.0 - 2.0 * Modell.nu;
        const double Gamma9  = 1.0 - 2.0 * Modell.nu;
        const double Gamma11 = 1.0 - 3.0 * Modell.nu;
        
        // Längen unter Berücksichtigung der Korrektursummanden
        double l8;
        double l9;
        double l11;
        double lR1;
        double lR2;

        internal Querfahne(Integration integrator, Func<double, double> cW, Func<double, double> cA) : base(integrator, cW, cA)
        {
            Name = Art.Querfahne;
            SetzeProfil();
        }

        internal override void Aktualisiere(Punkte punkte, double alpha, double beta)
        {
            l8  = BerechneWindschattenLaenge(punkte.P8.y, punkte.P12.y, K8, beta);
            l9  = BerechneWindschattenLaenge(punkte.P9.y, punkte.P12.y, K9, beta);
            l11 = BerechneWindschattenLaenge(punkte.P11.y, punkte.P12.y, K11, beta);
            lR1 = BerechneWindschattenLaenge(punkte.PR1.y, punkte.P12.y, KR1, beta);
            lR2 = BerechneWindschattenLaenge(punkte.PR2.y, punkte.P12.y, KR2, beta);
        }

        internal override Vektor2 BerechneWiderstandskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(integrator.Integriere(integrand, -Parameter.l4, Parameter.l6b), 0.0);
        }

        internal override Vektor2 BerechneAuftriebskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(0.0, -integrator.Integriere(integrand, -Parameter.l4, Parameter.l6b));
        }

        internal override double BerechneWiderstandsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Sin(Punkte.Phi3(beta)) * l * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -punkte.P12.y * integrator.Integriere(integrand1, -Parameter.l4, Parameter.l6b) - integrator.Integriere(integrand2, -Parameter.l4, Parameter.l6b);
        }

        internal override double BerechneAuftriebsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Cos(Punkte.Phi3(beta)) * l * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return punkte.P12.x * integrator.Integriere(integrand1, -Parameter.l4, Parameter.l6b) + integrator.Integriere(integrand2, -Parameter.l4, Parameter.l6b);
        }

        private void SetzeProfil()
        {
            profil = l =>
            {
                if (l < -Parameter.l4)
                    throw new ValueOutOfRangeException("Das Argument von f_Q liegt außerhalb des erlaubten Bereichs.");
                else if (l <= Parameter.l6a)
                    return a;
                else if (l <= Parameter.l6b)
                    return c / h * (l - Parameter.l1b) + a;
                else
                    throw new ValueOutOfRangeException("Das Argument von f_Q liegt außerhalb des erlaubten Bereichs.");
            };
        }

        private double BerechneWindschattenLaenge(double y1, double y2, double K, double beta)
        {
            return (y1 + K - y2) / Math.Sin(Punkte.Phi3(beta));
        }

        private protected override double cW(double l, double alpha, double beta)
        {
            if (l < -Parameter.l4)
                throw new ValueOutOfRangeException("Das Argument von cW_Q liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l6b)
                return cW_Basis(beta);
            else
                throw new ValueOutOfRangeException("Das Argument von cW_Q liegt außerhalb des erlaubten Bereichs.");
        }

        private protected override double cA(double l, double alpha, double beta)
        {
            if (l < -Parameter.l4)
                throw new ValueOutOfRangeException("Das Argument von cA_Q liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l6b)
                return cA_Basis(beta);
            else
                throw new ValueOutOfRangeException("Das Argument von cA_Q liegt außerhalb des erlaubten Bereichs.");
        }

        private protected override double vW(double l, double v)
        {
            return GammaRotor(l) * GammaRest(l) * v;
        }

        private double GammaRotor(double l)
        {
            if (l < -Parameter.l4)
                throw new ValueOutOfRangeException("Das Argument von GammaRotor_Q liegt außerhalb des erlaubten Bereichs.");
            else if (l <= lR1)
                return 1.0;
            else if (l <= lR2)
                return Modell.GammaRotor;
            else
                return 1.0;
        }

        private double GammaRest(double l)
        {
            if (l < -Parameter.l4)
                throw new ValueOutOfRangeException("Das Argument von GammaRest_Q liegt außerhalb des erlaubten Bereichs.");
            else if (l <= l11)
                return Gamma11;
            else if (l <= l8)
                return Gamma8;
            else if (l <= l9)
                return Gamma9;
            else
                return 1.0;
        }
    }
}
