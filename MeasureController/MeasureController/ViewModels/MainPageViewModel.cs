using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using MeasureController.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace MeasureController.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region PublicMembers

        public List<Models.Measure> MeasureList { get; set; }

        public List<FilesModel> SavedFilesList
        {
            get
            {
                return savedFilesList;
            }
            set
            {
                if (savedFilesList != value)
                {
                    savedFilesList = value;
                    NotifyPropertyCanged("SavedFilesList");
                }
            }
        }

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

        #endregion PublicMembers

        #region PrivateMembers

        private List<FilesModel> savedFilesList;

        private string connectionText;

        public bool IsConnect { get; set; }

        private Brick MyBrick;

        #endregion PrivateMembers

        public MainPageViewModel()
        {
            MeasureList = new List<Models.Measure>();
            SavedFilesList = new List<Models.FilesModel>();
            IsConnect = false;
            ConnectionText = "Connect";
        }

        #region HelperMethods

        private async Task ScanFromRight(int power)
        {
            for (int i = 0; i < 30; i++)
            {
                await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, -power, 8, true);
                await Task.Delay(300);
                MyBrick.BrickChanged += MyBrick_BrickChanged;
                //Debug.WriteLine(i + "");
            }
        }

        private void MyBrick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            Models.Measure measure = new Models.Measure() { SensorTheta = e.Ports[InputPort.Four].SIValue };
            MeasureList.Add(measure);
            MyBrick.BrickChanged -= MyBrick_BrickChanged;
        }

        public void ControllDirection(int power)
        {
            MyBrick.DirectCommand.StepMotorAtSpeedAsync(OutputPort.A, power, 5, true);
        }

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

        public async Task MoveScannarToStartposition()
        {
            while (MyBrick.Ports[InputPort.Four].RawValue <= Helper.Config.RawValueLimit)
            {
                await MyBrick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, 30, 50, true);
                await Task.Delay(50);
            }
        }

        public void Stop()
        {
            MyBrick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, 0);
            MyBrick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, 0);
        }

        #endregion HelperMethods

        #region ClickEvents

        public async void TurnRight(object sender, RoutedEventArgs e)
        {
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.B, 40, 239, true);
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.C, -40, 239, true);
            await MyBrick.BatchCommand.SendCommandAsync();
        }

        public async void TurnLeft(object sender, RoutedEventArgs e)
        {
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.B, -40, 239, true);
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.C, 40, 239, true);
            await MyBrick.BatchCommand.SendCommandAsync();
        }

        public async void MoveScannarToStartposition(object sender, RoutedEventArgs e)
        {
            while (MyBrick.Ports[InputPort.Four].RawValue <= Helper.Config.RawValueLimit)
            {
                await MyBrick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, 30, 50, true);
                await Task.Delay(50);
            }

        }

        public async Task scanPart(int power)
        {
            await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, power, 60, true);
            Debug.WriteLine(power);
            await Task.Delay(1000);
           
           
        }
        
        //cél az hogy középen tőle egy bizonyos szögben jobbra és balra mérjen egyet és 
        // ebből tudjunk vmit kiolvasni
        public async void Scan2(object sender, RoutedEventArgs e)
        {
            await scanPart(50);
            await scanPart(-50);
            await scanPart(-50);
            await scanPart(50);
            await scanPart(50);
            await scanPart(-50);
            await scanPart(-50);
            await scanPart(50);
            await scanPart(50);
            await scanPart(-50);
            await scanPart(-50);
            await scanPart(50);
            await scanPart(50);
            await scanPart(-50);
            await scanPart(-50);
            await scanPart(50);
            await scanPart(50);
            await scanPart(-50);
            ////////////////////////////////////////////////////////////DateTime now = DateTime.Now;
            ////////////////////////////////////////////////////////////await scanPart(50);
            ////////////////////////////////////////////////////////////await Task.Delay(400);

            ////////////////////////////////////////////////////////////float dst = MyBrick.Ports[InputPort.One].SIValue;
            ////////////////////////////////////////////////////////////now = DateTime.Now;
            ////////////////////////////////////////////////////////////Debug.WriteLine("Distance 1 = " + dst +"\t---"+now.Second + " : " + now.Millisecond);
            ////////////////////////////////////////////////////////////await Task.Delay(1100);

            ////////////////////////////////////////////////////////////await scanPart(-50);
            ////////////////////////////////////////////////////////////await Task.Delay(400);

            ////////////////////////////////////////////////////////////dst = MyBrick.Ports[InputPort.One].SIValue;
            ////////////////////////////////////////////////////////////now = DateTime.Now;
            ////////////////////////////////////////////////////////////Debug.WriteLine("Distance 2 = " + dst +"\t---"+ now.Second + " : " + now.Millisecond);
            ////////////////////////////////////////////////////////////await Task.Delay(1100);

            ////////////////////////////////////////////////////////////await scanPart(-50);
            ////////////////////////////////////////////////////////////await Task.Delay(400);

            ////////////////////////////////////////////////////////////dst = MyBrick.Ports[InputPort.One].SIValue;
            ////////////////////////////////////////////////////////////now = DateTime.Now;
            ////////////////////////////////////////////////////////////Debug.WriteLine("Distance 3 = " + dst + "\t---" + now.Second + " : " + now.Millisecond);
            ////////////////////////////////////////////////////////////await Task.Delay(1100);

            ////////////////////////////////////////////////////////////await scanPart(50);
           

            //////////////////float dst = 0;
            ////////////////// Task.Delay(1400);
            ////////////////// MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, 50, 60, true);
            ////////////////// Task.Delay(1400);
            //////////////////MyBrick.BrickChanged += MyBrick_BrickChanged;
            //////////////////dst = MyBrick.Ports[InputPort.One].SIValue;
            //////////////////Debug.WriteLine("Distance 1 = "+ dst);
            ////////////////// Task.Delay(1400);

            ////////////////// MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, -50, 60, true);
            ////////////////// Task.Delay(1400);
            //////////////////MyBrick.BrickChanged += MyBrick_BrickChanged;
            //////////////////dst = MyBrick.Ports[InputPort.One].SIValue;
            //////////////////Debug.WriteLine("Distance 2 = " + dst);
            ////////////////// Task.Delay(1400);

            ////////////////// MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, -50, 60, true);
            ////////////////// Task.Delay(1400);
            //////////////////MyBrick.BrickChanged += MyBrick_BrickChanged;
            //////////////////dst = MyBrick.Ports[InputPort.One].SIValue; 
            //////////////////Debug.WriteLine("Distance 3 = " + dst);
            //////////////////Task.Delay(1400);

            //////////////////MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, 50, 60, true);
            //////////////////Task.Delay(1400);



            //  List<float> distance = new List<float>();
            //DateTime now = new DateTime();
            //await MoveScannarToStartposition();
            //////DateTime now = DateTime.Now;
            //////Debug.WriteLine(now.Second + " : " + now.Millisecond);
            //////MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, -10, 180, true);
            //////now = DateTime.Now;
            //////Debug.WriteLine(now.Second + " : " + now.Millisecond);
            //for (int i = 0; i < 37; i++)
            //{

            //    MyBrick.BrickChanged += MyBrick_BrickChanged;
            //    await Task.Delay(1000);

            //      float dst = MyBrick.Ports[InputPort.One].SIValue;
            //    float motorPos = MyBrick.Ports[InputPort.A].SIValue;
            //      now = DateTime.Now;
            //    Debug.WriteLine(dst + "  "+now.Second+" : "+ now.Millisecond+"---"+motorPos);
            //    distance.Add(dst);
            //}
            //await MoveScannarToStartposition();
            //    int a = 0;
            //    foreach (var item in distance)
            //    {
            //        a++;
            //        Debug.WriteLine(a+": "+item + ", ");
            //    }
        }

        public async void StartScan(object sender, RoutedEventArgs e)
        {
            MyBrick.BrickChanged += MyBrick_BrickChanged;
            await ScanFromRight(90);
            await ScanFromRight(-90);
        }

        #endregion ClickEvents

        #region ConnectDisconnect

        public async void ConnectToRobot(object sender, RoutedEventArgs e)
        {
            if (!IsConnect)
            {
                // MyBrick = new Brick(new BluetoothCommunication("COM3"));
                MyBrick = new Brick(new UsbCommunication());
                try
                {
                    await MyBrick.ConnectAsync();
                    await MyBrick.DirectCommand.PlayToneAsync(5, 1000, 300);
                    IsConnect = !IsConnect;
                    NotifyPropertyCanged("IsConnect");
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
                IsConnect = !IsConnect;
                NotifyPropertyCanged("IsConnect");
            }
        }

        #endregion ConnectDisconnect

        #region NotifyPropertyCangedEventHandler

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyCanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion NotifyPropertyCangedEventHandler
    }
}