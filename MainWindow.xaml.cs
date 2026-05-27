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

namespace Perm_Dynamics
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Classes.PointInfo> pointInfo = new List<Classes.PointInfo>();

        public MainWindow()
        {
            InitializeComponent();
            OpenPages(pages.main);
        }

        public enum pages
        {
            main,
            chart
        }

        public void OpenPages(pages _pages)
        {
            if (_pages == pages.main)
            {
                frame.Navigate(new Pages.Main(this));
            }
            else if (_pages == pages.chart)
            {
                frame.Navigate(new Pages.Chart(this));
            }
        }
    }
}