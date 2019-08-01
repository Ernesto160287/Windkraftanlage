using Kennlinienmodell;
using System;
using System.ComponentModel;
using System.Windows;


namespace Anwendung
{
    /// <summary>
    /// Interaktionslogik für KennlinienberechnungWindow.xaml
    /// </summary>
    public partial class KennlinienberechnungWindow : Window
    {
        private BackgroundWorker hintergrundArbeiter;

        public KennlinienberechnungWindow()
        {
            InitializeComponent();

            ButtonBrecheBerechnungAb.IsEnabled = false;
            ButtonBrecheBerechnungAb.Click += BrecheBerechnungAb;
        }

        internal void StarteHintergrundprozess(KennlinienmodellArgs args)
        {
            InitialisiereHintergrundarbeiter();
            hintergrundArbeiter.RunWorkerAsync(args);
            ButtonBrecheBerechnungAb.IsEnabled = hintergrundArbeiter.IsBusy;
        }
        
        private void InitialisiereHintergrundarbeiter()
        {
            hintergrundArbeiter = new BackgroundWorker();

            // Ausführung des Hintergrund-Prozesses
            hintergrundArbeiter.DoWork += FuehreArbeitAus;
            hintergrundArbeiter.RunWorkerCompleted += ArbeitAbgeschlossen;

            // Fortschrittsmeldung des Hintergrund-Prozesses
            hintergrundArbeiter.WorkerReportsProgress = true;
            hintergrundArbeiter.ProgressChanged += FortschrittGeaendert;

            // Abbruch des Hintergrund-Prozesses
            hintergrundArbeiter.WorkerSupportsCancellation = true;
        }

                // Läuft auf dem Hintergrund-Thread
        private void FuehreArbeitAus(object sender, DoWorkEventArgs e)
        {
            Modell modell = new Modell(
                              ((KennlinienmodellArgs)e.Argument).Startgeschwindigkeit,
                              ((KennlinienmodellArgs)e.Argument).Endgeschwindigkeit,
                              ((KennlinienmodellArgs)e.Argument).AnzahlPunkte,
                              ((KennlinienmodellArgs)e.Argument).Genauigkeit,
                              ((KennlinienmodellArgs)e.Argument).AlleKraefte
                            );
        }

        // Läuft auf dem UI-Thread
        private void FortschrittGeaendert(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        // Läuft auf dem UI-Thread
        private void ArbeitAbgeschlossen(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO: genaue Verarbeitung der Ausgabe
            ButtonBrecheBerechnungAb.IsEnabled = hintergrundArbeiter.IsBusy;
        }

        // Läuft auf dem UI-Thread
        private void BrecheBerechnungAb(object sender, RoutedEventArgs e)
        {
            hintergrundArbeiter.CancelAsync();
        }
    }

}
