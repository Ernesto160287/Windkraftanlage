using System;
using System.Windows;

namespace Anwendung
{
    class InconsistentInputException : Exception
    {
        public InconsistentInputException()
        {
        }

        public InconsistentInputException(String message) : base(message)
        {
        }

        public InconsistentInputException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }

    class KennlinienberechnungArgs
    {
        internal double Genauigkeit { get; set; }
        internal double Startgeschwindigkeit { get; set; }
        internal double Endgeschwindigkeit { get; set; }
        internal int AnzahlPunkte { get; set; }
        internal bool AlleKraefte { get; set; }


        internal void BelegeGenauigkeit(double value)
        {
            Genauigkeit = Math.Pow(10, -value);
        }

        internal void BelegeStartgeschwindigkeit(string text)
        {
            try
            {
                Startgeschwindigkeit = Double.Parse(text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Bitte geben Sie eine Gleitkommazahl für die Startwindgeschwindigkeit ein.");
                throw;
            }
        }
               
        internal void BelegeEndgeschwindigkeit(string text)
        {
            try
            {
                Endgeschwindigkeit = Double.Parse(text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Bitte geben Sie eine Gleitkommazahl für die Endwindgeschwindigkeit ein.");
                throw;
            }
        }

        internal void BelegeAnzahlPunkte(string text)
        {
            try
            {
                AnzahlPunkte = Int32.Parse(text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Bitte geben Sie eine ganze Zahl für die Anzahl der Simulationspunkte ein.");
                throw;
            }
        }

        internal void BelegeAlleKraefte(bool value)
        {
            AlleKraefte = value;
        }

        internal void PruefeKonsistenz()
        {
            try
            {
                PruefeKonsistenzStartgeschwindigkeit();
                PruefeKonsistenzEndgeschwindigkeit();
                PruefeKonsistenzAnzahlPunkte();
            }
            catch (InconsistentInputException)
            {
                throw;
            }
        }

        private void PruefeKonsistenzStartgeschwindigkeit()
        {
            if (Startgeschwindigkeit < 0.0)
            {
                MessageBox.Show("Bitte geben Sie eine nicht-negative Startwindgeschwindigkeit ein.");
                throw new InconsistentInputException("Nicht-negative Startwindgeschwindigkeit");
            }
        }

        private void PruefeKonsistenzEndgeschwindigkeit()
        {
            if (Endgeschwindigkeit <= Startgeschwindigkeit)
            {
                MessageBox.Show("Bitte geben Sie eine Endwindgeschwindigkeit ein, die größer als die Startwindgeschwindigkeit ist.");
                throw new InconsistentInputException("Endwindgeschwindigkeit ist kleiner/gleich der Startgeschwindigkeit");
            }
        }

        private void PruefeKonsistenzAnzahlPunkte()
        {
            if (AnzahlPunkte <= 0)
            {
                MessageBox.Show("Bitte geben Sie eine positive Zahl für die Anzahl der Simulationspunkte ein.");
                throw new InconsistentInputException("Endwindgeschwindigkeit ist kleiner/gleich der Startgeschwindigkeit");
            }
        }
    }
}
