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

namespace Perm_Dynamics.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private MainWindow mainWindow;
        public Main()
        {
            InitializeComponent();
        }

        public void OpenPageChart(object sender, RoutedEventArgs e)
        {
            float value = Convert.ToInt32(tb_value.Text);
            mainWindow.pointsInfo.Add(new Classes.PointInfo(value));
            mainWindow.OpenPages(MainWindow.pages.chart);
        }
    }
}
