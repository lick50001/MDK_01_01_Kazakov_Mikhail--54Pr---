using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Perm_Dynamics.Classes;

namespace Perm_Dynamics.Pages
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : Page
    {
        public double actualHeightCanvas = 0;
        public double maxValue = 0;
        public double averageValue = 0;

        public MainWindow mainWindow;
        public DispatcherTimer dispatcherTimer;

        public Chart(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Tick += CreateNewValue;

            actualHeightCanvas = this.ActualHeight > 0 ? this.ActualHeight - 50 : 300;

            if (mainWindow.pointInfos == null)
                mainWindow.pointInfos = new List<PointInfo>();

            if (mainWindow.pointInfos.Count == 0)
            {
                double startValue = mainWindow.InitialStockValue > 0 ? mainWindow.InitialStockValue : 100;
                mainWindow.pointInfos.Add(new PointInfo(startValue));
            }

            dispatcherTimer.Start();

            ControlCreateChart();
        }

        public void CreateNewValue(object sender, EventArgs e)
        {
            Random random = new Random();

            double lastValue = mainWindow.pointInfos[mainWindow.pointInfos.Count - 1].value;

            double coefficient = 0.5 + random.NextDouble();
            double newValue = lastValue * coefficient;

            mainWindow.pointInfos.Add(new PointInfo(newValue));

            ControlCreateChart();
        }

        public void CreateChart()
        {
            _canvas.Children.Clear();

            maxValue = 0;
            foreach (var point in mainWindow.pointInfos)
            {
                if (point.value > maxValue)
                {
                    maxValue = point.value;
                }
            }

            if (mainWindow.pointInfos.Count > 0)
            {
                averageValue = mainWindow.pointInfos.Average(p => p.value);
            }

            for (int i = 0; i < mainWindow.pointInfos.Count; i++)
            {
                Line line = new Line();

                line.X1 = i * 20;
                line.X2 = (i + 1) * 20;

                if (i == 0)
                {
                    line.Y1 = actualHeightCanvas - ((mainWindow.pointInfos[i].value / maxValue) * actualHeightCanvas);
                    line.Y2 = line.Y1;
                }
                else
                {
                    line.Y1 = actualHeightCanvas - ((mainWindow.pointInfos[i - 1].value / maxValue) * actualHeightCanvas);
                    line.Y2 = actualHeightCanvas - ((mainWindow.pointInfos[i].value / maxValue) * actualHeightCanvas);
                }

                line.StrokeThickness = 2;

                mainWindow.pointInfos[i].line = line;

                _canvas.Children.Add(line);
            }
        }

        public void CreatePoint()
        {
            int count = mainWindow.pointInfos.Count;
            if (count < 2) return;

            Line line = new Line();

            int prevIndex = count - 2;
            int currIndex = count - 1;

            line.X1 = prevIndex * 20;
            line.X2 = currIndex * 20;

            line.Y1 = actualHeightCanvas - ((mainWindow.pointInfos[prevIndex].value / maxValue) * actualHeightCanvas);
            line.Y2 = actualHeightCanvas - ((mainWindow.pointInfos[currIndex].value / maxValue) * actualHeightCanvas);

            line.StrokeThickness = 2;

            mainWindow.pointInfos[currIndex].line = line;
            _canvas.Children.Add(line);
        }

        public void ControlCreateChart()
        {
            double currentValue = mainWindow.pointInfos[mainWindow.pointInfos.Count - 1].value;

            if (currentValue > maxValue)
            {
                CreateChart();
            }
            else
            {
                CreatePoint();
            }

            ColorChart();
        }

        public void ColorChart()
        {
            double currentValue = mainWindow.pointInfos[mainWindow.pointInfos.Count - 1].value;

            averageValue = mainWindow.pointInfos.Average(p => p.value);

            for (int i = 0; i < mainWindow.pointInfos.Count; i++)
            {
                var point = mainWindow.pointInfos[i];
                if (point.line != null)
                {
                    if (point.value < averageValue)
                    {
                        point.line.Stroke = Brushes.Red;
                    }
                    else
                    {
                        point.line.Stroke = Brushes.Green;
                    }
                }
            }
            _canvas.Width = mainWindow.pointInfos.Count * 20 + 50;

            _scroll.ScrollToHorizontalOffset(_canvas.Width);

            if (current_value != null)
                current_value.Content = "Тек. знач: " + Math.Round(currentValue, 2);

            if (average_value != null)
                average_value.Content = "Сред. знач: " + Math.Round(averageValue, 2);
        }

        public void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            actualHeightCanvas = this.ActualHeight - 50d;
            CreateChart();
            ColorChart();
        }
    }
}