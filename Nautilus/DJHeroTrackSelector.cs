using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class DJHeroTrackSelector : Form
    {
        public int SelectedIndex;

        public DJHeroTrackSelector(List<DJHeroTrack> tracks)
        {
            InitializeComponent();

            SelectedIndex = -1;
            
            try
            {
                btnTrack1.Text = string.Join(", ", tracks[0].MixArtists) + " - " + string.Join(", ", tracks[0].MixNames);
                if (tracks.Count > 1)
                {
                    btnTrack2.Text = string.Join(", ", tracks[1].MixArtists) + " - " + string.Join(", ", tracks[1].MixNames);
                    if (tracks.Count > 2)
                    {
                        btnTrack3.Text = string.Join(", ", tracks[2].MixArtists) + " - " + string.Join(", ", tracks[2].MixNames);
                    }
                }              
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error assigning tracks to buttons\nError says: " + ex.Message, "DJ Hero Track Selector", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private void btnTrack1_Click(object sender, EventArgs e)
        {            
            SelectedIndex = 0;
            Close();
        }

        private void btnTrack2_Click(object sender, EventArgs e)
        {
            if (btnTrack2.Text == "No track available") return;
            SelectedIndex = 1;
            Close();
        }

        private void btnTrack3_Click(object sender, EventArgs e)
        {
            if (btnTrack3.Text == "No track available") return;
            SelectedIndex = 2;
            Close();
        }
    }
}
