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
            await mainPageViewModel.ConnectToRobot();
        }


        public  void Scan(object sender, RoutedEventArgs e)
        {
            
            mainPageViewModel.Scanning();
        }

        public void Left_Click(object sender, RoutedEventArgs e)
        {
            mainPageViewModel.LeftClick();
        }

        public void Right_Click(object sender, RoutedEventArgs e)
        {
            mainPageViewModel.RightClick();
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            mainPageViewModel.Close();
  
           
        }


    }
}
