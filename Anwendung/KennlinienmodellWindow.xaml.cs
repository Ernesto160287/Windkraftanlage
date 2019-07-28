using System;
using System.ComponentModel;
using System.Windows;

namespace Anwendung
{
    /// <summary>
    /// Interaktionslogik für KennlinienmodellWindow.xaml
    /// </summary>
    ///  
    public partial class KennlinienmodellWindow : Window
    {
        internal event EventHandler<KennlinienberechnungArgs> KennlinienberechnungAngefordert;

        public KennlinienmodellWindow()
        {
            InitializeComponent();
            KennlinienberechnungAngefordert += StarteBerechnung;
        }

        private void StarteBerechnung(object sender, KennlinienberechnungArgs args)
        {
            Console.WriteLine("vmin = " + args.Startgeschwindigkeit);
            Console.WriteLine("vmax = " + args.Endgeschwindigkeit);
            Console.WriteLine("anzahlSchritte = " + args.AnzahlPunkte);
            Console.WriteLine("--> vSchritt = " + ((args.Endgeschwindigkeit - args.Startgeschwindigkeit) / args.AnzahlPunkte));
            Console.WriteLine("genauigkeit = " + args.Genauigkeit);
            Console.WriteLine("alleKraefte = " + args.AlleKraefte);
        }

        private void ButtonStarteBerechnung_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = KennlinienberechnungAngefordert;

            if (eventHandler != null)
            {
                try
                {
                    KennlinienberechnungArgs args = BelegeKennlinienberechnungArgumente();
                    args.PruefeKonsistenz();
                    eventHandler(this, args);
                }
                catch (FormatException)
                {
                }
                catch (InconsistentInputException)
                {
                }
            }
        }

        private KennlinienberechnungArgs BelegeKennlinienberechnungArgumente()
        {
            KennlinienberechnungArgs args = new KennlinienberechnungArgs();

            try
            {
                args.BelegeGenauigkeit(Genauigkeit.Value);
                args.BelegeStartgeschwindigkeit(Startgeschwindigkeit.Text);
                args.BelegeEndgeschwindigkeit(Endgeschwindigkeit.Text);
                args.BelegeAnzahlPunkte(AnzahlPunkte.Text);
                args.BelegeAlleKraefte(AlleKraefte.IsChecked.Value);
            }
            catch (FormatException)
            {
                throw;
            }

            return args;
        }

        private void KennlinienmodellWindow_Closing(object sender, CancelEventArgs e)
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
