using System;
using System.Windows;

namespace Anwendung.WindowKennlinie
{
    public class KennlinienmodellArgs
    {
        public double Genauigkeit { get; set; }
        public double Startgeschwindigkeit { get; set; }
        public double Endgeschwindigkeit { get; set; }
        public int AnzahlPunkte { get; set; }
        public bool AlleKraefte { get; set; }

        internal void BelegeArgumente(double valueGenauigkeit,
                                      double textStartgeschwindigkeit,
                                      double textEndgeschwindigkeit,
                                      int textAnzahlPunkte,
                                      bool valueAlleKraefte
                                      )
        {
            BelegeGenauigkeit(valueGenauigkeit);
            BelegeStartgeschwindigkeit(textStartgeschwindigkeit);
            BelegeEndgeschwindigkeit(textEndgeschwindigkeit);
            BelegeAnzahlPunkte(textAnzahlPunkte);
            BelegeAlleKraefte(valueAlleKraefte);
        }
         
        void BelegeGenauigkeit(double value)
        {
            Genauigkeit = Math.Pow(10, -value);
        }

        void BelegeStartgeschwindigkeit(double text)
        {
            Startgeschwindigkeit = text;
        }
               
        void BelegeEndgeschwindigkeit(double text)
        {
                Endgeschwindigkeit = text;
        }

        void BelegeAnzahlPunkte(int text)
        {
            AnzahlPunkte = text;
        }

        void BelegeAlleKraefte(bool value)
        {
            AlleKraefte = value;
        }

        internal void PruefeKonsistenz()
        {
            PruefeKonsistenzStartgeschwindigkeit();
            PruefeKonsistenzEndgeschwindigkeit();
            PruefeKonsistenzAnzahlPunkte();
        }

        void PruefeKonsistenzStartgeschwindigkeit()
        {
            if (Startgeschwindigkeit < 0.0)
            {
                MessageBox.Show("Bitte geben Sie eine nicht-negative Startwindgeschwindigkeit ein.");
                throw new CharacteristicCurveInputException("Nicht-negative Startwindgeschwindigkeit");
            }
        }

        void PruefeKonsistenzEndgeschwindigkeit()
        {
            if (Endgeschwindigkeit <= Startgeschwindigkeit)
            {
                MessageBox.Show("Bitte geben Sie eine Endwindgeschwindigkeit ein, die größer als die Startwindgeschwindigkeit ist.");
                throw new CharacteristicCurveInputException("Endwindgeschwindigkeit ist kleiner/gleich der Startgeschwindigkeit");
            }
        }

        void PruefeKonsistenzAnzahlPunkte()
        {
            if (AnzahlPunkte <= 0)
            {
                MessageBox.Show("Bitte geben Sie eine positive Zahl für die Anzahl der Simulationspunkte ein.");
                throw new CharacteristicCurveInputException("Endwindgeschwindigkeit ist kleiner/gleich der Startgeschwindigkeit");
            }
        }
    }
}
