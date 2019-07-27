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
        }

        private void ButtonKennlinienmodell_Click(object sender, RoutedEventArgs e)
        {
            KennlinienmodellWindow kennlinienmodellWindow = new KennlinienmodellWindow();
            kennlinienmodellWindow.Show();
        }

        private void ButtonBahnkurve_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            MessageBox.Show("Simulationen zur Bahnkurve sind noch nicht implementiert");
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
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
