using System.Windows.Forms;

namespace FingerMatchingApp
{
    public partial class LoadingWindow : Form
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        public LoadingWindow(Form parent)
        {
            InitializeComponent();
        }


        public void closeForm()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
