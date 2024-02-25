using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Nautilus.Properties;
using Nautilus.x360;
using Un4seen.Bass;
using System.Drawing;

namespace Nautilus
{
    partial class VolumeNormalizer : Form
    {
        private static string inputDir;
        private static List<string> inputFiles;
        private DateTime endTime;
        private DateTime startTime;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private string attenuationValues;

        // Our target loudness that we want. Normally -6.4 dB.
        private double targetDB = -6.4;


        public VolumeNormalizer(Color ButtonBackColor, Color ButtonTextColor)
        {
            InitializeComponent();

            //initialize
            Tools = new NemoTools();
            Parser = new DTAParser();

            inputFiles = new List<string>();
            inputDir = Application.StartupPath;

            if (!Directory.Exists(inputDir))
            {
                Directory.CreateDirectory(inputDir);
            }

            var formButtons = new List<Button> { btnReset, btnRefresh, btnFolder, btnBegin };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }

            toolTip1.SetToolTip(btnBegin, "Click to begin process");
            toolTip1.SetToolTip(btnFolder, "Click to select the input folder");
            toolTip1.SetToolTip(btnRefresh, "Click to refresh if the contents of the folder have changed");
            toolTip1.SetToolTip(txtFolder, "This is the working directory");
            toolTip1.SetToolTip(lstLog, "This is the application log. Right click to export");
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            //if user selects new folder, assign that value
            //if user cancels or selects same folder, this forces the text_changed event to run again
            var tFolder = txtFolder.Text;

            var folderUser = new FolderBrowserDialog
            {
                SelectedPath = txtFolder.Text,
                Description = "Select the folder where your CON files are",
            };
            txtFolder.Text = "";
            var result = folderUser.ShowDialog();
            txtFolder.Text = result == DialogResult.OK ? folderUser.SelectedPath : tFolder;
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

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            if (picWorking.Visible) return;
            inputFiles.Clear();

            if (string.IsNullOrWhiteSpace(txtFolder.Text))
            {
                btnRefresh.Visible = false;
            }
            btnRefresh.Visible = true;

            if (txtFolder.Text != "")
            {
                Tools.CurrentFolder = txtFolder.Text;
                Log("");
                Log("Reading input directory ... hang on");

                try
                {
                    var inFiles = Directory.GetFiles(txtFolder.Text);
                    foreach (var file in inFiles)
                    {
                        try
                        {
                            if (VariousFunctions.ReadFileType(file) == XboxFileType.STFS)
                            {
                                inputFiles.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Path.GetExtension(file) != "") continue;
                            Log("There was a problem accessing file " + Path.GetFileName(file));
                            Log("The error says: " + ex.Message);
                        }
                    }
                    if (!inputFiles.Any())
                    {
                        Log("Did not find any CON files ... try a different directory");
                        Log("You can also drag and drop CON files here");
                        Log("Ready");
                        btnBegin.Visible = false;
                        btnRefresh.Visible = true;
                    }
                    else
                    {
                        Log("Found " + inputFiles.Count + " CON " + (inputFiles.Count > 1 ? "files" : "file"));
                        Log("Ready to begin");
                        btnBegin.Visible = true;
                        btnRefresh.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Log("There was an error: " + ex.Message);
                }
            }
            else
            {
                btnBegin.Visible = false;
                btnRefresh.Visible = false;
            }
            txtFolder.Focus();
        }

        private bool ProcessFiles()
        {
            var counter = 0;
            var success = 0;
            foreach (var file in inputFiles.Where(File.Exists).TakeWhile(file => !backgroundWorker1.CancellationPending))
            {
                try
                {
                    if (VariousFunctions.ReadFileType(file) != XboxFileType.STFS) continue;

                    try
                    {
                        counter++;

                        Parser.ExtractDTA(file);
                        Parser.ReadDTA(Parser.DTA);

                        if (Parser.Songs.Count > 1)
                        {
                            Log("File " + Path.GetFileName(file) + " is a pack, try dePACKing first, skipping...");
                            continue;
                        }
                        if (!Parser.Songs.Any())
                        {
                            Log("There was an error processing the songs.dta file");
                            continue;
                        }

                        Log("Song #" + counter + " is " + Parser.Songs[0].Artist + " - " + Parser.Songs[0].Name);

                        string internal_name = Parser.Songs[0].InternalName;
                        attenuationValues = "";

                        if (!chkRestore.Checked)
                        {
                            // We are going to output files to a folder in order to process and remove them after.
                            string songfolder = Tools.CurrentFolder + "\\" + internal_name + "_ext\\";

                            if (!Directory.Exists(songfolder))
                            {
                                Directory.CreateDirectory(songfolder);
                            }

                            var xPackage = new STFSPackage(file);
                            if (!xPackage.ParseSuccess)
                            {
                                throw new Exception("Failed to parse '" + Path.GetFileName(file) + "'");
                            }

                            if (Parser.Songs[0].AttenuationValues.Trim() != Parser.Songs[0].OriginalAttenuationValues.Trim())
                            {
                                Log("Original volume levels will be used for processing.");
                            }

                            var xMOGG = xPackage.GetFile("songs/" + internal_name + "/" + internal_name + ".mogg");
                            if (xMOGG != null)
                            {
                                // We are down mixing the audio in order to determine the loudness later.
                                xPackage.CloseIO();
                                DownMixAudio(file, songfolder);
                            }
                            else
                            {
                                xPackage.CloseIO();
                                throw new Exception("Could not find an audio file!");
                            }

                            xPackage.CloseIO();

                            double attenuationOffset = CalculateVolumeOffset(songfolder + "song.ogg");

                            Tools.DeleteFile(songfolder + "song.ogg");
                            Tools.DeleteFolder(songfolder);

                            // Check to see if audio needs to be increased in volume.
                            if (attenuationOffset > 0)
                            {
                                // Backup the CON file first if we want a backup.
                                if (chkBackupAudio.Checked)
                                {
                                    File.Copy(file, file + "_clean");
                                }

                                AdjustAudio(file, attenuationOffset);

                                // Set the offset to 0, since we don't need to change it anymore.
                                attenuationOffset = 0;

                            }


                            // Offset Attenuation values
                            var values = Parser.Songs[0].OriginalAttenuationValues.Trim().Split(' ');

                            foreach (var value in values)
                            {
                                double preFinal = double.Parse(value) - attenuationOffset;
                                attenuationValues += FormatDB(preFinal) + " ";
                            }

                            // Trim the white space off the end of the string.
                            attenuationValues = attenuationValues.Trim();

                        }
                        else
                        {
                            // Restore attenuation values to the original levels if requested
                            attenuationValues = Parser.Songs[0].OriginalAttenuationValues.Trim();

                            if (Parser.Songs[0].AttenuationValues.Trim() != Parser.Songs[0].OriginalAttenuationValues.Trim())
                            {
                                Log("Restoring original volume levels for the song...");
                            }
                            else
                            {
                                Log("Volume levels are not different than the original.");
                            }
                        }

                        WriteDTA();

                        WriteCON(file);

                        success++;

                    }
                    catch (Exception ex)
                    {
                        Log("There was an error: " + ex.Message);
                        Log("Attempting to continue with the next file");
                    }
                }
                catch (Exception ex)
                {
                    Log("There was a problem accessing that file");
                    Log("The error says: " + ex.Message);
                }
            }
            Log("Successfully processed " + success + " of " + counter + " files");
            return true;
        }

        private void WriteCON(string con)
        {
            // This function is prety much the same steps as the QuickDTAEditor.
            // The only real difference is that we try to continue with other files
            // instead of aborting everything on a problem.

            Tools.CurrentFolder = Path.GetDirectoryName(con);

            var dta = Path.GetTempPath() + "temp_dta.txt";
            var backup = con + "_backup";

            Tools.DeleteFile(backup);

            Log("Backing up file before modifying it...");
            File.Copy(con, backup);

            // Make a new package
            var song = new STFSPackage(con);
            var xDTA = song.GetFile("/songs/songs.dta");

            if (!xDTA.Replace(dta))
            {
                Tools.DeleteFile(con);
                Log(Tools.MoveFile(backup, con) ? "Restored from backup." : "Failed to restore from backup!");
                throw new Exception("Error replacing DTA file with modified one!");
            }
            Log("Replaced DTA file successfully!");

            song.Header.MakeAnonymous();
            song.Header.ThisType = PackageType.SavedGame;

            Log("Saving changes to file...");
            RSAParams signature = new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
            song.RebuildPackage(signature);
            song.FlushPackage(signature);
            song.CloseIO();

            if (!Tools.UnlockCON(con))
            {
                Tools.DeleteFile(con);
                Log(Tools.MoveFile(backup, con) ? "Restored from backup." : "Failed to restore from backup!");
                throw new Exception("Failed to unlock CON file!");
            }

            if (!Tools.SignCON(con))
            {
                Tools.DeleteFile(con);
                Log(Tools.MoveFile(backup, con) ? "Restored from backup." : "Failed to restore from backup!");
                throw new Exception("Failed to sign CON file!");
            }

            Tools.DeleteFile(dta);

            Log("Process completed successfully! Removing backup...");
            Tools.DeleteFile(backup);

            return;

        }

        private void WriteDTA()
        {
            // We write the DTA, and patch in the changed volume numbers on the fly.
            Log("Writing changes to DTA...");

            var dta = Path.GetTempPath() + "temp_dta.txt";

            Tools.DeleteFile(dta);

            if (!Parser.WriteDTAToFile(dta))
            {
                throw new Exception("Error writing temporary DTA file!");
            }

            var dtaLines = new List<string>();
            var dtaLinesNew = new List<string>();

            using (var streamReader = new StreamReader(dta))
            {
                int indexOfVol = 100;
                int indexOfEnd = 0;
                bool valuesExist = false;

                do
                {
                    dtaLines.Add(streamReader.ReadLine());
                } while (!streamReader.EndOfStream);

                // We want to check if the original values exist before we write them later.
                foreach (var item in dtaLines)
                {
                    if (item.Contains(";OriginalAttenuationValues="))
                    {
                        valuesExist = true;
                    }
                }

                // Go through all of the lines of the DTA
                for (int i = 0; i < dtaLines.Count; i++)
                {
                    var line = dtaLines[i];

                    if (line.Contains("'vols'"))
                    {
                        if (!line.Contains("(vols"))
                        {
                            // We don't need this line, we need the next one.
                            dtaLinesNew.Add(line);
                            i++;
                            line = dtaLines[i];
                        }

                        // Find when the volume starts.
                        for (int j = 0; j < line.Length; j++)
                        {
                            if (Char.IsDigit(line[j]) && j < indexOfVol)
                            {
                                indexOfVol = j;
                            }
                        }

                        // If the first number is negative, we want that.
                        if (line[indexOfVol - 1] == '-')
                        {
                            indexOfVol--;
                        }

                        // Find the closing )
                        indexOfEnd = line.IndexOf(')');

                        // Write the modified line.
                        line = line.Substring(0, indexOfVol) + attenuationValues + line.Substring(indexOfEnd);

                    }

                    // Write line
                    dtaLinesNew.Add(line);

                    // We write our own line to the DTA file here.
                    if (line.Contains(";ExpertOnly"))
                    {
                        if (!valuesExist)
                        {
                            dtaLinesNew.Add(";OriginalAttenuationValues=" + Parser.Songs[0].OriginalAttenuationValues.Trim());
                        }
                    }
                }
                streamReader.Close();
            }

            // Actually output the file.
            var streamWriter = new StreamWriter(dta, false);
            using (streamWriter)
            {
                foreach (var line in dtaLinesNew)
                {
                    streamWriter.WriteLine(line);
                }
                streamWriter.Close();
            }
        }

        private void DownMixAudio(string CON, string folder)
        {
            if (backgroundWorker1.CancellationPending) return;
            string ogg = folder + "song.ogg";
            Log("Downmixing audio file to stereo file...");
            var Splitter = new MoggSplitter();
            var mixed = Splitter.DownmixMogg(CON, ogg, true, MoggSplitter.MoggSplitFormat.OGG, 3, "allstems");
            foreach (var error in Splitter.ErrorLog)
            {
                throw new Exception(error);
            }
            Log(mixed && File.Exists(ogg) ? "Success" : "Failed");
        }

        private void AdjustAudio(string CON, double volume)
        {
            // TODO
        }


        private double CalculateVolumeOffset(string ogg)
        {

            // How we are determining the average loudness is to grab the top percentage of
            // levels, and then averaging them out. We grab a percentage of the levels because
            // we can ignore the most quiet parts of the song, as well as some of the loudest parts.
            int topPercentToKeep = 80;
            int topPercentToStrip = 15;

            // We will remove some of the levels from the beginning of the song
            // to compensate for the count-in of the song.
            int secondsToRemoveFromStart = 4;

            Log("Determining loudness of song...");

            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);

            var BassStream = Bass.BASS_StreamCreateFile(ogg, 0, 0, BASSFlag.BASS_STREAM_DECODE);
            var level = new float[1];
            List<double> dBLevels = new List<double>();

            // Get the audio levels of the OGG file.
            while (Bass.BASS_ChannelGetLevel(BassStream, level, 1, BASSLevel.BASS_LEVEL_STEREO))
            {

                double levelDouble = Convert.ToDouble(level[0]);

                // Translate the level to dB.
                double dblevel = levelDouble > 0 ? 20 * Math.Log10(levelDouble) : -1000;

                // We want to not use any section of the song that is too quiet in our checking.
                // This also has the added benefit of ignoring any silence in the song.
                if (dblevel > -24)
                {
                    dBLevels.Add(dblevel);
                }

                //Log(dblevel.ToString());
            }
            Bass.BASS_StreamFree(BassStream);
            Bass.BASS_Free();

            // Remove the beginning of the song
            dBLevels.RemoveRange(0, secondsToRemoveFromStart);

            // Sort by volume
            dBLevels.Sort();

            // Calculate and remove the amount of levels requested.
            int countToKeep = (int)Math.Floor(dBLevels.Count() * (topPercentToKeep * 0.01));
            int countToStrip = (int)Math.Floor(dBLevels.Count() * (topPercentToStrip * 0.01));

            dBLevels.RemoveRange(0, dBLevels.Count() - countToKeep);
            dBLevels.RemoveRange(dBLevels.Count() - countToStrip, countToStrip);

            /*
            foreach (var item in dBLevels)
            {
                Log(item.ToString());
            }
            */

            double dBAverage = dBLevels.Average();
            double offset = dBAverage - targetDB;


            // Strip most of the decimal places, because we don't need to be *that* precise.
            offset = double.Parse(FormatDB(offset));

            Log("Average dB of song is: " + FormatDB(dBAverage) + " dB, " + FormatDB(offset) + " dB away from the target dB of: " + targetDB.ToString() + ".");

            return offset;
        }

        string FormatDB(double level)
        {
            // We do this in order to not deal with *very* small numbers in our string output.
            if (level > -0.01 && level < 0.01)
            {
                return "0.00";
            }
            else
            {
                // Here we limit the length of the output string to two decimal places.
                string levelString = level.ToString();
                string _tempString = levelString.Split('.')[0] + "." + levelString.Split('.')[1].Substring(0, Math.Min(2, levelString.Split('.')[1].Length));

                return _tempString;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var tFolder = txtFolder.Text;
            txtFolder.Text = "";
            txtFolder.Text = tFolder;
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (picWorking.Visible) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (btnReset.Visible)
            {
                btnReset.PerformClick();
            }
            if (VariousFunctions.ReadFileType(files[0]) == XboxFileType.STFS)
            {
                txtFolder.Text = Path.GetDirectoryName(files[0]);
                Tools.CurrentFolder = txtFolder.Text;
            }
            else
            {
                MessageBox.Show("That's not a valid file to drop here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("vn");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }

        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Log("Resetting...");
            inputFiles.Clear();
            EnableDisable(true);
            btnBegin.Visible = true;
            btnBegin.Enabled = true;
            btnReset.Visible = false;
            btnFolder.Enabled = true;
            btnRefresh.Enabled = true;
            btnRefresh.PerformClick();
        }

        private void EnableDisable(bool enabled)
        {
            btnFolder.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            menuStrip1.Enabled = enabled;
            txtFolder.Enabled = enabled;
            picWorking.Visible = !enabled;
            lstLog.Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
            Cursor = lstLog.Cursor;
            chkRestore.Enabled = enabled;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {

            if (btnBegin.Text == "Cancel")
            {
                backgroundWorker1.CancelAsync();
                Log("User cancelled process... Stopping as soon as possible.");
                btnBegin.Enabled = false;
                return;
            }
            else
            {

                string _tempPrepString = "This will modify all of the CON files in this folder.";

                if (radioAllowRender.Checked)
                {
                    _tempPrepString += "\n\nIf the volume needs to be increased, audio will be modified!";
                }

                if (MessageBox.Show(_tempPrepString, "Are you sure you want to proceed?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            startTime = DateTime.Now;
            Tools.CurrentFolder = txtFolder.Text;
            EnableDisable(false);

            targetDB = Double.Parse(numTargetValue.Value.ToString());

            try
            {
                var files = Directory.GetFiles(txtFolder.Text);
                if (files.Count() != 0)
                {
                    btnBegin.Text = "Cancel";
                    toolTip1.SetToolTip(btnBegin, "Click to cancel process");
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    Log("No files found... There's nothing to do");
                    EnableDisable(true);
                }
            }
            catch (Exception ex)
            {
                Log("Error retrieving files to process");
                Log("The error says:" + ex.Message);
                EnableDisable(true);
            }
        }

        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void VolumeNormalizerPrep_Resize(object sender, EventArgs e)
        {
            btnRefresh.Left = txtFolder.Left + txtFolder.Width - btnRefresh.Width;
            btnBegin.Left = txtFolder.Left + txtFolder.Width - btnBegin.Width;
            picWorking.Left = (Width / 2) - (picWorking.Width / 2);
        }

        private void VolumeNormalizerPrep_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picWorking.Visible)
            {
                //Tools.DeleteFolder(Application.StartupPath + "\\phaseshift\\");
                return;
            }
            MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        private void VolumeNormalizerPrep_Shown(object sender, EventArgs e)
        {
            Log("Welcome to " + Text);
            Log("Drag and drop the CON /LIVE file(s) to be processed here");
            Log("Or click 'Change Input Folder' to select the files");
            Log("Ready to begin");
            txtFolder.Text = inputDir;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (ProcessFiles()) return;
            Log("There was an error processing the files... Stopping here.");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Log("Done!");
            endTime = DateTime.Now;
            var timeDiff = endTime - startTime;
            Log("Process took " + timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute" : " minutes") + " and " + (timeDiff.Minutes == 0 && timeDiff.Seconds == 0 ? "1 second" : timeDiff.Seconds + " seconds"));
            Log("Click 'Reset' to start again or just close me down");

            btnReset.Enabled = true;
            btnReset.Visible = true;
            picWorking.Visible = false;
            lstLog.Cursor = Cursors.Default;
            Cursor = lstLog.Cursor;
            toolTip1.SetToolTip(btnBegin, "Click to begin");
            btnBegin.Text = "&Begin";
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