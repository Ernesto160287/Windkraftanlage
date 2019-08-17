using System.Windows;

namespace Anwendung.WindowKennlinie
{
    /// <summary>
    /// Interaktionslogik für KennlinieView.xaml
    /// </summary>
    public partial class KennlinieView : Window
    {
        internal KennlinieViewModel viewModel;

        internal KennlinieView()
        {
            InitializeComponent();
            viewModel = new KennlinieViewModel();
            DataContext = viewModel;

            ButtonStarteBerechnung.Click += viewModel.StarteBerechnung;
            ButtonBrecheBerechnungAb.Click += viewModel.BrecheBerechnungAb;
            ButtonSpeichereErgebnisse.Click += viewModel.SpeichereErgebnisse;

            Closing += viewModel.SchliesseFenster;
        }
    }
}
