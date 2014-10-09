using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ev3.DuoSlam
{
    public class SensorMeasuremntDisplay
    {
        int _x;
        int _y;
        float _sensorMotorPos;
        float _sensorValue;
        float _density;

        public SensorMeasuremntDisplay(float sensorMotorPos, float sensorValue, float density)
        {
            _sensorMotorPos = sensorMotorPos;
            _sensorValue = sensorValue;
            _density = density;
        }


        public int x
        {
            get
            {
                _x = (int)(Math.Cos((Math.PI / _density * (_sensorMotorPos + 180)) + 1.0 * Math.PI) * _sensorValue * 2);
                return _x;
            }
        }

        public int y
        {
            get
            {
                _y = (int)(Math.Sin((Math.PI / _density * (_sensorMotorPos + 180)) + 1.0 * Math.PI) * _sensorValue * 2);
                return _y;
            }
        }
    }
}
