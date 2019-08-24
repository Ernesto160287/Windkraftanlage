using System;

namespace Mathematik.Integration
{
    public class SimpsonRegel : Integration
    {
        double flaecheTrapez;
        double flaecheTrapezVorher = 0.0;

        double flaecheSimpson;
        double flaecheSimpsonVorher = 0.0;

        public SimpsonRegel(double genauigkeit) : base(genauigkeit)
        {
        }

        public override double Integriere(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            int n = 1;

            for (int i = 1; i <= 20; i++)
            {
                AktualisiereFlaecheTrapez(funktion, untereGrenze, obereGrenze, n);
                AktualisiereFlaecheSimpson();

                if (IstApproximationErfolgreich())
                {
                    return flaecheSimpson;
                }

                n = BereiteNaechstenIterationsschrittVor(n);
            }

            throw new NumericsFailedException("Die numerische Integration mittels Simpon-Regel konvergiert nicht.");
        }

        void AktualisiereFlaecheTrapez(Func<double, double> funktion, double untereGrenze, double obereGrenze, int n)
        {
            flaecheTrapez = Trapezregel(funktion, untereGrenze, obereGrenze, n);
        }

        double Trapezregel(Func<double, double> funktion, double untereGrenze, double obereGrenze, int n)
        {
            double schrittweite = (obereGrenze - untereGrenze) / n;
            double summeRandwerte = funktion(untereGrenze) + funktion(obereGrenze);
            double summeStuetzstellen = 0.0;

            if (n > 1)
            {
                Func<int, double> x = j => untereGrenze + j * schrittweite;

                for (int j = 1; j <= n - 1; j++)
                {
                    summeStuetzstellen += funktion(x(j));
                }
            }

            return 0.5 * schrittweite * (summeRandwerte + 2 * summeStuetzstellen);

        }

        void AktualisiereFlaecheSimpson()
        {
            flaecheSimpson = (4.0 * flaecheTrapez - flaecheTrapezVorher) / 3.0;
        }

        protected override bool IstApproximationErfolgreich()
        {
            return Math.Abs(flaecheSimpson - flaecheSimpsonVorher) < genauigkeit * Math.Abs(flaecheSimpsonVorher);
        }

        int BereiteNaechstenIterationsschrittVor(int n)
        {
            flaecheSimpsonVorher = flaecheSimpson;
            flaecheTrapezVorher = flaecheTrapez;

            return 2 * n;
        }
    }
}
