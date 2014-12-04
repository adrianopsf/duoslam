using MeasureController.Helper;
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
           
            ConnectButton.Click += mainPageViewModel.ConnectToRobot;
            RobotRightButton.Click += mainPageViewModel.TurnRight;
            ScanButton.Click += mainPageViewModel.Scan2;
            StartButton.Click += mainPageViewModel.StartRobot;
            StopButton.Click += mainPageViewModel.StopRobot;
            RobotLeftButton.Click += mainPageViewModel.TurnLeft;
        }

        #region Click Events

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            mainPageViewModel.ControllDirection(-50);
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            mainPageViewModel.ControllDirection(50);
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathText.Text;
            if (filePath.Length >= 3)
            {
                mainPageViewModel.SavedFilesList = FileHandlerHelper.GetAllFileFromFolder(filePath);
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (isDataFileValid())
                Helper.FileHandlerHelper.SaveListToTXTFile(mainPageViewModel.MeasureList, SaveFileNameText.Text, SaveFilePathText.Text);
        }

        private void DrawSavedData(object sender, MouseButtonEventArgs e)
        {
            Models.FilesModel file = SavedMapData.SelectedItem as FilesModel;
            string fileName = SavedMapData.SelectedItem.ToString();
            List<Measure> mapList = Helper.FileHandlerHelper.GetTXTFileToList(file.FileName, file.FilePath);
            DrawMap(mapList);
        }

        #endregion Click Events

        #region Helper Methods

        private bool isDataFileValid()
        {
            if (mainPageViewModel.MeasureList == null)
                return false;
            if (mainPageViewModel.MeasureList.Count <= 0)
                return false;
            if (SaveFilePathText.Text.Length < 3)
                return false;
            if (SaveFileNameText.Text.Length <= 0)
                return false;
            return true;
        }

        private void DrawMap(List<Measure> dataList)
        {
            //TODO: draw the Map from dataList to the Canvas!!
        }

        #endregion Helper Methods

       
    }
}