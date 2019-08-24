using System;

namespace Mathematik.Nullstelle
{
    public class Bisektionsverfahren : NullstellenApproximationAusIntervall
    {
        double positionVorher;

        double position;
        double funktionswert;

        double intervallbreite;  // vorzeichenbehaftet

        public Bisektionsverfahren(double genauigkeit) : base(genauigkeit)
        {
        }

        public override double ErmittleNullstelle(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            if (!HatKonsistenteEingaben(funktion, untereGrenze, obereGrenze))
                throw new InconsistentInputException("Die Eingaben sind unzulässig.");

            (positionVorher, intervallbreite) = BelegeStartposition(funktion, untereGrenze, obereGrenze);

            for (int j = 1; j <= 200; j++)
            {
                AktualisierePosition();

                funktionswert = funktion(position);

                if (IstApproximationErfolgreich())
                    return position;
                else
                    AktualisierePositionVorher();
            }

            throw new NumericsFailedException("Das Bisektionsverfahren konvergiert nicht.");
        }

        (double, double) BelegeStartposition(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            if (funktion(untereGrenze) < 0.0)
                return (untereGrenze, obereGrenze - untereGrenze);
            else
                return (obereGrenze, untereGrenze - obereGrenze);
        }

        void AktualisierePosition()
        {
            intervallbreite /= 2;
            position = positionVorher + intervallbreite;
        }

        protected override bool IstApproximationErfolgreich()
        {
            return (Math.Abs(intervallbreite) < genauigkeit || funktionswert == 0.0);
        }

        void AktualisierePositionVorher()
        {
            if (funktionswert < 0.0)
                positionVorher = position;
        }
    }
}
