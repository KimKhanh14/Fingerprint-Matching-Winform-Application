using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FingerMatchingApp
{
    public partial class Form1 : Form
    {
        public string path = "";

        string input;

        List<Finger> fingers1 = new List<Finger>();
        List<Finger> fingers2 = new List<Finger>();
        List<Finger> fingers3 = new List<Finger>();
        List<Finger> fingers4 = new List<Finger>();


        int minuNumberLimit1 = 9;

        bool checkFile1 = false;
        bool checkFile2 = false;

        public Form1()
        {
            InitializeComponent();
            matchingButton.Enabled = false;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;

            checkFile1 = false;
            checkFile2 = false;

            path = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName;
            path = path.Remove(path.IndexOf("bin"));

            label7.Text += path;

            path += "Group\\";

            for (int i = 15; i >= 0; i--)
            {
                domainUpDown1.Items.Add(i);
            }

            domainUpDown2.Items.Add("Hough Transform Voting");
            domainUpDown2.Items.Add("Rotate and Moving");
            domainUpDown2.Items.Add("Polar Coordinate System");
        }


        private void datasetFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fingers1.Clear();
            GC.Collect();
            WaitFormFunc frm = new WaitFormFunc();
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string dataset = dlg.FileName;
                    if (dataset.EndsWith(".txt"))
                    {
                        //WaitFormFunc frm = new WaitFormFunc();
                        frm.Show();
                        Thread.Sleep(500);
                        string check = "";
                        fingers1 = File.ReadF(dataset, ref check);

                        GlobalFeature.Classification(fingers1, path);

                        textBox1.Text = dataset.Split('\\').Last();

                        frm.Close();

                        if (check != "")
                        {
                            MessageBox.Show(check);
                        }

                        checkFile1 = true;

                        if ((checkFile1 == true) && (checkFile2 == true))
                        {
                            matchingButton.Enabled = true;
                        }

                        label3.Text = "Result: ";
                        label4.Text = "Time matching: ";
                        textBox3.Clear();
                    }
                    else
                    {
                        checkFile1 = true;
                        MessageBox.Show("Không phải file .txt");
                    }
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(900);
                frm.Close();
                MessageBox.Show(ex.Message, "Notice");
            }
        }


        private void inputFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fingers2.Clear();
            GC.Collect();
            WaitFormFunc frm = new WaitFormFunc();

            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = ".txt";
                dlg.Filter = "Text documents (.txt)|*.txt";

                if ((dlg.ShowDialog() == DialogResult.OK))
                {
                    input = dlg.FileName;
                    if (input.EndsWith(".txt"))
                    {
                        frm.Show();
                        Thread.Sleep(500);
                        string check = "";
                        fingers2 = File.ReadF(input, ref check);
                        textBox2.Text = input.Split('\\').Last();


                        frm.Close();

                        if (check != "")
                        {
                            MessageBox.Show(check);
                        }


                        label3.Text = "Result: ";
                        label4.Text = "Time matching: ";
                        textBox3.Clear();

                        checkFile2 = true;

                        if ((checkFile1 == true) && (checkFile2 == true))
                        {
                            matchingButton.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không phải file .txt");
                    }
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(900);
                frm.Close();
                MessageBox.Show(ex.Message, "Notice");
            }
        }


        private void Matching_Polar()
        {
            int r;
            int theta;

            string top1 = "";
            string top2 = "";
            string top3 = "";
            string top4 = "";

            int numTop1 = 0;
            int numTop2 = 0;
            int numTop3 = 0;
            int numTop4 = 0;

            foreach (Finger aFinger in fingers1)
            {

                foreach (Minutiae aMinu in aFinger.Minutiaes)
                {
                    r = aMinu.X * aMinu.X + aMinu.Y * aMinu.Y;
                    theta = (int)(Math.Atan2((double)aMinu.X, (double)aMinu.Y) * 100000);
                    aMinu.X = r;
                    aMinu.Y = theta;
                }
            }

            foreach (Finger aFinger in fingers4)
            {
                foreach (Minutiae aMinu in aFinger.Minutiaes)
                {
                    r = aMinu.X * aMinu.X + aMinu.Y * aMinu.Y;
                    theta = (int)(Math.Atan2((double)aMinu.X, (double)aMinu.Y) * 100000);
                    aMinu.X = r;
                    aMinu.Y = theta;
                }
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();

            int count1;
            int count2;

            int n4 = fingers4.Count();
            int max = 0;
            string maxName = "";
            int mean;
            int per;

            foreach (Finger aFinger1 in fingers1)
            {
                mean = fingers2[0].Minutiaes.Count();
                count2 = 0;
                for (int j = 0; j < n4; j++)
                {

                    count1 = Matching.CountMinuMatching3(aFinger1, fingers4[j]);
                    if (count1 < minuNumberLimit1)
                    {
                        goto GETOUT;
                    }
                    else count2 += count1;
                }
                per = (int)(((double)count2 / (double)mean) * 100);
                if (per > max)
                {
                    max = per;
                    maxName = aFinger1.Name;
                }
                else if (per > numTop1)
                {
                    numTop1 = per;
                    top1 = aFinger1.Name;
                }
                else if (per > numTop2)
                {
                    numTop2 = per;
                    top2 = aFinger1.Name;
                }
                else if (per > numTop3)
                {
                    numTop3 = per;
                    top3 = aFinger1.Name;
                }
                else if (per > numTop4)
                {
                    numTop4 = per;
                    top4 = aFinger1.Name;
                }
            GETOUT:
                { }
            }

            label4.Text = "Time matching: " + watch.ElapsedMilliseconds + " ms";
            watch.Stop();

            if (top1 != "") textBox3.Text += top1 + " (" + numTop1 + "%)\r\n";
            if (top2 != "") textBox3.Text += top2 + " (" + numTop2 + "%)\r\n";
            if (top3 != "") textBox3.Text += top3 + " (" + numTop3 + "%)\r\n";
            if (top4 != "") textBox3.Text += top4 + " (" + numTop4 + "%)";

            if (maxName != "")
            {
                label3.Text = "Result: " + maxName + " (" + max + "%)";
                MessageBox.Show("Done!");
            }
            else
            {
                textBox3.Clear();
                MessageBox.Show("Not found!");
            }
        }

        private void Matching_RM()
        {
            int polarX;
            int polarY;

            double polarDirect;

            string top1 = "";
            string top2 = "";
            string top3 = "";
            string top4 = "";

            int numTop1 = 0;
            int numTop2 = 0;
            int numTop3 = 0;
            int numTop4 = 0;

            foreach (Finger aFinger in fingers1)
            {
                polarX = (int)GlobalFeature.linear(aFinger.Minutiaes).X;
                polarY = (int)GlobalFeature.linear(aFinger.Minutiaes).Y;

                polarDirect = GlobalFeature.deltaDirect(new Vector(polarX, polarY), new Vector(0, 1)) * Math.PI / 180;

                foreach (Minutiae aMinu in aFinger.Minutiaes)
                {
                    aMinu.Direct += polarDirect;
                    if (aMinu.Direct > 2 * Math.PI)
                    {
                        aMinu.Direct -= 2 * Math.PI;
                    }
                    aMinu.X += polarX;
                    aMinu.Y += polarY;
                }
            }

            foreach (Finger aFinger in fingers4)
            {
                polarX = (int)GlobalFeature.linear(aFinger.Minutiaes).X;
                polarY = (int)GlobalFeature.linear(aFinger.Minutiaes).Y;

                polarDirect = GlobalFeature.deltaDirect(new Vector(polarX, polarY), new Vector(0, 1)) * Math.PI / 180;
                foreach (Minutiae aMinu in aFinger.Minutiaes)
                {
                    aMinu.Direct += polarDirect;
                    if (aMinu.Direct > 2 * Math.PI)
                    {
                        aMinu.Direct -= 2 * Math.PI;
                    }
                    aMinu.X += polarX;
                    aMinu.Y += polarY;
                }
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();

            int count1;
            int count2;

            int n4 = fingers4.Count();
            int max = 0;
            string maxName = "";
            int mean;
            int per;

            foreach (Finger aFinger1 in fingers1)
            {
                mean = fingers2[0].Minutiaes.Count();
                count2 = 0;
                for (int j = 0; j < n4; j++)
                {

                    count1 = Matching.CountMinuMatching2(aFinger1, fingers4[j]);
                    if (count1 < minuNumberLimit1)
                    {
                        goto GETOUT;
                    }
                    else count2 += count1;
                }

                per = (int)(((double)count2 / (double)mean) * 100);
                if (per > max)
                {
                    max = per;
                    maxName = aFinger1.Name;
                }
                else if (per > numTop1)
                {
                    numTop1 = per;
                    top1 = aFinger1.Name;
                }
                else if (per > numTop2)
                {
                    numTop2 = per;
                    top2 = aFinger1.Name;
                }
                else if (per > numTop3)
                {
                    numTop3 = per;
                    top3 = aFinger1.Name;
                }
                else if (per > numTop4)
                {
                    numTop4 = per;
                    top4 = aFinger1.Name;
                }

            GETOUT:
                { }
            }

            label4.Text = "Time matching: " + watch.ElapsedMilliseconds + " ms";
            watch.Stop();

            if (top1 != "") textBox3.Text += top1 + " (" + numTop1 + "%)\r\n";
            if (top2 != "") textBox3.Text += top2 + " (" + numTop2 + "%)\r\n";
            if (top3 != "") textBox3.Text += top3 + " (" + numTop3 + "%)\r\n";
            if (top4 != "") textBox3.Text += top4 + " (" + numTop4 + "%)";

            if (maxName != "")
            {
                label3.Text = "Result: " + maxName + " (" + max + "%)";
                MessageBox.Show("Done!");
            }
            else
            {
                textBox3.Clear();
                MessageBox.Show("Not found!");
            }
        }

        private void Matching_Hough()
        {
            string top1 = "";
            string top2 = "";
            string top3 = "";
            string top4 = "";

            int numTop1 = 0;
            int numTop2 = 0;
            int numTop3 = 0;
            int numTop4 = 0;

            int angleStart = -30;
            int angleUnit = 3;
            int angleFinish = 30;
            int anglesCount = Convert.ToInt32((angleFinish - angleStart) / angleUnit) + 1;
            int[] angleSet = new int[anglesCount];
            int i = 0;
            int angle = angleStart;
            while (i < anglesCount)
            {
                angleSet[i] = angle;
                i++;
                angle += angleUnit;
            }
            //DELTAXSET
            int deltaXStart = -319; //Image Width
            int deltaXFinish = 319;
            int deltaXUnit = 2;
            int deltaXCount = Convert.ToInt32((deltaXFinish - deltaXStart) / deltaXUnit) + 1;
            int[] deltaXSet = new int[deltaXCount];
            i = 0;
            int deltaX = deltaXStart;
            while (i < deltaXCount)
            {
                deltaXSet[i] = deltaX;
                i++;
                deltaX += deltaXUnit;
            }
            //DELTAYSET
            int deltaYStart = -479; //Image Height
            int deltaYFinish = 479;
            int deltaYUnit = 2;
            int deltaYCount = Convert.ToInt32((deltaYFinish - deltaYStart) / deltaYUnit) + 1;
            int[] deltaYSet = new int[deltaYCount];
            i = 0;
            int deltaY = deltaYStart;
            while (i < deltaYCount)
            {
                deltaYSet[i] = deltaY;
                i++;
                deltaY += deltaYUnit;
            }

            Minutiae minuResult;

            int angleLimit = 5;
            int distanceLimit = 25;
            //int minuNumberLimit1 = 25;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            int count1;
            int count2;

            int n4 = fingers4.Count();
            int max = 0;
            string maxName = "";
            int mean;
            int per;

            foreach (Finger aFinger1 in fingers1)
            {
                mean = fingers2[0].Minutiaes.Count();
                minuResult = Matching.GetMinutiaeChanging_UseHoughTransform(aFinger1, fingers2[0], angleSet, deltaXSet, deltaYSet, angleLimit * Math.PI / 180, 320 / 2, 480 / 2);
                count2 = 0;
                for (int j = 0; j < n4; j++)
                {
                    count1 = Matching.CountMinuMatching1(aFinger1, fingers4[j], minuResult, distanceLimit, angleLimit * Math.PI / 180);
                    if (count1 < minuNumberLimit1)
                    {
                        goto GETOUT;
                    }
                    else count2 += count1;
                }
                per = (int)(((double)count2 / (double)mean) * 100);
                if (per > max)
                {
                    max = per;
                    maxName = aFinger1.Name;
                }
                else if (per > numTop1)
                {
                    numTop1 = per;
                    top1 = aFinger1.Name;
                }
                else if (per > numTop2)
                {
                    numTop2 = per;
                    top2 = aFinger1.Name;
                }
                else if (per > numTop3)
                {
                    numTop3 = per;
                    top3 = aFinger1.Name;
                }
                else if (per > numTop4)
                {
                    numTop4 = per;
                    top4 = aFinger1.Name;
                }

            GETOUT:
                { }
            }

            label4.Text = "Time matching: " + watch.ElapsedMilliseconds + " ms";
            watch.Stop();

            if (top1 != "") textBox3.Text += top1 + " (" + numTop1 + "%)\r\n";
            if (top2 != "") textBox3.Text += top2 + " (" + numTop2 + "%)\r\n";
            if (top3 != "") textBox3.Text += top3 + " (" + numTop3 + "%)\r\n";
            if (top4 != "") textBox3.Text += top4 + " (" + numTop4 + "%)";

            if (maxName != "")
            {
                label3.Text = "Result: " + maxName + " (" + max + "%)";
                MessageBox.Show("Done!");
            }
            else
            {
                textBox3.Clear();
                MessageBox.Show("Not found!");
            }


        }

        private void Matching_Click(object sender, EventArgs e)
        {
            textBox3.Clear();

            matchingButton.Enabled = false;
            string checkF = "";
            fingers2 = File.ReadF(input, ref checkF);
            fingers3 = File.ReadF(input, ref checkF);

            if (fingers2.Count() == 0)
            {
                MessageBox.Show("Not found!");
            }
            else
            {
                string nameFile = "";

                if (GlobalFeature.whichClass(fingers2[0]) == 0)
                {
                    nameFile += "0";
                }
                else if (GlobalFeature.whichClass(fingers2[0]) == 1)
                {
                    nameFile += "1";
                }
                else if (GlobalFeature.whichClass(fingers2[0]) == 2)
                {
                    nameFile += "2";
                }
                else
                {
                    nameFile += "3";
                }

                fingers1.Clear();
                checkF = "";
                fingers1 = File.ReadF(path + nameFile + ".txt", ref checkF);

                fingers4 = Hierarchical.Divide(fingers3[0]);
                fingers3.Clear();

                int algorithm = domainUpDown2.SelectedIndex;

                if (algorithm == 0)
                {
                    Matching_Hough();
                }
                else if (algorithm == 2)
                {
                    Matching_Polar();
                }
                else
                {
                    Matching_RM();
                }
            }

            fingers1.Clear();
            fingers2.Clear();
            fingers3.Clear();
            fingers4.Clear();

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fingers1.Clear();
            fingers2.Clear();
            fingers3.Clear();
            fingers4.Clear();

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            matchingButton.Enabled = false;

            label3.Text = "Result: ";
            label4.Text = "Time matching: ";

            checkFile1 = false;
            checkFile2 = false;

            GC.Collect();
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            minuNumberLimit1 = (int)domainUpDown1.Items[domainUpDown1.SelectedIndex];
            if ((checkFile1 == true) && (checkFile2 == true))
            {
                matchingButton.Enabled = true;
            }

            textBox3.Clear();
            label3.Text = "Result: ";
            label4.Text = "Time matching: ";
        }

        private void changeDirectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Can not open folder!");
            }
            else
            {
                path = dlg.SelectedPath + "\\";

                fingers1.Clear();
                fingers2.Clear();
                fingers3.Clear();
                fingers4.Clear();

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();

                matchingButton.Enabled = false;

                label3.Text = "Result: ";
                label4.Text = "Time matching: ";
                label7.Text = "Working space: " + path;

                checkFile1 = false;
                checkFile2 = false;

            }
        }

        private void domainUpDown2_SelectedItemChanged(object sender, EventArgs e)
        {
            if ((checkFile1 == true) && (checkFile2 == true))
            {
                matchingButton.Enabled = true;
            }
            textBox3.Clear();
            label3.Text = "Result: ";
            label4.Text = "Time matching: ";
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help helpFrm = new Help();
            Thread.Sleep(200);
            helpFrm.Show();
        }
    }
}
