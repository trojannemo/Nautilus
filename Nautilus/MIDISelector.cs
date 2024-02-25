using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class MIDISelector : Form
    {
        private readonly string midifile;

        public MIDISelector(string arg)
        {
            InitializeComponent();
            midifile = arg;
        }

        private void btnCleaner_Click(object sender, EventArgs e)
        {
            var newCleaner = new MIDICleaner(midifile, Color.FromArgb(230, 215, 0), Color.White) { ExitonClose = true };
            newCleaner.Show();
            Hide();
        }

        private void btnAnalyzer_Click(object sender, EventArgs e)
        {
            var newAnalyzer = new SongAnalyzer(midifile) {ExitonClose = true};
            newAnalyzer.Show();
            Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Opacity = Opacity - 5;
            if (Opacity == 0)
            {
                Environment.Exit(0);
            }
        }
    }
}
