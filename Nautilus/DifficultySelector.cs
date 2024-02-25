using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class DifficultySelector : Form
    {
        public int Difficulty;
        public string Tier;
        private readonly Point StartLocation;

        public DifficultySelector(Point start, int startingDiff)
        {
            InitializeComponent();
            ControlBox = false;
            var tools = new NemoTools();
            try
            {
                diffNoPart.Image = tools.NemoLoadImage(Application.StartupPath + "\\res\\nopart.png");
                diff0.Image = tools.NemoLoadImage(Application.StartupPath + "\\res\\diff0.png");
                diff1.Image = tools.NemoLoadImage(Application.StartupPath + "\\res\\diff1.png");
                diff2.Image = tools.NemoLoadImage(Application.StartupPath + "\\res\\diff2.png");
                diff3.Image = tools.NemoLoadImage(Application.StartupPath + "\\res\\diff3.png");
                diff4.Image = tools.NemoLoadImage(Application.StartupPath + "\\res\\diff4.png");
                diff5.Image = tools.NemoLoadImage(Application.StartupPath + "\\res\\diff5.png");
                diff6.Image = tools.NemoLoadImage(Application.StartupPath + "\\res\\diff6.png");
            }
            catch
            {
                MessageBox.Show("Looks like one or more of the images I use are missing.\nPlease re-download this program and don't delete\nthe 'res' folder next time.",
                    "Missing Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Difficulty = startingDiff;
            StartLocation = start;
        }
        
        private void ChooseDifficulty(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var box = (PictureBox) sender;
            Difficulty = Convert.ToInt16((box.Tag));
            Close();
        }

        private void DifficultySelector_Shown(object sender, EventArgs e)
        {
            Location = new Point(StartLocation.X, StartLocation.Y - (Height/2));
            ActiveMarker.Left = (Width - ActiveMarker.Width) / 2;
            MoveMarker(Difficulty);
        }

        private void DifficultySelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            Tier = new SongData().GetDifficulty(Difficulty);
        }

        private void DifficultySelector_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
            {
                Close();
            }
        }

        private void MoveMarker(int diff)
        {
            var control = diffNoPart;
            switch (diff)
            {
                case 1:
                    control = diff0;
                    break;
                case 2:
                    control = diff1;
                    break;
                case 3:
                    control = diff2;
                    break;
                case 4:
                    control = diff3;
                    break;
                case 5:
                    control = diff4;
                    break;
                case 6:
                    control = diff5;
                    break;
                case 7:
                    control = diff6;
                    break;
            }
            ActiveMarker.Top = control.Top + control.Height;
        }
    }
}
