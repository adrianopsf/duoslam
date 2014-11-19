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
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using MeasureController.ViewModels;
using MeasureController.Models;

namespace MeasureController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPageViewModel mainPageViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainPageViewModel = new MainPageViewModel();
            DataContext = mainPageViewModel;
            InitButton.Click += mainPageViewModel.StartScan;
            ConnectButton.Click += mainPageViewModel.ConnectToRobot;
            RobotRightButton.Click += mainPageViewModel.TurnRight;
            ScanButton.Click += mainPageViewModel.Scan2;
            ScanInitButton.Click += mainPageViewModel.MoveScannarToStartposition;
            RobotLeftButton.Click += mainPageViewModel.TurnLeft;
        }


        private void Left_Click(object sender, RoutedEventArgs e)
        {
            mainPageViewModel.ControllDirection(-50);
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            mainPageViewModel.ControllDirection(50);
        }

        private void DrawSavedData(object sender, MouseButtonEventArgs e)
        {
            string fileName = SavedMapData.SelectedItem.ToString();
            List<Measure> MapList = Helper.FileHandlerHelper.GetTXTFileToList(fileName);
            // TODO: DRAW!
        }
    }
}
