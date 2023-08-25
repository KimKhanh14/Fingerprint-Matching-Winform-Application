using System;

namespace FingerMatchingApp
{
    public class Matching
    {
        public Matching()
        {

        }
        public static int GetMinuDistance(Minutiae m1, Minutiae m2)
        {
            //return Convert.ToInt32(Math.Sqrt(Math.Pow(m1.X - m2.X, 2) + Math.Pow(m1.Y - m2.Y, 2)));
            return Convert.ToInt32(Math.Pow(m1.X - m2.X, 2) + Math.Pow(m1.Y - m2.Y, 2));
        }

        public static bool IsMinutiaeMatching1(Minutiae m1, Minutiae m2, int distanceLimit, double angleLimit, Minutiae minuChanging)
        {
            if (m1.Type != m2.Type) return false;
            Minutiae m = m2.GetMinutiaeAfterChange(minuChanging.X, minuChanging.Y, minuChanging.Direct, 319, 479);
            int distance = GetMinuDistance(m1, m);
            double angleRotate = Math.Min(Math.Abs(m.Direct - m1.Direct), Math.PI * 2 - Math.Abs(m.Direct - m1.Direct));
            if (distance <= distanceLimit)
                return true;
            if (angleRotate <= angleLimit)
                return true;
            return false;
        }

        public static bool IsMinutiaeMatching2(Minutiae m1, Minutiae m2)
        {
            if (m1.Type != m2.Type) return false;
            int distance = GetMinuDistance(m1, m2);
            double angleRotate = Math.Min(Math.Abs(m1.Direct - m2.Direct), Math.PI * 2 - Math.Abs(m1.Direct - m2.Direct));
            if (distance <= 1)
                return true;
            if (angleRotate <= 0.08)
                return true;
            return false;
        }

        public static bool IsMinutiaeMatching3(Minutiae m1, Minutiae m2)
        {
            if (m1.Type != m2.Type) return false;
            double angleRotate = Math.Min(Math.Abs(m1.Direct - m2.Direct), Math.PI * 2 - Math.Abs(m1.Direct - m2.Direct));
            if (m1.X - m2.X == 0)
                return true;
            if (m1.Y - m2.Y == 0)
                return true;
            if (angleRotate <= 0.08)
                return true;
            return false;
        }

        public static int CountMinuMatching1(Finger minuSet1, Finger minuSet2, Minutiae minuChanging, int distanceLimit, double angleLimit)
        {
            int i, j;
            bool[] mark = new bool[minuSet2.Minutiaes.Count];
            for (i = 0; i < minuSet2.Minutiaes.Count; i++)
                mark[i] = false;

            for (i = 0; i < minuSet1.Minutiaes.Count; i++)
            {
                Minutiae m1 = (Minutiae)minuSet1.Minutiaes[i];
                for (j = 0; j < minuSet2.Minutiaes.Count; j++)
                {
                    if (!mark[j])
                    {
                        Minutiae m2 = (Minutiae)minuSet2.Minutiaes[j];
                        if (Matching.IsMinutiaeMatching1(m1, m2, distanceLimit, angleLimit, minuChanging))
                        {
                            mark[j] = true;
                            break;
                        }
                    }
                }
            }
            int count = 0;
            for (i = 0; i < minuSet2.Minutiaes.Count; i++)
                if (mark[i]) count++;
            return count;
        }

        public static int CountMinuMatching2(Finger minuSet1, Finger minuSet2)
        {
            int i, j;
            bool[] mark = new bool[minuSet2.Minutiaes.Count];
            for (i = 0; i < minuSet2.Minutiaes.Count; i++)
                mark[i] = false;

            Minutiae m1, m2;

            for (i = 0; i < minuSet1.Minutiaes.Count; i++)
            {
                m1 = (Minutiae)minuSet1.Minutiaes[i];
                for (j = 0; j < minuSet2.Minutiaes.Count; j++)
                {
                    if (!mark[j])
                    {
                        m2 = (Minutiae)minuSet2.Minutiaes[j];
                        if (Matching.IsMinutiaeMatching2(m1, m2))
                        {
                            mark[j] = true;
                            break;
                        }
                    }
                }
            }
            int count = 0;
            for (i = 0; i < minuSet2.Minutiaes.Count; i++)
                if (mark[i]) count++;

            return count;
        }

        public static int CountMinuMatching3(Finger minuSet1, Finger minuSet2)
        {
            int i, j;
            bool[] mark = new bool[minuSet2.Minutiaes.Count];
            for (i = 0; i < minuSet2.Minutiaes.Count; i++)
                mark[i] = false;

            Minutiae m1, m2;

            for (i = 0; i < minuSet1.Minutiaes.Count; i++)
            {
                m1 = (Minutiae)minuSet1.Minutiaes[i];
                for (j = 0; j < minuSet2.Minutiaes.Count; j++)
                {
                    if (!mark[j])
                    {
                        m2 = (Minutiae)minuSet2.Minutiaes[j];
                        if (Matching.IsMinutiaeMatching3(m1, m2))
                        {
                            mark[j] = true;
                            break;
                        }
                    }
                }
            }
            int count = 0;
            for (i = 0; i < minuSet2.Minutiaes.Count; i++)
                if (mark[i]) count++;

            return count;
        }


        public static Minutiae GetMinutiaeChanging_UseHoughTransform(Finger minuSet1, Finger minuSet2, int[] angleSet, int[] deltaXSet, int[] deltaYSet, double angleLimit, int xRoot, int yRoot)
        {
            int i, j, k;
            int length = deltaXSet.Length * deltaYSet.Length * angleSet.Length;
            int[,,] A = new int[deltaXSet.Length, deltaYSet.Length, angleSet.Length];
            Minutiae m1;
            Minutiae m2;
            double _deltaX;
            double _deltaY;
            for (i = 0; i < deltaXSet.Length; i++)
                for (j = 0; j < deltaYSet.Length; j++)
                    for (k = 0; k < angleSet.Length; k++)
                        A[i, j, k] = 0;


            for (i = 0; i < minuSet1.Minutiaes.Count; i++)
            {
                m1 = (Minutiae)minuSet1.Minutiaes[i];
                for (j = 0; j < minuSet2.Minutiaes.Count; j++)
                {
                    m2 = (Minutiae)minuSet2.Minutiaes[j];
                    for (int angleIndex = 0; angleIndex < angleSet.Length; angleIndex++)
                    {
                        double tempAngle = Math.Abs((m2.Direct + angleSet[angleIndex] * Math.PI / 180) - m1.Direct);
                        if ((tempAngle < angleLimit) || ((Math.PI * 2 - tempAngle) < angleLimit))
                        {
                            int c1X = m1.X - xRoot;
                            int c1Y = yRoot - m1.Y;
                            int c2X = m2.X - xRoot;
                            int c2Y = yRoot - m2.Y;

                            _deltaX = c1X - (Math.Cos(angleSet[angleIndex] * Math.PI / 180) * c2X - Math.Sin(angleSet[angleIndex] * Math.PI / 180) * c2Y);
                            _deltaY = c1Y - (Math.Sin(angleSet[angleIndex] * Math.PI / 180) * c2X + Math.Cos(angleSet[angleIndex] * Math.PI / 180) * c2Y);

                            #region quantization deltax,deltay
                            int deltaXSelect = deltaXSet[0];
                            int deltaXIndexSelect = 0;
                            int deltaYSelect = deltaYSet[0];
                            int deltaYIndexSelect = 0;
                            for (int deltaXIndex = 0; deltaXIndex < deltaXSet.Length; deltaXIndex++)
                                if (Math.Abs(Convert.ToDouble(deltaXSet[deltaXIndex]) - _deltaX) < Math.Abs(Convert.ToDouble(deltaXSelect) - _deltaX))
                                {
                                    deltaXSelect = deltaXSet[deltaXIndex];
                                    deltaXIndexSelect = deltaXIndex;
                                }
                            for (int deltaYIndex = 0; deltaYIndex < deltaYSet.Length; deltaYIndex++)
                                if (Math.Abs(Convert.ToDouble(deltaYSet[deltaYIndex]) - _deltaY) < Math.Abs(Convert.ToDouble(deltaYSelect) - _deltaY))
                                {
                                    deltaYSelect = deltaYSet[deltaYIndex];
                                    deltaYIndexSelect = deltaYIndex;
                                }
                            #endregion

                            A[deltaXIndexSelect, deltaYIndexSelect, angleIndex]++;
                        }
                    }
                }
            }
            #region Select Return value
            int _deltaXIndex = 0;
            int _deltaYIndex = 0;
            int _angleIndex = 0;
            for (i = 0; i < deltaXSet.Length; i++)
                for (j = 0; j < deltaYSet.Length; j++)
                    for (k = 0; k < angleSet.Length; k++)
                    {
                        if (A[i, j, k] > A[_deltaXIndex, _deltaYIndex, _angleIndex])
                        {
                            _deltaXIndex = i;
                            _deltaYIndex = j;
                            _angleIndex = k;
                        }
                    }

            return new Minutiae(0, deltaYSet[_deltaYIndex] + 1, deltaXSet[_deltaXIndex] + 1, angleSet[_angleIndex] * Math.PI / 180);
            #endregion
        }
    }
}