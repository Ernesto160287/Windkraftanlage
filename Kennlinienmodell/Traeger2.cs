using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windkraftanlage.Mathematikwerkzeuge;
using Windkraftanlage.Mathematikwerkzeuge.Integration;

namespace Windkraftanlage.Kennlinienmodell
{
    class Traeger2 : Bauteil
    {
        // Ausmaße des Trägers 2
        const double a = 0.04;

        // Korrektursummanden
        const double KR1 = Modell.KRotor;

        // Windfaktoren

        // Längen unter Berücksichtigung der Korrektursummanden
        double lR1;


        internal Traeger2(Integration integrator, Func<double, double> cW, Func<double, double> cA) : base(integrator, cW, cA)
        {
            Name = Art.Traeger1;
            SetzeProfil();
        }

        internal override void Aktualisiere(Punkte punkte, double alpha, double beta)
        {
            lR1 = BerechneWindschattenLaenge(punkte.PR1.y, punkte.P5b.y, KR1, beta);
        }

        internal override Vektor2 BerechneWiderstandskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(integrator.Integriere(integrand, 0.0, Parameter.l5c), 0.0);
        }

        internal override Vektor2 BerechneAuftriebskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(0.0, -integrator.Integriere(integrand, 0.0, Parameter.l5c));
        }

        internal override double BerechneWiderstandsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Sin(Punkte.Phi5(beta)) * l * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -punkte.P5b.y * integrator.Integriere(integrand1, 0.0, Parameter.l5c) - integrator.Integriere(integrand2, 0.0, Parameter.l5c);
        }

        internal override double BerechneAuftriebsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Cos(Punkte.Phi5(beta)) * l * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -punkte.P5b.x * integrator.Integriere(integrand1, 0.0, Parameter.l5c) - integrator.Integriere(integrand2, 0.0, Parameter.l5c);
        }

        private void SetzeProfil()
        {
            profil = l =>
            {
                if (l < 0.0)
                    throw new ValueOutOfRangeException("Das Argument von f_T2 liegt außerhalb des erlaubten Bereichs.");
                else if (l <= Parameter.l5c)
                    return a;
                else
                    throw new ValueOutOfRangeException("Das Argument von f_T2 liegt außerhalb des erlaubten Bereichs.");
            };
        }

        private double BerechneWindschattenLaenge(double y1, double y2, double K, double beta)
        {
            return (y1 + K - y2) / Math.Sin(Punkte.Phi5(beta));
        }

        private protected override double cW(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cW_T2 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l5c)
                return cW_Basis(Punkte.Phi5(beta));
            else
                throw new ValueOutOfRangeException("Das Argument von cW_T2 liegt außerhalb des erlaubten Bereichs.");
        }

        private protected override double cA(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cA_T2 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l5c)
                return cA_Basis(Punkte.Phi5(beta));
            else
                throw new ValueOutOfRangeException("Das Argument von cA_T2 liegt außerhalb des erlaubten Bereichs.");
        }

        private protected override double vW(double l, double v)
        {
            return GammaRotor(l) * v;
        }

        private double GammaRotor(double l)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von GammaRotor_T2 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= lR1)
                return Modell.GammaRotor;
            else
                return 1.0;
        }
    }
}
