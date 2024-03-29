﻿using System;
using System.Windows;

namespace Anwendung
{
    public class KennlinienmodellArgs
    {
        public double Genauigkeit { get; set; }
        public double Startgeschwindigkeit { get; set; }
        public double Endgeschwindigkeit { get; set; }
        public int AnzahlPunkte { get; set; }
        public bool AlleKraefte { get; set; }


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
                throw new CharacteristicCurveInputException("Falsches Format für die Startwindgeschwindigkeit");
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
                throw new CharacteristicCurveInputException("Falsches Format für die Endwindgeschwindigkeit");
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
                throw new CharacteristicCurveInputException("Falsches Format für die Anzahl der Simulationspunkte");
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
            catch (CharacteristicCurveInputException)
            {
                throw;
            }
        }

        private void PruefeKonsistenzStartgeschwindigkeit()
        {
            if (Startgeschwindigkeit < 0.0)
            {
                MessageBox.Show("Bitte geben Sie eine nicht-negative Startwindgeschwindigkeit ein.");
                throw new CharacteristicCurveInputException("Nicht-negative Startwindgeschwindigkeit");
            }
        }

        private void PruefeKonsistenzEndgeschwindigkeit()
        {
            if (Endgeschwindigkeit <= Startgeschwindigkeit)
            {
                MessageBox.Show("Bitte geben Sie eine Endwindgeschwindigkeit ein, die größer als die Startwindgeschwindigkeit ist.");
                throw new CharacteristicCurveInputException("Endwindgeschwindigkeit ist kleiner/gleich der Startgeschwindigkeit");
            }
        }

        private void PruefeKonsistenzAnzahlPunkte()
        {
            if (AnzahlPunkte <= 0)
            {
                MessageBox.Show("Bitte geben Sie eine positive Zahl für die Anzahl der Simulationspunkte ein.");
                throw new CharacteristicCurveInputException("Endwindgeschwindigkeit ist kleiner/gleich der Startgeschwindigkeit");
            }
        }
    }
}
