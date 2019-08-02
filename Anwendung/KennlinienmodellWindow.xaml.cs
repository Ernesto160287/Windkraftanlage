using Kennlinienmodell;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Anwendung
{
    /// <summary>
    /// Interaktionslogik für KennlinienmodellWindow.xaml
    /// </summary>
    ///  
    public partial class KennlinienmodellWindow : Window
    {
        private BackgroundWorker hintergrundArbeiter = new BackgroundWorker();
        private event EventHandler<KennlinienmodellArgs> KennlinienberechnungAngefordert;


        public KennlinienmodellWindow()
        {
            InitializeComponent();

            InitialisiereHintergrundarbeiter();

            KennlinienberechnungAngefordert += StarteBerechnung;

            InitialisiereButtonStarteBerechnung();
            InitialisiereButtonBrecheBerechnungAb();

            Closing += KennlinienmodellWindow_Schliessen;

        }
        private void InitialisiereHintergrundarbeiter()
        {
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

            Thread.Sleep(5000);

            Console.WriteLine("vmin = " + ((KennlinienmodellArgs)e.Argument).Startgeschwindigkeit);
            Console.WriteLine("vmax = " + ((KennlinienmodellArgs)e.Argument).Endgeschwindigkeit);
            Console.WriteLine("anzahlSchritte = " + ((KennlinienmodellArgs)e.Argument).AnzahlPunkte);
            Console.WriteLine("genauigkeit = " + ((KennlinienmodellArgs)e.Argument).Genauigkeit);
            Console.WriteLine("alleKraefte = " + ((KennlinienmodellArgs)e.Argument).AlleKraefte);
        }

        // Läuft auf dem UI-Thread
        private void ArbeitAbgeschlossen(object sender, RunWorkerCompletedEventArgs e)
        {
            //TODO: genaue Verarbeitung der Ausgabe mit e.Result
            ButtonStarteBerechnung.IsEnabled = !hintergrundArbeiter.IsBusy;
            ButtonBrecheBerechnungAb.IsEnabled = hintergrundArbeiter.IsBusy;
        }

        // Läuft auf dem UI-Thread
        private void FortschrittGeaendert(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        // Läuft auf dem UI-Thread
        private void StarteBerechnung(object sender, KennlinienmodellArgs args)
        {
            hintergrundArbeiter.RunWorkerAsync(args);
            ButtonStarteBerechnung.IsEnabled = !hintergrundArbeiter.IsBusy;
            ButtonBrecheBerechnungAb.IsEnabled = hintergrundArbeiter.IsBusy;
        }

        private void InitialisiereButtonStarteBerechnung()
        {
            ButtonStarteBerechnung.IsEnabled = true;
            ButtonStarteBerechnung.Click += ButtonStarteBerechnung_Anklicken;
        }

        // Läuft auf dem UI-Thread
        private void ButtonStarteBerechnung_Anklicken(object sender, RoutedEventArgs e)
        {
            var eventHandler = KennlinienberechnungAngefordert;

            if (eventHandler != null)
            {
                try
                {
                    KennlinienmodellArgs args = BelegeKennlinienberechnungArgumente();
                    args.PruefeKonsistenz();
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
                args.BelegeGenauigkeit(Genauigkeit.Value);
                args.BelegeStartgeschwindigkeit(Startgeschwindigkeit.Text);
                args.BelegeEndgeschwindigkeit(Endgeschwindigkeit.Text);
                args.BelegeAnzahlPunkte(AnzahlPunkte.Text);
                args.BelegeAlleKraefte(AlleKraefte.IsChecked.Value);
            }
            catch (CharacteristicCurveInputException)
            {
                throw;
            }

            return args;
        }

        private void InitialisiereButtonBrecheBerechnungAb()
        {
            ButtonBrecheBerechnungAb.IsEnabled = false;
            ButtonBrecheBerechnungAb.Click += ButtonBrecheBerechnungAb_Anklicken;
        }

        // Läuft auf dem UI-Thread
        private void ButtonBrecheBerechnungAb_Anklicken(object sender, RoutedEventArgs e)
        {
            hintergrundArbeiter.CancelAsync();
        }

        private void KennlinienmodellWindow_Schliessen(object sender, CancelEventArgs e)
        {
            switch (MessageBox.Show("Möchten Sie die Daten vor dem Schließen speichern?", "Kennlinienmodell", MessageBoxButton.YesNoCancel))
            {
                case MessageBoxResult.Yes:
                    //TODO
                    MessageBox.Show("Speichervorgang ist noch nicht implementiert");
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
    }
}