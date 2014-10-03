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
using System.Windows.Media; //mátrix
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ev3.MatrixTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        float[,] a = new float[3, 3];
        float[,] b = new float[3, 1];
        float[,] c = new float[3, 1];
        public MainWindow()
        {
            InitializeComponent();
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            a[0, 0] = rnd.Next(1, 10); 
            a[0, 1] = 3;
            a[0, 2] = 4;
            a[1, 0] = rnd.Next(1, 10); 
            a[2, 2] = (float)5.3;
            b[0, 0] = 10;
            b[1, 0] = 100;
            b[2, 0] = 1000;
            //b[1, 0] = 2;
            MultiplyMatrix();
            DisplayMatrix();

        }
        public void MultiplyMatrix()
        {
            if (a.GetLength(1) == b.GetLength(0))
            {
                c = new float[a.GetLength(0), b.GetLength(1)];
                for (int i = 0; i < c.GetLength(0); i++)
                {
                    for (int j = 0; j < c.GetLength(1); j++)
                    {
                        c[i, j] = 0;
                        for (int k = 0; k < a.GetLength(1); k++) // vagy k<b.GetLength(0)
                            c[i, j] = c[i, j] + a[i, k] * b[k, j];
                    }
                }
            }
            else
            {
                MessageBox.Show("A mátrix oszlopainak száma nem egyezik B mátrix sorainak számával");
            }
        }


        public void DisplayMatrix()
        {
            ma.Text = MatrixToString(a);
            mb.Text = MatrixToString(b);
            mc.Text = MatrixToString(c);
        }

        public string MatrixToString(float[,] m){
            string s = "";
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    s += ("  " + m[i, j]);
                }
                s += "\n";
            }
            return s;
        }


    }
}
