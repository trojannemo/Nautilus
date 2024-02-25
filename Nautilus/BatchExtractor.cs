using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Nautilus.Properties;
using Nautilus.x360;
using NautilusFREE;

namespace Nautilus
{
    public partial class BatchExtractor : Form
    {
        private readonly List<string> inputFiles;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private STFSPackage xPackage;
        private readonly string configFile;
        private static Color mMenuBackground;

        public BatchExtractor(Color ButtonBackColor, Color ButtonTextColor)
        {
            InitializeComponent();
            Tools = new NemoTools();
            Parser = new DTAParser();
            inputFiles = new List<string>();
            mMenuBackground = menuStrip1.BackColor;
            menuStrip1.Renderer = new DarkRenderer();
            var formButtons = new List<Button> { btnRefresh, btnFolder, btnBegin, btnSelect, btnDeselect, btnConverter };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }

            configFile = Application.StartupPath + "\\bin\\config\\extractor.config";
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
            inputFiles.Clear();

            if (txtFolder.Text != "")
            {
                Tools.CurrentFolder = txtFolder.Text;
                Log("");
                Log("Reading input directory ... hang on");
                EnableDisable(false);
                folderScanner.RunWorkerAsync();
            }
            else
            {
                btnBegin.Visible = false;
                btnRefresh.Visible = false;
                txtFolder.Focus();
            }
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

        private void btnFolder_Click(object sender, EventArgs e)
        {
            //if user selects new folder, assign that value
            //if user cancels or selects same folder, this forces the text_changed event to run again
            var tFolder = txtFolder.Text;            
            var ofd = new FolderPicker
            {
                InputPath = tFolder,
                Title = "Select folder where your source files are",
            };
            if (ofd.ShowDialog(IntPtr.Zero) == true)
            {
                txtFolder.Text = ofd.ResultPath;
                Tools.CurrentFolder = txtFolder.Text;
            }
            else
            {
                txtFolder.Text = tFolder;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var tFolder = txtFolder.Text;
            txtFolder.Text = "";
            txtFolder.Text = tFolder;
        }

        private string arrangeName(string song, string artist, string songid)
        {
            var arranged = "";

            if (songIDToolStripMenuItem.Checked)
            {
                arranged = songid;
            }
            else if (artistSongToolStripMenuItem.Checked)
            {
                arranged = artist + " - " + song;
            }
            else if (artistTheSongToolStripMenuItem.Checked)
            {
                if (artist.Length > 6)
                {
                    if (artist.Substring(0, 4) == "The ")
                    {
                        arranged = artist.Substring(4, artist.Length - 4) + ", The - " + song;
                    }
                    else
                    {
                        arranged = artist + " - " + song;
                    }
                }
                else
                {
                    arranged = artist + " - " + song;
                }
            }
            else if (songArtistToolStripMenuItem.Checked)
            {
                arranged = song + " - " + artist;
            }
            else if (songArtistTheToolStripMenuItem.Checked)
            {
                if (artist.Length > 6)
                {
                    if (artist.Substring(0, 4) == "The ")
                    {
                        arranged = song + " - " + artist.Substring(4, artist.Length - 4) + ", The";
                    }
                    else
                    {
                        arranged = song + " - " + artist;
                    }
                }
                else
                {
                    arranged = song + " - " + artist;
                }
            }

            if (removeSpacesFromFileName.Checked)
            {
                arranged = arranged.Replace(" ", "");
            }
            else if (replaceSpacesWithUnderscores.Checked)
            {
                arranged = arranged.Replace(" ", "_");
            }

            return arranged;
        }

        private void artistSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            artistTheSongToolStripMenuItem.Checked = !artistSongToolStripMenuItem.Checked;
            songArtistToolStripMenuItem.Checked = !artistSongToolStripMenuItem.Checked;
            songArtistTheToolStripMenuItem.Checked = !artistSongToolStripMenuItem.Checked;
            songIDToolStripMenuItem.Checked = !artistSongToolStripMenuItem.Checked;
        }

        private void artistTheSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            artistSongToolStripMenuItem.Checked = !artistTheSongToolStripMenuItem.Checked;
            songArtistToolStripMenuItem.Checked = !artistTheSongToolStripMenuItem.Checked;
            songArtistTheToolStripMenuItem.Checked = !artistTheSongToolStripMenuItem.Checked;
            songIDToolStripMenuItem.Checked = !artistTheSongToolStripMenuItem.Checked;
        }

        private void songArtistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            artistSongToolStripMenuItem.Checked = !songArtistToolStripMenuItem.Checked;
            artistTheSongToolStripMenuItem.Checked = !songArtistToolStripMenuItem.Checked;
            songArtistTheToolStripMenuItem.Checked = !songArtistToolStripMenuItem.Checked;
            songIDToolStripMenuItem.Checked = !songArtistToolStripMenuItem.Checked;
        }

        private void songArtistTheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            artistSongToolStripMenuItem.Checked = !songArtistTheToolStripMenuItem.Checked;
            artistTheSongToolStripMenuItem.Checked = !songArtistTheToolStripMenuItem.Checked;
            songArtistToolStripMenuItem.Checked = !songArtistTheToolStripMenuItem.Checked;
            songIDToolStripMenuItem.Checked = !songArtistTheToolStripMenuItem.Checked;
        }

        private void songIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            artistSongToolStripMenuItem.Checked = !songIDToolStripMenuItem.Checked;
            artistTheSongToolStripMenuItem.Checked = !songIDToolStripMenuItem.Checked;
            songArtistToolStripMenuItem.Checked = !songIDToolStripMenuItem.Checked;
            songArtistTheToolStripMenuItem.Checked = !songIDToolStripMenuItem.Checked;
        }

        private void replaceSpacesWithUnderscoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (replaceSpacesWithUnderscores.Checked)
            {
                removeSpacesFromFileName.Checked = false;
            }
       }

        private void removeSpacesFromFileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (removeSpacesFromFileName.Checked)
            {
                replaceSpacesWithUnderscores.Checked = false;
            }
        }

        private bool ExtractFile(string extension, bool sublevel, string internalname, string rename, bool keep = false)
        {
            if (xPackage == null || !xPackage.ParseSuccess) return false;

            var outputfolder = txtFolder.Text + "\\" + (organizeFilesByType.Checked ? (extension == "mid" ? "midi" : extension) + "_files\\" : "");
            var ext = "." + extension;
            var dir = "songs/" + internalname + (sublevel ? "/gen/" : "/") + internalname;
            var file = dir + (keep ? "_keep" : "") + ext;

            var xfile = xPackage.GetFile(file);
            if (xfile == null)
            {
                Log("Could not find " + extension.ToUpper() + " file inside " + Path.GetFileName(file));
                return false;
            }
            try
            {
                var newfile = outputfolder + rename + (keep && !removekeepFromPNGXBOXFiles.Checked ? "_keep" : "") + ext;
                extension = extension.ToLowerInvariant() == "mid" ? "MIDI" : extension.ToUpper();

                Log("Extracting " + extension + " file " + Path.GetFileName(newfile));

                if (!Directory.Exists(outputfolder))
                {
                    Directory.CreateDirectory(outputfolder);
                }

                Tools.DeleteFile(newfile);
                if (xfile.ExtractToFile(newfile))
                {
                    if (Path.GetExtension(newfile) == ".mogg")
                    {
                        var nautilus = new nTools();
                        nautilus.WriteOutData(nautilus.ObfM(File.ReadAllBytes(newfile)), newfile);
                    }
                    Log("File " + rename + ext + " extracted successfully");
                }
                else
                {
                    Log("Extracting file " + rename + ext + " failed");
                }
            }
            catch (Exception ex)
            {
                Log("There was an error extracting file " + rename + ext);
                Log("The error says: " + ex.Message);
                return false;
            }
            return true;
        }

        private void EnableDisable(bool enabled)
        {
            chkDTA.Enabled = enabled;
            chkPNG.Enabled = enabled;
            chkMIDI.Enabled = enabled;
            chkMOGG.Enabled = enabled;
            chkMILO.Enabled = enabled;
            chkThumbs.Enabled = enabled;
            menuStrip1.Enabled = enabled;
            btnFolder.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            btnSelect.Enabled = enabled;
            btnDeselect.Enabled = enabled;
            txtFolder.Enabled = enabled;
            picWorking.Visible = !enabled;
            lstLog.Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
            Cursor = lstLog.Cursor;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (btnBegin.Text == "Cancel")
            {
                Log("User cancelled process...stopping as soon as possible");
                FileExtractor.CancelAsync();
                btnBegin.Enabled = false;
                return;
            }
            if (!inputFiles.Any()) return;
            EnableDisable(false);
            btnConverter.Visible = false;
            btnBegin.Text = "Cancel";
            toolTip1.SetToolTip(btnBegin, "Click to cancel extracting process");
            FileExtractor.RunWorkerAsync();
        }
        
        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (picWorking.Visible) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            txtFolder.Text = Path.GetDirectoryName(files[0]);
            Tools.CurrentFolder = txtFolder.Text;
        }

        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }
        
        private void chkDTA_CheckedChanged(object sender, EventArgs e)
        {
            var box = (CheckBox) sender;

            btnBegin.Visible = (chkDTA.Checked || chkMIDI.Checked || chkMILO.Checked || chkMOGG.Checked || chkPNG.Checked || chkThumbs.Checked) && inputFiles.Any();

            if (box.Checked && !btnBegin.Visible)
            {
                btnRefresh.PerformClick();
            }
        }
        
        private void BatchExtractor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (picWorking.Visible)
            {
                MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
                return;
            }
            
            //save current directory for next time
            var sw = new StreamWriter(configFile, false);
            sw.WriteLine(txtFolder.Text);
            sw.Dispose();
        }

        private void CheckBoxes(bool check)
        {
            chkDTA.Checked = check;
            chkPNG.Checked = check;
            chkMIDI.Checked = check;
            chkMOGG.Checked = check;
            chkMILO.Checked = check;
            chkThumbs.Checked = check;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            CheckBoxes(true);
        }

        private void btnDeselect_Click(object sender, EventArgs e)
        {
            CheckBoxes(false);
        }

        private void btnConverter_Click(object sender, EventArgs e)
        {
            var folder = txtFolder.Text + "\\" + (organizeFilesByType.Checked ? "png_xbox_files\\" : "");
            var newAlbum = new AdvancedArtConverter(folder, Color.FromArgb(230, 215, 0), Color.White);
            newAlbum.Show();
        }

        private void FileExtractor_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var dtafolder = txtFolder.Text + "\\" + (organizeFilesByType.Checked ? "dta_files\\" : "");
            var dtacount = 0;
            var midicount = 0;
            var moggcount = 0;
            var milocount = 0;
            var pngcount = 0;
            var thumbcount = 0;
            var hasdta = false;
            var rename = "";
            var counter = 0;

            foreach (var file in inputFiles.Where(File.Exists).TakeWhile(file => !FileExtractor.CancellationPending))
            {
                try
                {
                    if (VariousFunctions.ReadFileType(file) != XboxFileType.STFS) continue;
                    if (!Parser.ExtractDTA(file))
                    {
                        Log("Error extracting songs.dta from file: '" + Path.GetFileName(file) + "' ... skipping");
                        continue;
                    }
                    if (!Parser.ReadDTA(Parser.DTA) || !Parser.Songs.Any())
                    {
                        Log("Error reading that songs.dta file ... skipping");
                        continue;
                    }

                    try
                    {
                        xPackage = new STFSPackage(file);
                        if (!xPackage.ParseSuccess)
                        {
                            Log("Error opening file '" + Path.GetFileName(file) + "' ... skipping");
                            xPackage.CloseIO();
                        }
                        else
                        {
                            hasdta = true;

                            Log("CON file '" + Path.GetFileNameWithoutExtension(file) + "' contains " + Parser.Songs.Count + " " + (Parser.Songs.Count == 1 ? "song" : "songs"));
                            for (var i = 0; i < Parser.Songs.Count; i++)
                            {
                                if (FileExtractor.CancellationPending)
                                {
                                    xPackage.CloseIO();
                                    break;
                                }
                                var song = Parser.Songs[i];
                                counter++;
                                Log("Extracting files for song #" + (counter) + ": '" + song.Artist + " - " + song.Name + "'");
                                var songid = song.InternalName;

                                var name = Tools.CleanString(song.Name, true);
                                var artist = Tools.CleanString(song.Artist, true);
                                songid = Tools.CleanString(songid, true);

                                rename = arrangeName(name, artist, songid).Replace("!", "").Replace("'", "");
                                rename = Tools.CleanString(rename, false);

                                if (chkMIDI.Checked && !FileExtractor.CancellationPending)
                                {
                                    if (ExtractFile("mid", false, songid, rename))
                                    {
                                        midicount++;
                                    }
                                }
                                if (chkMOGG.Checked && !FileExtractor.CancellationPending)
                                {
                                    if (ExtractFile("mogg", false, songid, rename))
                                    {
                                        moggcount++;
                                    }
                                }
                                if (chkPNG.Checked && !FileExtractor.CancellationPending)
                                {
                                    if (ExtractFile("png_xbox", true, songid, rename, true))
                                    {
                                        pngcount++;
                                    }
                                }
                                if (!chkMILO.Checked) continue;
                                if (ExtractFile("milo_xbox", true, songid, rename))
                                {
                                    milocount++;
                                }
                            }

                            if (FileExtractor.CancellationPending)
                            {
                                xPackage.CloseIO();
                                break;
                            }
                            var xUpgrade = xPackage.GetFile("songs_upgrades/upgrades.dta");
                            if (xUpgrade != null)
                            {
                                var temp_upg = Path.GetTempPath() + "upg.dta";
                                Tools.DeleteFile(temp_upg);

                                if (xUpgrade.ExtractToFile(temp_upg))
                                {
                                    var upg_midi = "";
                                    var sr = new StreamReader(temp_upg);
                                    while (sr.Peek() >= 0)
                                    {
                                        var line = sr.ReadLine();
                                        if (string.IsNullOrWhiteSpace(line)) continue;

                                        if (line.Contains("midi_file"))
                                        {
                                            upg_midi = line.Replace("midi_file", "").Replace("songs_upgrades", "")
                                                .Replace("(", "").Replace(")", "").Replace("\"", "")
                                                .Replace("/", "").Replace("'", "").Trim();
                                        }

                                        if (string.IsNullOrWhiteSpace(upg_midi)) continue;
                                        if (chkMIDI.Checked)
                                        {
                                            var xmidi = xPackage.GetFile("songs_upgrades/" + upg_midi);
                                            if (xmidi != null)
                                            {
                                                var outputfolder = txtFolder.Text + "\\" + (organizeFilesByType.Checked ? "midi_files\\" : "");
                                                if (!Directory.Exists(outputfolder))
                                                {
                                                    Directory.CreateDirectory(outputfolder);
                                                }
                                                var out_midi = outputfolder + upg_midi;
                                                Log("Extracting MIDI file " + Path.GetFileName(out_midi));
                                                Tools.DeleteFile(out_midi);
                                                if (xmidi.ExtractToFile(out_midi))
                                                {
                                                    Log("Extracted " + Path.GetFileName(out_midi) + " successfully");
                                                    midicount++;
                                                }
                                                else
                                                {
                                                    Log("There was an error extracting upgrade MIDI file " + upg_midi);
                                                }
                                            }
                                            else
                                            {
                                                Log("Could not find upgrade MIDI file " + upg_midi + " in that file");
                                            }
                                        }
                                        upg_midi = "";
                                    }
                                    sr.Dispose();

                                    if (chkDTA.Checked)
                                    {
                                        if (!Directory.Exists(dtafolder))
                                        {
                                            Directory.CreateDirectory(dtafolder);
                                        }
                                        var upgdta = (string.IsNullOrWhiteSpace(xPackage.Header.Title_Display) ? Path.GetFileName(file) : xPackage.Header.Title_Display).Replace("!", "")
                                            .Replace("'", "").Replace(" ", replaceSpacesWithUnderscores.Checked ? "_" : (removeSpacesFromFileName.Checked ? "" : " "));
                                        var upg_out = dtafolder + "\\" + Tools.CleanString(upgdta, false) + "_upgrade.dta";
                                        Tools.DeleteFile(upg_out);
                                        if (Tools.MoveFile(temp_upg, upg_out))
                                        {
                                            Log("Extracted " + Path.GetFileName(upg_out) + " successfully");
                                            dtacount++;
                                        }
                                        else
                                        {
                                            Log("There was an error extracting the upgrades.dta for " + Path.GetFileName(file));
                                        }
                                    }
                                }
                                else
                                {
                                    Log("There was an error extracting the upgrades.dta for " + Path.GetFileName(file));
                                }
                            }
                            else if (!hasdta)
                            {
                                Log("Could not find songs.dta or upgrades.dta inside '" + Path.GetFileName(file) + "'");
                            }
                        }

                        var packname = (string.IsNullOrWhiteSpace(xPackage.Header.Title_Display) ? Path.GetFileName(file) : xPackage.Header.Title_Display).Replace("!", "")
                            .Replace("'", "").Replace(" ", replaceSpacesWithUnderscores.Checked ? "_" : (removeSpacesFromFileName.Checked ? "" : " "));
                        packname = Parser.Songs.Count == 1 && !string.IsNullOrWhiteSpace(rename) ? rename : Tools.CleanString(packname, false);

                        if (chkDTA.Checked && hasdta)
                        {
                            try
                            {
                                var newDTA = dtafolder + packname + (appendsongsToFiles.Checked ? "_songs" : "") + ".dta";
                                Log("Extracting DTA file " + Path.GetFileName(newDTA));
                                if (!Directory.Exists(dtafolder))
                                {
                                    Directory.CreateDirectory(dtafolder);
                                }
                                if (Parser.WriteDTAToFile(newDTA))
                                {
                                    Log(Path.GetFileName(newDTA) + " extracted successfully");
                                    dtacount++;
                                }
                                else
                                {
                                    Log("Looks like extracting the DTA file for " + Path.GetFileName(file) + " failed. Sorry.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Log("Error extracting DTA file for " + Path.GetFileName(file));
                                Log("The error says: " + ex.Message);
                            }
                        }

                        if (chkThumbs.Checked && !FileExtractor.CancellationPending)
                        {
                            var thumbfolder = txtFolder.Text + "\\" + (organizeFilesByType.Checked ? "thumbnails\\" : "");
                            var package = thumbfolder + packname + " Package.png";
                            var content = thumbfolder + packname + " Content.png";
                            if (!Directory.Exists(thumbfolder))
                            {
                                Directory.CreateDirectory(thumbfolder);
                            }
                            try
                            {
                                Tools.DeleteFile(content);
                                var img = xPackage.Header.ContentImage;
                                img.Save(content, ImageFormat.Png);
                                img.Dispose();
                                thumbcount++;

                                Tools.DeleteFile(package);
                                img = xPackage.Header.PackageImage;
                                img.Save(package, ImageFormat.Png);
                                img.Dispose();
                                thumbcount++;

                                Log("Extracted thumbnails successfully");
                            }
                            catch (Exception ex)
                            {
                                Log("There was an error extracting the thumbnails");
                                Log("The error says: " + ex.Message);
                            }
                        }
                        xPackage.CloseIO();
                    }
                    catch (Exception ex)
                    {
                        Log("Error processing file '" + Path.GetFileName(file) + "'");
                        Log("The error says: " + ex.Message);
                        xPackage.CloseIO();
                    }
                }
                catch (Exception ex)
                {
                    Log("There was a problem accessing that file");
                    Log("The error says: " + ex.Message);
                    xPackage.CloseIO();
                }
            }
            
            if (dtacount + midicount + moggcount + milocount + pngcount + thumbcount == 0)
            {
                Log("Nothing was extracted ... please see the log for any failure reports");
            }
            else
            {
                if (dtacount > 0)
                {
                    Log("Extracted " + dtacount + " DTA " + (dtacount > 1 ? "files" : "file"));
                }
                if (pngcount > 0)
                {
                    Log("Extracted " + pngcount + " PNG_XBOX " + (pngcount > 1 ? "files" : "file"));
                    btnConverter.Invoke(new MethodInvoker(() => btnConverter.Visible = true));
                }
                if (midicount > 0)
                {
                    Log("Extracted " + midicount + " MIDI " + (midicount > 1 ? "files" : "file"));
                }
                if (moggcount > 0)
                {
                    Log("Extracted " + moggcount + " MOGG " + (moggcount > 1 ? "files" : "file"));
                }
                if (milocount > 0)
                {
                    Log("Extracted " + milocount + " MILO_XBOX " + (milocount > 1 ? "files" : "file"));
                }
                if (thumbcount > 0)
                {
                    Log("Extracted " + thumbcount + " " + (thumbcount > 1 ? "thumbnails" : "thumbnail"));
                }
            }
            xPackage.CloseIO();
        }

        private void FileExtractor_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            EnableDisable(true);
            toolTip1.SetToolTip(btnBegin, "Click to begin extracting files");
            btnBegin.Text = "&Extract";
            btnBegin.Enabled = true;
            Log("Ready");
        }

        private void folderScanner_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                var inFiles = new string[] { };
                txtFolder.Invoke(new MethodInvoker(() => inFiles = Directory.GetFiles(txtFolder.Text)));
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
            }
            catch (Exception ex)
            {
                Log("There was an error: " + ex.Message);
            }
        }

        private void folderScanner_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
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
                btnBegin.Visible = chkDTA.Checked || chkMIDI.Checked || chkMILO.Checked ||
                                   chkMOGG.Checked || chkPNG.Checked || chkThumbs.Checked;
                btnRefresh.Visible = true;
            }
            EnableDisable(true);
            txtFolder.Focus();
        }

        private sealed class DarkRenderer : ToolStripProfessionalRenderer
        {
            public DarkRenderer() : base(new DarkColors()) { }
        }

        private sealed class DarkColors : ProfessionalColorTable
        {
            public override Color ImageMarginGradientBegin
            {
                get { return mMenuBackground; }
            }
            public override Color ImageMarginGradientEnd
            {
                get { return mMenuBackground; }
            }
            public override Color ImageMarginGradientMiddle
            {
                get { return mMenuBackground; }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return mMenuBackground; }
            }
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

        private void BatchExtractor_Shown(object sender, EventArgs e)
        {
            //load last used directory if saved and still exists
            Log("Welcome to Batch Extractor");
            Log("Drag and drop the CON / LIVE file(s) here");
            Log("Or click 'Change Input Folder' to select the files");
                        
            if (!File.Exists(configFile)) return;
            var sr = new StreamReader(configFile);
            var line = sr.ReadLine();
            sr.Dispose();
            if (string.IsNullOrWhiteSpace(line))
            {
                Tools.DeleteFile(configFile);
            }
            else if (line != "" && Directory.Exists(line))
            {
                Log("");
                Log("Loaded last directory used: " + line);
                txtFolder.Text = line;
            }
            else
            {
                Tools.DeleteFile(configFile);
            }
            if (string.IsNullOrWhiteSpace(txtFolder.Text))
            {
                Log("Ready to begin");
            }
        }
    }
}
