using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nautilus.Properties;
using Nautilus.x360;
using Un4seen.Bass;
using SearchOption = System.IO.SearchOption;
using System.Drawing;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Opus;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.EncOgg;
using NautilusFREE;

namespace Nautilus
{
    public partial class PhaseShiftConverter : Form
    {
        private static string inputDir;
        private static List<string> inputFiles;
        private DateTime endTime;
        private DateTime startTime;
        private string PSFolder;
        private readonly string rar;
        private readonly PhaseShiftSong Song;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private readonly nTools nautilus3;
        private bool BASS_INIT;
        private const int BassBuffer = 1000;
        private int BassMixer;
        private int BassStream;
        public PhaseShiftConverter(Color ButtonBackColor, Color ButtonTextColor)
        {
            InitializeComponent();
            
            //initialize
            Song = new PhaseShiftSong();
            Tools = new NemoTools();
            Parser = new DTAParser();
            nautilus3 = new nTools();

            inputFiles = new List<string>();
            inputDir = Application.StartupPath + "\\phaseshift\\";
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

            rar = Application.StartupPath + "\\bin\\rar.exe";
            if (!File.Exists(rar))
            {
                MessageBox.Show("Can't find rar.exe ... I won't be able to create RAR files for your songs without it",
                                "Missing Executable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                chkRAR.Checked = false;
                chkRAR.Enabled = false;
            }
            
            toolTip1.SetToolTip(btnBegin, "Click to begin process");
            toolTip1.SetToolTip(btnFolder, "Click to select the input folder");
            toolTip1.SetToolTip(btnRefresh, "Click to refresh if the contents of the folder have changed");
            toolTip1.SetToolTip(txtFolder, "This is the working directory");
            toolTip1.SetToolTip(lstLog, "This is the application log. Right click to export");
            domainQuality.SelectedIndex = 5;
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

        public bool loadDTA(uint TitleID)
        {
            try
            {
                Song.Name = Parser.Songs[0].Name;
                Song.Artist = Parser.Songs[0].Artist;
                if (string.IsNullOrEmpty(Song.Artist) && (TitleID == 0x454108B1 || TitleID == (uint)4294838225)) //for TBRB customs
                {
                    Song.Artist = "The Beatles";
                }
                Song.Album = Parser.Songs[0].Album;
                Song.Year = Parser.Songs[0].YearReleased;
                Song.HopoThreshold = Parser.Songs[0].HopoThreshold;
                Song.DrumKit = Parser.GetDrumKit(Parser.Songs[0].DrumBank);
                Song.TrackNumber = Parser.Songs[0].TrackNumber;
                switch (Parser.Songs[0].VocalParts)
                {
                    case 0:
                        {
                            Song.DiffVocals = -1;
                            Song.HasHarmonies = false;
                        }
                        break;
                    case 1:
                        {
                            Song.HasHarmonies = false;
                        }
                        break;
                    case 2:
                        {
                            Song.HasHarmonies = true;
                        }
                        break;
                    case 3:
                        {
                            Song.HasHarmonies = true;
                        }
                        break;
                    default:
                        {
                            Song.DiffVocals = -1;
                            Song.HasHarmonies = false;
                        }
                        break;
                }
                Song.DiffDrums = Parser.Songs[0].DrumsDiff-1;
                Song.DiffBand = Parser.Songs[0].BandDiff-1;
                Song.DiffBass = Parser.Songs[0].BassDiff-1;
                Song.DiffGuitar = Parser.Songs[0].GuitarDiff-1;
                Song.DiffKeys = Parser.Songs[0].KeysDiff-1;
                Song.DiffProKeys = Parser.Songs[0].DisableProKeys ? -1 : Parser.Songs[0].ProKeysDiff-1;
                Song.DiffProBass = Parser.Songs[0].ProBassDiff-1;
                Song.DiffProGuitar = Parser.Songs[0].ProGuitarDiff-1;
                Song.DiffVocals = Parser.Songs[0].VocalsDiff-1;
                Song.PreviewStart = Parser.Songs[0].PreviewStart;
                Song.Length = Parser.Songs[0].Length;
                Song.Genre = Parser.Songs[0].Genre;
                Song.SubGenre = Parser.Songs[0].SubGenre;
                Song.BassTuning = Parser.Songs[0].ProBassTuning;
                Song.GuitarTuning = Parser.Songs[0].ProGuitarTuning;
                Song.Charter = Parser.Songs[0].ChartAuthor;
                Song.RhythmOnBass = Parser.Songs[0].RhythmBass;
                Song.RhythmOnKeys = Parser.Songs[0].RhythmKeys;
                return true;
            }
            catch (Exception ex)
            {
                Log("There was an error processing that songs.dta file");
                Log("The error says: " + ex.Message);
                return false;
            }
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
                    Song.NewSong();
                    Song.ReplaceGenreWithSub = useSubgenreInsteadOfGenreToolStripMenuItem.Checked;

                    try
                    {
                        if (!Directory.Exists(PSFolder))
                        {
                            Directory.CreateDirectory(PSFolder);
                        }
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
                        uint TitleID;
                        var xCON = new STFSPackage(file);
                        if (xCON.ParseSuccess)
                        {
                            TitleID = xCON.Header.TitleID;
                        }
                        else
                        {
                            TitleID = 0;
                        }
                        xCON.CloseIO();
                        if (loadDTA(TitleID))
                        {
                            Log("Loaded and processed songs.dta file for song #" + counter + " successfully");
                            Log("Song #" + counter + " is " + Song.Artist + " - " + Song.Name);
                        }
                        else
                        {
                            return false;
                        }
                        
                        var songfolder = PSFolder + Tools.CleanString(Song.Artist,false) + " - " + Tools.CleanString(Song.Name, false) + "\\";
                        if (!Directory.Exists(songfolder))
                        {
                            Directory.CreateDirectory(songfolder);
                        }
                        var internal_name = Parser.Songs[0].InternalName;

                        var xPackage = new STFSPackage(file);
                        if (!xPackage.ParseSuccess)
                        {
                            Log("Failed to parse '" + Path.GetFileName(file) + "'");
                            Log("Skipping this file");
                            continue;
                        }
                        var xArt = xPackage.GetFile("songs/" + internal_name + "/gen/" + internal_name + "_keep.png_xbox");
                        if (xArt != null)
                        {
                            var newart = songfolder + "album.png_xbox";
                            if (xArt.ExtractToFile(newart))
                            {
                                Log("Extracted album art file " + internal_name + "_keep.png_xbox successfully");
                                fromXbox(newart);
                            }
                            else
                            {
                                Log("There was a problem extracting the album art file");
                            }
                        }
                        else
                        {
                            Log("WARNING: Did not find album art file in that CON file");
                        }
                        var xMIDI = xPackage.GetFile("songs/" + internal_name + "/" + internal_name + ".mid");
                        if (xMIDI != null)
                        {
                            var newmidi = songfolder + "notes.mid";
                            if (xMIDI.ExtractToFile(newmidi))
                            {
                                Log("Extracted MIDI file " + internal_name + ".mid successfully");
                                ProcessMidi(newmidi);
                            }
                            else
                            {
                                Log("There was a problem extracting the MIDI file");
                                Log("Skipping this song...");
                                xPackage.CloseIO();
                                continue;
                            }
                        }
                        else
                        {
                            Log("ERROR: Did not find a MIDI file in that CON file!");
                            Log("Skipping this song...");
                            xPackage.CloseIO();
                            continue;
                        }
                        var xMOGG = xPackage.GetFile("songs/" + internal_name + "/" + internal_name + ".mogg");
                        if (xMOGG != null)
                        {
                            var newMogg = songfolder + internal_name + ".mogg";
                            if (radioSeparate.Checked)
                            {
                                xPackage.CloseIO();
                                SeparateAudio(file, newMogg, songfolder);
                            }
                            else if (radioDownmix.Checked)
                            {
                                xPackage.CloseIO();
                                DownMixAudio(file, songfolder);
                            }
                            else
                            {
                                var msg = "Extracting audio file " + (radioAudacity.Checked ? "to send to Audacity" : "and leaving it as is");
                                Log(msg);
                                var mData = xMOGG.Extract();
                                if (mData != null && mData.Length > 0)
                                {
                                    var nautilus3 = new nTools();
                                    Log("Successfully extracted audio file '" + Path.GetFileName(newMogg) + "'");
                                    if (radioAudacity.Checked)
                                    { 
                                        Log("Sending audio file to Audacity now");
                                        if (Tools.isV17(mData))
                                        {
                                            unsafe
                                            {
                                                var bytes = mData;
                                                fixed (byte* ptr = bytes)
                                                {
                                                    if (!TheMethod3.decrypt_mogg(ptr, (uint)bytes.Length)) continue;
                                                    nautilus3.WriteOutData(bytes, newMogg);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            nautilus3.DecM(mData, false, false, DecryptMode.ToFile, newMogg);
                                        }
                                        Log(Tools.SendtoAudacity(newMogg));
                                    }
                                    else
                                    {
                                        nautilus3.WriteOutData(nautilus3.ObfM(mData), newMogg);
                                    }
                                }
                                else
                                {
                                    Log("There was a problem extracting the audio file");
                                    Log("Skipping this song...");
                                    xPackage.CloseIO();
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            Log("ERROR: Did not find an audio file in that CON file!");
                            Log("Skipping this song...");
                            xPackage.CloseIO();
                            continue;
                        }
                        xPackage.CloseIO();

                        if (!Directory.Exists(songfolder))
                        {
                            Directory.CreateDirectory(songfolder);
                        }
                        if (markAsC3.Checked)
                        {
                            Song.icon = "ccc";
                            Song.LoadingPhrase = "Brought to you by Customs Creators Collective";
                        }
                        else if (markAsRV.Checked)
                        {
                            Song.icon = "rv";
                            Song.LoadingPhrase = "Brought to you by Rhythm Verse";
                        }
                        else if (markAsCustom.Checked)
                        {
                            Song.icon = txtCustomLabel.Text;
                            Song.LoadingPhrase = "";
                        }
                        Song.WriteINIFile(songfolder, !dontAddHopoThreshold.Checked);                                              

                        success++;
                        if (!chkRAR.Checked || backgroundWorker1.CancellationPending) continue;
                        var archive = Path.GetFileName(file);
                        archive = archive.Replace(" ", "").Replace("-", "_").Replace("\\", "").Replace("'", "").Replace(",", "").Replace("_rb3con", "");
                        archive = Tools.CleanString(archive, false);
                        archive = PSFolder + archive + "_ps.rar";
                        
                        var arg = "a -m5 -ep1 -r \"" + archive + "\" \"" + songfolder.Substring(0,songfolder.Length-1) + "\"";
                        Log("Creating RAR archive");

                        Log(Tools.CreateRAR(rar, archive, arg)? "Created RAR archive successfully" : "RAR archive creation failed");
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
        
        private void SeparateAudio(string CON, string mogg, string folder)
        {
            if (backgroundWorker1.CancellationPending) return;
            Log("Separating mogg file into its component ogg files");
            var Splitter = new MoggSplitter();
            var split = Splitter.SplitMogg(CON, folder, "allstems|rhythm|song", MoggSplitter.MoggSplitFormat.OGG, Convert.ToInt16(domainQuality.Text));
            if (!split)
            {
                foreach (var error in Splitter.ErrorLog)
                {
                    Log(error);
                }
                Log("Failed...will try to downmix");
                DownMixAudio(CON, folder);
                return;
            }
            FinishAudioSeparation(mogg, folder);
        }

        private void FinishAudioSeparation(string mogg, string folder)
        {
            var oggs = Directory.GetFiles(folder, "*.ogg");
            if (!oggs.Any())
            {
                Log("Failed");
                return;
            }
            Log("Success");
            Tools.DeleteFile(mogg);
            AnalyzeOggs(oggs);
            if (!useguitaroggForNonmultitrackSongs.Checked) return;
            oggs = Directory.GetFiles(folder, "*.ogg");
            if (!oggs.Any() || oggs.Count() > 1 || !oggs[0].Contains("song.ogg")) return;
            Tools.MoveFile(oggs[0], folder + "guitar.ogg");
        }

        private void DownMixAudio(string CON, string folder)
        {
            if (backgroundWorker1.CancellationPending) return;
            var ogg = folder + (useguitaroggForNonmultitrackSongs.Checked ? "guitar.ogg" : "song.ogg");
            Log("Downmixing audio file to stereo file:");
            Log(ogg);
            var Splitter = new MoggSplitter();
            var mixed = Splitter.DownmixMogg(CON, ogg, MoggSplitter.MoggSplitFormat.OGG, Convert.ToInt16(domainQuality.Text));
            foreach (var error in Splitter.ErrorLog)
            {
                Log(error);
            }
            Log(mixed && File.Exists(ogg) ? "Success" : "Failed");
        }
        
        private void AnalyzeOggs(IEnumerable<string> oggs)
        {
            Log("Analyzing ogg files and deleting silent files");
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            foreach (var ogg in oggs)
            {
                var BassStream = Bass.BASS_StreamCreateFile(ogg, 0, 0, BASSFlag.BASS_STREAM_DECODE);
                var level = new float[1];
                while (Bass.BASS_ChannelGetLevel(BassStream, level, 1, BASSLevel.BASS_LEVEL_MONO))
                {
                    if (level[0] != 0) break;
                }
                Bass.BASS_StreamFree(BassStream);
                if (level[0] == 0)
                {
                    Tools.DeleteFile(ogg);
                }
            }
            Bass.BASS_Free();
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

        private void fromXbox(string image)
        {
            var pngfile = Path.GetDirectoryName(image) + "\\album.png";
            try
            {
                Log(Tools.ConvertRBImage(image, pngfile, "png", cleanUppngxboxFiles.Checked) ? "Converted album art file to 'album.png' successfully" : "There was an error when converting the album art file");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong when trying to convert the album art file\nfrom the native Rock Band format\nSorry\n\nThe message says: " + ex.Message,
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Log("There was an error when converting the album art file");
                Log("The error says: " + ex.Message);
            }
        }
        
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("psc");
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
            markAsRV.Enabled = enabled;
            markAsC3.Enabled = enabled;
            btnFolder.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            menuStrip1.Enabled = enabled;
            txtFolder.Enabled = enabled;
            chkRAR.Enabled = enabled;
            grpMogg.Enabled = enabled;
            picWorking.Visible = !enabled;
            lstLog.Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
            Cursor = lstLog.Cursor;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (btnBegin.Text == "Cancel")
            {
                backgroundWorker1.CancelAsync();
                Log("User cancelled process...stopping as soon as possible");
                btnBegin.Enabled = false;
                return;
            }

            startTime = DateTime.Now;
            PSFolder = txtFolder.Text + "\\Music\\";
            Tools.CurrentFolder = txtFolder.Text;
            EnableDisable(false);
            
            if (!Directory.Exists(PSFolder))
            {
                Directory.CreateDirectory(PSFolder);
            }

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
                    Log("No files found ... there's nothing to do");
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
        
        private void PhaseShiftPrep_Resize(object sender, EventArgs e)
        {
            btnRefresh.Left = txtFolder.Left + txtFolder.Width - btnRefresh.Width;
            btnBegin.Left = txtFolder.Left + txtFolder.Width - btnBegin.Width;
            picWorking.Left = (Width / 2) - (picWorking.Width / 2);
        }

        private void PhaseShiftPrep_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picWorking.Visible)
            {
                Tools.DeleteFolder(Application.StartupPath + "\\phaseshift\\");
                return;
            }
            MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        private void ProcessMidi(string midifile)
        {
            var songMidi = Tools.NemoLoadMIDI(midifile);
            if (songMidi == null)
            {
                Log("Error parsing MIDI file ... can't analyze contents");
                return;
            }
            for (var i = 0; i < songMidi.Events.Tracks; i++)
            {
                if (Tools.GetMidiTrackName(songMidi.Events[i][0].ToString()).ToLowerInvariant().Contains("real_guitar_22"))
                {
                    Song.ProGuitar22 = true;
                }
                else if (Tools.GetMidiTrackName(songMidi.Events[i][0].ToString()).ToLowerInvariant().Contains("real_bass_22"))
                {
                    Song.ProBass22 = true;
                }
            }
        }

        private void PhaseShiftPrep_Shown(object sender, EventArgs e)
        {
            Log("Welcome to " + Text);
            Log("Drag and drop the CON / LIVE file(s) to be converted here");
            Log("Or click 'Change Input Folder' to select the files");
            Log("Ready to begin");
            txtFolder.Text = inputDir;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (ProcessFiles()) return;
            Log("There was an error processing the files ... stopping here");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Log("Done!");
            endTime = DateTime.Now;
            var timeDiff = endTime - startTime;
            Log("Process took " + timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute" : " minutes") + " and " + (timeDiff.Minutes == 0 && timeDiff.Seconds == 0 ? "1 second" : timeDiff.Seconds + " seconds"));
            Log("Click 'Reset' to start again or just close me down");
            if (cleanUppngxboxFiles.Checked)
            {
                //clear up any leftover png_xbox files
                var xbox_files = Directory.GetFiles(txtFolder.Text, "*.png_xbox", SearchOption.AllDirectories);
                foreach (var xbox_file in xbox_files)
                {
                    Tools.DeleteFile(xbox_file);
                }
            }
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

        private void radioSeparate_CheckedChanged(object sender, EventArgs e)
        {
            lblQuality.Enabled = radioSeparate.Checked || radioDownmix.Checked;
            domainQuality.Enabled = radioSeparate.Checked || radioDownmix.Checked;
        }

        private void decodeSNG_Click(object sender, EventArgs e)
        {
            var convert = opusToOgg.Checked ? "\n\nConvert .opus to .ogg enabled" : "";
            MessageBox.Show("This will batch decode and extract Clone Hero SNG files" + convert, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            var ofd = new FolderPicker
            {
                InputPath = Environment.CurrentDirectory,
                Title = "Select the folder where your SNG files are",
            };
            if (ofd.ShowDialog(IntPtr.Zero) != true || string.IsNullOrEmpty(ofd.ResultPath)) return;
            var folder = ofd.ResultPath;

            var sngFiles = new List<string>();
            var inFiles = Directory.GetFiles(folder);
            foreach (var file in inFiles.Where(sng => Path.GetExtension(sng) == ".sng"))
            {
                sngFiles.Add(file);
            }
            ExtractSNGFiles(sngFiles);
        }

        private void ExtractSNGFiles(List<string> sngFiles)
        {
            Log("");
            Log("Found " + sngFiles.Count + " SNG files");
            var counter = 0;
            var success = 0;

            List<string> folders = new List<string>();

            foreach (var sng in sngFiles)
            {
                var outFolder = Path.GetDirectoryName(sng) + "\\" + Path.GetFileNameWithoutExtension(sng).Trim();
                Tools.DeleteFolder(outFolder, true);
                Log("Decoding SNG file '" + Path.GetFileName(sng) + "'");
                counter++;                
                if (!Tools.ExtractSNG(sng, outFolder))
                {
                    Log("Failed");
                    continue;
                }
                if (opusToOgg.Checked)
                {                    
                    var opusFiles = Directory.GetFiles(outFolder, "*.opus");
                    Log("Converting " + opusFiles.Count() + " .opus files to .ogg format");
                    foreach (var opus in opusFiles)
                    {
                        Log("Converting '" + Path.GetFileName(opus) + "' to .ogg format");
                        if (!BASS_INIT)
                        {
                            //initialize BASS.NET
                            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, Handle))
                            {
                                MessageBox.Show("Error initializing BASS.NET\n" + Bass.BASS_ErrorGetCode());
                                return;
                            }
                            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, BassBuffer);
                            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 50);
                            BASS_INIT = true;
                        }

                        BassStream = BassOpus.BASS_OPUS_StreamCreateFile(opus, 0L, File.ReadAllBytes(opus).Length, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT);

                        // create a decoder for the OGG file(s)
                        var channel_info = Bass.BASS_ChannelGetInfo(BassStream);

                        // create a stereo mixer with same frequency rate as the input file
                        BassMixer = BassMix.BASS_Mixer_StreamCreate(channel_info.freq, 2, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_MIXER_END);

                        BassMix.BASS_Mixer_StreamAddChannel(BassMixer, BassStream, BASSFlag.BASS_MIXER_MATRIX);

                        var outFile = opus.Replace(".opus", ".ogg");
                        BassEnc_Ogg.BASS_Encode_OGG_StartFile(BassMixer, "-q 3", BASSEncode.BASS_ENCODE_AUTOFREE, outFile);

                        while (true)
                        {
                            var buffer = new byte[20000];
                            var c = Bass.BASS_ChannelGetData(BassMixer, buffer, buffer.Length);
                            if (c <= 0) break;
                        }

                        Bass.BASS_Free();
                        BASS_INIT = false;

                        if (File.Exists(outFile))
                        {
                            Tools.DeleteFile(opus);
                            Log("Success");
                        }
                        else
                        {
                            Log("Failed");
                        }
                    }
                }
                Log("Decoded file successfully");
                success++;                
            }
            Log("Decoded " + success + " SNG files out of " + counter + " files attempted");
        }

        private void markAsC3_CheckedChanged(object sender, EventArgs e)
        {
            if (markAsC3.Checked)
            {
                markAsRV.Checked = false;
                markAsCustom.Checked = false;
            }
        }

        private void markAsRV_CheckedChanged(object sender, EventArgs e)
        {
            if (markAsRV.Checked)
            {
                markAsC3.Checked = false;
                markAsCustom.Checked = false;
            }
        }

        private void markAsCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (markAsCustom.Checked)
            {
                markAsC3.Checked = false;
                markAsRV.Checked = false;
            }
            txtCustomLabel.Enabled = markAsCustom.Checked;
        }

        private void decodeYARG_Click(object sender, EventArgs e)
        {            
            MessageBox.Show("This will batch decrypt and extract YARG \".yargsong\" game files", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            var ofd = new FolderPicker
            {
                InputPath = Environment.CurrentDirectory,
                Title = "Select the folder where your \".yargsong\" files are",
            };
            if (ofd.ShowDialog(IntPtr.Zero) != true || string.IsNullOrEmpty(ofd.ResultPath)) return;
            var folder = ofd.ResultPath;

            var yargFiles = new List<string>();
            var inFiles = Directory.GetFiles(folder);
            foreach (var file in inFiles.Where(yarg => Path.GetExtension(yarg) == ".yargsong"))
            {
                yargFiles.Add(file);
            }
            DecryptExtractYARG(yargFiles);
        }

        private void DecryptExtractYARG(List<string> yargFiles)
        {
            Log("");
            Log("Found " + yargFiles.Count + " \".yargsong\" files");
            Log("Starting process...");
            var counter = 0;
            var success = 0;

            foreach (var file in yargFiles)
            {
                counter++;
                Log("Decrypting file " + counter + " of " + yargFiles.Count());
                var tempFile = file.Replace(".yargsong", ".sng");
                var outFolder = Path.GetDirectoryName(tempFile) + "\\" + Path.GetFileNameWithoutExtension(tempFile).Trim();                
                if (!Tools.DecryptExtractYARGSONG(file, outFolder))
                {
                    Log("Failed");
                    continue;
                }
                success++;
                Log("Decrypted file " + counter + " of " + yargFiles.Count() + " successfully");
                Tools.DeleteFile(tempFile);
            }
            Log("Decrypted " + success + " file(s) successfully");
        }
    }

    public class PhaseShiftSong
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string LoadingPhrase { get; set; }
        public string Genre { get; set; }
        public string SubGenre { get; set; }
        public int Year { get; set; }
        public string Charter { get; set; }

        public string GuitarTuning { get; set; }
        public string BassTuning { get; set; }

        public bool RhythmOnKeys { get; set; }
        public bool RhythmOnBass { get; set; }
        public bool ProGuitar22 { get; set; }
        public bool ProBass22 { get; set; }
        public bool HasHarmonies { get; set; }
        public bool ReplaceGenreWithSub { get; set; }
        public long Length { get; set; }
        public long PreviewStart { get; set; }

        public int DrumKit { get; set; }
        public int HopoThreshold { get; set; }

        //Valid values = 0-6
        //-1 is disabled
        public int DiffDrums { get; set; }
        public int DiffBass { get; set; }
        public int DiffProBass { get; set; }
        public int DiffGuitar { get; set; }
        public int DiffProGuitar { get; set; }
        public int DiffKeys { get; set; }
        public int DiffProKeys { get; set; }
        public int DiffVocals { get; set; }
        public int DiffBand { get; set; }
        public int TrackNumber { get; set; }
        
        public string icon { get; set; }


        /// <summary>
        /// Sets all the song's values to defaults
        /// </summary>
        public void NewSong()
        {
            Name = "";
            Artist = "";
            Album = "";
            LoadingPhrase = "";
            icon = "";
            Genre = "";
            Year = -1;
            Charter = "";
            DiffDrums = -1;
            DiffBass = -1;
            DiffProBass = -1;
            DiffGuitar = -1;
            DiffProGuitar = -1;
            DiffKeys = -1;
            DiffProKeys = -1;
            DiffVocals = -1;
            DiffBand = -1;
            RhythmOnBass = false;
            RhythmOnKeys = false;
            ProGuitar22 = false;
            ProBass22 = false;
            HasHarmonies = false;
            Length = -1;
            PreviewStart = -1;
            GuitarTuning = "";
            BassTuning = "";
            DrumKit = -1;
            HopoThreshold = -1;
            ReplaceGenreWithSub = false;
            TrackNumber = 0;
        }

        /// <summary>
        /// Outputs song.ini file with all the information currently attached to that song
        /// </summary>
        /// <param name="output_folder">Folder path where the song.ini file should be created</param>
        /// <returns></returns>
        public bool WriteINIFile(string output_folder, bool DontAddHopoThreshold)
        {
            var ini = output_folder + "\\song.ini";
            var ps = Application.StartupPath + "\\bin\\loading_phrase.txt";
            if (File.Exists(ps))
            {
                try
                {
                    var sr = new StreamReader(ps, Encoding.Default);
                    LoadingPhrase = sr.ReadLine();
                    sr.Dispose();
                }
                catch (Exception)
                {
                    LoadingPhrase = "";
                }
            }
            try
            {
                if (File.Exists(ini))
                {
                    File.Delete(ini);
                }
            }
            catch (Exception)
            {}
            try
            {
                var sw = new StreamWriter(ini, false, Encoding.Default);
                sw.WriteLine("[song]");
                sw.WriteLine("delay = 0"); //is this necessary?
                sw.WriteLine("multiplier_note = 116"); //is this necessary?
                sw.WriteLine("artist = " + Artist);
                sw.WriteLine("name = " + Name);
                if (Album != "")
                {
                    sw.WriteLine("album = " + Album);
                }
                if (TrackNumber > 0)
                {
                    sw.WriteLine("track = " + TrackNumber);
                }
                if (Year > -1)
                {
                    sw.WriteLine("year = " + Year);
                }
                if (Genre != "")
                {
                    sw.WriteLine("genre = " + ((ReplaceGenreWithSub && SubGenre != "") ? SubGenre : Genre));
                }
                if (DiffDrums > -1)
                {
                    sw.WriteLine("pro_drums = True"); //always true for RB3 content

                    if (DrumKit > -1)
                    {
                        sw.WriteLine("kit_type = " + DrumKit);
                    }
                }
                sw.WriteLine("diff_drums = " + DiffDrums);
                sw.WriteLine("diff_drums_real = " + DiffDrums);
                var DiffRhythm = -1;
                if (RhythmOnBass)
                {
                    DiffRhythm = DiffBass;
                    DiffBass = -1;
                    DiffProBass = -1;
                    ProBass22 = false;
                }
                sw.WriteLine("diff_bass = " + DiffBass);
                sw.WriteLine("diff_bass_real = " + DiffProBass);
                if (ProBass22)
                {
                    sw.WriteLine("diff_bass_real_22 = " + DiffProBass);
                }
                else
                {
                    sw.WriteLine("diff_bass_real_22 = -1");
                }
                if (BassTuning != "")
                {
                    sw.WriteLine("real_bass_tuning = " + BassTuning);
                }
                sw.WriteLine("diff_rhythm = " + DiffRhythm);
                sw.WriteLine("diff_guitar = " + DiffGuitar);
                sw.WriteLine("diff_guitar_real = " + DiffProGuitar);
                if (ProGuitar22)
                {
                    sw.WriteLine("diff_guitar_real_22 = " + DiffProGuitar);
                }
                else
                {
                    sw.WriteLine("diff_guitar_real_22 = -1");
                }
                if (GuitarTuning != "")
                {
                    sw.WriteLine("real_guitar_tuning = " + GuitarTuning);
                }
                var DiffCoop = -1;
                if (RhythmOnKeys)
                {
                    DiffCoop = DiffKeys;
                    DiffKeys = -1;
                    DiffProKeys = -1;
                }
                sw.WriteLine("diff_keys = " + DiffKeys);
                sw.WriteLine("diff_keys_real = " + DiffProKeys);
                sw.WriteLine("diff_guitar_coop = " + DiffCoop);
                sw.WriteLine("diff_vocals = " + DiffVocals);
                if (HasHarmonies)
                {
                    sw.WriteLine("diff_vocals_harm = " + DiffVocals);
                }
                else
                {
                    sw.WriteLine("diff_vocals_harm = -1");
                }
                sw.WriteLine("diff_band = " + DiffBand);
                if (HopoThreshold > -1 && !DontAddHopoThreshold)
                {
                    sw.WriteLine("hopo_frequency = " + HopoThreshold);
                }
                if (PreviewStart > 0)
                {
                    sw.WriteLine("preview_start_time = " + PreviewStart);
                }
                if (Length > 0)
                {
                    sw.WriteLine("song_length = " + Length);
                }                
                sw.WriteLine("charter = " + Charter);                
                sw.WriteLine("loading_phrase = " + LoadingPhrase);
                sw.WriteLine("icon = " + icon);
                //these values will always be -1 for RB conversions but add them
                //makes it clearer in the interface that these instruments are disabled
                sw.WriteLine("diff_drums_real_ps = -1");
                sw.WriteLine("diff_keys_real_ps = -1");
                sw.WriteLine("diff_dance = -1");                
                sw.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}