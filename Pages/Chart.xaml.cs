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
using System.Windows.Threading;

namespace Perm_Dynamics.Pages
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : Page
    {
        public double actualHeightCanvas = 0;
        public double maxValue = 0;
        double averageValue = 0;

        public MainWindow mainWindow;

        public DispatcherTimer dispatcherTimer;
        public Chart(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            actualHeightCanvas = mainWindow.Height - 50d;

            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Tick += CreateNewValue;
            dispatcherTimer.Start();
        }

        public void CreateNewValue(object sender, EventArgs e)
        {
            Random random = new Random();
            double value = mainWindow.pointInfos[mainWindow.pointInfos.Count - 1].value;
            double newValue = value * (random.NextDouble() + 0.5d);
            mainWindow.pointInfos.Add(new Classes.PointInfo(newValue));
            ControlCreateChart();
        }

        public void CreateChart()
        {
            _canvas.Children.Clear();
            for (int i = 0; i < mainWindow.pointInfos.Count; i++)
            {
                if (mainWindow.pointInfos[i].value > maxValue)
                {
                    maxValue = mainWindow.pointInfos[i].value;
                }
            }
            for (int i = 0;i < mainWindow.pointInfos.Count;i++)
            {
                Line line = new Line();
                line.X1 = i * 20;
                line.X2 = (i +1) * 20;
                if (i == 0)
                {
                    line.Y1 = actualHeightCanvas;
                }else
                    line.Y1 = actualHeightCanvas - ((mainWindow.pointInfos[(i - 1)].value / double.MaxValue) * actualHeightCanvas);
                line.Y2 = actualHeightCanvas - ((mainWindow.pointInfos[i].value / maxValue) * actualHeightCanvas);
                line.StrokeThickness = 2;
                mainWindow.pointInfos[i].line = line;
                _canvas.Children.Add(line);
            }
        }

        public void CreatePoint()
        {
            Line line = new Line();
            line.X1 = (mainWindow.pointInfos.Count - 1) * 20;
            line.X2 = mainWindow.pointInfos.Count * 20;
            line.Y1 = actualHeightCanvas - ((mainWindow.pointInfos[(mainWindow.pointInfos.Count - 2)].value / maxValue) * actualHeightCanvas);
            line.Y2 = actualHeightCanvas - ((mainWindow.pointInfos[(mainWindow.pointInfos.Count - 1)].value / maxValue) * actualHeightCanvas);
            line.StrokeThickness = 2;
            mainWindow.pointInfos[mainWindow.pointInfos.Count - 1].line = line;
            _canvas.Children.Add(line);
        }

        public void ControlCreateChart()
        {
            double value = mainWindow.pointInfos[mainWindow.pointInfos.Count - 1].value;
            if (value < maxValue)
                CreatePoint();
            else
                CreateChart();
            ColorChart();
        }

        public void ColorChart()
        {
            double value = mainWindow.pointInfos[mainWindow.pointInfos.Count-1].value;
            for (int i = 0; i < mainWindow.pointInfos.Count; i++)
            {
                if (value < averageValue)
                {
                    mainWindow.pointInfos[i].line.Stroke = Brushes.Red;
                }
                else
                    mainWindow.pointInfos[i].line.Stroke = Brushes.Green;
            }

            _canvas.Width = mainWindow.pointInfos.Count * 20 + 300;
            _scroll.ScrollToHorizontalOffset(_canvas.Width);
            current_value.Content = "Тек. знач: " + Math.Round(value, 2);
            average_value.Content = "Сред. знач: " + Math.Round(averageValue, 2);
        }

        public void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            actualHeightCanvas = mainWindow.Height - 50d;
            CreateChart();
            ColorChart();
        }
    }
}
