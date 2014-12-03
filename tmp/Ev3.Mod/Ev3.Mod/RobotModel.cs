using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Diagnostics;

namespace Ev3.Mod
{
    class RobotModel
    {
        private double _x;
        private double _y;
        private double _theta;
        /// <summary>
        /// The speed of the right wheel
        /// </summary>
        private double _vl;
        /// <summary>
        /// The speed of the right wheel
        /// </summary>
        private double _vr;
        /// <summary>
        /// The speed of the robot
        /// </summary>
        private double _v;
        /// <summary>
        /// The distance between the wheels [cm]
        /// </summary>
        private double _l;
        /// <summary>
        /// The wheel radius [cm] //not used
        /// </summary>
        private double _r;
        /// <summary>
        /// Angular velocity of the wheel
        /// </summary>
        private double _omega;
        /// <summary>
        /// ΔT (For the sake of simplicity ΔT = 1 sec )
        /// </summary>
        private double _deltaT;
        /// <summary>
        /// Instance constructor of the robot with 3 parameters
        /// </summary>
        /// <param name="x">X poz</param>
        /// <param name="y">Y poz</param>
        /// <param name="theta">Orientation - Theta</param>
        public RobotModel(double x, double y, double theta)
        {
            _x = x;
            _y = y;
            _theta = theta;
            _vl = 10;
            _vr = 2;
            _l = 80; //distance [cm]
            _deltaT = 1; //[sec]

        }
        public void Reset()
        {
            _x = 300;
            _y = 300;
            _theta = 0;
            _vl = 10;
            _vr = 2;
            _l = 80; //distance [cm]
            _deltaT = 1; //[sec]

        }
        #region Get and Set - Simple
        public double x
        {
            get { return _x; }
            set { _x = value; }
        }
        public double y
        {
            get { return _y; }
            set { _y = value; }
        }
        public double vl
        {
            get { return _vl; }
            set { _vl = value; }
        }
        public double vr
        {
            get { return _vr; }
            set { _vr = value; }
        }
        public double omega
        {
            get { return _omega; }
            set { _omega = value; }
        }
        public double r
        {
            get { return _r; }
            set { _r = value; }
        }
        public double deltaT
        {
            get { return _deltaT; }
            set { _deltaT = value; }
        }
        #endregion

        #region Get and Set - Calculated
        public double GetThetaDeg()
        {
            return _theta % (2 * Math.PI);
        }
        public double GetThetaRad()
        {
            return (Math.PI / 180.0 * _theta);
        }
        public void SetThetaDeg(double theta)
        {
            _theta = theta;
            if (_theta < 0) _theta += 360;
            _theta = _theta % 360;
        }

        public void AddThetaDeg(double theta)
        {
            _theta += theta;
            if (_theta < 0) _theta += 360;
            _theta = _theta % 360;
        }
        public double GetOmegaRad()
        {
            _omega = (_vr - _vl) / _l;
            return (Math.PI / 180.0 * _omega);
        }
        public double GetOmegDeg()
        {
            _omega = (_vr - _vl) / _l;
            return _omega;
        }
        #endregion
        public void Update(double vl, double vr)
        {
            //the linear and the angular speed of the robot
            _v = 0.5 * (_vr + _vl);
            _omega = (_vr - _vl) / _l;
            // the orientation and position of the robot
            AddThetaDeg(_omega * _deltaT);
            _x = _x + _v * Math.Cos(GetThetaRad()) * _deltaT;
            _y = _y + _v * Math.Sin(GetThetaRad()) * _deltaT;
            Debug.WriteLine("v:["+ _vl + "," + _vr + "]" +" x: " + _x + " y: " + _y + " theta: " + _theta + " omega: " + _omega);
        }

        #region MatrixOperations
        public void MatrixDisplay(double[,] m)
        {
            string s = "";
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    s += ("  " + m[i, j]);
                }
                s += "\n";
            }
            System.Diagnostics.Debug.Write(s);
            System.Diagnostics.Debug.Write("\n-------\n\n");
        }

        public double[,] MultiplyMatrix(double[,] a, double[,] b)
        {
            double[,] c = new double[a.GetLength(0), b.GetLength(1)];
            if (a.GetLength(1) == b.GetLength(0))
            {
                for (int i = 0; i < c.GetLength(0); i++)
                {
                    for (int j = 0; j < c.GetLength(1); j++)
                    {
                        c[i, j] = 0;
                        for (int k = 0; k < a.GetLength(1); k++)
                            c[i, j] = c[i, j] + a[i, k] * b[k, j];
                    }
                }
            }
            else
            {
                //todo
                //MessageBox.Show("A mátrix oszlopainak száma nem egyezik B mátrix sorainak számával");
            }
            return c;
        }
        public double[,] AddMatrix(double[,] a, double[,] b)
        {
            double[,] c = new double[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    c[i, j] = a[i, j] + b[i, j];
                }
            }
            return c;
        }
        #endregion
    }
}
