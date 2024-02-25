using System;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class SMDialog : Form
    {
        public int UserAction = 0;

        public SMDialog()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            UserAction = 1;
            Close();
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            UserAction = 2;
            Close();
        }
    }
}
