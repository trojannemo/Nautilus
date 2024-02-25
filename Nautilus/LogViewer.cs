using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Nautilus.Properties;

namespace Nautilus
{
    public partial class LogViewer : Form
    {
        private readonly NemoTools Tools;
        private readonly string EventsFolder;
        private const string logheader = "//Event Log created by Event Manager. Do not modify this file manually.";
        private readonly List<string> Logs;

        public LogViewer()
        {
            InitializeComponent();
            Tools = new NemoTools();

            EventsFolder = Application.StartupPath + "\\bin\\events\\";
            Logs = new List<string>();

            if (!Directory.Exists(EventsFolder))
            {
                NothingToShow();
            }
        }

        private void exitTool_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NothingToShow()
        {
            MessageBox.Show("No logs found", "Nothing to show", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
        private void LogViewer_Shown(object sender, EventArgs e)
        {
            var logs = Directory.GetFiles(EventsFolder, "*.log");
            if (!logs.Any())
            {
                NothingToShow();
            }

            foreach (var log in logs)
            {
                var sr = new StreamReader(log);
                if (sr.ReadLine() != logheader)
                {
                    sr.Dispose();
                    continue;
                }
                var date = Tools.GetConfigString(sr.ReadLine());
                var count = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));

                if (count <= 0)
                {
                    sr.Dispose();
                    continue;
                }

                Logs.Add(log);
                cboLogs.Items.Add(date);
                sr.Dispose();
            }

            picWorking.Visible = false;
        }

        private void cboLogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            picWorking.Visible = true;
            lstLogs.Items.Clear();
            var index = cboLogs.SelectedIndex;
            var bands = new List<string>();
            var songs = new List<string>();
            var artists = new List<string>();

            if (!File.Exists(Logs[index]))
            {
                MessageBox.Show("Can't find that log file", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var sr = new StreamReader(Logs[index]);
            try
            {
                if (sr.ReadLine() != logheader)
                {
                    MessageBox.Show("That log file is corrupt, can't use it", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    sr.Dispose();
                    return;
                }
                sr.ReadLine();
                var count = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));

                if (count <= 0)
                {
                    MessageBox.Show("That log file has no performances logged", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    sr.Dispose();
                    return;
                }

                var counter = 0;
                for (var i = 0; i < count; i++)
                {
                    var line = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(line.Trim())) continue;

                    counter++;
                    var performance = line.Split(new[] {','});
                    var entry = new ListViewItem(counter.ToString(CultureInfo.InvariantCulture));
                    entry.SubItems.Add(performance[0].Trim());
                    entry.SubItems.Add(performance[1].Trim());
                    entry.SubItems.Add(performance[2].Trim());
                    entry.SubItems.Add(performance[3].Trim() == "N/A" ? "No" : "Yes");
                    entry.SubItems.Add(performance[3].Trim());
                    entry.SubItems.Add(performance[4].Trim());
                    entry.SubItems.Add(performance[5].Trim());
                    entry.SubItems.Add(performance[6].Trim() == "0" ? "No" : "Yes (" + performance[6].Trim() + ")");
                    entry.SubItems.Add(performance[7].Trim().Contains("True") ? "Yes" : "No");
                    lstLogs.Items.Add(entry);

                    if (!bands.Contains(performance[0].ToLowerInvariant()))
                    {
                        bands.Add(performance[0].ToLowerInvariant());
                    }
                    if (!songs.Contains(performance[1].ToLowerInvariant()))
                    {
                        songs.Add(performance[1].ToLowerInvariant());
                    }
                    if (!artists.Contains(performance[2].ToLowerInvariant()))
                    {
                        artists.Add(performance[2].ToLowerInvariant());
                    }
                }

                sr.Dispose();
            }
            catch (Exception)
            {
                sr.Dispose();
            }

            lblStats.Text = "Total Performances: " + lstLogs.Items.Count + " | Unique Bands: " + bands.Count + " | Unique Songs: " +
                            songs.Count + " | Unique Artists: " + artists.Count;

            exportLogTool.Enabled = true;
            picWorking.Visible = false;
        }

        private void exportLogTool_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
                {
                    Title = "Export Log File",
                    FileName = "EventManagerLog_" + cboLogs.Text.Replace(",", "").Replace(" ", "").Replace("/","") + ".csv",
                    Filter = "CSV Files|*.csv"
                };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            var sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
            try
            {
                sw.WriteLine("\"Band\",\"Song\",\"Artist\",\"Performed?\",\"Time Added\",\"Start Time\",\"Stop Time\",\"Skipped?\",\"Deleted?");
                for (var i = 0; i < lstLogs.Items.Count; i++)
                {
                    sw.WriteLine("\"" + lstLogs.Items[i].SubItems[1].Text + "\",\"" + lstLogs.Items[i].SubItems[2].Text + "\",\"" +
                        lstLogs.Items[i].SubItems[3].Text + "\",\"" + lstLogs.Items[i].SubItems[4].Text + "\",\"" +
                        lstLogs.Items[i].SubItems[5].Text + "\",\"" + lstLogs.Items[i].SubItems[6].Text + "\",\"" +
                        lstLogs.Items[i].SubItems[7].Text + "\",\"" + lstLogs.Items[i].SubItems[8].Text + "\",\"" +
                        lstLogs.Items[i].SubItems[9].Text + "\"");
                }
                sw.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error exporting the log file, sorry", Text, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                sw.Dispose();
            }

            MessageBox.Show("Log exported successfully", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void picPin_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            switch (picPin.Tag.ToString())
            {
                case "pinned":
                    picPin.Image = Resources.unpinned;
                    picPin.Tag = "unpinned";
                    break;
                case "unpinned":
                    picPin.Image = Resources.pinned;
                    picPin.Tag = "pinned";
                    break;
            }
            TopMost = picPin.Tag.ToString() == "pinned";
        }
    }
}
