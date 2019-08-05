using Kennlinienmodell;
using Mathematik;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Anwendung.WindowKennlinie
{
    /// <summary>
    /// Interaktionslogik für KennlinieView.xaml
    /// </summary>
    public partial class KennlinieView : Window
    {
        private BackgroundWorker backgroundWorker;
        private event EventHandler<KennlinienmodellArgs> KennlinienberechnungAngefordert;

        public KennlinieView()
        {
            InitializeComponent();

            InitialisiereHintergrundarbeiter();

            InitialisiereButtonStarteBerechnung();
            InitialisiereButtonBrecheBerechnungAb();
            InitialisiereButtonSpeichereErgebnisse();
            InitialisiereTextBoxStatus();

            KennlinienberechnungAngefordert += StarteBerechnung;
            Closing += KennlinienmodellWindow_Schliessen;

        }

        private void InitialisiereHintergrundarbeiter()
        {
            backgroundWorker = new BackgroundWorker();

            // Start des Hintergrund-Prozesses
            backgroundWorker.DoWork += FuehrHintergrundProzessAus;

            // Fortschrittsmeldung des Hintergrund-Prozesses
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.ProgressChanged += HintergrundProzessFortschrittGeaendert;

            // Ende des Hintergrund-Prozesses
            backgroundWorker.RunWorkerCompleted += HintergrundProzessAbgeschlossen;

            // Abbruch des Hintergrund-Prozesses
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void InitialisiereButtonStarteBerechnung()
        {
            ButtonStarteBerechnung.IsEnabled = true;
            ButtonStarteBerechnung.Click += ButtonStarteBerechnung_Anklicken;
        }

        private void InitialisiereButtonBrecheBerechnungAb()
        {
            ButtonBrecheBerechnungAb.IsEnabled = false;
            ButtonBrecheBerechnungAb.Click += ButtonBrecheBerechnungAb_Anklicken;
        }

        private void InitialisiereButtonSpeichereErgebnisse()
        {
            ButtonSpeichereErgebnisse.IsEnabled = false;
            ButtonSpeichereErgebnisse.Click += ButtonSpeichereErgebnisse_Anklicken;
        }

        private void InitialisiereTextBoxStatus()
        {
            TextBoxStatus.Text = "Berechnung noch nicht gestartet";
        }

        // EVENT HANDLER DES HINTERGRUNDARBEITERS

        // Läuft auf dem Hintergrund-Thread
        private void FuehrHintergrundProzessAus(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            KennlinienmodellArgs args = (KennlinienmodellArgs)e.Argument;

            Modell modell = new Modell(args.Startgeschwindigkeit,
                                       args.Endgeschwindigkeit,
                                       args.AnzahlPunkte,
                                       args.Genauigkeit,
                                       args.AlleKraefte
                                       );

            modell.Initialisiere();

            for (int i = 1; i <= args.AnzahlPunkte; i++)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                try
                {
                    Thread.Sleep(1000);
                    // modell.VerarbeiteSchritt();
                }
                catch (NumericsFailedException)
                {
                    continue;
                }

                if (worker.WorkerReportsProgress)
                {
                    string aktuelleWerte = BerechneAktuelleWerte(i);
                    int fortschritt = BerechneFortschritt(i, args.AnzahlPunkte);
                    worker.ReportProgress(fortschritt, aktuelleWerte);
                }
            }
        }

        private string BerechneAktuelleWerte(int i)
        {
            return "Aktueller Schritt:" + i;
        }

        private int BerechneFortschritt(int aktuellerPunkte, int anzahlPunkte)
        {
            return (int)((float)aktuellerPunkte / (float)anzahlPunkte * 100);
        }

        // Läuft auf dem UI-Thread
        private void HintergrundProzessFortschrittGeaendert(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarForschrittanzeige.Value = e.ProgressPercentage;
            ListBoxErgebnisse.Items.Add(e.UserState.ToString());
        }

        // Läuft auf dem UI-Thread
        private void HintergrundProzessAbgeschlossen(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
            }
            else if (e.Cancelled)
            {
                BrecheBerechnungAb();
            }
            else
            {
                SchliesseBerechnungAb(e);
            }
            SetzeButtonsRelativeZuBackgroundWorker();
        }

        private void BrecheBerechnungAb()
        {
            ProgressBarForschrittanzeige.Value = 0.0;
            TextBoxStatus.Text = "Berechnung abgebrochen";
        }

        private void SchliesseBerechnungAb(RunWorkerCompletedEventArgs e)
        {
            //TODO: genaue Verarbeitung der Ausgabe mit e.Result
            ProgressBarForschrittanzeige.Value = 0.0;
            TextBoxStatus.Text = "Berechnung abgeschlossen";
            ButtonSpeichereErgebnisse.IsEnabled = true;
        }

        private void SetzeButtonsRelativeZuBackgroundWorker()
        {
            ButtonStarteBerechnung.IsEnabled = !backgroundWorker.IsBusy;
            ButtonBrecheBerechnungAb.IsEnabled = backgroundWorker.IsBusy;
        }

        // EVENT HANDLER DER BUTTONS

        // Läuft auf dem UI-Thread
        private void ButtonStarteBerechnung_Anklicken(object sender, RoutedEventArgs e)
        {
            var eventHandler = KennlinienberechnungAngefordert;

            if (eventHandler != null)
            {
                try
                {
                    KennlinienmodellArgs args = BelegeKennlinienberechnungArgumente();
                    eventHandler(this, args);
                }
                catch (CharacteristicCurveInputException)
                {
                }
            }
        }

        private KennlinienmodellArgs BelegeKennlinienberechnungArgumente()
        {
            KennlinienmodellArgs args = new KennlinienmodellArgs();

            try
            {
                args.BelegeArgumente(Genauigkeit.Value,
                                     Startgeschwindigkeit.Text,
                                     Endgeschwindigkeit.Text,
                                     AnzahlPunkte.Text,
                                     AlleKraefte.IsChecked.Value
                                     );
            }
            catch (CharacteristicCurveInputException)
            {
                throw;
            }

            return args;
        }

        private void StarteBerechnung(object sender, KennlinienmodellArgs args)
        {
            IntialisiereBerechnung();
            backgroundWorker.RunWorkerAsync(args);
            SetzeButtonsRelativeZuBackgroundWorker();
        }

        private void IntialisiereBerechnung()
        {
            ListBoxErgebnisse.Items.Clear();
            ButtonSpeichereErgebnisse.IsEnabled = false;
            TextBoxStatus.Text = "Berechnung gestartet";
        }

        private void ButtonBrecheBerechnungAb_Anklicken(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        private void ButtonSpeichereErgebnisse_Anklicken(object sender, RoutedEventArgs e)
        {
            SpeichereErgebnisse();
        }

        private void SpeichereErgebnisse()
        {
            //TODO
            MessageBox.Show("Speichervorgang ist noch nicht implementiert");
        }

        private void KennlinienmodellWindow_Schliessen(object sender, CancelEventArgs e)
        {
            if (ButtonSpeichereErgebnisse.IsEnabled)
            {
                AbfrageErgebnisseSpeichern(ref e);
            }
            else
            {
                AbfrageFensterSchliessen(ref e);
            }
        }

        private void AbfrageErgebnisseSpeichern(ref CancelEventArgs e)
        {
            switch (MessageBox.Show("Möchten Sie die Daten vor dem Schließen speichern?", "Kennlinienmodell", MessageBoxButton.YesNoCancel))
            {
                case MessageBoxResult.Yes:
                    SpeichereErgebnisse();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }

        private void AbfrageFensterSchliessen(ref CancelEventArgs e)
        {
            switch (MessageBox.Show("Möchten Sie die Kennlinienberechnung wirklich beenden?", "Kennlinienmodell", MessageBoxButton.YesNo))
            {
                case MessageBoxResult.Yes:
                    break;
                case MessageBoxResult.No:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }
    }
}
