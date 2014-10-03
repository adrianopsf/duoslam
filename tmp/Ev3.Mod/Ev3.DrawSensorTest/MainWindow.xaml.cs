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

namespace Ev3.DrawSensorTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int density = 45;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void RandomClick(object sender, RoutedEventArgs e)
        {
            double x, y, r;
            canvSensor.Children.Clear();
            Random rnd = new Random();
            for (int i = 0; i < density; i++)
            {
                r = rnd.Next(1, 40);                
                x = Math.Cos((Math.PI/density*i) + 1.0 * Math.PI ) * (100 + r);
                y = Math.Sin((Math.PI/density*i) + 1.0 * Math.PI ) * (100 + r);
                DrawLine(x, y);
                await Task.Delay(0);
            }
        }
        private void DrawLine(double x, double y)
        {
            canvSensor.Children.Add(new Line
            {
                Stroke = System.Windows.Media.Brushes.DarkGray,
                StrokeThickness = 2,
                X1 = 150,
                Y1 = 150,
                X2 = (int)(150 + x),
                Y2 = (int)(150 + y)
            });
        }

        private void SlidDensChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                txDens.Text = "" + (int)slidDens.Value;
                density = (int)slidDens.Value;
            }
            catch (Exception) { }
        }
    }
}
