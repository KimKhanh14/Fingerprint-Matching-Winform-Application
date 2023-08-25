using System;

namespace FingerMatchingApp
{
    public class Minutiae
    {
        private int x, y, type;
        private double direct;

        #region Properties
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public double Direct
        {
            get
            {
                return direct;
            }
            set
            {
                direct = value;
            }
        }

        public int Type
        {
            get
            {
                return type;
            }
        }
        #endregion

        #region Contructor
        public Minutiae()
        {

        }

        public Minutiae(int _type, int _y, int _x, double _direct)
        {
            type = _type;
            x = _x;
            y = _y;
            direct = _direct;
        }
        #endregion

        public void SetMinutiae(int _type, int _y, int _x, double _direct)
        {
            type = _type;
            x = _x;
            y = _y;
            direct = _direct;
        }

        public Minutiae GetMinutiaeAfterChange(int deltaX, int deltaY, double angleRotation, int xRoot, int yRoot)
        {
            double _direct = direct + angleRotation;
            if (_direct > Math.PI)
                _direct = _direct - Math.PI;
            return new Minutiae(type, y + deltaY, x + deltaX, _direct);
        }
    }
}
