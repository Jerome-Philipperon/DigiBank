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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AffichageInfoClient(object sender, RoutedEventArgs e)
        {
            Info_Client.Visibility = Visibility.Visible;
            Statistiques.Visibility = Visibility.Collapsed;
        }

        private void AffichageStatistiques(object sender, RoutedEventArgs e)
        {
            Statistiques.Visibility = Visibility.Visible;
            Info_Client.Visibility = Visibility.Collapsed;
        }
    }
}
