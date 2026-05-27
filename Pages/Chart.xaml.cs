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
        public MainWindow mainWindow;

        public double actualHeightCanvas = 0;
        public double maxValue = 0;
        double averageValue = 0;

        private Line _averageLine;

        public DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public Chart(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            actualHeightCanvas = mainWindow.Height - 50d;

            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Tick += CreateNewValue;
            dispatcherTimer.Start();

            CreateChart();
            ColorChart();
        }

        private void CreateNewValue(object sender, EventArgs e)
        {
            Random random = new Random();

            double value = mainWindow.pointInfo[mainWindow.pointInfo.Count - 1].value;
            double newValue = value * (random.NextDouble() + 0.5d);
            mainWindow.pointInfo.Add(new Classes.PointInfo(newValue));

            ControlCreateChart();
        }

        public void CreateChart()
        {
            canvas.Children.Clear();

            for (int i = 0; i < mainWindow.pointInfo.Count; i++)
            {
                if (mainWindow.pointInfo[i].value > maxValue)
                    maxValue = mainWindow.pointInfo[i].value;
            }
            for (int i = 0; i < mainWindow.pointInfo.Count; i++)
            {
                Line line = new Line();
                line.X1 = i * 20;
                line.X2 = (i + 1) * 20;

                if (i == 0)
                {
                    line.Y1 = actualHeightCanvas;
                }
                else
                {
                    line.Y1 = actualHeightCanvas - ((mainWindow.pointInfo[(i - 1)].value / maxValue) * actualHeightCanvas);
                }
                line.Y2 = actualHeightCanvas - ((mainWindow.pointInfo[i].value / maxValue) * actualHeightCanvas);
                line.StrokeThickness = 2;
                mainWindow.pointInfo[i].line = line;

                canvas.Children.Add(line);
            }
        }

        public void CreatePoint()
        {
            Line line = new Line();
            line.X1 = (mainWindow.pointInfo.Count - 1) * 20;
            line.X2 = mainWindow.pointInfo.Count * 20;
            line.Y1 = actualHeightCanvas - ((mainWindow.pointInfo[(mainWindow.pointInfo.Count - 2)].value / maxValue) * actualHeightCanvas);
            line.Y2 = actualHeightCanvas - ((mainWindow.pointInfo[(mainWindow.pointInfo.Count - 1)].value / maxValue) * actualHeightCanvas);
            line.StrokeThickness = 2;
            mainWindow.pointInfo[(mainWindow.pointInfo.Count - 1)].line = line;
            canvas.Children.Add(line);
        }

        public void ColorChart()
        {
            double value = mainWindow.pointInfo[mainWindow.pointInfo.Count - 1].value;

            for (int i = 0; i < mainWindow.pointInfo.Count; i++)
            {
                averageValue += mainWindow.pointInfo[i].value;
            }
            averageValue = averageValue / mainWindow.pointInfo.Count;

            for (int i = 0; i < mainWindow.pointInfo.Count; i++)
            {
                if (value < averageValue)
                {
                    mainWindow.pointInfo[i].line.Stroke = Brushes.Red;
                }
                else
                {
                    mainWindow.pointInfo[i].line.Stroke = Brushes.Green;
                }
            }

            if (_averageLine != null)
                canvas.Children.Remove(_averageLine);

            double averageY = actualHeightCanvas - ((averageValue / maxValue) * actualHeightCanvas);
            _averageLine = new Line();
            _averageLine.X1 = 0;
            _averageLine.X2 = mainWindow.pointInfo.Count * 20;
            _averageLine.Y1 = averageY;
            _averageLine.Y2 = averageY;
            _averageLine.Stroke = Brushes.Blue;
            _averageLine.StrokeThickness = 2;
            canvas.Children.Add(_averageLine);

            canvas.Width = mainWindow.pointInfo.Count * 20 + 300;
            scroll.ScrollToHorizontalOffset(canvas.Width);

            current_value.Content = "Тек. знач: " + Math.Round(value, 2);
            average_value.Content = "Сред. знач: " + Math.Round(averageValue, 2);
        }

        public void ControlCreateChart()
        {
            double value = mainWindow.pointInfo[mainWindow.pointInfo.Count - 1].value;

            if (value < maxValue)
                CreatePoint();
            else
                CreateChart();

            ColorChart();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            actualHeightCanvas = mainWindow.Height - 50d;

            CreateChart();
            ColorChart();
        }
    }
}