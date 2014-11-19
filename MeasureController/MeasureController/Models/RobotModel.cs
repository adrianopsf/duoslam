using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureController
{
    public class RobotModel
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }
        public double VLeft { get; set; }
        public double VRight { get; set; }
        public double IccX { get; set; }
        public double IccY { get; set; }
        public double WheelDistance { get; set; }
        public double Omega { get; set; }
        public double DistIccCenter { get; set; }
        public double[,] Matrix = new double[3, 1];
        public double DeltaT { get; set; }
    }
}
