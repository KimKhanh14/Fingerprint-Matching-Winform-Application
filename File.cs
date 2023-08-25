using System;
using System.Collections.Generic;
using System.IO;

namespace FingerMatchingApp
{
    public class File
    {
        public File()
        {

        }

        public static List<Finger> ReadF(string fileName, ref string check)
        {
            string _name = "";
            int _type, _x, _y;
            double _direct;
            List<Finger> fingers = new List<Finger>();

            StreamReader sr = new StreamReader(fileName);
            string line = "";
            Minutiae tempMinu;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line != string.Empty)
                {
                    string[] array = line.Split(' ');
                    if (array.Length == 1)
                    {
                        _name = array[0].ToString();
                    }
                    else if ((_name.EndsWith(".jpg") || _name.EndsWith(".bmp")) && (array.Length % 4 == 0))
                    {
                        List<Minutiae> minutiaes = new List<Minutiae>();
                        for (int i = 0; i < array.Length; i += 4)
                        {
                            _type = int.Parse(array[i + 0]);
                            _y = int.Parse(array[i + 1]);
                            _x = int.Parse(array[i + 2]);
                            _direct = double.Parse(array[i + 3]);
                            if ((_type == 1) || (_type == 2))
                            {
                                if ((_y >= 0) && (_y <= 480))
                                {
                                    if ((_x >= 0) && (_x <= 320))
                                    {
                                        if ((_direct >= 0) && (_direct <= 2 * Math.PI))
                                        {
                                            tempMinu = new Minutiae(_type, _y, _x, _direct);
                                            minutiaes.RemoveAll(delegate (Minutiae v)
                                            {
                                                return (v.X == _x) && (v.Y == _y);
                                            });
                                            minutiaes.Add(tempMinu);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                check += "Wrong type of minutiae.\r\n";
                            }
                        }
                        fingers.Add(new Finger(_name, minutiaes));
                    }
                    else
                    {
                        check += "Not .bmp/.txt file.\r\n";
                    }
                }
            }
            sr.Close();
            return fingers;
        }

        public static void WriteF(List<Finger> fingers, string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName);
            foreach (Finger aFinger in fingers)
            {
                sw.WriteLine(aFinger.Name);
                foreach (Minutiae aMinu in aFinger.Minutiaes)
                {
                    sw.Write(aMinu.Type.ToString() + " " + aMinu.Y.ToString() + " " + aMinu.X.ToString() + " " + aMinu.Direct.ToString() + " ");
                }
                sw.WriteLine();
                sw.WriteLine();
            }
            sw.Close();
        }

    }
}
