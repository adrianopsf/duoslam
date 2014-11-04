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

        private Brick MyBrick;
        public MainPageViewModel()
        {
            MeasureList = new List<Measure.Measure>();
        }
        public List<Measure.Measure> MeasureList { get; set; }
        
        public async void Scanning()
        {
            //Thread t = new Thread();
            for (int i = 0; i < 20; i++)
            {
                await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, 30, 2, true);
                MyBrick.BrickChanged += MyBrick_BrickChanged;
            }
           
            
        }

        private void MyBrick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            Measure.Measure measure = new Measure.Measure() {MotorDataSI=e.Ports[InputPort.Three].SIValue, SensorDataSI=e.Ports[InputPort.A].SIValue};
            Debug.WriteLine("Data: "+measure.MotorDataSI);
            
            MyBrick.BrickChanged -= MyBrick_BrickChanged;
        }



        public void Close()
        {
            MyBrick.DirectCommand.PlayToneAsync(7, 200, 300);
            MyBrick.Disconnect();
        }

        public async Task ConnectToRobot()
        {
            bool _connected = false;
            if (!_connected)
            {
                MyBrick = new Brick(new BluetoothCommunication("COM3"));

                try
                {
                    await MyBrick.ConnectAsync();
                    await MyBrick.DirectCommand.PlayToneAsync(5, 1000, 300);

                    _connected = true;

                }
                catch (Exception)
                {

                    MessageBox.Show("No connection :(");
                    _connected = false;
                }
            }
        }

        public void RightClick()
        {
            MyBrick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, 50, 2, true);            
        }

        public void LeftClick()
        {
            MyBrick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, -50, 2, true);
        }
    }
}
