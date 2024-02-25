using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class EventManagerEditor : Form
    {
        private readonly EventManager eManager;
        private readonly int suggestionType;
        
        public EventManagerEditor(EventManager Manager, int type)
        {
            InitializeComponent();
            eManager = Manager;
            List<string> suggestions;
            //type 0 = bands
            //type 1 = songs
            //type 2 = artists
            switch (type)
            {
                default:
                    grpEdit.Text = "Edit Auto-Suggest: Bands";
                    lblEdit.Text = "Band:";
                    btnNew.Text = "New Band";
                    suggestions = eManager.Bands;
                    break;
                case 1:
                    grpEdit.Text = "Edit Auto-Suggest: Songs";
                    lblEdit.Text = "Song:";
                    btnNew.Text = "New Song";
                    suggestions = eManager.Songs;
                    break;
                case 2:
                    grpEdit.Text = "Edit Auto-Suggest: Artists";
                    lblEdit.Text = "Artist:";
                    btnNew.Text = "New Artist";
                    suggestions = eManager.Artists;
                    break;
            }
            suggestionType = type;
            foreach (var t in suggestions)
            {
                lstEdit.Items.Add(t);
            }
            UpdateSuggestions();
        }

        private void UpdateSuggestions()
        {
            var suggests = new AutoCompleteStringCollection();
            foreach (var t in lstEdit.Items)
            {
                suggests.Add(t.ToString());
            }
            txtEdit.AutoCompleteCustomSource.Clear();
            txtEdit.AutoCompleteCustomSource = suggests;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lstEdit.SelectedIndex = -1;
            txtEdit.Text = "";
            txtEdit.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEdit.Text.Trim())) return;
            if (lstEdit.SelectedIndex > -1)
            {
                lstEdit.Items.RemoveAt(lstEdit.SelectedIndex);
            }
            var bExists = lstEdit.Items.Cast<object>().Any(t => t.ToString().ToLowerInvariant().Equals(txtEdit.Text.Trim().ToLowerInvariant()));
            if (!bExists)
            {
                lstEdit.Items.Add(txtEdit.Text);
            }
            UpdateSuggestions();
            btnNew_Click(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstEdit.SelectedIndex < 0) return;
            lstEdit.Items.RemoveAt(lstEdit.SelectedIndex);
            UpdateSuggestions();
            txtEdit.Text = "";
            txtEdit.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EventManagerEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            var suggestions = (from object t in lstEdit.Items select t.ToString()).ToList();
            switch (suggestionType)
            {
                default:
                    eManager.Bands = suggestions;
                    eManager.UpdateBandSuggestions();
                    break;
                case 1:
                    eManager.Songs = suggestions;
                    eManager.UpdateSongSuggestions();
                    break;
                case 2:
                    eManager.Artists = suggestions;
                    eManager.UpdateArtistSuggestions();
                    break;
            }
        }

        private void lstEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = lstEdit.SelectedIndex > -1;
            if (lstEdit.SelectedIndex > -1)
            {
                txtEdit.Text = lstEdit.Items[lstEdit.SelectedIndex].ToString();
            }
            //txtEdit.Text = lstEdit.SelectedIndex > -1 ? lstEdit.Items[lstEdit.SelectedIndex].ToString() : "";
        }

        private void txtEdit_TextChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = txtEdit.Text.Length > 0;
        }

        private void EventManagerEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnClose_Click(sender, e);
            }
        }
    }
}
