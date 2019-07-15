using System;

namespace MathematikWerkzeuge.Nullstelle
{
    public abstract class NullstellenApproximationAusIntervall : Nullstellenapproximation, INullstelleAusIntervall
    {
        public NullstellenApproximationAusIntervall(double genauigkeit) : base(genauigkeit)
        {
        }

        protected bool HatKonsistenteEingaben(Func<double, double> funktion, double untereGrenze, double obereGrenze)
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

        protected bool ZulaessigeIntervallgrenzen(double untereGrenze, double obereGrenze)
        {
            return (untereGrenze >= obereGrenze);
        }

        protected bool UnterschiedlicheVorzeichenAnIntervallgrenzen(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            return (funktion(untereGrenze) * funktion(obereGrenze) < 0.0);
        }

        protected abstract override bool IstApproximationErfolgreich();

        public abstract double ErmittleNullstelle(Func<double, double> funktion, double untereGrenze, double obereGrenze);
    }
}
