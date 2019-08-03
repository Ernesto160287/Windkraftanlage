using System;

namespace Mathematik.Nullstelle
{
    public class Bisektionsverfahren : NullstellenApproximationAusIntervall
    {
        double xVorher;

        double xAktuell;
        double yAktuell;

        double intervallbreite;  // vorzeichenbehaftet

        public Bisektionsverfahren(double genauigkeit) : base(genauigkeit)
        {
        }

        public override double ErmittleNullstelle(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            if (!HatKonsistenteEingaben(funktion, untereGrenze, obereGrenze))
                throw new InconsistentInputException("Die Eingaben sind unzulässig.");

            (xVorher, intervallbreite) = BelegeStartposition(funktion, untereGrenze, obereGrenze);

            for (int j = 1; j <= 200; j++)
            {
                ErmittleNeueApproximation(funktion);

                if (IstApproximationErfolgreich())
                    return xAktuell;
                else
                    BereiteNaechstenIterationsschrittVor();
            }

            throw new NumericsFailedException("Das Bisektionsverfahren konvergiert nicht.");
        }

        private (double, double) BelegeStartposition(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            if (funktion(untereGrenze) < 0.0)
                return (untereGrenze, obereGrenze - untereGrenze);
            else
                return (obereGrenze, untereGrenze - obereGrenze);
        }

        private void ErmittleNeueApproximation(Func<double, double> funktion)
        {
            intervallbreite /= 2;
            xAktuell = xVorher + intervallbreite;
            yAktuell = funktion(xAktuell);
        }

        protected override bool IstApproximationErfolgreich()
        {
            return (Math.Abs(intervallbreite) < genauigkeit || yAktuell == 0.0);
        }

        private void BereiteNaechstenIterationsschrittVor()
        {
            if (yAktuell < 0.0)
                xVorher = xAktuell;
        }
    }
}
