using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windkraftanlage.Mathematikwerkzeuge.Nullstelle
{
    abstract class NullstellenApproximationAusIntervall : Nullstellenapproximation, INullstelleAusIntervall
    {

        public NullstellenApproximationAusIntervall(double genauigkeit) : base(genauigkeit)
        {
        }

        private protected bool HatKonsistenteEingaben(Func<double,double> funktion, double untereGrenze, double obereGrenze)
        {
            if (!ZulaessigeIntervallgrenzen(untereGrenze, obereGrenze))
            {
                Console.WriteLine("Die untere Intervallgrenze ist nicht kleiner als die obere Intervallgrenze");
                return false;
            }

            if (!UnterschiedlicheVorzeichenAnIntervallgrenzen(funktion, untereGrenze, obereGrenze))
            {
                Console.WriteLine("Die Funktionwerte an den Intervallgrenzen haben keine entgesetzte Vorzeichen");
                return false;
            }

            return true;
        }
                
        private protected bool ZulaessigeIntervallgrenzen(double untereGrenze, double obereGrenze)
        {
            return (untereGrenze >= obereGrenze);
        }

        private protected bool UnterschiedlicheVorzeichenAnIntervallgrenzen(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            return (funktion(untereGrenze) * funktion(obereGrenze) < 0.0);
        }

        private protected abstract override bool IstApproximationErfolgreich();

        public abstract double ErmittleNullstelle(Func<double, double> funktion, double untereGrenze, double obereGrenze);

    }
}
