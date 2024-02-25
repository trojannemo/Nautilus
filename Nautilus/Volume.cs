using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class Volume : Form
    {
        private readonly Visualizer xParent;
        private readonly Point StartLocation;
        private int mouseX;
        private const double MinVolume = 50;
        private double CurrentVolume;

        public Volume(Visualizer parent, Point start)
        {
            InitializeComponent();
            xParent = parent;
            StartLocation = start;
            CurrentVolume = xParent.VolumeLevel;
        }

        private void Volume_Shown(object sender, EventArgs e)
        {
            Location = new Point(StartLocation.X - (Width / 2), StartLocation.Y - (Height / 2));
            picSlider.Parent = picBackground;
            picSlider.Left = 0;
            var percent = CurrentVolume / MinVolume ;
            picSlider.Left = (int)((Width - (picSlider.Width)) * (1.0 - percent));
            lblVolume.Text = CurrentVolume == 0 ? "100" : ((int)(100 - (CurrentVolume / MinVolume * 100))).ToString(CultureInfo.InvariantCulture);
        }

        private void picBackground_Click(object sender, EventArgs e)
        {
            SaveVolume();
        }

        private void Volume_Click(object sender, EventArgs e)
        {
            SaveVolume();
        }

        private void Volume_KeyUp(object sender, KeyEventArgs e)
        {
            SaveVolume();
        }
        
        private void picSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (picSlider.Cursor != Cursors.NoMoveHoriz) return;

            if (MousePosition.X != mouseX)
            {
                if (MousePosition.X > mouseX)
                {
                    picSlider.Left = picSlider.Left + (MousePosition.X - mouseX);
                }
                else if (MousePosition.X < mouseX)
                {
                    picSlider.Left = picSlider.Left - (mouseX - MousePosition.X);
                }
                mouseX = MousePosition.X;
            }

            if (picSlider.Left < 0)
            {
                picSlider.Left = 0;
            }
            else if (picSlider.Left > Width - picSlider.Width)
            {
                picSlider.Left = Width - picSlider.Width;
            }
            CurrentVolume = Math.Round(-1 * (-MinVolume + (MinVolume*picSlider.Left/(Width - picSlider.Width))),1);
            xParent.UpdateVolume(CurrentVolume);
            lblVolume.Text = CurrentVolume == 0 ? "100" : ((int)(100 - (CurrentVolume / MinVolume * 100))).ToString(CultureInfo.InvariantCulture);
        }

        private void picSlider_MouseUp(object sender, MouseEventArgs e)
        {
            SaveVolume();
        }

        private void picSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            picSlider.Cursor = Cursors.NoMoveHoriz;
            mouseX = MousePosition.X;
        }

        private void Volume_Deactivate(object sender, EventArgs e)
        {
            SaveVolume();
        }

        private void SaveVolume()
        {
            xParent.VolumeLevel = CurrentVolume;
            Dispose();
        }
    }
}
