using System;
using Mathematik.Nullstelle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathematikTest
{
    [TestClass]
    public class BisektionsverfahrenTest
    {
        [TestMethod]
        public void NullstelleLineareFunktion()
        {
            // Ausgangszustand
            int anzahlNachkommastellen = 6;
            double genauigkeit = Math.Pow(10, -anzahlNachkommastellen);
            double untereGrenze = -1.5;
            double obereGrenze = 1.0;
            Func<double, double> funktion = x => x;
            double nullstelleErwartet = 0.0;

            // Testausfuehrung
            Bisektionsverfahren bisektion = new Bisektionsverfahren(genauigkeit);
            double nullstelleTatsaechlich = Math.Round(bisektion.ErmittleNullstelle(funktion, untereGrenze, obereGrenze), anzahlNachkommastellen);

            // Ergebnisvergleich
            Assert.AreEqual(nullstelleErwartet, nullstelleTatsaechlich);
        }

        [TestMethod]
        public void NullstelleTrigonometrischeFunktion()
        {
            // Ausgangszustand
            int anzahlNachkommastellen = 6;
            double genauigkeit = Math.Pow(10, -anzahlNachkommastellen);
            double untereGrenze = -1.0;
            double obereGrenze = 2.0;
            Func<double, double> funktion = x => Math.Cos(x);
            double nullstelleErwartet = Math.Round(Math.PI / 2, anzahlNachkommastellen);

            // Testausfuehrung
            Bisektionsverfahren bisektion = new Bisektionsverfahren(genauigkeit);
            double nullstelleTatsaechlich = Math.Round(bisektion.ErmittleNullstelle(funktion, untereGrenze, obereGrenze), anzahlNachkommastellen);

            // Ergebnisvergleich
            Assert.AreEqual(nullstelleErwartet, nullstelleTatsaechlich);
        }
    }
}
