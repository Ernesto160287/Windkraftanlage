﻿using Mathematik;
using Mathematik.Integration;
using System;

namespace Kennlinienmodell
{
    class Traeger1 : Bauteil
    {
        // Ausmaße des Trägers 1
        const double a = 0.04;

        // Korrektursummanden
        const double KR1 = Modell.KRotor;

        // Längen unter Berücksichtigung der Korrektursummanden
        double lR1;


        internal Traeger1(Func<double, double> cW, Func<double, double> cA) : base(cW, cA)
        {
        }

        internal override void Aktualisiere(Punkte punkte, double alpha, double beta)
        {
            lR1 = BerechneWindschattenLaenge(punkte.PR1.y, punkte.P4.y, KR1, beta);
        }

        internal override Vektor2 BerechneWiderstandskraft(double v, double alpha, double beta, Integration integrator)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(integrator.Integriere(integrand, 0.0, Parameter.l5b), 0.0);
        }

        internal override Vektor2 BerechneAuftriebskraft(double v, double alpha, double beta, Integration integrator)
        {
            Func<double, double> integrand = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return new Vektor2(0.0, integrator.Integriere(integrand, 0.0, Parameter.l5b));
        }

        internal override double BerechneWiderstandsdrehmoment(Punkte punkte, double v, double alpha, double beta, Integration integrator)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Sin(Punkte.Phi1(beta)) * l * cW(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return -punkte.P4.y * integrator.Integriere(integrand1, 0.0, Parameter.l5b) - integrator.Integriere(integrand2, 0.0, Parameter.l5b);
        }

        internal override double BerechneAuftriebsdrehmoment(Punkte punkte, double v, double alpha, double beta, Integration integrator)
        {
            Func<double, double> integrand1 = l => Parameter.rhoL / 2 * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            Func<double, double> integrand2 = l => Parameter.rhoL / 2 * Math.Cos(Punkte.Phi1(beta)) * l * cA(l, alpha, beta) * Math.Pow(vW(l, v), 2) * profil(l);
            return punkte.P4.x * integrator.Integriere(integrand1, 0.0, Parameter.l5b) + integrator.Integriere(integrand2, 0.0, Parameter.l5b);
        }

        protected override void SetzeProfil()
        {
            profil = l =>
            {
                if (l < 0.0)
                    throw new ValueOutOfRangeException("Das Argument von f_T1 liegt außerhalb des erlaubten Bereichs.");
                else if (l <= Parameter.l5b)
                    return a;
                else
                    throw new ValueOutOfRangeException("Das Argument von f_T1 liegt außerhalb des erlaubten Bereichs.");
            };
        }

        double BerechneWindschattenLaenge(double y1, double y2, double K, double beta)
        {
            return (y1 + K - y2) / Math.Sin(Punkte.Phi1(beta));
        }

        protected override double cW(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cW_T1 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l5b)
                return cW_Basis(beta);
            else
                throw new ValueOutOfRangeException("Das Argument von cW_T1 liegt außerhalb des erlaubten Bereichs.");
        }

        protected override double cA(double l, double alpha, double beta)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von cA_T1 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= Parameter.l5b)
                return cA_Basis(beta);
            else
                throw new ValueOutOfRangeException("Das Argument von cA_T1 liegt außerhalb des erlaubten Bereichs.");
        }

        protected override double vW(double l, double v)
        {
            return GammaRotor(l) * v;
        }

        double GammaRotor(double l)
        {
            if (l < 0.0)
                throw new ValueOutOfRangeException("Das Argument von GammaRotor_T1 liegt außerhalb des erlaubten Bereichs.");
            else if (l <= lR1)
                return Modell.GammaRotor;
            else
                return 1.0;
        }
    }
}
