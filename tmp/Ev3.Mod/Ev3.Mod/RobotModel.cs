using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ev3.Mod
{
    //todo refactor
    class RobotModel
    {
        private double _x;
        private double _y;
        private double _theta;
        /// <summary>
        /// A robot bal kerekének aktuális sebessége
        /// </summary>
        private double _vl;
        /// <summary>
        /// A robot jobb kerekének aktuális sebessége
        /// </summary>
        private double _vr;
        /// <summary>
        /// ICC - Instantaneous Center of Curvatur, X Pillanatnyi fordulási középpont
        /// </summary>
        private double _iccx;
        /// <summary>
        /// ICC - Instantaneous Center of Curvatur, Y Pillanatnyi fordulási középpont
        /// </summary>
        private double _iccy;
        private double _l;
        private double _r;
        private double _omega;
        public double[,] m = new double[3, 1];
        private double _deltaT;
        /// <summary>
        /// A robot modell példányosítása három paraméterrel
        /// </summary>
        /// <param name="x">X poz</param>
        /// <param name="y">Y poz</param>
        /// <param name="theta">Orientáció</param>
        public RobotModel(double x, double y, double theta)
        {
            _x = x;
            _y = y;
            _theta = theta;
            _vl = 10;
            _vr = 9;
            _r = 100;
            _iccx = _x;
            _iccy = _y;
            m[0, 0] = m[1, 0] = m[2, 0] = 0;
            _l = 1.2;
            _omega = 0.1;
            _deltaT = 0.1;

        }
        public void Reset()
        {
            _x = 300;
            _y = 300;
            _theta = 0;
            _vl = 10;
            _vr = 9;
            _r = 100;
            _iccx = _x;
            _iccy = _y;
            m[0, 0] = m[1, 0] = m[2, 0] = 0;
            _l = 1.2;
            _omega = 20;
            _deltaT = 100;

        }
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
        /// <summary>
        /// ICC - Instantaneous Center of Curvatur, X Pillanatnyi fordulási középpont
        /// </summary>
        /// <returns></returns>
        public double GetIccX()
        {
            _r = (_l / 2) * ((_vl + _vr) / (_vr - _vl));
            _iccx = _x - _r * Math.Sin(GetThetaRad());
            return _iccx;
        }
        /// <summary>
        /// ICC - Instantaneous Center of Curvatur, Y Pillanatnyi fordulási középpont
        /// </summary>
        /// <returns></returns>
        public double GetIccY()
        {
            _r = ((_l / 2) * ((_vl + _vr) / (_vr - _vl)))*1;//nagyon kicsi!!!
            _iccy = (_y + _r * Math.Cos(GetThetaRad()));
            return _iccy;
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
        public double[,] Update(double vl, double vr)
        {
            //todo works, but needs to be refactored
            _vr = Math.Round(vr);
            _vl = Math.Round(vl);
            if (_vr == _vl)
            {
                _x = _x + _vr * Math.Cos(GetThetaRad()) * _deltaT;
                _y = _y + _vr * Math.Sin(GetThetaRad()) * _deltaT;
            }
            else if (_vl == -_vr || _vr == -_vl)
            {
                AddThetaDeg(2 * _vr * _deltaT / _l);
            }
            else
            {
                double ix = GetIccX();
                double iy = GetIccY();
                double o = GetOmegaRad();
                double[,] a = new double[3, 3];
                double[,] b = new double[3, 1];
                double[,] c = new double[3, 1];
                a[0, 0] = Math.Cos(o*_deltaT);
                a[0, 1] = -1 * Math.Sin(o*_deltaT);
                a[1, 0] = Math.Sin(o*_deltaT);
                a[1, 1] = Math.Cos(o*_deltaT);
                a[2, 2] = 1;
                b[0, 0] = _x - ix;
                b[1, 0] = _y - iy;
                b[2, 0] = _theta; ///?? thetarad  vagy _theta % (2 * Math.PI)
                c[0, 0] = ix;
                c[1, 0] = iy;
                c[2, 0] = o*_deltaT;
                m = MultiplyMatrix(a, b);
                m = AddMatrix(c, m);
                System.Diagnostics.Debug.Write("A\n");
                MatrixDisplay(a);
                System.Diagnostics.Debug.Write("B\n");
                MatrixDisplay(b);
                System.Diagnostics.Debug.Write("C\n");
                MatrixDisplay(c);
                System.Diagnostics.Debug.Write("M\n");
                MatrixDisplay(m);
                _x = m[0, 0];
                _y = m[1, 0];
                _theta = m[2, 0];
            }


            return m;
        }

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
    }
}
