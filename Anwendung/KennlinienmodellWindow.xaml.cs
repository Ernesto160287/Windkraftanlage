using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Anwendung
{
    /// <summary>
    /// Interaktionslogik für KennlinienmodellWindow.xaml
    /// </summary>
    public partial class KennlinienmodellWindow : Window
    {
        public KennlinienmodellWindow()
        {
            InitializeComponent();
            Closed += Schliessen;
        }

        private void Schliessen(object sender, EventArgs e)
        {
            // TODO: MainWindow wieder in den Vordergrund
        }
    }
}
