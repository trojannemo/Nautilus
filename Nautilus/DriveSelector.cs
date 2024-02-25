using System;
using System.IO;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class DriveSelector : Form
    {
        private readonly RBtoUSB parent;

        public DriveSelector(RBtoUSB xparent)
        {
            InitializeComponent();
            parent = xparent;
        }

        private void DriveSelector_Shown(object sender, EventArgs e)
        {
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                if (drive.Name == "C:\\") continue;
                if (drive.DriveType != DriveType.Fixed && drive.DriveType != DriveType.Removable && drive.DriveType != DriveType.Network) continue;
                if (!drive.IsReady) continue;
                var entry = new ListViewItem(drive.Name);
                entry.SubItems.Add(GetCustomDriveName(drive.Name, drive.VolumeLabel));
                entry.SubItems.Add(GetFormattedSize(drive.TotalSize));
                entry.SubItems.Add(GetFormattedSize(drive.AvailableFreeSpace));
                lstDrives.Items.Add(entry);
            }
        }

        private static string GetCustomDriveName(string drive, string label)
        {
            if (!File.Exists(drive + "name.txt")) return label;
            try
            {
                var sr = new StreamReader(drive + "name.txt");
                var name = sr.ReadLine();
                sr.Dispose();
                return string.IsNullOrWhiteSpace(name) ? label : name;
            }
            catch (Exception)
            {
                return label;
            }
        }

        private static string GetFormattedSize(long bytes)
        {
            const long MB = 1048576;
            const long GB = 1073741824;
            const long TB = 1099511627776;
            if (bytes > TB)
            {
                return Math.Round((double)bytes / TB, 2) + " TB";
            }
            if (bytes > GB)
            {
                return Math.Round((double)bytes / GB, 2) + " GB";
            }
            if (bytes > MB)
            {
                return Math.Round((double)bytes / MB, 2) + " MB";
            }
            return Math.Round((double)bytes / 1024, 2) + " KB";
        }

        private void lstDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            parent.DriveLetter = lstDrives.SelectedItems[0].SubItems[0].Text;
            Dispose();
        }

        private void DriveSelector_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                Dispose();
            }
        }
    }
}
