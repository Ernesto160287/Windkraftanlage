using System;

namespace MathematikWerkzeuge.Nullstelle
{
    interface INullstelle
    {
        double ErmittleNullstelle(Func<double, double> funktion);
    }

    interface INullstelleAusIntervall
    {
        double ErmittleNullstelle(Func<double, double> funktion, double untereGrenze, double obereGrenze);

    }

    public abstract class Nullstellenapproximation
    {
        protected double genauigkeit;

        public Nullstellenapproximation(double genauigkeit)
        {
            this.genauigkeit = genauigkeit;
        }

        protected abstract bool IstApproximationErfolgreich();
    }
}
