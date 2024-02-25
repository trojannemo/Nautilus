using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Nautilus.x360;

namespace Nautilus
{
    public partial class QuickDTAEditor : Form
    {
        private RSAParams signature;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private string con;

        public QuickDTAEditor(string input_con = "")
        {
            InitializeComponent();
            Tools = new NemoTools();
            Parser = new DTAParser();
            toolTip1.SetToolTip(lstLog, "This is the application log. Right click to export");
            InitLog();
            con = input_con;
        }

        private void InitLog()
        {
            Log("Welcome to the Quick DTA Editor");
            Log("Drag a CON file to this form or use the 'Open file' button");
            Log("I will extract and open the DTA file for you in Notepad");
            Log("Edit whatever you need to edit, then exit out of Notepad and click Yes to save the changes");
            Log("I will replace the DTA file in CON file with the modified one");
            Log("Ready to begin");
        }

        private void Log(string message)
        {
            if (lstLog.InvokeRequired)
            {
                lstLog.Invoke(new MethodInvoker(() => lstLog.Items.Add(message)));
                lstLog.Invoke(new MethodInvoker(() => lstLog.SelectedIndex = lstLog.Items.Count - 1));
            }
            else
            {
                lstLog.Items.Add(message);
                lstLog.SelectedIndex = lstLog.Items.Count - 1;
            }
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (picWorking.Visible) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            try
            {
                if (VariousFunctions.ReadFileType(files[0]) != XboxFileType.STFS)
                {
                    Log("That's not a valid file to drop here, only CON / LIVE files are allowed");
                }
                else
                {
                    DoDTA(files[0]);
                }
            }
            catch (Exception ex)
            {
                Log("There was a problem accessing that file");
                Log("The error says: " + ex.Message);
            }
        }

        private void DoDTA(string file)
        {
            con = file;
            picWorking.Visible = true;
            lstLog.Cursor = Cursors.WaitCursor;
            menuStrip1.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void ErrorOut()
        {
            Log("Nothing was changed");
            Log("Ready");
            lstLog.Cursor = Cursors.Default;
            picWorking.Visible = false;
            menuStrip1.Enabled = true;
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
        }
        
        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }
        
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("qde");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }
        
        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }

        private void clearLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
            InitLog();
        }
        
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Tools.CurrentFolder = Path.GetDirectoryName(con);
            Log("Reading CON file " + Path.GetFileName(con));
            
            var dta = Path.GetTempPath() + "temp_dta.txt";
            var backup = con + "_backup";
            Tools.DeleteFile(backup);

            if (!Parser.ExtractDTA(con))//grab byte[] content of DTA
            {
                Log("Error extracting DTA file");
                ErrorOut();
                return;
            }
            if (!Parser.WriteDTAToFile(dta)) //write it out to file
            {
                Log("Error extracting DTA file");
                ErrorOut();
                return;
            }

            var song = new STFSPackage(con);
            if (backUpCONFile.Checked)
            {
                Log("Found songs.dta file, making a backup of your file before starting");
                Log("THIS STEP MAY TAKE A WHILE. DON'T CLOSE ME DOWN!");

                song.CloseIO();
                File.Copy(con, backup);
                song = new STFSPackage(con);
            }

            var xDTA = song.GetFile("/songs/songs.dta");
            Log("Opening DTA file...");

            var process = Process.Start(dta);
            Log("DTA file is being edited by the user ... waiting...");

            do
            {
                //wait while user has DTA file opened
            } while (!process.HasExited);
            
            process.Dispose();

            Log("DTA file closed by user, continuing...");
            Log("Replacing old DTA file with modified DTA file");
            Log("THIS STEP MAY TAKE A WHILE. DON'T CLOSE ME DOWN!");
            
            if (!xDTA.Replace(dta))
            {
                Log("Error replacing DTA file with modified one");
                Tools.DeleteFile(backup);
                ErrorOut();
                return;
            }

            Log("Replaced DTA file successfully");
            song.Header.MakeAnonymous();
            song.Header.ThisType = PackageType.SavedGame;

            var success = false;
            try
            {
                Log("Saving changes to pack ... sit tight");
                Log("THIS STEP MAY TAKE A WHILE. DON'T CLOSE ME DOWN!");
                
                signature = new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
                song.RebuildPackage(signature);
                song.FlushPackage(signature);
                song.CloseIO();
                success = true;
            }
            catch (Exception ex)
            {
                Log("There was an error: " + ex.Message);
                song.CloseIO();
            }

            if (success)
            {
                Log("Trying to unlock CON file");
                if (Tools.UnlockCON(con))
                {
                    Log("Unlocked CON file successfully");
                }
                else
                {
                    Log("Error unlocking CON file");
                    success = false;
                }
            }

            if (success)
            {
                if (Tools.SignCON(con))
                {
                    Log("CON file signed successfully");
                }
                else
                {
                    Log("Error signing CON file");
                    success = false;
                }
            }

            Tools.DeleteFile(dta);

            Log(success ? "Process completed successfully!" : "Something went wrong along the way, sorry!");
            if (success) return;
            if (!backUpCONFile.Checked) return;
            Log("Restoring backup file");
            Tools.DeleteFile(con);
            Log(Tools.MoveFile(backup, con) ? "Backup file restored successfully, nothing was lost" : "Sorry, there was an error restoring the backup file");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lstLog.Cursor = Cursors.Default;
            picWorking.Visible = false;
            menuStrip1.Enabled = true;
            Log("Ready");
        }

        private void QuickDTAEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picWorking.Visible) return;
            MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Tools.CurrentFolder,
                Title = "Select CON / LIVE file to open",
                Multiselect = false
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (string.IsNullOrWhiteSpace(ofd.FileName)) return;

            if (VariousFunctions.ReadFileType(ofd.FileName) == XboxFileType.STFS)
            {
                DoDTA(ofd.FileName);
            }
            else
            {
                Log("That's not a valid STFS package, only CON / LIVE files are allowed");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void backUpCONFile_Click(object sender, EventArgs e)
        {
            Log("CON file " + (backUpCONFile.Checked ? "will" : "will not") + " be backed up before editing");
        }

        private void QuickDTAEditor_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(con) && File.Exists(con))
            {
                DoDTA(con);
            }
        }
    }
}