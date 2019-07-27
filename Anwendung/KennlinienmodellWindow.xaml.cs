using System;
using System.ComponentModel;
using System.Windows;

namespace Anwendung
{
    /// <summary>
    /// Interaktionslogik für KennlinienmodellWindow.xaml
    /// </summary>
    /// 

    public class KennlinienberechnungArgs
    {
        internal double Genauigkeit { get; set; }
    }

  
    public partial class KennlinienmodellWindow : Window
    {
        internal event EventHandler<KennlinienberechnungArgs> KennlinienberechnungAngefordert;

        public KennlinienmodellWindow()
        {
            InitializeComponent();
            KennlinienberechnungAngefordert += StarteBerechnung;
        }

        private void ButtonStarteBerechnung_Click(object sender, RoutedEventArgs e)
        {
            var eventHandler = KennlinienberechnungAngefordert;

            if (eventHandler != null)
            {
                KennlinienberechnungArgs args = SammleKennlinienberechnungArgumente();
                eventHandler(this, args);
            }
        }

        private KennlinienberechnungArgs SammleKennlinienberechnungArgumente()
        {
            KennlinienberechnungArgs args = new KennlinienberechnungArgs();

            args.Genauigkeit = Math.Round(NumerischeGenauigkeit.Value, 2);

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

        private void StarteBerechnung(object sender, KennlinienberechnungArgs args)
        {
            Console.WriteLine(args.Genauigkeit);
        }
    }
}
