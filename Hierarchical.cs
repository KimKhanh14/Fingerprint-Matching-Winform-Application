using System.Collections.Generic;

namespace FingerMatchingApp
{
    class Hierarchical
    {
        public static List<Finger> Divide(Finger finger)
        {
            int n = finger.Minutiaes.Count / 15;
            List<Finger> fingers = new List<Finger>();

            Finger temp = new Finger(finger.Name, new List<Minutiae>());

            for (int i = 0; i < n; i++)
            {
                fingers.Add(temp);
                temp = new Finger(finger.Name, new List<Minutiae>());
            }

            for (int i = 0; i < n; i++)
            {
                fingers[i].Minutiaes.Clear();
                fingers[i].Minutiaes.AddRange(finger.Minutiaes.GetRange(0, 15));
                finger.Minutiaes.RemoveRange(0, 15);
            }

            return fingers;
        }
    }
}
