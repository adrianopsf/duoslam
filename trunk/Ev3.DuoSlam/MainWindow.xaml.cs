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
using System.Collections;
using System.Threading;
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;

namespace Ev3.DuoSlam
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Brick _brick;
        bool _connected = false;
        int _fwd = 40;
        int _bck = -30;
        uint _time = 300;
        float _ultraSensor = 0;
        float _infraSensor = 0;
        /// <summary>
        /// Postition of the sensor-holding motor
        /// </summary>
        float _sensorMotorPos = 0;
        /// <summary>
        /// Defines if the scanniging is at the right or left end - it is needed for the left-rigth-left-right movement
        /// </summary>
        bool _scanEdge = true; 
        /// <summary>
        /// Defines if scaning is on
        /// </summary>
        bool _scan = false;
        /// <summary>
        /// The start position of the sensormotor
        /// </summary>
        float _sensorStartPos = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stackControls.Visibility = Visibility.Hidden;
            buttonScan.Visibility = Visibility.Hidden;
        }

        void _brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            double x, y;
            int density = 240;
            _sensorMotorPos = e.Ports[InputPort.A].SIValue;
            _infraSensor = e.Ports[InputPort.Three].SIValue;
            _ultraSensor = e.Ports[InputPort.Four].SIValue;
            e.Ports[InputPort.Four].SetMode(1);
            txtInfra.Text = "Infra: " + _infraSensor;
            txtUltra.Text = "Ultra: " + _ultraSensor;
            txtMotor.Text = "Motor: " + _sensorMotorPos;
            if (_scan)
            {
                if (_scanEdge)
                {
                    _brick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, 25, 10, true);
                    if (_sensorMotorPos >= _sensorStartPos + 80)
                        _scanEdge = false;
                }
                else
                {
                    _brick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, -30, 10, true);
                    if (_sensorMotorPos <= _sensorStartPos - 80)
                        _scanEdge = true;
                }
                
                DrawLine(
                    new SensorMeasuremntDisplay(_sensorMotorPos, _infraSensor, density).x, 
                    new SensorMeasuremntDisplay(_sensorMotorPos, _infraSensor, density).y, 
                    canvInfra);
                if (_ultraSensor != 255)
                {
                    DrawLine(
                        new SensorMeasuremntDisplay(_sensorMotorPos, _ultraSensor, density).x,
                        new SensorMeasuremntDisplay(_sensorMotorPos, _ultraSensor, density).y,
                        canvUltra);
                }
            }
        }


        private async void FwdClick(object sender, RoutedEventArgs e)
        {
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, _fwd, _time, false);
        }

        private async void BckwClick(object sender, RoutedEventArgs e)
        {
            await _brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B | OutputPort.C, _bck, _time, false);
        }

        private async void LeftClick(object sender, RoutedEventArgs e)
        {
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, _bck, _time, false);
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, _fwd, _time, false);
            await _brick.BatchCommand.SendCommandAsync();
        }
        private async void RightClick(object sender, RoutedEventArgs e)
        {
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, _bck, _time, false);
            _brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, _fwd, _time, false);
            await _brick.BatchCommand.SendCommandAsync();
        }
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            canvUltra.Children.Clear();
            canvInfra.Children.Clear();
        }
        private void ScanClick(object sender, RoutedEventArgs e)
        {
            _scan = !_scan;
            _brick.DirectCommand.SetLedPatternAsync(_scan ? LedPattern.OrangeFlash : LedPattern.Red);
            buttonScan.Background = _scan ? Brushes.DarkGray : Brushes.LightGray;

        }
        private async void ConnectClick(object sender, RoutedEventArgs e)
        {

            if (!_connected)
            {
                _brick = new Brick(new BluetoothCommunication("COM3"));
                _brick.BrickChanged += _brick_BrickChanged;
                try
                {
                    await _brick.ConnectAsync();
                    await _brick.DirectCommand.PlayToneAsync(5, 1000, 300);
                    buttonConnect.Background = Brushes.LightGreen;
                    _connected = true;
                    stackControls.Visibility = Visibility.Visible;
                    buttonConnect.Content = "Disconnect";
                    //setmode
                }
                catch (Exception)
                {
                    buttonConnect.Background = Brushes.OrangeRed;
                    MessageBox.Show("No connection :(");
                    _connected = false;
                    stackControls.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                _brick.DirectCommand.PlayToneAsync(7, 200, 300);
                _brick.Disconnect();
                buttonConnect.Background = Brushes.OrangeRed;
                buttonConnect.Content = "Reconnect";
                _connected = false;
                stackControls.Visibility = Visibility.Hidden;
            }


        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (_connected)
            {
                _brick.DirectCommand.PlayToneAsync(7, 200, 300);
                _brick.Disconnect();
            }
        }


        private void DrawLine(double x, double y, Canvas c)
        {
            c.Children.Add(new Line
            {
                Stroke = System.Windows.Media.Brushes.DarkGray,
                StrokeThickness = 2,
                X1 = 100,
                Y1 = 150,
                X2 = (int)(100 + x),
                Y2 = (int)(150 + y)
            });
        }

        private void SensorMotorLeftClick(object sender, RoutedEventArgs e)
        {
            _brick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, 50, 10, true);
        }

        private void SensorMotorRightClick(object sender, RoutedEventArgs e)
        {
            _brick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, -50, 10, true);
        }

        private void SetSensorMotorStartClick(object sender, RoutedEventArgs e)
        {
            _sensorStartPos = _sensorMotorPos;
            MessageBox.Show("Sensor motor start postion (middle of the scanning process) os now: " + _sensorStartPos);
            buttonScan.Visibility = Visibility.Visible;
        }

    }
}
