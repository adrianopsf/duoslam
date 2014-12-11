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

        public bool IsConnect { get; set; }

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

        public Boolean stop;

        #endregion PublicMembers

        #region PrivateMembers

        private float changing = 0;

        private List<Models.Measure> CacheMeasureList;

        private List<FilesModel> savedFilesList;

        private float startPositionOfSensorMotor;

        private string connectionText;

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

        public async Task Scan()
        {
            //CacheMeasureList = new List<Measure>();
            MyBrick.BrickChanged += MyBrick_BrickChanged;
            await Task.Delay(1500);
            startPositionOfSensorMotor = MyBrick.Ports[InputPort.A].RawValue;
            await scanPart(50);
            await scanPart(-50);
            await scanPart(-50);
            await scanPart(50);            
        }

        private async Task<bool> IsTurnLeft()
        {
            float summLeft = 0;
            float summRight = 0;
            int countLeft = 0;
            int countRight = 0;
            float avgLeft = 0;
            float avgRight = 0;
            foreach (var item in CacheMeasureList)
            {
                if (item.SensorMotorPosition < startPositionOfSensorMotor - 10)
                {
                    summLeft = summLeft + item.SensorDistance;
                    countLeft++;
                }
                else if (item.SensorMotorPosition > startPositionOfSensorMotor + 10)
                {
                    summRight = summRight + item.SensorDistance;
                    countRight++;
                }
            }
            avgLeft = summLeft / countLeft;
            avgRight = summRight / countRight;
            if (countLeft == 0 || countRight == 0)
            {
                await Scan();
                return await IsTurnLeft();
            }

            if (avgLeft > avgRight)
            {
                return true;
            }
            return false;
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
            MyBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.B, 70, Helper.Config.MotorPowerTime, true);
            MyBrick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.C, 70, Helper.Config.MotorPowerTime, true);
            await MyBrick.BatchCommand.SendCommandAsync();
            MyBrick.BrickChanged += MyBrick_BrickChanged;
        }

        private async Task scanPart(int power)
        {
            await MyBrick.DirectCommand.StepMotorAtPowerAsync(OutputPort.A, power, 60, true);
            await Task.Delay(1500);
        }

        public void Stop()
        {
            MyBrick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.B, 0);
            MyBrick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.C, 0);
        }

        private void FilterMeasureList(List<Measure> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j)
                    {
                        if (list[j].SensorMotorPosition >= list[i].SensorMotorPosition - 5 && list[j].SensorMotorPosition <= list[i].SensorMotorPosition + 5)
                        {
                            list.Remove(list[j]);
                            j--;

                        }
                    }
                }
            }
        }

        #endregion HelperMethods

        #region ClickEvents

        private void MyBrick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            Models.Measure measure = new Models.Measure() { RobotChangeing = changing, SensorMotorPosition = e.Ports[InputPort.A].RawValue, SensorDistance = e.Ports[InputPort.One].SIValue };
            //MeasureList.Add(measure);
            CacheMeasureList.Add(measure);
        }

        public async void TurnRight(object sender, RoutedEventArgs e)
        {
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.B, 40, Helper.Config.MotorStep, true);
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.C, -40, Helper.Config.MotorStep, true);
            await MyBrick.BatchCommand.SendCommandAsync();
        }

        public async void TurnLeft(object sender, RoutedEventArgs e)
        {
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.B, -40, Helper.Config.MotorStep, true);
            MyBrick.BatchCommand.StepMotorAtPower(OutputPort.C, 40, Helper.Config.MotorStep, true);
            await MyBrick.BatchCommand.SendCommandAsync();
        }

        //cél az hogy középen tőle egy bizonyos szögben jobbra és balra mérjen egyet és 
        // ebből tudjunk vmit kiolvasni
        public async void Scan2(object sender, RoutedEventArgs e)
        {
            CacheMeasureList = new List<Measure>();
            MyBrick.BrickChanged += MyBrick_BrickChanged;
            await Task.Delay(1500);
            startPositionOfSensorMotor = MyBrick.Ports[InputPort.A].RawValue;
            await scanPart(50);
            await scanPart(-50);
            await scanPart(-50);
            await scanPart(50);
            //FilterMeasureList(CacheMeasureList);
        }

        //at this point, the robot is scanning and select the distance ahead, and aftar that write it to the output console
        // TODO: if this distance is under 40, the robot  turn
        // TODO: turn decision methot
        // TODO: rescan & decision again
        // TODO: data back to the UI
        public async void StartRobot(object sender, RoutedEventArgs e)
        {
            CacheMeasureList = new List<Measure>();
            stop = false;
            bool b = false;
            float midDistance = -1;
            while (!stop)
            {
                await Scan();
                Debug.WriteLine(startPositionOfSensorMotor);
                foreach (var measures in CacheMeasureList)
                {
                    if (measures.SensorMotorPosition >= startPositionOfSensorMotor - 5 && measures.SensorMotorPosition <= startPositionOfSensorMotor + 5)
                    {
                        midDistance = measures.SensorDistance;
                        Debug.WriteLine("Tavolsag: " + midDistance + "fok" + measures.SensorMotorPosition);

                        break;
                    }
                }
                if (midDistance > 35)
                {
                    GoForwardOneUnit();
                    FilterMeasureList(CacheMeasureList);
                    MeasureList.AddRange(CacheMeasureList);
                    CacheMeasureList.Clear();
                    changing = b ? 5 : -5;
                    b = !b;          


                }
                else
                {
                    if (await IsTurnLeft())
                    {
                        MyBrick.BatchCommand.StepMotorAtPower(OutputPort.B, -40, 239, true);
                        MyBrick.BatchCommand.StepMotorAtPower(OutputPort.C, 40, 239, true);
                        await MyBrick.BatchCommand.SendCommandAsync();
                        changing = b ? 1 : -1;
                        b = !b;
                        FilterMeasureList(CacheMeasureList);
                        MeasureList.AddRange(CacheMeasureList);
                        CacheMeasureList.Clear();

                    }
                    else
                    {
                        MyBrick.BatchCommand.StepMotorAtPower(OutputPort.B, 40, 239, true);
                        MyBrick.BatchCommand.StepMotorAtPower(OutputPort.C, -40, 239, true);
                        await MyBrick.BatchCommand.SendCommandAsync();
                        changing = b ? 2 : -2;
                        b = !b;
                        FilterMeasureList(CacheMeasureList);
                        MeasureList.AddRange(CacheMeasureList);
                        CacheMeasureList.Clear();

                    }
                }
            }
            //MeasureController.Helper.FileHandlerHelper.SaveListToTXTFile(MeasureList,"meres","C:\Users\Hallgato\Documents");
        }

        public void StopRobot(object sender, RoutedEventArgs e)
        {
            stop = true;

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