using Kennlinienmodell;
using Mathematik;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Anwendung.WindowKennlinie
{
    public class KennlinieViewModel : INotifyPropertyChanged
    {
        BackgroundWorker backgroundWorker;

        #region Felder
        double genauigkeit;
        double startgeschwindigkeit;
        double endgeschwindigkeit;
        int anzahlPunkte;
        bool alleKraefte;

        bool startenMoeglich = true;
        bool abbrechenMoeglich = false;
        bool speichernMoeglich = false;
        string status = "Berechnung noch nicht gestartet";
        double fortschritt = 0.0;
        ObservableCollection<String> ergebnisse = new ObservableCollection<string>();
        #endregion

        #region Eingebundene Eigenschaften
        public double Genauigkeit
        {
            get { return genauigkeit; }
            set
            {
                if (genauigkeit != value)
                {
                    genauigkeit = value;
                    LoeseGeaenderteEigenschaftAus("Genauigkeit");
                }
            }
        }

        public double Startgeschwindigkeit
        {
            get { return startgeschwindigkeit; }
            set
            {
                if (startgeschwindigkeit != value)
                {
                    startgeschwindigkeit = value;
                    LoeseGeaenderteEigenschaftAus("Startgeschwindigkeit");
                }
            }
        }

        public double Endgeschwindigkeit
        {
            get { return endgeschwindigkeit; }
            set
            {
                if (endgeschwindigkeit != value)
                {
                    endgeschwindigkeit = value;
                    LoeseGeaenderteEigenschaftAus("Endgeschwindigkeit");
                }
            }
        }

        public int AnzahlPunkte
        {
            get { return anzahlPunkte; }
            set
            {
                if (anzahlPunkte != value)
                {
                    anzahlPunkte = value;
                    LoeseGeaenderteEigenschaftAus("AnzahlPunkte");
                }
            }
        }

        public bool AlleKraefte
        {
            get { return alleKraefte; }
            set
            {
                if (alleKraefte != value)
                {
                    alleKraefte = value;
                    LoeseGeaenderteEigenschaftAus("AlleKraefte");
                }
            }
        }

        public bool StartenMoeglich
        {
            get { return startenMoeglich; }
            set
            {
                if (startenMoeglich != value)
                {
                    startenMoeglich = value;
                    LoeseGeaenderteEigenschaftAus("StartenMoeglich");
                }
            }
        }

        public bool AbbrechenMoeglich
        {
            get { return abbrechenMoeglich; }
            set
            {
                if (abbrechenMoeglich != value)
                {
                    abbrechenMoeglich = value;
                    LoeseGeaenderteEigenschaftAus("AbbrechenMoeglich");
                }
            }
        }

        public bool SpeichernMoeglich
        {
            get { return speichernMoeglich; }
            set
            {
                if (speichernMoeglich != value)
                {
                    speichernMoeglich = value;
                    LoeseGeaenderteEigenschaftAus("SpeichernMoeglich");
                }
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    LoeseGeaenderteEigenschaftAus("Status");
                }
            }
        }

        public double Fortschritt
        {
            get { return fortschritt; }
            set
            {
                if (fortschritt != value)
                {
                    fortschritt = value;
                    LoeseGeaenderteEigenschaftAus("Fortschritt");
                }
            }
        }

        public ObservableCollection<string> Ergebnisse
        {
            get { return ergebnisse; }
            set
            {
                if (ergebnisse != value)
                {
                    ergebnisse = value;
                    LoeseGeaenderteEigenschaftAus("Ergebnisse");
                }
            }
        }
        #endregion

        public KennlinieViewModel()
        {
            InitialisiereHintergrundarbeiter();
        }

        #region Hintergrundarbeiter
        void InitialisiereHintergrundarbeiter()
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

        void FuehrHintergrundProzessAus(object sender, DoWorkEventArgs e)
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
                    modell.VerarbeiteSchritt();
                }
                catch (NumericsFailedException)
                {
                    continue;
                }

                if (worker.WorkerReportsProgress)
                {
                    int fortschritt = BestimmeFortschritt(i, args);
                    string ergebnis = modell.GebeAktuelleModellwerteAus();
                    worker.ReportProgress(fortschritt, ergebnis);
                }
            }
        }

        // Läuft auf dem UI-Thread
        void HintergrundProzessFortschrittGeaendert(object sender, ProgressChangedEventArgs e)
        {
            Fortschritt = e.ProgressPercentage;
            Ergebnisse.Add(e.UserState.ToString());
        }

        // Läuft auf dem UI-Thread
        void HintergrundProzessAbgeschlossen(object sender, RunWorkerCompletedEventArgs e)
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

        int BestimmeFortschritt(int i, KennlinienmodellArgs args)
        {
            return (int)((float)i / (float)args.AnzahlPunkte * 100);
        }

        void BrecheBerechnungAb()
        {
            Fortschritt = 0.0;
            Status = "Berechnung abgebrochen";
        }

        void SchliesseBerechnungAb(RunWorkerCompletedEventArgs e)
        {
            //TODO: genaue Verarbeitung der Ausgabe mit e.Result
            // Fortschritt = 0.0;
            Status = "Berechnung abgeschlossen";
            SpeichernMoeglich = true;
        }
        #endregion

        internal void StarteBerechnung(object sender, RoutedEventArgs e)
        {
            try
            {
                KennlinienmodellArgs args = BelegeKennlinienberechnungArgumente();

                IntialisiereBerechnung();

                if (!backgroundWorker.IsBusy)
                    backgroundWorker.RunWorkerAsync(args);

                SetzeButtonsRelativeZuBackgroundWorker();
            }
            catch (CharacteristicCurveInputException)
            {
            }
        }

        KennlinienmodellArgs BelegeKennlinienberechnungArgumente()
        {
            KennlinienmodellArgs args = new KennlinienmodellArgs();

            args.BelegeArgumente(Genauigkeit, Startgeschwindigkeit, Endgeschwindigkeit, AnzahlPunkte, AlleKraefte);
            args.PruefeKonsistenz();

            return args;
        }

        void IntialisiereBerechnung()
        {
            Ergebnisse.Clear();
            Fortschritt = 0.0;
            SpeichernMoeglich = false;
            Status = "Berechnung gestartet";
        }

        void SetzeButtonsRelativeZuBackgroundWorker()
        {
            StartenMoeglich = !backgroundWorker.IsBusy;
            AbbrechenMoeglich = backgroundWorker.IsBusy;
        }

        internal void BrecheBerechnungAb(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
        }

        internal void SpeichereErgebnisse(object sender, RoutedEventArgs e)
        {
            //TODO
            MessageBox.Show("Speichervorgang ist noch nicht implementiert");
        }

        internal void SchliesseFenster(object sender, CancelEventArgs e)
        {
            if (SpeichernMoeglich)
            {
                AbfrageErgebnisseSpeichern(ref e);
            }
            else
            {
                AbfrageFensterSchliessen(ref e);
            }
        }

        void AbfrageErgebnisseSpeichern(ref CancelEventArgs e)
        {
            switch (MessageBox.Show("Möchten Sie die Daten vor dem Schließen speichern?", "Kennlinienmodell", MessageBoxButton.YesNoCancel))
            {
                case MessageBoxResult.Yes:
                    SpeichereErgebnisse(this, null);
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

        void AbfrageFensterSchliessen(ref CancelEventArgs e)
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

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void LoeseGeaenderteEigenschaftAus(string eigenschaftsname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(eigenschaftsname));
        }        
        #endregion
    }
}
