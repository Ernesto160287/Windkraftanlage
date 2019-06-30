using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windkraftanlage.Mathematikwerkzeuge.Integration
{
    internal interface IIntegration
    {
        double Integriere(Func<double, double> funktion, double untereGrenze, double obereGrenze);
    }

    abstract class Integration : IIntegration
    {
        private protected double genauigkeit;

        public Integration(double genauigkeit)
        {
            this.genauigkeit = genauigkeit;
        }

        public abstract double Integriere(Func<double, double> funktion, double untereGrenze, double obereGrenze);

        private protected abstract bool IstApproximationErfolgreich();

    }
}
