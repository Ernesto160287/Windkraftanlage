using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windkraftanlage.Mathematikwerkzeuge;
using Windkraftanlage.Mathematikwerkzeuge.Integration;

namespace Windkraftanlage.Kennlinienmodell
{
    class LinkeRotorgondelseite : Bauteil
    {
        // Ausmaße der linken Rotorgondelseite
        const double a = 0.68;
        const double c = 0.45;
        const double h = 0.03;

        // Korrektursummanden
        const double KR1 = Modell.KRotor;

        // Windfaktoren

        // Längen unter Berücksichtigung der Korrektursummanden
        double lR1;


        internal LinkeRotorgondelseite(Integration integrator, Func<double, double> cW, Func<double, double> cA) : base(integrator, cW, cA)
        {
        }

        internal override void Aktualisiere(Punkte punkte, double alpha, double beta)
        {
            lR1 = BerechneWindschattenLaenge(punkte.PR1.y, punkte.P4.y, KR1, beta);
        }

        internal override Vektor2 BerechneWiderstandskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(integrator.Integriere(integrand, 0.0, Parameter.l9), 0.0);
        }

        internal override Vektor2 BerechneAuftriebskraft(double v, double alpha, double beta)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(0.0, -integrator.Integriere(integrand, 0.0, Parameter.l9));
        }

        internal override double BerechneWiderstandsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Sin(Punkte.Phi2(beta)) * l * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -punkte.P4.y * integrator.Integriere(integrand1, 0.0, Parameter.l9) - integrator.Integriere(integrand2, 0.0, Parameter.l9);
        }

        internal override double BerechneAuftriebsdrehmoment(Punkte punkte, double v, double alpha, double beta)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Cos(Punkte.Phi2(beta)) * l * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -punkte.P4.x * integrator.Integriere(integrand1, 0.0, Parameter.l9) - integrator.Integriere(integrand2, 0.0, Parameter.l9);
        }

        private protected override void SetzeProfil()
        {
            profil = l =>
            {
                if (l < 0.0)
                    throw new ValueOutOfRangeException("Das Argument von f_R liegt außerhalb des erlaubten Bereichs.");
                else if (l <= Parameter.l11)
                    return a;
                else if (l <= Parameter.l8)
                    return h;
                else if (l <= Parameter.l9)
                    return c;
                else
                    throw new ValueOutOfRangeException("Das Argument von f_R liegt außerhalb des erlaubten Bereichs.");
            };
        }

        private double BerechneWindschattenLaenge(double y1, double y2, double K, double beta)
        {
            return (y1 + K - y2) / Math.Sin(Punkte.Phi2(beta));
        }

        private protected override double cW(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cW_R liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l9)
                return cW_Basis(Punkte.Phi5(beta));
            else
                throw new ValueOutOfRangeException("Das Argument von cW_R liegt außerhalb des erlaubten Bereichs.");
        }

        private protected override double cA(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cA_R liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l9)
                return cA_Basis(Punkte.Phi5(beta));
            else
                throw new ValueOutOfRangeException("Das Argument von cA_R liegt außerhalb des erlaubten Bereichs.");
        }

        private protected override double vW(double l, double v)
        {
            return GammaRotor(l) * v;
        }

        private double GammaRotor(double l)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von GammaRotor_R liegt außerhalb des erlaubten Bereichs.");
            else if (l <= lR1)
                return Modell.GammaRotor;
            else
                return 1.0;
        }
    }
}
