namespace FingerMatchingApp
{
    public class Vector
    {
        public double x, y;

        #region Properties
        public double X
        {
            get
            {
                return x;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
        }

        #endregion

        #region Contructor
        public Vector()
        {

        }

        public Vector(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
        #endregion
    }
}
