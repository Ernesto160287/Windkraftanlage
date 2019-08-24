using System;
using Mathematik.Interpolation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathematikTest
{
    [TestClass]
    public class KubischeInterpolationTest
    {
        [TestMethod]
        public void KonstanteFunktion()
        {
            // Ausgangszustand
            double[] xWerte = { 0.0, 1.0, 2.0, 3.0, 4.0 };
            double[] yWerte = { 1.0, 1.0, 1.0, 1.0, 1.0 };
            double[] punkte = { 0.0, 1.3, 2.5, 3.4 };
            double[] funktionswerteErwartet = { 1.0, 1.0, 1.0, 1.0 };

            // Testausfuehrung
            KubischeInterpolation daten = new KubischeInterpolation(xWerte, yWerte);

            double[] funktionswerteTatsaechlich = new double[punkte.Length];

            for (int i = 0; i < funktionswerteTatsaechlich.Length; i++)
            {
                funktionswerteTatsaechlich[i] = daten.Interpoliere(punkte[i]);
            }

            // Ergebnisvergleich
            for (int i = 0; i < funktionswerteTatsaechlich.Length; i++)
            {
                Assert.AreEqual(funktionswerteErwartet[i], funktionswerteTatsaechlich[i]);
            }
        }

        [TestMethod]
        public void LineareFunktion()
        {
            // Ausgangszustand
            double[] xWerte = { 0.0, 1.0, 2.0, 3.0, 4.0 };
            double[] yWerte = { 1.0, 3.0, 5.0, 7.0, 9.0 };
            double[] punkte = { 0.0, 1.3, 2.5, 3.4 };
            double[] funktionswerteErwartet = { 1.0, 3.6, 6.0, 7.8 };

            // Testausfuehrung
            KubischeInterpolation daten = new KubischeInterpolation(xWerte, yWerte);

            double[] funktionswerteTatsaechlich = new double[punkte.Length];

            for (int i = 0; i < funktionswerteTatsaechlich.Length; i++)
            {
                funktionswerteTatsaechlich[i] = daten.Interpoliere(punkte[i]);
            }

            // Ergebnisvergleich
            for (int i = 0; i < funktionswerteTatsaechlich.Length; i++)
            {
                Assert.AreEqual(funktionswerteErwartet[i], funktionswerteTatsaechlich[i]);
            }
        }

        [TestMethod]
        public void QuadratischeFunktion()
        {
            // Ausgangszustand
            double[] xWerte = { 0.0, 1.0, 2.0, 3.0, 4.0 };
            double[] yWerte = { 0.0, 1.0, 4.0, 9.0, 16.0 };
            double[] punkte = { 0.0, 1.3, 2.5, 3.4 };
            double[] funktionswerteErwartet = { 0.0, 1.69, 6.25, 11.56 };
            double genauigkeit = 0.08;

            // Testausfuehrung
            KubischeInterpolation daten = new KubischeInterpolation(xWerte, yWerte);

            double[] funktionswerteTatsaechlich = new double[punkte.Length];

            for (int i = 0; i < funktionswerteTatsaechlich.Length; i++)
            {
                funktionswerteTatsaechlich[i] = daten.Interpoliere(punkte[i]);
            }

            // Ergebnisvergleich
            for (int i = 0; i < funktionswerteTatsaechlich.Length; i++)
            {
                double differenz = Math.Abs(funktionswerteTatsaechlich[i] - funktionswerteErwartet[i]);
                Assert.AreEqual(0.0, Math.Floor(differenz / genauigkeit));
            }
        }

        [TestMethod]
        public void TrigonometrischeFunktion()
        {
            // Ausgangszustand
            double[] xWerte = { 0.0, Math.PI / 2, 3 * Math.PI / 4, Math.PI, 5 * Math.PI / 4, 7 * Math.PI / 4, 2 * Math.PI };
            double[] yWerte = { 1.0, 0.0, -Math.Sqrt(2) / 2, -1.0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2, 1.0 };
            double[] punkte = { Math.PI / 12, Math.PI / 6, 17 * Math.PI / 12 };
            double[] funktionswerteErwartet = { (Math.Sqrt(6) + Math.Sqrt(2)) / 4, Math.Sqrt(3) / 2, (-Math.Sqrt(6) + Math.Sqrt(2)) / 4 };
            double genauigkeit = 0.58;

            // Testausfuehrung
            KubischeInterpolation daten = new KubischeInterpolation(xWerte, yWerte);

            double[] funktionswerteTatsaechlich = new double[punkte.Length];

            for (int i = 0; i < funktionswerteTatsaechlich.Length; i++)
            {
                funktionswerteTatsaechlich[i] = daten.Interpoliere(punkte[i]);
            }

            // Ergebnisvergleich
            for (int i = 0; i < funktionswerteTatsaechlich.Length; i++)
            {
                double differenz = Math.Abs(funktionswerteTatsaechlich[i] - funktionswerteErwartet[i]);
                Assert.AreEqual(0.0, Math.Floor(differenz / genauigkeit));
            }
        }
    }
}
