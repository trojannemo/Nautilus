using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Nautilus.Properties;
using Nautilus.x360;
using NAudio.Midi;

namespace Nautilus
{
    public partial class ProUpgradeBundler : Form
    {
        private RSAParams signature;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private string orig_midi;
        private string orig_con;
        private string orig_dta;
        private string newMIDI;
        private string newDTA;
        private string songID;
        private string song_int_name;
        private string upgradeID;
        private string upgrade_int_name;
        private readonly string temp_folder;
        private readonly List<string> SongInstruments;
        private readonly List<string> UpgradeInstruments; 
        private readonly List<string> UpgradeMidis;
        private string UpgradeCON; 
        private string newsongdta;
        private string newupgdta;
        private string proguitartuning;
        private string probasstuning;
        private string proguitardiff;
        private string probassdiff;
        private const string NA = "NOT FOUND IN DTA";
 
        public ProUpgradeBundler(Color ButtonBackColor, Color ButtonTextColor)
        {
            InitializeComponent();
            
            Tools = new NemoTools();
            Parser = new DTAParser();
            SongInstruments = new List<string>();
            UpgradeInstruments = new List<string>();
            UpgradeMidis = new List<string>();
            upgradeID = NA;
            songID = NA;
            song_int_name = NA;
            upgrade_int_name = NA;

            var formButtons = new List<Button> { btnReset, btnBundle };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }
            
            var bundlerFolder = Application.StartupPath + "\\bundler\\";
            if (!Directory.Exists(bundlerFolder))
            {
                Directory.CreateDirectory(bundlerFolder);
            }
            temp_folder = bundlerFolder + "temp\\";
            if (!Directory.Exists(temp_folder))
            {
                Directory.CreateDirectory(temp_folder);
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
        
        private void CheckIDMatch()
        {
            if (string.IsNullOrWhiteSpace(songID) || string.IsNullOrWhiteSpace(upgradeID) || songID == upgradeID || songID == NA || upgradeID == NA) return;
            MessageBox.Show("Song ID '" + songID + "' doesn't match Upgrade ID '" + upgradeID + "'\nMake sure you have the right files before bundling", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        
        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Log("Resetting");
            lstSong.Items.Clear();
            lstSong.Items.Add("No CON or MIDI loaded...");
            lstUpgrades.Items.Clear();
            lstUpgrades.Items.Add("No upgrades loaded...");
            btnBundle.Enabled = false;
            SongInstruments.Clear();
            UpgradeInstruments.Clear();
            UpgradeMidis.Clear();
            UpgradeCON = "";
            newsongdta = "";
            newupgdta = "";
            orig_midi = "";
            orig_con = "";
            orig_dta = "";
            upgradeID = NA;
            songID = NA;
            song_int_name = NA;
            upgrade_int_name = NA;
            proguitartuning = "";
            probasstuning = "";
            proguitardiff = "";
            probassdiff = "";
            CleanUp();
            EnableDisable(true);
            Log("Ready");
            btnBundle.Visible = true;
        }

        private void EnableDisable(bool enable)
        {
            picWorking.Visible = !enable;
            menuStrip1.Enabled = enable;
            btnReset.Enabled = enable;
            btnBundle.Enabled = false;
            Cursor = enable ? Cursors.Default : Cursors.WaitCursor;
            lstLog.Cursor = Cursor;
        }

        private void ReadUpgDTA(string dta)
        {
            if (!File.Exists(dta))
            {
                return;
            }

            var sr = new StreamReader(dta, Encoding.Default);
            // read one line at a time until the end
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                if (line.ToLowerInvariant().Contains("real_bass") && !line.Contains("tuning"))
                {
                    probassdiff= line;
                }
                else if (line.ToLowerInvariant().Contains("real_guitar") && !line.Contains("tuning"))
                {
                    proguitardiff = line;
                }
                else if (line.Contains("real_guitar") && line.Contains("tuning"))
                {
                    proguitartuning = line;
                }
                else if (line.Contains("real_bass") && line.Contains("tuning"))
                {
                    probasstuning = line;
                }
                else if (line.Contains("song_id"))
                {
                    upgradeID = Parser.GetSongID(line);
                }
            }
            sr.Dispose();
            CheckIDMatch();
            Log("Upgrades.dta file processed successfully");
        }

        private void MIDIFixFailed(string file, bool isBadMIDI = true)
        {
            if (isBadMIDI)
            {
                Log("MIDI file " + Path.GetFileName(file) + " was not in a format that this library can read");
            }
            Log("Stopping here");
            CleanUp();
        }

        private void btnBundle_Click(object sender, EventArgs e)
        {
            if (!File.Exists(orig_midi))
            {
                Log("MIDI file " + Path.GetFileName(orig_midi) + " is missing! Did you just delete it?");
                MIDIFixFailed(orig_midi, false);
                return;
            }
            if (!File.Exists(orig_dta) && !string.IsNullOrEmpty(orig_dta))
            {
                Log("DTA file " + Path.GetFileName(orig_dta) + " is missing! Did you just delete it?");
                MIDIFixFailed(orig_dta, false);
                return;
            }

            newMIDI = Path.GetDirectoryName(orig_midi) + "\\" + Path.GetFileNameWithoutExtension(orig_midi) + " (bundled).mid";
            newDTA = Path.GetDirectoryName(orig_dta) + "\\" + Path.GetFileNameWithoutExtension(orig_dta) + " (bundled).dta";
            var skip_tracks = ignoreNoninstrumentTracks.Checked ? new List<string> {"EVENTS", "VENUE", "BEAT"} : new List<string>();

            var midi = Tools.NemoLoadMIDI(orig_midi);
            if (midi == null)
            {
                MIDIFixFailed(orig_midi);
                return;
            }

            var harm1 = false;
            var harm2 = false;
            var harm3 = false;
            foreach (var UpgMidi in UpgradeMidis)
            {
                if (!File.Exists(UpgMidi))
                {
                    Log("Upgrade MIDI file " + Path.GetFileName(UpgMidi) + " is missing! Did you just delete it?");
                    continue;
                }
                var upgrade = Tools.NemoLoadMIDI(UpgMidi);
                if (upgrade == null)
                {
                    MIDIFixFailed(UpgMidi);
                    return;
                }
                Tools.DeleteFile(newMIDI);
                Tools.DeleteFile(newDTA);
                var origtracks = new List<string>();
                //check which tracks are in original midi
                for (var i = 0; i < midi.Events.Tracks; i++)
                {
                    var trackname = Tools.GetMidiTrackName(midi.Events[i][0].ToString());
                    if (!origtracks.Contains(trackname) && !skip_tracks.Contains(trackname))
                    {
                        origtracks.Add(trackname);
                    }
                }
                var upgtracks = new List<string>();
                //check what tracks are in the upgrade midi
                //skip track 0 = tempo track
                for (var i = 1; i < upgrade.Events.Tracks; i++)
                {
                    var trackname = Tools.GetMidiTrackName(upgrade.Events[i][0].ToString());
                    if (trackname.Contains("HARM1") || trackname.Contains("VOCALS"))
                    {
                        harm1 = true;
                    }
                    else if (trackname.Contains("HARM2"))
                    {
                        harm2 = true;
                    }
                    else if (trackname.Contains("HARM3"))
                    {
                        harm3 = true;
                    }
                    if (!upgtracks.Contains(trackname) && !skip_tracks.Contains(trackname))
                    {
                        upgtracks.Add(trackname);
                    }
                }
                if (overwriteExistingTrack.Checked) //only remove if checked to overwrite
                {
                    var to_remove = new List<int>();
                    for (var i = 0; i < midi.Events.Tracks; i++)
                    {
                        var trackname = Tools.GetMidiTrackName(midi.Events[i][0].ToString());
                        if (upgtracks.Contains(trackname))
                        {
                            to_remove.Add(i); //remove only if found in the upgrade midi and overwrite is checked
                        }
                    }
                    to_remove.Sort();
                    for (var i = to_remove.Count - 1; i >= 0; i--)
                    {
                        var trackname = Tools.GetMidiTrackName(midi.Events[to_remove[i]][0].ToString());
                        try
                        {
                            midi.Events.RemoveTrack(to_remove[i]);
                        }
                        catch (Exception ex)
                        {
                            Log("There was an error deleting track " + trackname);
                            Log("Error: " + ex.Message);
                        }
                    }
                }
                var rbhp_xkeys = false;
                var rbhp_ekeys = false;
                //combine upgrade with original
                for (var i = 0; i < upgrade.Events.Tracks; i++)
                {
                    var trackname = Tools.GetMidiTrackName(upgrade.Events[i][0].ToString());
                    if (!upgtracks.Contains(trackname) || (origtracks.Contains(trackname) && onlyAddNewTracks.Checked))
                        continue;
                    try
                    {
                        if (trackname.Contains("KEYS_X"))
                        {
                            if (!rbhp_xkeys)
                            {
                                midi.Events.AddTrack(upgrade.Events[i]);
                            }
                            rbhp_xkeys = true; //sometimes rbhp uses two pro keys x tracks for whatever reason, only add one
                            continue;
                        }
                        if (trackname.Contains("KEYS_E"))
                        {
                            if (!rbhp_ekeys)
                            {
                                midi.Events.AddTrack(upgrade.Events[i]);
                            }
                            rbhp_ekeys = true; //sometimes rbhp uses two pro keys e tracks for whatever reason, only add one
                            continue;
                        }
                        midi.Events.AddTrack(upgrade.Events[i]);
                    }
                    catch (Exception ex)
                    {
                        Log("There was an error processing the upgrade MIDI file");
                        Log("Error: " + ex.Message);
                    }
                }
            }
            try
            {
                MidiFile.Export(newMIDI, midi.Events);
            }
            catch (Exception ex)
            {
                Log("There was an error exporting the combined MIDI file");
                Log("Error: " + ex.Message);
            }
            if (File.Exists(newMIDI))
            {
                Log("Combined MIDI files successfully");
            }
            else
            {
                Log("There was an error creating the combined MIDI file");
                return;
            }
            if (string.IsNullOrWhiteSpace(orig_con))
            {
                Log("Process completed successfully");
                Log("Bundled MIDI file can be found in:");
                Log(newMIDI);
                return;
            }
            if (!string.IsNullOrWhiteSpace(newsongdta) && File.Exists(newsongdta))
            {
                Log("Replacing original songs.dta with RBHP songs.dta file");
                orig_dta = newsongdta; //if we got RBHP dta, completely replace original one
            }
            if (!string.IsNullOrWhiteSpace(newupgdta) && File.Exists(newupgdta))
            {
                Log("Merging upgrades.dta information into songs.dta file");
                var sr = new StreamReader(orig_dta, Encoding.Default);
                var sw = new StreamWriter(newDTA, false, Encoding.Default);
                var doneTuning = false;
                //first pass, replace values if already present
                while (sr.Peek() > 0)
                {
                    var line = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        if (line.Contains("rank"))
                        {
                            sw.WriteLine(line);
                            while (line != null && !line.Contains("format"))
                            {
                                line = sr.ReadLine();
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                if (line.Contains("bass") && !line.Contains("real_bass"))
                                {
                                    sw.WriteLine(line);
                                    sw.WriteLine(probassdiff);
                                    line = "";
                                }
                                else if (line.Contains("guitar") && !line.Contains("real_guitar"))
                                {
                                    sw.WriteLine(line);
                                    sw.WriteLine(proguitardiff);
                                    line = "";
                                }
                                else if (line.Contains("real_guitar") || line.Contains("real_bass"))
                                {
                                    line = "";
                                }
                                if (!string.IsNullOrWhiteSpace(line))
                                {
                                    sw.WriteLine(line);
                                }
                            }
                        }
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        if (line.Contains("real_guitar") || line.Contains("real_bass"))
                        {
                            line = "";
                        }
                        else if (line.Contains(";The following values"))
                        {
                            sw.WriteLine(proguitartuning);
                            sw.WriteLine(probasstuning);
                            doneTuning = true;
                        }
                        else if (line == ")" && !doneTuning)
                        {
                            if (sr.Peek() <= 0)
                            {
                                sw.WriteLine(proguitartuning);
                                sw.WriteLine(probasstuning);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        sw.WriteLine(line);
                    }
                }
                sr.Dispose();
                sw.Dispose();
                if (File.Exists(newDTA))
                {
                    Log("Merged DTA information successfully");
                }
                else
                {
                    Log("There was an error merging DTA information");
                    return;
                }
            }
            else
            {
                Tools.DeleteFile(newDTA);
                File.Copy(orig_dta, newDTA);
            }
            var vocals = harm3 ? 3 : (harm2 ? 2 : (harm1) ? 1 : 0);
            if (showSongIDPrompt.Checked)
            {
                var popup = new PasswordUnlocker(songID == NA || useUpgradeID.Checked ? upgradeID : songID);
                popup.IDChanger();
                popup.ShowDialog();
                var newID = popup.EnteredText;
                popup.Dispose();
                if (!string.IsNullOrWhiteSpace(newID))
                {
                    songID = newID;
                }
            }
            else if (useUpgradeID.Checked)
            {
                songID = upgradeID;
            }
            Tools.ReplaceSongID(newDTA, songID, vocals.ToString(CultureInfo.InvariantCulture));
            Log("Creating the bundled file");
            EnableDisable(false);
            btnBundle.Visible = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void CleanUp()
        {
            if (!cleanUpAfterBundlingFiles.Checked) return;
            Tools.DeleteFolder(temp_folder, true);
            Directory.CreateDirectory(temp_folder);
        }
        
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("pub");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }

        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }

        private void ProBundler_Shown(object sender, EventArgs e)
        {
            Log("Welcome to " + Text);
            Log("Drag and drop the files in the squares above to get started");
            Log("Ready to begin");
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!File.Exists(orig_con))
            {
                Log("CON file " + Path.GetFileName(orig_con) + " seems to have been deleted, can't continue without it");
                return;
            }

            var BundleFile = orig_con + " (bundled)";
            Tools.DeleteFile(BundleFile);
            File.Copy(orig_con, BundleFile);
            

            var xPackage = new STFSPackage(BundleFile);
            if (!xPackage.ParseSuccess)
            {
                Log("There was an error parsing CON file to bundle");
                xPackage.CloseIO();
                return;
            }

            var xent = xPackage.GetFile("/songs/songs.dta");
            if (xent != null)
            {
                if (xent.Replace(newDTA))
                {
                    Log("Bundled DTA file successfully");
                }
            }

            xent = xPackage.GetFile("/songs/" + Path.GetFileNameWithoutExtension(orig_midi) + "/" + Path.GetFileName(orig_midi));
            if (xent != null)
            {
                if (xent.Replace(newMIDI))
                {
                    Log("Bundled MIDI file successfully");
                }
            }

            xPackage.Header.MakeAnonymous();
            xPackage.Header.ThisType = PackageType.SavedGame;

            var success = false;
            try
            {
                Log("Rebuilding CON file ... this might take a little while");
                signature = new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
                if (ChangeGameID.Checked)
                {
                    xPackage.Header.TitleID = 0x45410914;
                    xPackage.Header.Title_Package = "Rock Band 3";
                    xPackage.Header.ContentImageBinary = Resources.RB3.ImageToBytes(ImageFormat.Png);
                }
                xPackage.RebuildPackage(signature);
                xPackage.FlushPackage(signature);
                xPackage.CloseIO();
                success = true;
            }
            catch (Exception ex)
            {
                Log("There was an error: " + ex.Message);
                xPackage.CloseIO();
            }

            if (success)
            {
                Log("Trying to unlock CON file");
                if (Tools.UnlockCON(BundleFile))
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
                Log("Trying to sign CON file");
                if (Tools.SignCON(BundleFile))
                {
                    Log("CON file signed successfully");
                }
                else
                {
                    Log("Error signing CON file");
                    success = false;
                }
            }

            Log(success ? "Your files were bundled successfully!" : "Something went wrong along the way, sorry!");
            
            if (!cleanUpAfterBundlingFiles.Checked) return;
            Log("Cleaning up");
            CleanUp();
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Log("Done");
            EnableDisable(true);
        }

        private void ProUpgradeBundler_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picWorking.Visible)
            {
                Tools.DeleteFolder(Application.StartupPath + "\\bundler\\", true);
                return;
            }
            MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        private void lstSong_DragDrop(object sender, DragEventArgs e)
        {
            var file = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (Path.GetExtension(file[0]).ToLowerInvariant() == ".mid")
            {
                if (UpgradeMidis.Contains(file[0]))
                {
                    DisplayConflict(file[0], "an upgrade MIDI");
                    return;
                }
                SongInstruments.Clear();
                lstSong.Items.Clear();
                orig_con = "";
                orig_midi = file[0];
                Log("Received MIDI file " + Path.GetFileName(orig_midi));
            }
            else if (VariousFunctions.ReadFileType(file[0]) == XboxFileType.STFS)
            {
                SongInstruments.Clear();
                orig_con = file[0];
                Log("Received CON file " + Path.GetFileName(orig_con) + ", searching for MIDI to extract");
                orig_midi = ExtractMIDI(file[0], false);
                if (string.IsNullOrWhiteSpace(orig_midi) || !File.Exists(orig_midi))
                {
                    orig_con = "";
                    orig_midi = "";
                    return;
                }
                lstSong.Items.Clear();
                Log("Extracted MIDI file " + Path.GetFileName(orig_midi));
                lstSong.Items.Add("CON File: " + Path.GetFileName(orig_con));
                lstSong.Items.Add("DTA File: songs.dta");
                lstSong.Items.Add("Song ID: " + songID);
                lstSong.Items.Add("Internal Name: " + song_int_name);
            }
            else
            {
                NotValid(file[0]);
                return;
            }

            lstSong.Items.Add("MIDI File: " + Path.GetFileName(orig_midi));
            Log("Reading MIDI file for contents...");
            ReadMIDIForContents(orig_midi, lstSong, "..........");

            Log("Ready");
            ValidateBundleButton();
        }

        private void ReadMIDIForContents(string midi, ListBox box, string leader)
        {
            var skip_tracks = ignoreNoninstrumentTracks.Checked ? new List<string> { "EVENTS", "VENUE", "BEAT" } : new List<string>();
            var midifile = Tools.NemoLoadMIDI(midi);
            if (midifile == null)
            {
                Log("Unable to load input MIDI file '" + Path.GetFileName(midi) + "'");
                return;
            }
            var counter = 0;
            var rbhp_xkeys = false;
            for (var i = 1; i < midifile.Events.Tracks; i++)
            {
                var trackname = Tools.GetMidiTrackName(midifile.Events[i][0].ToString());
                if (skip_tracks.Contains(trackname) || (trackname.Contains("KEYS_X") && rbhp_xkeys)) continue;
                
                if (trackname.Contains("KEYS_X"))
                {
                    rbhp_xkeys = true; //rbhp uses two pro keys x tracks for whatever reason, only add one
                }
                counter++;
                box.Items.Add(leader + trackname);
                if (box == lstSong && SongInstruments.Contains(trackname))
                {
                    MessageBox.Show("Duplicate track " + trackname + " found, this may cause problems in the game!",
                                    "Track Conflict", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    continue;
                }
                if (box == lstUpgrades && UpgradeInstruments.Contains(trackname))
                {
                    MessageBox.Show("Track " + trackname + " is already present in one of the upgrade files, one will overwrite the other!",
                        "Track Conflict", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    continue;
                }
                if (box == lstSong)
                {
                    SongInstruments.Add(trackname);
                }
                else if (box == lstUpgrades)
                {
                   UpgradeInstruments.Add(trackname);
                }
            }
            if (counter == 0) return;
            Log("Found " + counter + " " + (counter == 1 ? "track" : "tracks"));
        }

        private string ExtractMIDI(string con, bool isUpgrade)
        {
            var midi = "";
            
            var xPackage = new STFSPackage(con);
            if (!xPackage.ParseSuccess)
            {
                MessageBox.Show("There was an error parsing CON file, can't extract MIDI", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log("Can't work with this CON file, try again");
                return "";
            }

            try
            {
                var xent = xPackage.GetFolder("songs");
                if (xent == null && !isUpgrade)
                {
                    xent = xPackage.GetFolder("songs_upgrades");
                    MessageBox.Show(xent != null ? "This looks like a pro upgrade, only song files are valid here" : 
                        "I can't find a 'songs' folder in that CON file, make sure sure it's a Rock Band song file",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    xPackage.CloseIO();
                    Log("Can't work with this CON file, try again");
                    return "";
                }

                //we can't work with packs or pro upgrades, so check and skip
                xent = xPackage.GetFolder("songs_upgrades");
                if (xent != null && !isUpgrade)
                {
                    xent = xPackage.GetFolder("songs");
                    MessageBox.Show(xent != null ? "It looks like this is a pack, only individual song files are valid here"
                            : "This looks like a pro upgrade, only song files are valid here",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    xPackage.CloseIO();
                    Log("Can't work with this CON file, try again");
                    return "";
                }

                var folder = isUpgrade ? "songs_upgrades/" : "songs/";
                var dtaFile = isUpgrade ? "upgrades.dta" : "songs.dta";
                var dta = temp_folder + dtaFile;

                if (Parser.ExtractDTA(xPackage, false, isUpgrade))
                {
                    if (Parser.ReadDTA(Parser.DTA) && Parser.Songs.Count > 1)
                    {
                        MessageBox.Show("It looks like this is a pack, only individual song files are valid here",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        xPackage.CloseIO();
                        Log("Can't work with this CON file, try again");
                        return "";
                    }
                }

                var xFile = xPackage.GetFile(folder + dtaFile);
                if (xFile == null)
                {
                    MessageBox.Show("Can't find " + dtaFile + " inside this CON file\nI can't work without it", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    xPackage.CloseIO();
                    Log("Can't work with this CON file, try again");
                    return "";
                }
                
                var fileName = Path.GetFileName(con);
                if (fileName != null)
                {
                    if (!Parser.WriteDTAToFile(dta))
                    {
                        MessageBox.Show("Something went wrong in extracting the " + dtaFile + " file\nI can't work without it", Text,
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        xPackage.CloseIO();
                        Log("Can't work with this CON file, try again");
                        return "";
                    }
                }
                
                var artists = 0;
                var songName = "";

                var sr = new StreamReader(dta, Parser.GetDTAEncoding(Parser.DTA));
                // read one line at a time until the end
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(line.Trim())) continue;

                    if (line.ToLowerInvariant().Contains("artist") && !line.ToLowerInvariant().Contains(";") && !isUpgrade)
                    {
                        artists++;
                    }
                    else if (line.ToLowerInvariant().Contains("songs/") && !line.Contains("midi_file") && !isUpgrade && string.IsNullOrEmpty(songName))
                    {
                        songName = Parser.GetInternalName(line);
                        song_int_name = songName;
                    }
                    else if (line.Contains("song_id"))
                    {
                        if (isUpgrade)
                        {
                            upgradeID = Parser.GetSongID(line);
                        }
                        else
                        {
                            songID = Parser.GetSongID(line);
                            CheckIDMatch();
                        }
                    }
                    else if (line.Contains("midi_file") && isUpgrade)
                    {
                        var midipath = line.Replace("(", "");
                        midipath = midipath.Replace(")", "");
                        midipath = midipath.Replace("midi_file", "");
                        midipath = midipath.Replace("songs_upgrades", "");
                        midipath = midipath.Replace("\"", "");
                        midipath = midipath.Replace("/", "");
                        songName = midipath.Trim();
                        upgrade_int_name = songName.Replace(".mid","");
                    }
                }
                sr.Dispose();

                if (artists > 1) //if single song, packs will have values > 1
                {
                    MessageBox.Show("It looks like this is a pack, only individual song files are valid here",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    xPackage.CloseIO();
                    Log("Can't work with this CON file, try again");
                    return "";
                }

                xFile = xPackage.GetFile(folder + songName + (isUpgrade ? "" : "/" + songName + ".mid"));
                if (xFile != null)
                {
                    midi = temp_folder + songName + (isUpgrade? "" : ".mid");
                    Tools.DeleteFile(midi);
                    if (!xFile.ExtractToFile(midi))
                    {
                        MessageBox.Show("Can't find a MIDI file inside this CON file\nI can't work without it", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        xPackage.CloseIO();
                        Log("Can't work with this CON file, try again");
                        return "";
                    }
                }
                else
                {
                    MessageBox.Show("Failed to extract the MIDI file from the provided CON file", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Log("Failed to extract the MIDI file from the provided CON file");
                }
                xPackage.CloseIO();

                if (isUpgrade)
                {
                    newupgdta = dta;
                    ReadUpgDTA(newupgdta);
                }
                else
                {
                    orig_dta = dta;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error:\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                xPackage.CloseIO();
            }
            return midi;
        }

        private static void DisplayConflict(string file, string description)
        {
            MessageBox.Show("'" + Path.GetFileName(file) + "' is already in use as " + description + " file",
                                    "File Conflict", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private static bool WantToReplace(string description, string old_file, string new_file)
        {
            return MessageBox.Show("You can only add one " + description + " file\nYou already have one:\n'" + old_file +
                               "'\n\nDo you want to replace it with this one:\n'" + new_file + "'?", "File Conflict",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void lstUpgrades_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (var file in files)
            {
                var midi = "";
                if (Path.GetExtension(file).ToLowerInvariant() == ".mid")
                {
                    if (orig_midi == file)
                    {
                        DisplayConflict(file, "the original MIDI");
                        return;
                    }
                    if (UpgradeMidis.Contains(file))
                    {
                        DisplayConflict(file, "an upgrade MIDI");
                        return;
                    }
                    if (lstUpgrades.Items.Count > 0 && lstUpgrades.Items[0].ToString() == "No upgrades loaded...")
                    {
                        lstUpgrades.Items.Clear();
                    }
                    midi = file;
                    Log("Received MIDI file " + Path.GetFileName(midi));
                }
                else if (VariousFunctions.ReadFileType(file) == XboxFileType.STFS)
                {
                    if (!string.IsNullOrWhiteSpace(UpgradeCON))
                    {
                        if (!WantToReplace("upgrade CON", UpgradeCON, file)) return;
                    }
                    Log("Received CON file " + Path.GetFileName(file) + ", searching for MIDI to extract");
                    midi = ExtractMIDI(file, true);
                    if (string.IsNullOrWhiteSpace(midi) || !File.Exists(midi))
                    {
                        continue;
                    }
                    UpgradeCON = file;
                    UpgradeMidis.Clear();
                    UpgradeInstruments.Clear();
                    newsongdta = "";
                    Log("Extracted MIDI file " + Path.GetFileName(midi));
                    lstUpgrades.Items.Clear();
                    lstUpgrades.Items.Add("CON File: " + Path.GetFileName(file));
                    lstUpgrades.Items.Add("DTA File: upgrades.dta");
                    lstUpgrades.Items.Add("Upgrade ID: " + upgradeID);
                    lstUpgrades.Items.Add("Internal Name: " + upgrade_int_name);
                }
                else switch (Path.GetFileName(file))
                {
                    case "songs.dta":
                    case "upgrades.dta":
                        if (string.IsNullOrWhiteSpace(orig_con))
                        {
                            MessageBox.Show("You must add the original CON on the left before you can bundle DTA files", Text,
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        if (lstUpgrades.Items.Count > 0 && lstUpgrades.Items[0].ToString() == "No upgrades loaded...")
                        {
                            lstUpgrades.Items.Clear();
                        }
                        ReadUpgDTA(file);
                        switch (Path.GetFileName(file))
                        {
                            case "songs.dta":
                                if (!string.IsNullOrWhiteSpace(newsongdta))
                                {
                                    if (!WantToReplace("replacement songs.dta", newsongdta, file)) return;
                                }
                                newsongdta = file;
                                lstUpgrades.Items.Add("Song DTA File: " + Path.GetFileName(file));
                                lstUpgrades.Items.Add("Song ID: " + upgradeID);
                                break;
                            case "upgrades.dta":
                                if (!string.IsNullOrWhiteSpace(newupgdta))
                                {
                                    if (!WantToReplace("upgrades.dta", newupgdta, file)) return;
                                }
                                newupgdta = file;
                                lstUpgrades.Items.Add("Upgrade DTA File: " + Path.GetFileName(file));
                                lstUpgrades.Items.Add("Upgrade ID: " + upgradeID);
                                break;
                        }
                        break;
                    default:
                        NotValid(file);
                        break;
                }

                if (string.IsNullOrWhiteSpace(midi)) continue;
                lstUpgrades.Items.Add("MIDI File: " + Path.GetFileName(midi));
                UpgradeMidis.Add(midi);
                Log("Reading MIDI file for contents...");
                ReadMIDIForContents(midi, lstUpgrades, "..........");
            }

            Log("Ready");
            ValidateBundleButton();
        }

        private void NotValid(string file)
        {
            Log("Not a valid input file: " + Path.GetFileName(file));
            MessageBox.Show("That's not a valid file to drop here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ValidateBundleButton()
        {
            try
            {
                if (lstUpgrades.Items[0].ToString() != "No upgrades loaded..." && lstSong.Items[0].ToString() != "No CON or MIDI loaded...")
                {
                    btnBundle.Enabled = true;
                }
            }
            catch (Exception)
            {}
        }

        private void overwriteExistingTrack_Click(object sender, EventArgs e)
        {
            overwriteExistingTrack.Checked = true;
            onlyAddNewTracks.Checked = false;
        }

        private void onlyAddNewTracks_Click(object sender, EventArgs e)
        {
            overwriteExistingTrack.Checked = false;
            onlyAddNewTracks.Checked = true;
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
