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

        private float startPositionOfSensorMotor;

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

  

        private void MyBrick_BrickChanged(object sender, BrickChangedEventArgs e)
        {

            Models.Measure measure = new Models.Measure() { SensorMotorPosition = e.Ports[InputPort.A].RawValue, SensorDistance = e.Ports[InputPort.One].SIValue };
            MeasureList.Add(measure);
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


        public async Task scanPart(int power)
        {

            await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, power, 60, true);
            await Task.Delay(1500);
        }

        //cél az hogy középen tőle egy bizonyos szögben jobbra és balra mérjen egyet és 
        // ebből tudjunk vmit kiolvasni
        public async void Scan2(object sender, RoutedEventArgs e)
        {
            startPositionOfSensorMotor = MyBrick.Ports[InputPort.A].RawValue;
            MyBrick.BrickChanged += MyBrick_BrickChanged;
            await Task.Delay(1500);
            await scanPart(50);
            await scanPart(-50);
            await scanPart(-50);
            await scanPart(50);
            FilterMeasureList();

            Debug.WriteLine("SEMMI");
          
        }

        public void FilterMeasureList()
        {
            Debug.WriteLine("Mert adatok:");
            foreach (var item in MeasureList)
            {
                Debug.WriteLine(item.SensorMotorPosition);
            }
            for (int i = 0; i < MeasureList.Count; i++)
            {
                for (int j = 0; j < MeasureList.Count; j++)
                {
                    if (i != j)
                    {
                        if (MeasureList[j].SensorMotorPosition >= MeasureList[i].SensorMotorPosition - 5 && MeasureList[j].SensorMotorPosition <= MeasureList[i].SensorMotorPosition + 5)
                        {
                            MeasureList.Remove(MeasureList[j]);
                        }
                    }
                }
            }
            Debug.WriteLine("Szurt adatok:");
            foreach (var item in MeasureList)
            {
                Debug.WriteLine(item.SensorMotorPosition);
            }
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