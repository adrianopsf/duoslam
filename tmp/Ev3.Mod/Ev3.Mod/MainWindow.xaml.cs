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

namespace Ev3.Mod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RobotModel rm = new RobotModel(300, 300, 0);
        double scaleRate = 1.1;
        double mouseVerticalPosition = 0;
        double mouseHorizontalPosition = 0;
        bool isMouseCaptured = false;
        public MainWindow()
        {
            InitializeComponent();
            rm.Reset();
            sliderLoop.Minimum = 1;
        }

        private void DrawRobot(double x, double y, double angleRadian)
        {
            canvRobot.Children.Add(new Line
            {
                Stroke = System.Windows.Media.Brushes.DarkBlue,
                StrokeThickness = 4,
                X1 = (int)x,
                Y1 = (int)y,
                X2 = (int)(x + (Math.Cos(angleRadian)) * 10),
                Y2 = (int)(y + (Math.Sin(angleRadian)) * 10)
            });
            canvRobot.Children.Add(new Line
            {
                Stroke = System.Windows.Media.Brushes.DarkGray,
                StrokeThickness = 4,
                X1 = (int)x,
                Y1 = (int)y,
                X2 = (int)(x - (Math.Cos(angleRadian)) * 10),
                Y2 = (int)(y - (Math.Sin(angleRadian)) * 10)
            });
            txX.Text = "x: " + rm.x.ToString();
            txY.Text = "y: " + rm.y.ToString();
            txT.Text = "θ: " + rm.GetThetaDeg();
            txO.Text = "o: " + rm.GetOmegDeg().ToString();
        }

        private void GoClick(object sender, RoutedEventArgs e)
        {
            for (int i=0; i < (int)sliderLoop.Value; i++)
            {
                rm.Update((int)sliderVLeft.Value, (int)sliderVRight.Value);
                DrawRobot(rm.x, rm.y, rm.GetThetaRad());
            }
        }

        private void sliderVLeftChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rm.vl = (int)sliderVLeft.Value;
        }
        private void sliderVRightChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //txVr.Text = "Vr: " + (int)sliderVRight.Value;
            rm.vr = (int)sliderVRight.Value;
        }
        private void StraightClick(object sender, RoutedEventArgs e)
        {
            sliderVLeft.Value = (int)sliderVLeft.Value;
            sliderVRight.Value = (int)sliderVLeft.Value;
        }

        private void LeftClick(object sender, RoutedEventArgs e)
        {
            if (sliderVLeft.Value > 0)
                sliderVLeft.Value = -1 * (int)sliderVLeft.Value;
            sliderVRight.Value = -1 * (int)sliderVLeft.Value;
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            if (sliderVLeft.Value < 0)
                sliderVLeft.Value = -1 * (int)sliderVLeft.Value;
            sliderVRight.Value = -1 * (int)sliderVLeft.Value;
        }
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            rm.Reset();
            canvRobot.Children.Clear();
            txX.Text = "x: " + rm.x.ToString();
            txY.Text = "y: " + rm.y.ToString();
            txT.Text = "θ: " + rm.GetThetaDeg();
            txO.Text = "o: " + rm.GetOmegDeg().ToString();
        }
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            st.ScaleX *= scaleRate;
            st.ScaleY *= scaleRate;
        }

        private void ZoomOUt_Click(object sender, RoutedEventArgs e)
        {
            st.ScaleX /= scaleRate;
            st.ScaleY /= scaleRate;
        }

        private void canvRobot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseCaptured = true;
            canvRobot.CaptureMouse();
            mouseVerticalPosition = e.GetPosition(null).Y;
            mouseHorizontalPosition = e.GetPosition(null).X;
        }
        private void canvRobot_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseCaptured = false;
            canvRobot.ReleaseMouseCapture();
            mouseVerticalPosition = -1;
            mouseHorizontalPosition = -1;
        }
        private void canvRobot_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseCaptured)
            {
                // Calculate the current position of the object. (null)  >> Parent ?
                double deltaV = e.GetPosition(null).X - mouseVerticalPosition;
                double deltaH = e.GetPosition(null).Y - mouseHorizontalPosition;
                double newTop = deltaV + (double)canvRobot.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)canvRobot.GetValue(Canvas.LeftProperty);
                // Set new position of object.
                canvRobot.Margin = new Thickness(deltaV, deltaH, 0, 0);
                System.Diagnostics.Debug.WriteLine(deltaV + " " + deltaH);
            }
        }

        private void sliderLoop_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

    }
}
