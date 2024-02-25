using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class ThemeSelector : Form
    {
        private readonly List<string> ThemeFiles;
        private readonly string ThemesFolder;
        private readonly Visualizer Visualizer;
        private bool NoThemesFound;

        public ThemeSelector(Visualizer ParentForm)
        {
            InitializeComponent();
            Visualizer = ParentForm;
            ThemeFiles = new List<string>();
            ThemesFolder = Application.StartupPath + "\\res\\vis_themes\\";
        }

        private void Themes_Shown(object sender, EventArgs e)
        {
            Left = Visualizer.Left - Width-10;
            Top = Visualizer.Top + (int)(Visualizer.Height*0.5);

            var files = Directory.GetFiles(ThemesFolder, "*.png");
            if (!files.Any())
            {
                MessageBox.Show("No themes found!\nMake sure you're using PNG files and naming the files:\nname_overlay.png and name_button.png",
                    "Visualizer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                NoThemesFound = true;
                Close();
                return;
            }
            foreach (var name in from file in files.Where(file => Path.GetFileName(file).ToLowerInvariant().Contains("_overlay.png")) where File.Exists(file.ToLowerInvariant().Replace("_overlay.png", "_button.png")) select Path.GetFileName(file).ToLowerInvariant().Replace("_overlay.png", ""))
            {
                ThemeFiles.Add(name);
            }

            ThemeFiles.Sort();
            lstThemes.Items.Clear();
            lstThemes.Items.Add("(no theme)");
            
            foreach (var theme in ThemeFiles)
            {
                lstThemes.Items.Add(theme);
            }
            lstThemes.SelectedIndex = 0;

            if (lstThemes.Items.Count == 0)
            {
                MessageBox.Show("No themes found!\nMake sure you're using PNG files and naming the files:\nname_overlay.png and name_button.png",
                    "Visualizer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                NoThemesFound = true;
                Close();
                return;
            }

            if (string.IsNullOrWhiteSpace(Visualizer.ThemeName)) return;

            for (var i = 0; i < lstThemes.Items.Count; i++)
            {
                if (!String.Equals(lstThemes.Items[i].ToString(), Visualizer.ThemeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                lstThemes.SelectedIndex = i;
                return;
            }
        }

        private void lstThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            DetermineTheme();
        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            UpdateTheme();
            Close();
        }

        private void DetermineTheme()
        {
            if (lstThemes.SelectedIndex == 0)
            {
                Visualizer.UseOverlay = false;
            }
            else
            {
                UpdateTheme();
            }
        }

        private void UpdateTheme()
        {
            Visualizer.ThemeName = lstThemes.Items[lstThemes.SelectedIndex].ToString();
            Visualizer.UpdateTheme();
        }

        private void Themes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NoThemesFound) return;
            DetermineTheme();
        }
    }
}
