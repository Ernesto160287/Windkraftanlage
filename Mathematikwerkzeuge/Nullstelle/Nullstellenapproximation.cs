using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windkraftanlage.Mathematikwerkzeuge.Nullstelle
{
    internal interface INullstelle
    {
        double ErmittleNullstelle(Func<double, double> funktion);

    }

    internal interface INullstelleAusIntervall
    {
        double ErmittleNullstelle(Func<double, double> funktion, double untereGrenze, double obereGrenze);

    }


    abstract class Nullstellenapproximation
    {
        private protected double genauigkeit;

        public Nullstellenapproximation(double genauigkeit)
        {
            this.genauigkeit = genauigkeit;
        }

        private protected abstract bool IstApproximationErfolgreich();
    }
}
