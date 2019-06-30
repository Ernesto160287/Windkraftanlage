using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windkraftanlage.Mathematikwerkzeuge.Integration
{
    class SimpsonRegel : Integration
    {
        double flaecheTrapez;
        double flaecheTrapezVorher = 0.0;

        double flaecheSimpson;
        double flaecheSimpsonvorher = 0.0;

        public SimpsonRegel(double genauigkeit) : base(genauigkeit)
        {
        }

        public override double Integriere(Func<double, double> funktion, double untereGrenze, double obereGrenze)
        {
            int n = 1;

            for (int i = 1; i <= 20; i++)
            {
                AktualisiereFlaechen(funktion, untereGrenze, obereGrenze, n);

                if (IstApproximationErfolgreich())
                {
                    return flaecheSimpson;
                }

                UebertrageFlaechen();

                n = 2 * n;
            }

            throw new NumericsFailedException("Die numerische Integration mittels Simpon-Regel konvergiert nicht.");
        }

        private void AktualisiereFlaechen(Func<double, double> funktion, double untereGrenze, double obereGrenze, int n)
        {
            flaecheTrapez = Trapezregel(funktion, untereGrenze, obereGrenze, n);
            flaecheSimpson = (4 * flaecheTrapez - flaecheTrapezVorher) / 4;
        }

        private void UebertrageFlaechen()
        {
            flaecheSimpsonvorher = flaecheSimpson;
            flaecheTrapezVorher = flaecheTrapez;
        }

        private double Trapezregel(Func<double, double> funktion, double untereGrenze, double obereGrenze, int n)
        {
            double schrittweite = (obereGrenze - untereGrenze) / n;
            Func<int, double> x = j => untereGrenze + j * schrittweite;

            double integal = 0.5 * schrittweite * (funktion(untereGrenze) + funktion(obereGrenze));

            for (int j = 1; j <= n - 1; j++)
            {
                integal += funktion(x(j));
            }

            return integal;
        }

        private protected override bool IstApproximationErfolgreich()
        {
            return Math.Abs(flaecheSimpson - flaecheSimpsonvorher) < genauigkeit * Math.Abs(flaecheSimpsonvorher);
        }


    }
}
