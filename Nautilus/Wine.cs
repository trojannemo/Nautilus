using System;
using System.IO;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class Wine : Form
    {
        public Wine()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Dispose();
            var path = Application.StartupPath + "\\bin\\wine";
            var sw = new StreamWriter(path, false);
            sw.WriteLine("V0lORSdvcyBhcmUgQ09PTCE=");
            sw.Dispose();
            timer2.Start();
        }

        private void Wine_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.05; // Decrease opacity step
            }
            else
            {
                timer2.Stop();
                timer2.Dispose();
                this.Close(); // Close the form when fully transparent
            }
        }
    }
}
