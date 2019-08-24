using System;

namespace Mathematik.Interpolation
{
    public class KubischeInterpolation : Interpolation
    {
        // Koeffizienten des Polynoms dritten Grades:
        // x -> a * x^3 + b * x^2 + c * x + d
        double[] a;
        double[] b;
        double[] c;
        double[] d;

        public KubischeInterpolation(double[] x, double[] y, bool sortierteDaten = true) : base(x, y, sortierteDaten)
        {
            if (!GenuegendDatenVorhanden())
            {
                throw new InconsistentInputException("Die Menge an Datenpaare ist nicht ausreichend.");
            }

            BelegeKoeffizienten();
        }

        public override double Interpoliere(double p)
        {
            if (PunktAusserhalbWertebereich(p))
                throw new InconsistentInputException("Der Punkt liegt nicht in dem für die Interpolation relevanten Intervall.");

            if (PunktAufLinkemRand(p))
                return y[0];

            if (PunktAufRechtemRand(p))
                return y[anzahlDaten - 1];

            int i = IndexFuerXwertVorPunkt(p);
            // -> x[i] < p <= x[i + 1]

            return a[i] * Math.Pow((p - x[i]), 3) + b[i] * Math.Pow((p - x[i]), 2) + c[i] * (p - x[i]) + d[i];
        }

        bool GenuegendDatenVorhanden()
        {
            return x.Length >= 4;
        }

        void BelegeKoeffizienten()
        {
            double[] y2 = LoeseKubischeInterpolation();  // Stützstellen für die zweite Ableitung

            a = BelegeKoeffizientenA(y2);
            b = BelegeKoeffizientenB(y2);
            c = BelegeKoeffizientenC(y2);
            d = BelegeKoeffizientenD();
        }

        double[] LoeseKubischeInterpolation()
        {
            double[] v = new double[anzahlDaten];

            // natürliche Interpolation: an den Rändern verschwindet die zweite Ableitung
            v[0] = 0;
            v[anzahlDaten - 1] = 0;

            Tridiagonalmatrix T = ErzeugeTridiagonalmatrix();
            double[] konstantenvektorNatuerlich = ErzeugeKonstantenvektor();

            double[] loesungsvektor = T.LoeseLGS(konstantenvektorNatuerlich);

            for (int i = 1; i < anzahlDaten - 1; i++)
            {
                v[i] = loesungsvektor[i - 1];
            }

            return v;
        }

        Tridiagonalmatrix ErzeugeTridiagonalmatrix()
        {
            Tridiagonalmatrix T = new Tridiagonalmatrix(anzahlDaten - 2);

            for (int i = 0; i < T.Dimension - 1; i++)
            {
                T.Matrix[i, i + 1] = xDiff[i + 1];
                T.Matrix[i, i] = 2 * (xDiff[i] + xDiff[i + 1]);
                T.Matrix[i + 1, i] = xDiff[i + 1];
            }
            T.Matrix[T.Dimension - 1, T.Dimension - 1] = 2 * (xDiff[T.Dimension - 1] + xDiff[T.Dimension]);

            return T;
        }

        double[] ErzeugeKonstantenvektor()
        {
            double[] v = new double[anzahlDaten - 2];

            for (int i = 0; i < v.Length; i++)
            {
                v[i] = 6 * (y[i + 2] - y[i + 1]) / xDiff[i + 1] - 6 * (y[i + 1] - y[i]) / xDiff[i];
            }

            return v;
        }

        double[] BelegeKoeffizientenA(double[] v)
        {
            double[] a = new double[anzahlDaten - 1];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (v[i + 1] - v[i]) / (6 * xDiff[i]);
            }
            return a;
        }

        double[] BelegeKoeffizientenB(double[] v)
        {
            double[] b = new double[anzahlDaten - 1];

            for (int i = 0; i < b.Length; i++)
            {
                b[i] = v[i] / 2;
            }
            return b;
        }

        double[] BelegeKoeffizientenC(double[] v)
        {
            double[] c = new double[anzahlDaten - 1];

            for (int i = 0; i < c.Length; i++)
            {
                c[i] = (y[i + 1] - y[i]) / xDiff[i] - xDiff[i] * (v[i + 1] + 2 * v[i]) / 6;
            }
            return c;
        }

        double[] BelegeKoeffizientenD()
        {
            double[] d = new double[anzahlDaten - 1];
            for (int i = 0; i < d.Length; i++)
            {
                d[i] = y[i];
            }
            return d;
        }
    }
}
