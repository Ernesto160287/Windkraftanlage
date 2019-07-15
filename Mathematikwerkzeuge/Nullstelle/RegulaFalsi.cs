using System;

namespace MathematikWerkzeuge.Nullstelle
{
    public class RegulaFalsi : NullstellenApproximationAusIntervall
    {
        double xAktuell;
        double yAktuell;

        double xHilf1;
        double xHilf2;

        public RegulaFalsi(double genauigkeit) : base(genauigkeit)
        {
        }

        public override double ErmittleNullstelle(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            if (!HatKonsistenteEingaben(funktion, untereGrenze, obereGrenze))
                throw new InconsistentInputException("Die Eingaben sind unzulässig.");

            InitialisiereHilfswerte(untereGrenze, obereGrenze);

            for (int j = 1; j <= 200; j++)
            {
                ErmittleNeueApproximation(funktion);

                if (IstApproximationErfolgreich())
                    return xAktuell;
            }

            throw new NumericsFailedException("Das Regula-Falsi-Verfahren konvergiert nicht.");
        }

        void InitialisiereHilfswerte(double untereGrenze, double obereGrenze)
        {
            xHilf1 = untereGrenze;
            xHilf2 = obereGrenze;
        }

        void ErmittleNeueApproximation(Func<double, double> funktion)
        {
            xHilf2 = xHilf1;
            xHilf1 = xAktuell;
            xAktuell = xHilf1 - (xHilf2 - xHilf1) / (funktion(xHilf2) - funktion(xHilf1)) * funktion(xHilf1);
            yAktuell = funktion(xAktuell);
        }

        protected override bool IstApproximationErfolgreich()
        {
            return (Math.Abs(yAktuell) <= genauigkeit);
        }
    }
}
