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
        RobotModel rm = new RobotModel(300, 300, 10);
        public MainWindow()
        {
            InitializeComponent();
            //DrawRobot(rm.x, rm.y, rm.GetThetaRad());
            txVl.Text = "Vl: " + rm.vl;
            txVr.Text = "Vr: " + rm.vr;
            txSzog.Text = "Theta: " + rm.GetThetaDeg();
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
            txR.Text = "r: " + rm.r.ToString();
            txO.Text = "o: " + rm.GetOmegDeg().ToString();
            txXI.Text = "x: " + rm.GetIccX().ToString() + " icc";
        }

        private void GoClick(object sender, RoutedEventArgs e)
        {
            for (int i=0; i < (int)sliderLoop.Value; i++)
            {
                rm.Update((int)sliderVLeft.Value, (int)sliderVRight.Value);
                txSzog.Text = "Theta: " + rm.GetThetaDeg();
                DrawRobot(rm.x, rm.y, rm.GetThetaRad());
            }
        }

        private void sliderVLeftChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txVl.Text = "Vl: " + (int)sliderVLeft.Value;
            rm.vl = (int)sliderVLeft.Value;
            //System.Diagnostics.Debug.Write("xx");
        }
        private void sliderVRightChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txVr.Text = "Vr: " + (int)sliderVRight.Value;
            rm.vr = (int)sliderVRight.Value;
        }

        private void sliderDeltaTChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txDeltaT.Text = String.Format("{0,12:C2}", sliderDeltaT.Value);
        }
        private void sliderLoopChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txLoop.Text = (int)sliderLoop.Value + "L";
        }

        private void StraightClick(object sender, RoutedEventArgs e)
        {
            sliderVLeft.Value = (int)sliderVLeft.Value;
            sliderVRight.Value = (int)sliderVLeft.Value;
            txVl.Text = "Vl: " + (int)sliderVLeft.Value;
            txVr.Text = "Vr: " + (int)sliderVRight.Value;
        }

        private void LeftClick(object sender, RoutedEventArgs e)
        {
            if (sliderVLeft.Value > 0)
                sliderVLeft.Value = -1 * (int)sliderVLeft.Value;
            sliderVRight.Value = -1 * (int)sliderVLeft.Value;
            txVl.Text = "Vl: " + (int)sliderVLeft.Value;
            txVr.Text = "Vr: " + (int)sliderVRight.Value;
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            if (sliderVLeft.Value < 0)
                sliderVLeft.Value = -1 * (int)sliderVLeft.Value;
            sliderVRight.Value = -1 * (int)sliderVLeft.Value;
            txVl.Text = "Vl: " + (int)sliderVLeft.Value;
            txVr.Text = "Vr: " + (int)sliderVRight.Value;
        }
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            rm.Reset();
            canvRobot.Children.Clear();
        }
    }
}
