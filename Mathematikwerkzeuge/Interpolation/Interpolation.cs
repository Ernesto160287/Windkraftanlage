using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windkraftanlage.Mathematikwerkzeuge.Interpolation
{
    internal interface IInterpolation
    {
        double Interpoliere(double p);
    }

    abstract class Interpolation : IInterpolation
    {
        private protected double[] x;
        private protected double[] y;
        private protected double[] xDiff;  // Abstand zwischen zwei aufeinanderfolgenden x-Werten
        private protected int anzahlDaten;

        public Interpolation(double[] x, double[] y, bool sortierteDaten = true)
        {
            if (!AnzahlDatenIdentisch(x, y))
            {
                throw new InconsistentInputException("Anzahl der x- und y-Werte sind verschieden.");
            }

            if (!DatenVorhanden(x))
            {
                throw new InconsistentInputException("Es wurden keine Daten übergeben.");
            }

            BelegeDaten(x, y, sortierteDaten);
            BelegeDifferenzen();
        }

        public abstract double Interpoliere(double p);

        private bool AnzahlDatenIdentisch(double[] x, double[] y)
        {
            return x.Length == y.Length;
        }

        private bool DatenVorhanden(double[] x)
        {
            return x.Length > 0;
        }

        private void BelegeDaten(double[] x, double[] y, Boolean sortierteDaten)
        {
            anzahlDaten = x.Length;

            if (sortierteDaten)
            {
                Array.Sort(x, y);
            }

            this.x = x;
            this.y = y;
        }

        private void BelegeDifferenzen()
        {
            xDiff = new double[anzahlDaten - 1];
            for (int i = 0; i < (anzahlDaten - 1); i++)
            {
                xDiff[i] = x[i + 1] - x[i];
            }
        }

        private protected bool PunktAusserhalbWertebereich(double p)
        {
            return ((p < x[0]) || (p > x[anzahlDaten - 1]));
        }

        private protected bool PunktAufLinkemRand(double p)
        {
            return (p == x[0]);
        }

        private protected bool PunktAufRechtemRand(double p)
        {
            return (p == x[anzahlDaten - 1]);
        }

        private protected int IndexFuerXwertVorPunkt(double p)
        {
            double vorherigerXwert = x.Last(s => s < p);
            return Array.IndexOf(x, vorherigerXwert);
        }
    }
}
