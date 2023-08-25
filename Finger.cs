using System.Collections.Generic;

namespace FingerMatchingApp
{
    public class Finger
    {
        private string name;
        private List<Minutiae> minutiaes = new List<Minutiae>();

        public string Name
        {
            get
            {
                return name;
            }
        }

        public List<Minutiae> Minutiaes
        {
            get
            {
                return minutiaes;
            }
        }

        public Finger()
        {

        }

        public Finger(string _name, List<Minutiae> _minutiaes)
        {
            name = _name;
            minutiaes = _minutiaes;
        }

        public override string ToString()
        {
            return name;
        }

    }
}
