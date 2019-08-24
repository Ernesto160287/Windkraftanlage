using System;
using Mathematik.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathematikTest
{
    [TestClass]
    public class SimpsonRegelTest
    {
        [TestMethod]
        public void IntegrationKonstanteFunktion()
        {
            // Ausgangszustand
            double genauigkeit = 0.00001;
            double untereGrenze = 0.0;
            double obereGrenze = 1.0;
            Func<double, double> funktion = x => 1.0;
            double integralErwartet = 1.0;

            // Testausfuehrung
            SimpsonRegel simpson = new SimpsonRegel(genauigkeit);
            double integralTatsaechlich = simpson.Integriere(funktion, untereGrenze, obereGrenze);

            // Ergebnisvergleich
            Assert.AreEqual(integralErwartet, integralTatsaechlich);
        }

        [TestMethod]
        public void IntegrationLineareFunktion()
        {
            // Ausgangszustand
            double genauigkeit = 0.00001;
            double untereGrenze = 0.0;
            double obereGrenze = 1.0;
            Func<double, double> funktion = x => 1.0 + x;
            double integralErwartet = 1.5;

            // Testausfuehrung
            SimpsonRegel simpson = new SimpsonRegel(genauigkeit);
            double integralTatsaechlich = simpson.Integriere(funktion, untereGrenze, obereGrenze);

            // Ergebnisvergleich
            Assert.AreEqual(integralErwartet, integralTatsaechlich);
        }

        [TestMethod]
        public void IntegrationQuadratischeFunktion()
        {
            // Ausgangszustand
            double genauigkeit = 0.00001;
            double untereGrenze = 0.0;
            double obereGrenze = 1.0;
            Func<double, double> funktion = x => 1.0 + Math.Pow(x,2);
            double integralErwartet = 4.0/3.0;

            // Testausfuehrung
            SimpsonRegel simpson = new SimpsonRegel(genauigkeit);
            double integralTatsaechlich = simpson.Integriere(funktion, untereGrenze, obereGrenze);

            // Ergebnisvergleich
            Assert.AreEqual(integralErwartet, integralTatsaechlich);
        }

        [TestMethod]
        public void IntegrationTrigonometrischeFunktion()
        {
            // Ausgangszustand
            double genauigkeit = 1.0e-10;
            double untereGrenze = 0.0;
            double obereGrenze = Math.PI;
            Func<double, double> funktion = x => Math.Pow(Math.Cos(x),2);
            double integralErwartet = Math.PI / 2;

            // Testausfuehrung
            SimpsonRegel simpson = new SimpsonRegel(genauigkeit);
            double integralTatsaechlich = simpson.Integriere(funktion, untereGrenze, obereGrenze);

            // Ergebnisvergleich
            Assert.AreEqual(integralErwartet, integralTatsaechlich);
        }
    }
}
