using System;

namespace MathematikWerkzeuge.Integration
{
    interface IIntegration
    {
        double Integriere(Func<double, double> funktion, double untereGrenze, double obereGrenze);
    }

    public abstract class Integration : IIntegration
    {
        protected double genauigkeit;

        public Integration(double genauigkeit)
        {
            this.genauigkeit = genauigkeit;
        }

        public abstract double Integriere(Func<double, double> funktion, double untereGrenze, double obereGrenze);

        protected abstract bool IstApproximationErfolgreich();
    }
}
