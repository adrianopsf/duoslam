using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
namespace MeasureController.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public List<Models.Measure> MeasureList { get; set; }
        private string connectionText;
        public String ConnectionText
        {
            get
            {
                return connectionText;
            }
            set
            {
                if (value != connectionText)
                {
                    connectionText = value;
                    NotifyPropertyCanged("ConnectionText");
                }
            }
        }
        private bool isConnect = false;
        private Brick MyBrick;

        public MainPageViewModel()
        {
            MeasureList = new List<Models.Measure>();
            ConnectionText = "Connect";
        }

        public async void StartScan()
        {
            MyBrick.BrickChanged += MyBrick_BrickChanged;
            await ScanFromRight(90);
            await ScanFromRight(-90);
        }

        private async Task ScanFromRight(int power)
        {
            for (int i = 0; i < 30; i++)
            {
                await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, -power, 8, true);
                await Task.Delay(300);
                MyBrick.BrickChanged += MyBrick_BrickChanged;
                Debug.WriteLine(i + "");
            }
        }

        private void MyBrick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            Models.Measure measure = new Models.Measure() { MotorDataSI = e.Ports[InputPort.A].SIValue, SensorDataSI = e.Ports[InputPort.Four].SIValue };
            MeasureList.Add(measure);
           
            MyBrick.BrickChanged -= MyBrick_BrickChanged;
        }

        public void ControllDirection(int power)
        {
            MyBrick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, power, 5, true);
        }

        #region ConnectDisconnect
        public async Task ConnectToRobot()
        {

            if (!isConnect)
            {
                // MyBrick = new Brick(new BluetoothCommunication("COM3"));
                MyBrick = new Brick(new UsbCommunication());
                try
                {
                    await MyBrick.ConnectAsync();
                    await MyBrick.DirectCommand.PlayToneAsync(5, 1000, 300);
                    isConnect = !isConnect;
                    ConnectionText = "Disconnect";
                }
                catch (Exception)
                {
                    MessageBox.Show("No connection :(");
                }
            }
            else
            {
                await MyBrick.DirectCommand.PlayToneAsync(7, 200, 300);
                MyBrick.Disconnect();
                ConnectionText = "Connect";
                isConnect = !isConnect;
            }

        }
        #endregion


        /// <summary>
        /// Unit = 15 MotorPower=70 AND Millisecundum=950
        /// </summary>
        public async void GoForwardOneUnit()
        {
            MyBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, 70, 950, true);
            MyBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, 70, 950, true);
            await MyBrick.BatchCommand.SendCommandAsync();
            MyBrick.BrickChanged += MyBrick_BrickChanged;

        }

        public async void TurnRight()
        {
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.B, 40, 239, true);
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.C, -40, 239, true);
            await MyBrick.BatchCommand.SendCommandAsync();
        }


        public async void TurnLeft()
        {
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.B, -40, 239, true);
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.C, 40, 239, true);
            await MyBrick.BatchCommand.SendCommandAsync();
        }
        public async Task MoveScannarToStartposition()
        {
           
                while (MyBrick.Ports[InputPort.Four].RawValue <= 1000)
                {
                    await MyBrick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, 30, 50, true); 
                    await Task.Delay(50);
                }
            
        }

        public async Task Scan2()
        {
          //  List<float> distance = new List<float>();
                await MoveScannarToStartposition();  
           
            for (int i = 0; i < 37; i++)
            {
                await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, -35, 8, true);
                await Task.Delay(300);
                MyBrick.BrickChanged += MyBrick_BrickChanged;
              //  float dst = MyBrick.Ports[InputPort.One].SIValue;
                //Debug.WriteLine(dst + "");
            //    distance.Add(dst);
                
            }
            await MoveScannarToStartposition();
        //    int a = 0;
        //    foreach (var item in distance)
        //    {
        //        a++;
        //        Debug.WriteLine(a+": "+item + ", ");
        //    }
        }

        public void Stop()
        {
            MyBrick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, 0);
            MyBrick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, 0);
        }
        #region NotifyPropertyCangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyCanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion


       
    }


}
