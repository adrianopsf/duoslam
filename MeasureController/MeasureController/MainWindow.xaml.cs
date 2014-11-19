using MeasureController.Models;
using MeasureController.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

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