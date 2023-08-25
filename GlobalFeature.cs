using System;
using System.Collections.Generic;
using System.Linq;

namespace FingerMatchingApp
{
    public class GlobalFeature
    {
        public static Vector linear(List<Minutiae> minus)
        {
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double ssX;
            double sumCodeviates = 0;
            double sCo;
            double count = minus.Count;
            double x, y;

            for (int ctr = 0; ctr < count; ctr++)
            {
                x = (double)minus[ctr].X;
                y = (double)minus[ctr].Y;
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }
            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            double meanX = sumOfX / count;
            double meanY = sumOfY / count;
            double slope = sCo / ssX;

            return new Vector(meanY - slope * meanX, slope);
        }
        public static double deltaDirect(Vector a, Vector b)
        {
            double ab = (a.X * b.X) + (a.Y * b.Y);
            double distance = Math.Sqrt((a.X * a.X) + (a.Y * a.Y)) * Math.Sqrt((b.X * b.X) + (b.Y * b.Y));
            return (Math.Acos(ab / distance) * (180 / Math.PI)); // độ
        }

        private static int check_Direct(double _direct)
        {
            _direct = _direct * 180 / Math.PI;
            return (int)(_direct / 30);

        }

        private static int Direct(Finger finger)
        {
            double _direct;
            var list = new List<int>(12);
            list.AddRange(Enumerable.Repeat(0, 12));
            foreach (Minutiae aMinu in finger.Minutiaes)
            {
                _direct = aMinu.Direct;

                list[check_Direct(_direct)] = list[check_Direct(_direct)] + 1;
            }
            int _max = 0;
            int _index = 0;
            for (int i = 0; i < 12; i++)
            {
                if (_max < list[i])
                {
                    _max = list[i];
                    _index = i;
                }
            }
            return _index;
        }

        private static Vector libDirect(int value)
        {
            switch (value)
            {
                case 0:
                    return new Vector(Math.Sqrt(3) / 2, 1 / 2);
                case 1:
                    return new Vector(1 / 2, Math.Sqrt(3) / 2);
                case 2:
                    return new Vector(0, 1);
                case 3:
                    return new Vector(-1 / 2, Math.Sqrt(3) / 2);
                case 4:
                    return new Vector(-Math.Sqrt(3) / 2, 1 / 2);
                case 5:
                    return new Vector(-1, 0);
                case 6:
                    return new Vector(-Math.Sqrt(3) / 2, -1 / 2);
                case 7:
                    return new Vector(-1 / 2, -Math.Sqrt(3) / 2);
                case 8:
                    return new Vector(0, -1);
                case 9:
                    return new Vector(-1 / 2, Math.Sqrt(3) / 2);
                case 10:
                    return new Vector(Math.Sqrt(3) / 2, -1 / 2);
                default:
                    return new Vector(1, 0);
            }
        }

        public static void Classification(List<Finger> fingers, string path)
        {
            double delta;
            List<Finger> Group0 = new List<Finger>();
            List<Finger> Group1 = new List<Finger>();
            List<Finger> Group2 = new List<Finger>();
            List<Finger> Group3 = new List<Finger>();

            int temp;

            foreach (Finger aFinger in fingers)
            {

                delta = deltaDirect(libDirect(Direct(aFinger)), linear(aFinger.Minutiaes));
                temp = (int)delta / 45;

                switch (temp)
                {
                    case 0:
                        Group0.Add(aFinger);
                        break;
                    case 1:
                        Group1.Add(aFinger);
                        break;
                    case 2:
                        Group2.Add(aFinger);
                        break;
                    default:
                        Group3.Add(aFinger);
                        break;
                }
            }

            /*File.WriteF(Group0, path + "0_" + Group0.Count + ".txt");
            File.WriteF(Group1, path + "1_" + Group1.Count + ".txt");
            File.WriteF(Group2, path + "2_" + Group2.Count + ".txt");
            File.WriteF(Group3, path + "3_" + Group3.Count + ".txt");*/
            File.WriteF(Group0, path + "0.txt");
            File.WriteF(Group1, path + "1.txt");
            File.WriteF(Group2, path + "2.txt");
            File.WriteF(Group3, path + "3.txt");
        }


        public static int whichClass(Finger finger)
        {
            double delta = deltaDirect(libDirect(Direct(finger)), linear(finger.Minutiaes));
            int temp = (int)delta / 45;
            return temp;

        }

        /*public static int checkCount(Finger aFinger)
        {
            int one = 0;
            int two = 0;
            for (int j = 0; j < aFinger.Minutiaes.Count(); j++)
            {
                if (aFinger.Minutiaes[j].Type == 1)
                {
                    one += 1;
                }
                else
                {
                    two += 2;
                }
            }
            if (two > one)
            {
                return 1;
            }
            return 2;
        }*/

        /*public static void countType(string file1, List<Finger> fingers, string path)
        {
            int one, two;
            List<Finger> Group1 = new List<Finger>(); //am
            List<Finger> Group2 = new List<Finger>(); //duong
            int n;
            foreach (Finger aFinger in fingers)
            {
                one = 0;
                two = 0;
                n = aFinger.Minutiaes.Count();
                for (int j = 0; j < n; j++)
                {
                    if (aFinger.Minutiaes[j].Type == 1)
                    {
                        one += 1;
                    }
                    else
                    {
                        two += 2;
                    }
                }
                if (two > one)
                {
                    Group1.Add(aFinger);
                }
                else
                {
                    Group2.Add(aFinger);
                }

            }

            File.WriteF(Group1, path + file1 + "_1_" + Group1.Count + ".txt");
            File.WriteF(Group2, path + file1 + "_2_" + Group2.Count + ".txt");
            File.WriteF(Group1, path + file1 + "_1.txt");
            File.WriteF(Group2, path + file1 + "_2.txt");
        }*/

    }
}
