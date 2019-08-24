using System;

namespace Mathematik.Interpolation
{
    sealed class Tridiagonalmatrix
    {
        internal double[,] Matrix { get; set; }
        internal int Dimension { get; }

        public Tridiagonalmatrix(int dimension)
        {
            Dimension = dimension;
            Matrix = new double[Dimension, Dimension];
        }

        // Lösung des LGS mit dem Thomas-Algorithmus
        public double[] LoeseLGS(double[] d)
        {
            if (!IstLoesungsvektorKonsistent(d))
            {
                throw new InconsistentInputException("Die Länge des Vektors d entspricht nicht der Dimension der Tridiagonalmatrix.");
            }

            (double[] a, double[] b, double[] c) = InitialisiereTridiagonalelemente();
            // a enthält untere Nebendiagonalelemente
            // b enthält Hauptdiagonalelemente
            // c enthält obere Nebendiagonalelemente

            try
            {
                ModifiziereKoeffizienten(ref a, ref b, ref c, ref d);
                return LoeseDurchRueckwaertseinsetzen(c, d);
            }
            catch (DivideByZeroException)
            {
                throw new NumericsFailedException("Division durch null! Wahrscheinlich erfüllt die Matrix T nicht die diagonaldominante Bedingung |b[i]| > |a[i]| + |c[i]|.");
            }
        }

        (double[], double[], double[]) InitialisiereTridiagonalelemente()
        {
            double[] a = new double[Dimension];
            double[] b = new double[Dimension];
            double[] c = new double[Dimension];

            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    if (IstUnteresNebendiagonalelement(i, j))
                        a[i] = Matrix[i, j];
                    else if (IstHauptdiagonalelement(i, j))
                        b[i] = Matrix[i, j];
                    else if (IstOberesNebendiagonalelement(i, j))
                        c[i] = Matrix[i, j];
                }
            }

            return (a, b, c);
        }

        void ModifiziereKoeffizienten(ref double[] a, ref double[] b, ref double[] c, ref double[] d)
        {
            c[0] = c[0] / b[0];
            d[0] = d[0] / b[0];

            for (int i = 1; i < Dimension - 1; i++)
            {
                c[i] = c[i] / (b[i] - a[i] * c[i - 1]);
                d[i] = (d[i] - a[i] * d[i - 1]) / (b[i] - a[i] * c[i - 1]);
            }
            d[Dimension - 1] = (d[Dimension - 1] - a[Dimension - 1] * d[Dimension - 2])
                                 / (b[Dimension - 1] - a[Dimension - 1] * c[Dimension - 2]);
        }

        double[] LoeseDurchRueckwaertseinsetzen(double[] c, double[] d)
        {
            double[] v = new double[Dimension];

            v[Dimension - 1] = d[Dimension - 1];

            for (int i = (Dimension - 2); i >= 0; i--)
            {
                v[i] = d[i] - c[i] * v[i + 1];
            }

            return v;
        }

        bool IstLoesungsvektorKonsistent(double[] d)
        {
            return d.Length == Dimension;
        }

        bool IstUnteresNebendiagonalelement(int i, int j)
        {
            return i == (j + 1);
        }

        bool IstHauptdiagonalelement(int i, int j)
        {
            return i == j;
        }

        bool IstOberesNebendiagonalelement(int i, int j)
        {
            return i == (j - 1);
        }
    }
}
