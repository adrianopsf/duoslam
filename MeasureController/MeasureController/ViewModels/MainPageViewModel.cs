using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
namespace MeasureController.ViewModels
{
    public class MainPageViewModel
    {
        public List<Measure.Measure> MeasureList { get; set; }
        private Brick MyBrick;
        public MainPageViewModel()
        {
            MeasureList = new List<Measure.Measure>();
        }
        public async void StartScan()
        {
            MyBrick.BrickChanged += MyBrick_BrickChanged;
            await Scan();
        }
        private async Task Scan()
        {
            int j = 0;
            for (int i = 0; i < 60; i++)
            {
                await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, 90, 4, true);
                await Task.Delay(200);
                MyBrick.BrickChanged += MyBrick_BrickChanged;
                j++;
                Debug.WriteLine(j + "");
            }
            j = 0;
            for (int i = 0; i < 60; i++)
            {
                await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, -90, 4, true);
                await Task.Delay(200);

                j++;
                Debug.WriteLine(j + "");
            }
        }
        private void MyBrick_BrickChanged(object sender, BrickChangedEventArgs e)
        {

            Measure.Measure measure = new Measure.Measure() { MotorDataSI = e.Ports[InputPort.A].SIValue, SensorDataSI = e.Ports[InputPort.Four].SIValue };
            MeasureList.Add(measure);
            MyBrick.BrickChanged -= MyBrick_BrickChanged;
        }
        public void ControllDirection(int power)
        {
            MyBrick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, power, 10, true);
        }
        #region ConnectDisconnect
        public void Close()
        {
            MyBrick.DirectCommand.PlayToneAsync(7, 200, 300);
            MyBrick.Disconnect();
        }

        public async Task ConnectToRobot()
        {
            MyBrick = new Brick(new BluetoothCommunication("COM3"));

            try
            {
                await MyBrick.ConnectAsync();
                await MyBrick.DirectCommand.PlayToneAsync(5, 1000, 300);
            }
            catch (Exception)
            {
                MessageBox.Show("No connection :(");
            }

        #endregion


        }
    }
}
