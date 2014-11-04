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
using System.Threading;
using System.ComponentModel;

namespace Threading
{
    public class NewThread : INotifyPropertyChanged
    {
        Thread _t;
        public int i { get; private set; }
        public NewThread()
        {
            _t = new Thread(new ThreadStart(doWork));
            _t.Start();
        }
        void doWork()
        {
            for (i = 1; i <= 1000; i++)
            {
                Thread.Sleep(1000);
                this.NotifyPropertyCanged("i");
                System.Diagnostics.Debug.WriteLine(i);
            }
        }
        public void stop()
        {
            _t.Abort();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyCanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NewThread s = new NewThread();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = s;
        }

 
        private void Window_Closed_1(object sender, EventArgs e)
        {
            s.stop();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("aa");
        }
    }
}
