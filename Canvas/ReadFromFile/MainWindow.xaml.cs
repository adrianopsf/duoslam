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

namespace ReadFromFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public void addPoints(int x1, int y1, int x2, int y2)
        {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Black;
            myLine.SnapsToDevicePixels = true;
            myLine.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            myLine.X1 = x1;
            myLine.X2 = y1;  
            myLine.Y1 = x2;
            myLine.Y2 = y2;
            myLine.StrokeThickness = 1;
            canvas.Children.Add(myLine);
        }

        public MainWindow()
        { 
            InitializeComponent();

            addPoints(100,140,200,200);
            addPoints(10, 180, 200, 10);
            addPoints(150, 10, 20, 40);
        }
    }
}
