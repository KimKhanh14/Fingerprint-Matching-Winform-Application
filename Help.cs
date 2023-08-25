using System.Windows.Forms;

namespace FingerMatchingApp
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();

            textBox1.ReadOnly = true;
            textBox1.Text = "";
            textBox1.Text += "Step 1: Import dataset and input file (Only accept .txt file)\r\n";
            textBox1.Text += "  note: If the data in the file .txt is wrong, it will be automatically ignored and enter the next qualified data.\r\n\r\n";
            textBox1.Text += "Step 2: Choose the \"Threshold\" and \"Matching algorithm\"\r\n\r\n";
            textBox1.Text += "Step 3: Click \"Matching\" button. If dataset or input is no file, this button is rejected\r\n\r\n";
            textBox1.Text += "Step 4: When you change the \"Threshold\" or \"Matching algorithm\". The result is clean and the \"Matching\" button is enable.";

            textBox2.ReadOnly = true;
            textBox2.Text = "";
            textBox2.Text += "Intel(R) Core(TM) i5-1038NG7 CPU @ 2.00GHz (8 CPUs), ~2.0GHz";

            textBox3.ReadOnly = true;
            textBox3.Text = "";
            textBox3.Text += "\"Threshold\" is the threshold of the same number of minutieas at each input fingerprint layer. " +
                "The value of the threshold is from 0-15.";


        }
    }
}
