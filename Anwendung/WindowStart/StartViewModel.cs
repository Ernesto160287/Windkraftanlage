using System.ComponentModel;
using System.Windows;
using Anwendung.WindowKennlinie;

namespace Anwendung.WindowStart
{
    class StartViewModel
    {
        internal void StarteKennlinienberechnung(object sender, RoutedEventArgs e)
        {
            KennlinieView kennlinieWindow = new KennlinieView();
            kennlinieWindow.Show();
        }

        internal void StarteBahnkurvenberechnung(object sender, RoutedEventArgs e)
        {
            //TODO
            MessageBox.Show("Simulationen zur Bahnkurve sind noch nicht implementiert");
        }

        internal void SchliesseFenster(object sender, CancelEventArgs e)
        {
            AbfrageFensterSchliessen(ref e);
        }

        void AbfrageFensterSchliessen(ref CancelEventArgs e)
        {
            switch (MessageBox.Show("Möchten Sie die Windkraftanlage-Simulationen wirklich beenden?", "Windkraftanlage", MessageBoxButton.YesNo))
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
