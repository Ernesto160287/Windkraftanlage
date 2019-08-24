using System;

namespace Mathematik.Nullstelle
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
                Console.WriteLine("Die Funktionwerte an den Intervallgrenzen haben keine entgesetzten Vorzeichen");
                return false;
            }

            return true;
        }

        protected bool ZulaessigeIntervallgrenzen(double untereGrenze, double obereGrenze)
        {
            return (untereGrenze <= obereGrenze);
        }

        protected bool UnterschiedlicheVorzeichenAnIntervallgrenzen(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            Console.WriteLine("untere Grenze a = " + untereGrenze);
            Console.WriteLine("obere Grenze b =" + obereGrenze);
            Console.WriteLine("f(a) = " + funktion(untereGrenze));
            Console.WriteLine("f(b) = " + funktion(obereGrenze));

            for (int i = 0; i < 100; i++)
            {
                double x =  untereGrenze + i * (obereGrenze - untereGrenze) / 10;
                Console.WriteLine("x = " + x);
                Console.WriteLine("f(x) = " + funktion(x));

            }

            return (funktion(untereGrenze) * funktion(obereGrenze) < 0.0);
        }

        protected abstract override bool IstApproximationErfolgreich();

        public abstract double ErmittleNullstelle(Func<double, double> funktion, double untereGrenze, double obereGrenze);
    }
}
