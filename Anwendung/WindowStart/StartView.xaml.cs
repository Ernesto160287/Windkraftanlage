using System.ComponentModel;
using System.Windows;

namespace Anwendung.WindowStart
{
    /// <summary>
    /// Interaktionslogik für StartView.xaml
    /// </summary>
    public partial class StartView : Window
    {
        private StartViewModel viewModel;
        public StartView()
        {
            InitializeComponent();
            viewModel = new StartViewModel();

            ButtonKennlinie.Click += viewModel.StarteKennlinienberechnung;
            ButtonBahnkurve.Click += viewModel.StarteBahnkurvenberechnung;

            Closing += viewModel.SchliesseFenster;
        }
    }
}


