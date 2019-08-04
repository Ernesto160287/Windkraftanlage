using System.ComponentModel;
using System.Windows;

namespace Anwendung
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ButtonKennlinienmodell.Click += ButtonKennlinienmodell_Anklicken;
            ButtonBahnkurve.Click += ButtonBahnkurve_Anklicken;

            Closing += MainWindow_Schliessen;
        }

        private void ButtonKennlinienmodell_Anklicken(object sender, RoutedEventArgs e)
        {
            KennlinienmodellWindow kennlinienmodellWindow = new KennlinienmodellWindow();
            kennlinienmodellWindow.Show();
        }

        private void ButtonBahnkurve_Anklicken(object sender, RoutedEventArgs e)
        {
            //TODO
            MessageBox.Show("Simulationen zur Bahnkurve sind noch nicht implementiert");
        }

        private void MainWindow_Schliessen(object sender, CancelEventArgs e)
        {
            AbfrageFensterSchliessen(ref e);

        }

        private void AbfrageFensterSchliessen(ref CancelEventArgs e)
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
