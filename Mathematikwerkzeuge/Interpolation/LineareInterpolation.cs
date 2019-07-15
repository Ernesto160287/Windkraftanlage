namespace MathematikWerkzeuge.Interpolation
{
    public class LineareInterpolation : Interpolation
    {
        public LineareInterpolation(double[] x, double[] y, bool sortierteDaten = true) : base(x, y, sortierteDaten)
        {
        }

        public override double Interpoliere(double p)
        {
            if (PunktAusserhalbWertebereich(p))
                throw new InconsistentInputException("Der Punkt liegt nicht in dem für die Interpolation relevanten Intervall.");

            if (PunktAufLinkemRand(p))
                return y[0];

            if (PunktAufRechtemRand(p))
                return y[anzahlDaten - 1];

            int i = IndexFuerXwertVorPunkt(p);
            // -> x[i] < p <= x[i + 1]

            return y[i] + (p - x[i]) * (y[i + 1] - y[i]) / xDiff[i];
        }
    }
}
