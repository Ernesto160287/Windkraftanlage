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
            MessageBox.Show("Simulationen zur Bahnkurve sind noch nicht implementiert");
        }
    }
}
