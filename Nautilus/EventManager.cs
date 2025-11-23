using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Nautilus.Properties;
using FontStyle = System.Drawing.FontStyle;
using Size = System.Drawing.Size;

namespace Nautilus
{
    public partial class EventManager : Form
    {
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private int MouseX;
        private int MouseY;
        private readonly string config;
        private readonly string em_data;
        private readonly List<Performance> Performances;
        private List<SongData> SetlistSongs;
        private string SetlistPath;
        private string Password;
        private readonly MainForm xMainForm;
        public  List<string> Bands;
        public  List<string> Artists;
        public  List<string> Songs;
        private int CurrentIndex;
        private bool bPlaying;
        private int iPlaying;
        private bool EditQueue;
        private bool EditPlaying;
        private bool EditPrevious;
        private int EditIndex;
        private int GroupIndex;
        private readonly string EventsFolder;
        private readonly string logheader;
        private const int MIN_HEIGHT = 720;
        private const int MIN_WIDTH = 1300;
        private const string AppName = "Event Manager";
        private bool AllowExit;
        private static Color mMenuBackground;
        private const string UNKNOWN_FIELD = "?????";
        private readonly TimeSpan defaultLength = new TimeSpan(0, 0, 4, 0);
        private const string defaultLengthString = "est. 4:00";
        private readonly TimeSpan defaultDelay = new TimeSpan(0, 0, 1, 0);
        private readonly DateTime C3rapture = new DateTime(2013, 4, 9, 10, 0, 0);

        public EventManager(MainForm xParent = null)
        {
            InitializeComponent();
            Tools = new NemoTools();
            Parser = new DTAParser();
            xMainForm = xParent;
            mMenuBackground = MenuStrip1.BackColor;
            MenuStrip1.Renderer = new DarkRenderer();

            Performances = new List<Performance>();
            SetlistSongs = new List<SongData>();
            Bands = new List<string>();
            Artists = new List<string>();
            Songs = new List<string>();

            CurrentIndex = -1;
            logheader = "//Event Log created by " + AppName + ". Do not modify this file manually.";
            
            EventsFolder = Application.StartupPath + "\\bin\\events\\";
            if (!Directory.Exists(EventsFolder))
            {
                Directory.CreateDirectory(EventsFolder);
            }
            
            config = Application.StartupPath + "\\bin\\config\\event.config";
            LoadConfig();

            em_data = EventsFolder + "event.dat";
            LoadData();

            var dice = Application.StartupPath + "\\res\\dice.gif";
            picRandom.Image = File.Exists(dice) ? Image.FromFile(dice) : Resources.random;

            picWorking.Visible = false;
        }
        
        private void LoadConfig()
        {
            if (!File.Exists(config)) return;

            var relaxSecurity = false;
            var sr = new StreamReader(config);
            try
            {
                sr.ReadLine();
                if (sr.ReadLine().Contains("True"))
                {
                    WindowState = FormWindowState.Maximized;
                    EventManager_Resize(null, null);
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                }
                else
                {
                    Width = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                    Height = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                    Left = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                    Top = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                }
                if (!sr.ReadLine().Contains("True"))
                {
                    ShowOnForm.PerformClick();
                }
                if (sr.ReadLine().Contains("True"))
                {
                    ShowOnTitleBar.PerformClick();
                }
                lblClock.Left = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                lblClock.Top = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                enableAutosuggest.Checked = sr.ReadLine().Contains("True");
                chkAutoQueue.Checked = sr.ReadLine().Contains("True");
                showEndTime.Checked = sr.ReadLine().Contains("True");
                lblEndTime.Left = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                lblEndTime.Top = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                relaxSecurity = sr.ReadLine().Contains("True");

                sr.Dispose();
            }
            catch (Exception)
            {
                sr.Dispose();
                Tools.DeleteFile(config);
            }
            relaxedSecurity.Checked = relaxSecurity;
            maximumSecurity.Checked = !relaxSecurity;
            RelocateClockLabels();
        }

        private void SaveConfig()
        {
            var sw = new StreamWriter(config, false);
            sw.WriteLine("//Do not modify this file manually");
            sw.WriteLine("StartMaximized=" + (WindowState == FormWindowState.Maximized));
            sw.WriteLine("Width=" + Width);
            sw.WriteLine("Height=" + Height);
            sw.WriteLine("Left=" + Left);
            sw.WriteLine("Top=" + Top);
            sw.WriteLine("ShowClockOnForm=" + ShowOnForm.Checked);
            sw.WriteLine("ShowOnTitleBar=" + ShowOnTitleBar.Checked);
            sw.WriteLine("ClockLeft=" + lblClock.Left);
            sw.WriteLine("ClockTop=" + lblClock.Top);
            sw.WriteLine("EnableAutoSuggest=" + enableAutosuggest.Checked);
            sw.WriteLine("EnableAutoQueue=" + chkAutoQueue.Checked); 
            sw.WriteLine("ShowEndTimeClock=" + showEndTime.Checked);
            sw.WriteLine("EndClockLeft=" + lblEndTime.Left);
            sw.WriteLine("EndClockTop=" + lblEndTime.Top);
            sw.WriteLine("RelaxedSecurity=" + relaxedSecurity.Checked);
            sw.Dispose();
        }

        private void LoadData()
        {
            if (!File.Exists(em_data)) return;

            var sr = new StreamReader(em_data);
            try
            {
                sr.ReadLine();
                SetlistPath = Tools.GetConfigString(sr.ReadLine());
                if (!string.IsNullOrWhiteSpace(SetlistPath) && File.Exists(SetlistPath))
                {
                    LoadSetlist(SetlistPath);
                }
                else
                {
                    SetlistPath = "";
                }
                sr.ReadLine();
                var counter = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                for (var i = 0; i < counter; i++)
                {
                    Bands.Add(sr.ReadLine());
                }
                sr.ReadLine();
                counter = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                for (var i = 0; i < counter; i++)
                {
                    Artists.Add(sr.ReadLine());
                }
                sr.ReadLine();
                counter = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                for (var i = 0; i < counter; i++)
                {
                    Songs.Add(sr.ReadLine());
                }
                sr.Dispose();

                UpdateBandSuggestions();
                UpdateArtistSuggestions();
                UpdateSongSuggestions();
            }
            catch (Exception)
            {
                sr.Dispose();
                Tools.DeleteFile(em_data);
            }
        }

        public void UpdateBandSuggestions()
        {
            if (!enableAutosuggest.Checked) return;
            Bands.Sort();
            var suggests = new AutoCompleteStringCollection();
            suggests.AddRange(Bands.ToArray());
            cboAddBand.AutoCompleteCustomSource = suggests;
            cboAddBand.DataSource = Bands;
            cboAddBand.SelectedIndex = -1;
        }

        public void UpdateArtistSuggestions()
        {
            if (!enableAutosuggest.Checked) return;
            Artists.Sort();
            var suggests = new AutoCompleteStringCollection();
            suggests.AddRange(Artists.ToArray());
            txtAddArtist.AutoCompleteCustomSource = suggests;
        }

        public void UpdateSongSuggestions()
        {
            if (!enableAutosuggest.Checked) return;
            Songs.Sort();
            var suggests = new AutoCompleteStringCollection();
            suggests.AddRange(Songs.ToArray());
            txtAddSong.AutoCompleteCustomSource = suggests;
        }

        private void SaveData()
        {
            var sw = new StreamWriter(em_data, false);
            sw.WriteLine("//Do not modify this file manually");
            sw.WriteLine("SetlistPath=" + SetlistPath);
            sw.WriteLine("//");
            sw.WriteLine("Bands=" + Bands.Count);
            {
                foreach (var line in Bands)
                {
                    sw.WriteLine(line);
                }
            }
            sw.WriteLine("//");
            sw.WriteLine("Artists=" + Artists.Count);
            {
                foreach (var line in Artists)
                {
                    sw.WriteLine(line);
                }
            }
            sw.WriteLine("//");
            sw.WriteLine("Songs=" + Songs.Count);
            {
                foreach (var line in Songs)
                {
                    sw.WriteLine(line);
                }
            }
            sw.Dispose();
        }

        private void EventManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AllowExit)
            {
                e.Cancel = true;
                return;
            }
            if (!picLocked.Visible)
            {
                SaveConfig();
                SaveData();
                SaveLog();
                return;
            }
            MessageBox.Show("You must unlock the program first", "Locked", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        private static string RemoveControlCharsFromString(string line)
        {
            return new string(line.Where(c => !char.IsControl(c)).ToArray());
        }

        private string ActiveConsole;
        private string ActiveSetlist;
        private int CachePackages;
        private bool isBlitzCache;
        private List<SongData> GrabSongsFromSetlist(string file, bool isLoading = true)
        {
            var SongsGrabbed = new List<SongData>();
            if (!File.Exists(file)) return SongsGrabbed;

            var line = "";
            var linenum = 5;
            var sr = new StreamReader(file, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            try
            {
                int songcount;
                string format;
                if (isLoading)
                {
                    ActiveSetlist = Tools.GetConfigString(sr.ReadLine());                    
                    Text = "Setlist Manager - " + ActiveSetlist;
                    ActiveConsole = Tools.GetConfigString(sr.ReadLine());
                    songcount = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                    CachePackages = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                    format = Tools.GetConfigString(sr.ReadLine()).ToLowerInvariant();
                    isBlitzCache = format.Contains("blitz");
                }
                else
                {
                    sr.ReadLine();
                    sr.ReadLine();
                    songcount = Convert.ToInt16(Tools.GetConfigString(sr.ReadLine()));
                    sr.ReadLine();
                    format = Tools.GetConfigString(sr.ReadLine()).ToLowerInvariant();
                }


                for (var i = 0; i < songcount; i++)
                {
                    try
                    {
                        SongsGrabbed.Add(new SongData());
                        var index = SongsGrabbed.Count - 1;

                        //all Setlist Manager cache formats
                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Artist = RemoveControlCharsFromString(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Name = RemoveControlCharsFromString(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Album = RemoveControlCharsFromString(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].TrackNumber = Convert.ToInt32(Tools.GetConfigString(line));
                        if (SongsGrabbed[index].TrackNumber == 65535) //Clone Hero bug???
                        {
                            SongsGrabbed[index].TrackNumber = 1;
                        }

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Master = line.Contains("True");

                        line = sr.ReadLine();
                        linenum++;
                        var year = Convert.ToInt16(Tools.GetConfigString(line));
                        SongsGrabbed[index].YearRecorded = year < 0 || year > 2100 ? 0 : year;

                        line = sr.ReadLine();
                        linenum++;
                        year = Convert.ToInt16(Tools.GetConfigString(line));
                        SongsGrabbed[index].YearReleased = year < 0 || year > 2100 ? 0 : year;

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Genre = Tools.GetConfigString(line);

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Rating = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Gender = Tools.GetConfigString(line);

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].VocalParts = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].DrumsDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].BassDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].ProBassDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].GuitarDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].ProGuitarDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].KeysDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].ProKeysDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].VocalsDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].BandDiff = Convert.ToInt16(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Length = Convert.ToInt32(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].ShortName = Tools.GetConfigString(line);

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].SongId = Convert.ToInt32(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].Source = Tools.GetConfigString(line);
                        if (string.IsNullOrWhiteSpace(SongsGrabbed[index].Source))
                        {
                            SongsGrabbed[index].Source = "dlc";
                        }

                        if (!format.Contains("2") && !format.Contains("3") && !format.Contains("4") && !format.Contains("5") && !format.Contains("6")) continue;
                        //Setlist Manager cache format 2-4, both RB3 and Blitz
                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].FilePath = Tools.GetConfigString(line);

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].PreviewStart = Convert.ToInt32(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].PreviewEnd = Convert.ToInt32(Tools.GetConfigString(line));

                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].GameVersion = Convert.ToInt16(Tools.GetConfigString(line));

                        if (!format.ToLowerInvariant().Contains("blitz"))
                        {
                            //Setlist Manager cache format 2-4, only RB3 
                            line = sr.ReadLine();
                            linenum++;
                            SongsGrabbed[index].ScrollSpeed = Convert.ToInt16(Tools.GetConfigString(line));

                            line = sr.ReadLine();
                            linenum++;
                            SongsGrabbed[index].TonicNote = Convert.ToInt16(Tools.GetConfigString(line));

                            line = sr.ReadLine();
                            linenum++;
                            SongsGrabbed[index].Tonality = Convert.ToInt16(Tools.GetConfigString(line));

                            line = sr.ReadLine();
                            linenum++;
                            SongsGrabbed[index].PercussionBank = Tools.GetConfigString(line);

                            line = sr.ReadLine();
                            linenum++;
                            SongsGrabbed[index].DrumBank = Tools.GetConfigString(line);
                        }

                        if (!format.Contains("3") && !format.Contains("4") && !format.Contains("5") && !format.Contains("6")) continue;
                        //Setlist Manager cache format 3-4, both RB3 and Blitz
                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].DoNotExport = line.Contains("True");

                        if (!format.Contains("4") && !format.Contains("5") && !format.Contains("6")) continue;
                        if (!format.ToLowerInvariant().Contains("blitz"))
                        {
                            //Setlist Manager cache format 4-5, only RB3
                            line = sr.ReadLine();
                            linenum++;
                            SongsGrabbed[index].ProBassTuning = Tools.GetConfigString(line);

                            //Setlist Manager cache format 4-5, only RB3
                            line = sr.ReadLine();
                            linenum++;
                            SongsGrabbed[index].ProGuitarTuning = Tools.GetConfigString(line);
                        }

                        if (!format.Contains("5") && !format.Contains("6")) continue;
                        //Setlist Manager cache format 5, RB3 and Blitz
                        line = sr.ReadLine();
                        linenum++;
                        SongsGrabbed[index].SongLink = Tools.GetConfigString(line);

                        if (!format.Contains("6"))
                        {
                            SongsGrabbed[index].DateAdded = C3rapture;
                            continue;
                        }
                        line = sr.ReadLine();
                        linenum++;
                        string dateText = Tools.GetConfigString(line);
                        if (DateTime.TryParse(dateText, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        {
                            SongsGrabbed[index].DateAdded = parsedDate;
                        }
                        else
                        {
                            SongsGrabbed[index].DateAdded = C3rapture;
                        }

                        //add further checks for newer cache versions here
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was a problem loading song #" + (i + 1) + " in Setlist file:\n'" + Path.GetFileName(file) + "'\n\nThe error says:\n'" +
                            ex.Message + "'\n\nLine:\t'" + line + "'\nLine #:\t" + linenum + "\n\nSkipping this song...", AppName, MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        SongsGrabbed.RemoveAt(SongsGrabbed.Count - 1);

                        //calculate how many lines until next song, then read/skip those lines
                        var lines = (5 + ((i + 1) * 34)) - linenum;
                        for (var x = 0; x < lines; x++)
                        {
                            sr.ReadLine();
                        }
                    }
                }
                sr.Dispose();                
            }
            catch (Exception ex)
            {
                sr.Dispose();
                SongsGrabbed.Clear();
                MessageBox.Show("There was a problem loading Setlist '" + Path.GetFileName(file) + "'\n\nThe error says:\n'" +
                                ex.Message + "'\n\nTry re-importing if this problem continues", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                picWorking.Visible = false;
            }
            return SongsGrabbed;
        }

        private bool LoadSetlist(string file)
        {
            SetlistSongs = GrabSongsFromSetlist(file);
            viewSetlistDetails.Visible = SetlistSongs.Any();
            txtSearch.Enabled = SetlistSongs.Any();
            lstSearch.Enabled = SetlistSongs.Any();
            if (!SetlistSongs.Any()) return false;
            DoSearch("");
            return true;
        }

        private void EventManager_Resize(object sender, EventArgs e)
        {
            var mywidth = (Width - 60)/3;
            const int spacer1 = 11; //spaces between the group boxes
            const int spacer2 = 10; //spaces between controls inside each group box, etc

            if (Width < picMarquee.Width + (picLocked.Width*2) + 30) //'need at least marquee + lock sign to fit
            {
                Width = picMarquee.Width + (picLocked.Width*2) + 30;
                return;
            }
            
            if (Height < MIN_HEIGHT) //preset size that we need
            {
                Height = MIN_HEIGHT;
                return;
            }
            if (Width < MIN_WIDTH)
            {
                Width = MIN_WIDTH; //preset size that we need
                return;
            }

            grpNew.Width = mywidth;
            grpNew.Left = spacer1;

            grpNext.Width = grpNew.Width;
            grpNext.Left = grpNew.Left + grpNew.Width + spacer1;
            btnEdit.Left = (grpNext.Width - btnEdit.Width)/2;
            
            grpPrev.Width = grpNew.Width;
            grpPrev.Left = grpNext.Left + grpNext.Width + spacer1;
            btnEditPrev.Left = btnDetails.Left + btnDetails.Width + ((btnDelPrev.Left - (btnDetails.Left + btnDetails.Width) - btnEditPrev.Width)/2);
            
            txtAddArtist.Top = lstNext.Top + lstNext.Height - txtAddArtist.Height;
            lblAddArtist.Top = txtAddArtist.Top - lblAddArtist.Height;
            txtAddSong.Top = lblAddArtist.Top - txtAddSong.Height - 5;
            lblAddSong.Top = txtAddSong.Top - lblAddSong.Height;
            //cboAddBand.Top = lblAddSong.Top - cboAddBand.Height - 5;
            //lblAddBand.Top = cboAddBand.Top - lblAddBand.Height;
            //txtSearch.Top = cboAddBand.Top + cboAddBand.Height + 5;
            lstSearch.Height = lblAddSong.Top - 5 - lstSearch.Top;

            picLocked.Left = picPin.Left - picLocked.Width - 10;
            picUnlocked.Left = picLocked.Left;
            
            picMarquee.Left = (Width - picMarquee.Width)/2;
            btnStart.Left = picMarquee.Left + (spacer2*2);
            btnStop.Left = picMarquee.Left + picMarquee.Width - btnStop.Width - (spacer2*2);

            var spacer3 = (btnStop.Left - (btnStart.Left + btnStart.Width) - (btnEditPlay.Width*2))/3;
            btnSkip.Left = btnStart.Left + btnStart.Width + spacer3;
            btnEditPlay.Left = btnSkip.Left + btnSkip.Width + spacer3;
            
            RedrawMarqueeLabels();
            var sm = GetSystemMenu(Handle, false);
            EnableMenuItem(sm, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED);
        }

        private void lblClock_MouseDown(object sender, MouseEventArgs e)
        {
            ((Label)(sender)).Cursor = Cursors.NoMove2D;
            MouseX = MousePosition.X;
            MouseY = MousePosition.Y;
        }

        private void lblClock_MouseMove(object sender, MouseEventArgs e)
        {
            var label = (Label) sender;
            if (label.Cursor == Cursors.Default)
            {
                return;
            }
            if (MousePosition.X != MouseX)
            {
                if (MousePosition.X > MouseX)
                {
                    label.Left = label.Left + (MousePosition.X - MouseX);
                }
                else
                {
                    label.Left = label.Left - (MouseX - MousePosition.X);
                }
            }
            if (MousePosition.Y != MouseY)
            {
                if (MousePosition.Y > MouseY)
                {
                    label.Top = label.Top + (MousePosition.Y - MouseY);
                }
                else
                {
                    label.Top = label.Top - (MouseY - MousePosition.Y);
                }
            }
            MouseX = MousePosition.X;
            MouseY = MousePosition.Y;
            RelocateClockLabels();
        }

        private void RelocateClockLabels()
        {
            lblClockDesc.Left = lblClock.Left + ((lblClock.Width-lblClockDesc.Width)/2);
            lblClockDesc.Top = lblClock.Top - lblClockDesc.Height + 1;
            lblEndTimeDesc.Left = lblEndTime.Left + ((lblEndTime.Width-lblEndTimeDesc.Width) / 2);
            lblEndTimeDesc.Top = lblEndTime.Top - lblEndTimeDesc.Height + 1;
        }

        private void lblClock_MouseUp(object sender, MouseEventArgs e)
        {
            ((Label)(sender)).Cursor = Cursors.Default;
        }
        
        private void ShowOnFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblClock.Visible = ShowOnForm.Checked;
            tmrClock.Enabled = ShowOnForm.Checked || ShowOnTitleBar.Checked || showEndTime.Checked;
        }

        private void ShowOnTitleBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ShowOnTitleBar.Checked)
            {
                Text = AppName;
            }
            tmrClock.Enabled = ShowOnForm.Checked || ShowOnTitleBar.Checked || showEndTime.Checked;
        }

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            var clock = GetFormattedTime(DateTime.Now);

            if (ShowOnForm.Checked)
            {
                lblClock.Text = clock;
                lblClock.Visible = true;
            }
            if (showEndTime.Checked)
            {
                var durationToEnd = new TimeSpan(0, 0, 0, 0);
                for (var i = 0; i < Performances.Count; i++)
                {
                    if (Performances[i].Played) continue;
                    durationToEnd += defaultDelay;
                    var index = Performances[i].SetlistIndex;
                    if (index > -1 && SetlistSongs.Count >= index)
                    {
                        try
                        {
                            var songLength = TimeSpan.FromMilliseconds(SetlistSongs[index].Length);
                            durationToEnd += songLength;
                        }
                        catch (Exception)
                        {
                            durationToEnd += defaultLength;
                        }
                    }
                    else
                    {
                        durationToEnd += defaultLength;
                    }
                }
                lblEndTime.Text = GetFormattedTime(DateTime.Now + durationToEnd);
                lblEndTime.Visible = true;
            }

            if (!ShowOnTitleBar.Checked) return;
            Text = AppName + " - " + clock;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllowExit = true;
            Close();
        }

        private void picLocked_Click(object sender, EventArgs e)
        {
            LockForm(true);
        }

        private void LockForm(bool unlock)
        {
            const string masterPass = "theansweris42";

            var password = new PasswordUnlocker();
            password.LockManager();
            password.ShowDialog();

            var pass = password.EnteredText;
            if (string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(pass)) goto doExit;
            
            if (unlock)
            {
                if (pass != Password && pass != masterPass) goto doExit;
                picLocked.Visible = false;
            }
            else
            {
                Password = pass;
                picLocked.Visible = true;
            }
            
            try
            {
                var locked = picLocked.Visible;
                picUnlocked.Visible = !locked;
                if (xMainForm != null)
                {
                    xMainForm.isLocked = locked;
                }

                if (!locked)
                {
                    TopMost = false;
                    MenuStrip1.Enabled = true;
                    btnPlay.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnDelPrev.Visible = true;
                    btnEditPrev.Visible = true;
                    btnStart.Visible = true;
                    btnStop.Visible = true;
                    btnEditPlay.Visible = true;
                    btnSkip.Visible = true;
                    MinimizeBox = true;
                    MaximizeBox = true;
                    picRandom.Visible = true;
                    btnLoadSetlist.Visible = true;
                    chkAutoQueue.Enabled = true;
                }
                else
                {
                    TopMost = true;
                    MenuStrip1.Enabled = false;
                    btnDelPrev.Visible = false;
                    btnEditPrev.Visible = false;
                    MinimizeBox = false;
                    MaximizeBox = false;
                    picRandom.Visible = false;
                    btnLoadSetlist.Visible = false;
                    chkAutoQueue.Enabled = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnEditPlay.Visible = false;
                    
                    btnPlay.Visible = relaxedSecurity.Checked;
                    btnStart.Visible = relaxedSecurity.Checked;
                    btnSkip.Visible = relaxedSecurity.Checked;
                    btnStop.Visible = relaxedSecurity.Checked;
                }

                if (locked && maximumSecurity.Checked)
                {
                    picUp.Visible = false;
                    picDown.Visible = false;
                }
            }
            catch (Exception)
            {}

doExit:
            var sm = GetSystemMenu(Handle, false);
            EnableMenuItem(sm, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED);
        }

        private void picUnlocked_Click(object sender, EventArgs e)
        {
            LockForm(false);
        }

        private void RedrawMarqueeLabels()
        {
            lblNowPlayingBand.Parent = picMarquee;
            lblNowPlayingSong.Parent = picMarquee;
            lblNowPlayingArtist.Parent = picMarquee;

            lblNowPlayingBand.Left = (picMarquee.Width - lblNowPlayingBand.Width)/2;
            lblNowPlayingSong.Left = lblNowPlayingBand.Left;
            lblNowPlayingArtist.Left = lblNowPlayingBand.Left;

            lblNowPlayingBand.Top = 87;
            lblNowPlayingSong.Top = lblNowPlayingBand.Top + lblNowPlayingBand.Height;
            lblNowPlayingArtist.Top = lblNowPlayingSong.Top + lblNowPlayingSong.Height;
        }

        //disable close button
        [DllImport("user32")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        const int MF_BYCOMMAND = 0;
        const int MF_DISABLED = 2;
        const int SC_CLOSE = 0xF060;

        private void EventManager_Load(object sender, EventArgs e)
        {
            EventManager_Resize(sender, e);
            var sm = GetSystemMenu(Handle, false);
            EnableMenuItem(sm, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED);

            if (File.Exists(Application.StartupPath + "\\res\\marquee.png"))
            {
                picMarquee.BackgroundImage = Tools.NemoLoadImage(Application.StartupPath + "\\res\\marquee.png");
            }
            else
            {
                MessageBox.Show("Could not find marquee.png image in the /res folder \n" + AppName + " won't look too pretty now",
                    AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            RedrawMarqueeLabels();
            lblNowPlayingArtist.Text = "";
            lblNowPlayingSong.Text = "";
            lblNowPlayingBand.Text = "";
            ToolTip1.SetToolTip(lblNowPlayingBand, "");
            ToolTip1.SetToolTip(lblNowPlayingSong, "");
            ToolTip1.SetToolTip(lblNowPlayingArtist, "");
            
            //add tooltip messages to all controls
            ToolTip1.SetToolTip(btnAdd, "Click to add performance to the queue list");
            ToolTip1.SetToolTip(cboAddBand, "Enter the player's band name here");
            ToolTip1.SetToolTip(txtAddSong, "Enter the song's name here");
            ToolTip1.SetToolTip(txtAddArtist, "Enter the name of the original artist / band here");
            ToolTip1.SetToolTip(btnPlay, "Click to send above");
            ToolTip1.SetToolTip(btnEdit, "Click to edit this performance");
            ToolTip1.SetToolTip(btnEditPrev, "Click to edit this performance");
            ToolTip1.SetToolTip(btnEditPlay, "Click to edit this performance");
            ToolTip1.SetToolTip(btnDelete, "Click to delete this performance. CAN'T BE UNDONE!");
            ToolTip1.SetToolTip(btnDelPrev, "Click to delete this performance. CAN'T BE UNDONE!");
            ToolTip1.SetToolTip(btnDetails, "Click to view performance details");
            ToolTip1.SetToolTip(btnStart, "Click to start this performance");
            ToolTip1.SetToolTip(btnSkip, "Click to skip this performance and send to the queue list");
            ToolTip1.SetToolTip(btnStop, "Click to stop this performance");
            ToolTip1.SetToolTip(picLocked, "Click to unlock the program");
            ToolTip1.SetToolTip(picUnlocked, "Click to lock the program");
            ToolTip1.SetToolTip(picMarquee, "Details of the performance will appear here");
        }

        private void selectSetlistFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Application.StartupPath + "\\setlist\\",
                Title = "Select Setlist file",
                Multiselect = false,
                Filter = "Setlist Files (*.setlist)|*setlist"
            };
            ofd.ShowDialog();
            if (string.IsNullOrWhiteSpace(ofd.FileName)) return;
            lstSearch.Items.Clear();
            if (LoadSetlist(ofd.FileName))
            {
                MessageBox.Show("Setlist file '" + ActiveSetlist + "' loaded successfully\n" + string.Format("{0:n0}", SetlistSongs.Count) + " songs were loaded", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetlistPath = ofd.FileName;
            }
            else
            {
                MessageBox.Show("There was an error loading setlist file '" + ActiveSetlist + "'", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void txtAddSong_Click(object sender, EventArgs e)
        {
            lstNext.SelectedIndex = -1;
            lstPrevious.SelectedIndex = -1;
            txtAddSong.Focus();
        }

        private void txtAddArtist_Click(object sender, EventArgs e)
        {
            lstNext.SelectedIndex = -1;
            lstPrevious.SelectedIndex = -1;
            txtAddArtist.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //avoid keeping info from song that is no longer selected
            if (lstSearch.SelectedItems.Count == 0)
            {
                CurrentIndex = -1;
            }

            int index;
            if (btnAdd.Text == "Save")
            {
                index = EditIndex;
            }
            else
            {
                Performances.Add(new Performance());
                index = Performances.Count - 1;
                Performances[index].Initialize();
                Performances[index].AddedTime = GetFormattedTime(DateTime.Now);
            }

            Performances[index].BandName = cboAddBand.Text;
            Performances[index].SongName = string.IsNullOrWhiteSpace(txtAddSong.Text) ? UNKNOWN_FIELD : txtAddSong.Text;
            Performances[index].ArtistName = string.IsNullOrWhiteSpace(txtAddArtist.Text) ? UNKNOWN_FIELD : txtAddArtist.Text;
            Performances[index].SetlistIndex = CurrentIndex;

            string length;
            if (CurrentIndex > -1)
            {
                length = " (" + Parser.GetSongDuration(SetlistSongs[CurrentIndex].Length.ToString(CultureInfo.InvariantCulture)) + ")";
            }
            else
            {
                length = " (" + defaultLengthString + ")";
            }

            if (btnAdd.Text == "Add to &Queue >>>")
            {
                lstNext.Items.Add(Performances.Count + ". " + Performances[index].BandName + " - " + Performances[index].SongName + length);
            }
            else
            {
                if (lstSearch.SelectedIndex == -1)
                {
                    Performances[index].SetlistIndex = -1;
                    length = " (" + defaultLengthString + ")";
                }
                if (EditQueue)
                {
                    lstNext.Items[GroupIndex] = (index + 1) + ". " + Performances[index].BandName + " - " + Performances[index].SongName + length;
                }
                else if (EditPrevious)
                {
                    lstPrevious.Items[GroupIndex] = (index + 1) + ". " + Performances[index].BandName + " - " + Performances[index].SongName;
                }
                else if (EditPlaying)
                {
                    iPlaying = index;
                    UpdateNowPlaying();
                }
            }

            //avoid crash that would happen because no item was selected
            if (EditQueue && btnAdd.Text == "Save" && lstNext.SelectedItems.Count == 0 && GroupIndex > -1)
            {
                lstNext.SelectedItem = lstNext.Items[GroupIndex];
            }

            if (!Bands.Contains(cboAddBand.Text))
            {
                Bands.Add(cboAddBand.Text);
                UpdateBandSuggestions();
            }
            if (!Artists.Contains(txtAddArtist.Text) && !string.IsNullOrWhiteSpace(txtAddArtist.Text))
            {
                Artists.Add(txtAddArtist.Text);
                UpdateArtistSuggestions();
            }
            if (!Songs.Contains(txtAddSong.Text) && !string.IsNullOrWhiteSpace(txtAddSong.Text))
            {
                Songs.Add(txtAddSong.Text);
                UpdateSongSuggestions();
            }

            txtAddArtist.Text = "";
            cboAddBand.Text = "";
            txtAddSong.Text = "";
            txtSearch.Text = "Click to search Setlist...";
            btnSong.Enabled = false;
            btnAdd.Text = "Add to &Queue >>>";
            
            lstSearch.SelectedIndex = -1;
            CurrentIndex = -1;
            grpNew.BackColor = Color.Transparent;
        }
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lstSearch.Items.Clear();
            btnSong.Enabled = false;
            CurrentIndex = -1;
            string search;
            if (txtSearch.Text == "Click to search Setlist..." || string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                search = "";
            }
            else
            {
                search = txtSearch.Text.ToLowerInvariant();
            }
            DoSearch(search);
        }

        private void DoSearch(string search)
        {
            lstSearch.BeginUpdate();
            for (var i = 0; i < SetlistSongs.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(search) || SetlistSongs[i].Name.ToLowerInvariant().Contains(search) ||
                SetlistSongs[i].Artist.ToLowerInvariant().Contains(search))
                {
                    lstSearch.Items.Add(SetlistSongs[i].Artist + " - " + SetlistSongs[i].Name + " [#" + i + "]");
                }
            }
            lstSearch.EndUpdate();
        }

        private void UpdateNowPlaying()
        {
            const int max_width = 570;

            lblNowPlayingBand.Visible = false;
            lblNowPlayingSong.Visible = false;
            lblNowPlayingArtist.Visible = false;

            lblNowPlayingBand.Text = Performances[iPlaying].BandName.Trim().ToUpperInvariant();
            lblNowPlayingSong.Text = "'" + Performances[iPlaying].SongName.Trim().ToUpperInvariant() + "'";
            lblNowPlayingArtist.Text = "by " + Performances[iPlaying].ArtistName.Trim().ToUpperInvariant();

            ToolTip1.SetToolTip(lblNowPlayingBand, lblNowPlayingBand.Text);
            ToolTip1.SetToolTip(lblNowPlayingSong, lblNowPlayingSong.Text);
            ToolTip1.SetToolTip(lblNowPlayingArtist, lblNowPlayingArtist.Text);

            Single fSize;
            var f = new Font(lblNowPlayingBand.Font.FontFamily, 36, FontStyle.Bold);
            var band_name = TextRenderer.MeasureText(lblNowPlayingBand.Text, f);
            if (band_name.Width > max_width)
            {
                var factor = (float)max_width/band_name.Width;
                fSize = 36*factor;
            }
            else
            {
                fSize = 36;
            }

            lblNowPlayingBand.Font = new Font(f.FontFamily, fSize, FontStyle.Bold);
            f = new Font(lblNowPlayingBand.Font.FontFamily, 26, FontStyle.Bold);
            var song_name = TextRenderer.MeasureText(lblNowPlayingSong.Text, f);
            if (song_name.Width > max_width)
            {
                var factor = (float) max_width/song_name.Width;
                fSize = 26*factor;
            }
            else
            {
                fSize = 26;
            }

            lblNowPlayingSong.Font = new Font(f.FontFamily, fSize, FontStyle.Bold);
            f = new Font(lblNowPlayingBand.Font.FontFamily, 16, FontStyle.Bold);
            var artist_name = TextRenderer.MeasureText(lblNowPlayingArtist.Text, f);
            if (artist_name.Width > max_width)
            {
                var factor = max_width/artist_name.Width;
                fSize = 16*factor;
            }
            else
            {
                fSize = 16;
            }

            lblNowPlayingArtist.Font = new Font(f.FontFamily, fSize, FontStyle.Bold);

            lblNowPlayingBand.Visible = true;
            lblNowPlayingSong.Visible = true;
            lblNowPlayingArtist.Visible = true;

            btnStart.Enabled = true;
            btnSkip.Enabled = true;
            btnEditPlay.Enabled = true;
            btnStop.Enabled = false;
        }

        private void lstSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSearch.SelectedIndex == -1)
            {
                btnSong.Enabled = false;
                return;
            }
            var item = lstSearch.Items[lstSearch.SelectedIndex].ToString();
            var index1 = item.IndexOf("[#", StringComparison.Ordinal) + 2;
            var index2 = item.IndexOf("]", StringComparison.Ordinal);
            CurrentIndex = Convert.ToInt16(item.Substring(index1, index2 - index1));
            txtAddArtist.Text = SetlistSongs[CurrentIndex].Artist;
            txtAddSong.Text = SetlistSongs[CurrentIndex].Name;
            btnSong.Enabled = lstSearch.Items.Count > 0;
        }

        private void txtSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtSearch.Text == "Click to search Setlist...")
            {
                txtSearch.Text = "";
            }
            //lstSearch.SelectedIndex = -1;
            //CurrentIndex = -1;
        }

        private void lstNext_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            }
            btnPlay.Enabled = lstNext.SelectedIndex != - 1 && lstNext.SelectedItems.Count == 1;
            btnEdit.Enabled = lstNext.SelectedIndex != -1 && lstNext.SelectedItems.Count == 1;
            btnDelete.Enabled = lstNext.SelectedIndex != -1 && lstNext.SelectedItems.Count == 1;
            if (picLocked.Visible && maximumSecurity.Checked)
            {
                picUp.Visible = false;
                picDown.Visible = false;
            }
            else
            {
                picUp.Visible = lstNext.SelectedIndex > 0;// && picUnlocked.Visible;
                picDown.Visible = lstNext.SelectedIndex > -1 && lstNext.SelectedIndex <= lstNext.Items.Count - 2;// && picUnlocked.Visible;
            }

            var index = lstNext.SelectedIndex;
            lstPrevious.SelectedIndex = -1;
            lstNext.SelectedIndex = index;
        }

        private void lstPrevious_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            }
            btnDetails.Enabled = lstPrevious.SelectedIndex != -1;
            btnDelPrev.Enabled = lstPrevious.SelectedIndex != -1;
            btnEditPrev.Enabled = lstPrevious.SelectedIndex != -1;

            var index = lstPrevious.SelectedIndex;
            lstNext.SelectedIndex = -1;
            lstPrevious.SelectedIndex = index;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            }
            if (MessageBox.Show("Are you sure you want to do that?\nDeleted entries can't be recovered", AppName,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var entry = lstNext.SelectedItem.ToString();
            var index = Convert.ToInt16(entry.Substring(0, entry.IndexOf(".", StringComparison.Ordinal))) - 1;
            Performances[index].Deleted = true;
            lstNext.Items.RemoveAt(lstNext.SelectedIndex);
        }

        private void picUp_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            }
            var new_index = lstNext.SelectedIndex - 1;
            MoveSongEntries(new_index);
        }

        private void MoveSongEntries(int new_index)
        {
            var current_index = lstNext.SelectedIndex;

            var entry = lstNext.Items[new_index];
            lstNext.Items[new_index] = lstNext.Items[current_index];
            lstNext.Items[current_index] = entry;
            lstNext.SelectedIndex = new_index;
        }

        private void picDown_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            }
            var new_index = lstNext.SelectedIndex + 1;
            MoveSongEntries(new_index);
        }

        private void btnAdd_EnabledChanged(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = ((Button)sender).Enabled ? Color.Lavender : Color.LightGray;
        }

        private void btnPlay_EnabledChanged(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = ((Button)sender).Enabled ? Color.PaleGreen : Color.LightGray;
        }

        private void btnEdit_EnabledChanged(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = ((Button)sender).Enabled ? Color.LightCyan : Color.LightGray;
        }

        private void btnDelete_EnabledChanged(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = ((Button)sender).Enabled ? Color.LightCoral : Color.LightGray;
        }

        private void btnSkip_EnabledChanged(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = ((Button)sender).Enabled ? Color.Yellow : Color.LightGray;
        }

        private void btnDetails_EnabledChanged(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = ((Button)sender).Enabled ? Color.Thistle : Color.LightGray;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            }

            if (lstNext.SelectedItems.Count == 0 || lstNext.SelectedIndex == -1)
            {
                MessageBox.Show("Oops, something's wrong!\nYou clicked to start playing but no act is selected - try again", AppName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (btnStart.Enabled) //Act was sent up to marquee but hadn't pressed Play yet
            {
                btnStart.PerformClick();
            }

            if (bPlaying) //Act was playing but never pressed Stop or was just sent up by code above, stop and save
            {
                btnStop.PerformClick();
            }

            var entry = lstNext.SelectedItem.ToString();
            int iNext = Convert.ToInt16(entry.Substring(0, entry.IndexOf(".", StringComparison.Ordinal)));
            iPlaying = iNext - 1;

            UpdateNowPlaying();
            lstNext.Items.RemoveAt(lstNext.SelectedIndex);
            cboAddBand.Focus();
            bPlaying = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            } 
            
            lblNowPlayingArtist.Text = "";
            lblNowPlayingBand.Text = "";
            lblNowPlayingSong.Text = "";
            ToolTip1.SetToolTip(lblNowPlayingBand, "");
            ToolTip1.SetToolTip(lblNowPlayingSong, "");
            ToolTip1.SetToolTip(lblNowPlayingArtist, "");

            Performances[iPlaying].StopTime = GetFormattedTime(DateTime.Now);
            lstPrevious.Items.Insert(0, (iPlaying + 1) + ". " + Performances[iPlaying].BandName + " - " + Performances[iPlaying].SongName);
            bPlaying = false;

            if (chkAutoQueue.Checked)
            {
                Performances.Add(new Performance());
                var index = Performances.Count -1;
                Performances[index].Initialize();
                Performances[index].AddedTime = GetFormattedTime(DateTime.Now);
                Performances[index].BandName = Performances[iPlaying].BandName;
                Performances[index].SongName = UNKNOWN_FIELD;
                Performances[index].ArtistName = UNKNOWN_FIELD;
                Performances[index].SetlistIndex = -1;
                lstNext.Items.Add(Performances.Count + ". " + Performances[index].BandName + " - " + UNKNOWN_FIELD);
            }

            btnStart.Enabled = false;
            btnSkip.Enabled = false;
            btnEditPlay.Enabled = false;
            btnStop.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            } 
            
            EditQueue = true;
            EditPlaying = false;
            EditPrevious = false;
            GroupIndex = lstNext.SelectedIndex;
            DoEdit();
        }

        private void DoEdit()
        {
            btnAdd.Text = "Save";
            grpNew.BackColor = Color.LightGoldenrodYellow;

            var index = iPlaying;
            string entry;

            if (EditQueue)
            {
                entry = lstNext.SelectedItem.ToString();
                index = Convert.ToInt16(entry.Substring(0, entry.IndexOf(".", StringComparison.Ordinal))) - 1;
                CurrentIndex = Performances[index].SetlistIndex;
                EditIndex = index;
            }
            else if (EditPrevious)
            {
                entry = lstPrevious.SelectedItem.ToString();
                index = Convert.ToInt16(entry.Substring(0, entry.IndexOf(".", StringComparison.Ordinal))) - 1;
                CurrentIndex = Performances[index].SetlistIndex;
                EditIndex = index;
            }
            
            if (Performances[index].SetlistIndex >= 0 && Performances.Count > 0)
            {
                lstSearch.Items.Clear();
                var name = SetlistSongs[Performances[index].SetlistIndex].Artist + " - " + SetlistSongs[Performances[index].SetlistIndex].Name;
                txtSearch.Text = name;
                lstSearch.Items.Add(name + " [#" + Performances[index].SetlistIndex + "]");
                lstSearch.SelectedIndex = 0;
            }
            cboAddBand.Text = Performances[index].BandName;
            txtAddArtist.Text = Performances[index].ArtistName;
            txtAddSong.Text = Performances[index].SongName;
            cboAddBand.Focus();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            } 
            
            btnStart.Enabled = false;
            btnSkip.Enabled = false;
            btnEditPlay.Enabled = false;
            btnStop.Enabled = true;

            Performances[iPlaying].PlayTime = GetFormattedTime(DateTime.Now);
            Performances[iPlaying].Played = true;
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            } 
            
            Performances[iPlaying].TimesSkipped++;
            string length;
            if (Performances[iPlaying].SetlistIndex > -1)
            {
                length = " (" + Parser.GetSongDuration(SetlistSongs[Performances[iPlaying].SetlistIndex].Length.ToString(CultureInfo.InvariantCulture)) + ")";
            }
            else
            {
                length = "";
            }

            lstNext.Items.Add((iPlaying + 1) + ". " + Performances[iPlaying].BandName + " - " + Performances[iPlaying].SongName + length);

            lblNowPlayingArtist.Text = "";
            lblNowPlayingSong.Text = "";
            lblNowPlayingBand.Text = "";
            ToolTip1.SetToolTip(lblNowPlayingBand, "");
            ToolTip1.SetToolTip(lblNowPlayingSong, "");
            ToolTip1.SetToolTip(lblNowPlayingArtist, "");

            bPlaying = false;

            btnStart.Enabled = false;
            btnSkip.Enabled = false;
            btnEditPlay.Enabled = false;
            btnStop.Enabled = false;
        }

        private void btnEditPlay_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            }

            EditQueue = false;
            EditPlaying = true;
            EditPrevious = false;
            GroupIndex = -1;
            EditIndex = iPlaying;
            DoEdit();
        }

        private static string GetFormattedTime(DateTime date)
        {
            var hour = "0" + (date.Hour == 0 ? 12 : (date.Hour > 12 ? date.Hour - 12 : date.Hour)).ToString(CultureInfo.InvariantCulture);
            var minute = "0" + date.Minute;
            var second = "0" + date.Second;
            
            var formatted = hour.Substring(hour.Length - 2, 2) + ":" + minute.Substring(minute.Length - 2, 2) + ":"
                    + second.Substring(second.Length - 2, 2) + (date.Hour > 11 ? " PM" : " AM");

            return formatted;
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            } 
            var item = lstPrevious.SelectedItem.ToString();
            var index = Convert.ToInt16(item.Substring(0, item.IndexOf(".", StringComparison.Ordinal))) - 1;
            ShowPerformanceDetails(index);
        }

        private void ShowPerformanceDetails(int index, bool isWinner = false)
        {
            var winnerText = isWinner ? "WINNER:             " + Performances[index].BandName.ToUpperInvariant() +
                "\n______________________________\n\nDetails:\n" : "";
            var message = winnerText + "Band Name:      " + Performances[index].BandName +
                          "\nSong:                  " + Performances[index].SongName +
                          "\nArtist:                  " + Performances[index].ArtistName + 
                          (isWinner ? "" : "\n______________________________") +
                          "\n\nTime Added:      " + Performances[index].AddedTime +
                          "\nStart Time:          " + Performances[index].PlayTime +
                          "\nEnd Time:           " + Performances[index].StopTime +
                          "\nPerformed?        " + (Performances[index].Played ? "Yes" : "No") +
                          "\nSkipped?             " + (Performances[index].TimesSkipped == 0 ? "No"
                              : ("Yes (" + Performances[index].TimesSkipped +
                                 (Performances[index].TimesSkipped == 1 ? " time" : " times") + ")"));
            MessageBox.Show(message, isWinner ? "***** WINNER *****" : "Performance Details", MessageBoxButtons.OK);
        }

        private void btnDelPrev_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            }

            if (MessageBox.Show("Are you sure you want to do that?\nDeleted entries can't be recovered", AppName,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var entry = lstPrevious.SelectedItem.ToString();
            var index = Convert.ToInt16(entry.Substring(0, entry.IndexOf(".", StringComparison.Ordinal))) - 1;
            Performances[index].Deleted = true;
            lstPrevious.Items.RemoveAt(lstPrevious.SelectedIndex);
        }

        private void btnEditPrev_Click(object sender, EventArgs e)
        {
            if (grpNew.BackColor != Color.Transparent)
            {
                cboAddBand.Focus();
                return;
            } 
            
            EditQueue = false;
            EditPlaying = false;
            EditPrevious = true;
            GroupIndex = lstPrevious.SelectedIndex;
            DoEdit();
        }

        private void btnSong_EnabledChanged(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = ((Button)sender).Enabled ? Color.YellowGreen : Color.LightGray;
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Click to search Setlist...";
            }
        }

        private void btnSong_Click(object sender, EventArgs e)
        {
            if (CurrentIndex == -1) return;

            var message = "Song:                       " + SetlistSongs[CurrentIndex].Name + 
                          "\nArtist:                       " + SetlistSongs[CurrentIndex].Artist +
                          "\nLength:                     " + Parser.GetSongDuration(SetlistSongs[CurrentIndex].Length.ToString(CultureInfo.InvariantCulture)) +
                          "\nRating:                      " + SetlistSongs[CurrentIndex].GetRating() +
                          "\nVocal Parts:              " + SetlistSongs[CurrentIndex].VocalParts +
                          "\nVoc. Gender:            " + SetlistSongs[CurrentIndex].GetGender() +
                          "\n______________________________\n" +
                          "\nHas Drums:              " + (SetlistSongs[CurrentIndex].DrumsDiff > 0 ? "Yes" : "No") +
                          "\nHas Guitar:               " + (SetlistSongs[CurrentIndex].GuitarDiff > 0 ? "Yes" : "No") +
                          "\nHas Pro Guitar:        " + (SetlistSongs[CurrentIndex].ProGuitarDiff > 0 ? "Yes" : "No") +
                          "\nHas Bass:                  " + (SetlistSongs[CurrentIndex].BassDiff > 0 ? "Yes" : "No") +
                          "\nHas Pro Bass:           " + (SetlistSongs[CurrentIndex].ProBassDiff > 0 ? "Yes" : "No") +
                          "\nHas Keys:                  " + (SetlistSongs[CurrentIndex].KeysDiff > 0 ? "Yes" : "No") +
                          "\nHas Pro Keys:           " + (SetlistSongs[CurrentIndex].ProKeysDiff > 0 ? "Yes" : "No");
            MessageBox.Show(message, "Song Details", MessageBoxButtons.OK);
        }

        private void SaveLog()
        {
            if (Performances.Count == 0) return;

            var filename = EventsFolder + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour +
                           DateTime.Now.Minute + DateTime.Now.Second + ".log";

            var sw = new StreamWriter(filename, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            sw.WriteLine(logheader);
            sw.WriteLine("LogDate=" + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(DateTime.Now.Month) + " " + DateTime.Now.Day + ", " + DateTime.Now.Year);
            sw.WriteLine("TotalPerformances=" + Performances.Count);
            foreach (var performance in Performances)
            {
                sw.WriteLine(performance.BandName + "," + performance.SongName + "," + performance.ArtistName + "," + performance.AddedTime + "," + 
                    performance.PlayTime + "," + performance.StopTime + "," + performance.TimesSkipped + "," + performance.Deleted);
            }
            sw.Dispose();
        }

        private void ViewLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveLog();
            var logviewer = new LogViewer();
            logviewer.ShowDialog();
        }

        private void HeloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("em");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }
        
        private void resetSize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Size = new Size(MIN_WIDTH, MIN_HEIGHT);
        }

        private void viewSetlistDetails_Click(object sender, EventArgs e)
        {
            var details = new SetlistDetails(this, Color.FromArgb(200, 230, 215, 0), Color.White)
            {
                Songs = SetlistSongs,
                ActiveConsole = ActiveConsole,
                ActiveSetlist = ActiveSetlist,
                CachePackages = CachePackages,
                isBlitzCache = isBlitzCache
            };
            details.ShowDialog();
        }

        private void enableAutosuggest_Click(object sender, EventArgs e)
        {
            if (enableAutosuggest.Checked)
            {
                enableAutosuggest.Checked = false;
                txtAddArtist.AutoCompleteCustomSource.Clear();
                txtAddSong.AutoCompleteCustomSource.Clear();
                cboAddBand.AutoCompleteCustomSource.Clear();
            }
            else
            {
                enableAutosuggest.Checked = true;
                UpdateArtistSuggestions();
                UpdateBandSuggestions();
                UpdateSongSuggestions();
            }

            editAutosuggestArtists.Enabled = enableAutosuggest.Checked;
            editAutosuggestBands.Enabled = enableAutosuggest.Checked;
            editAutosuggestSongs.Enabled = enableAutosuggest.Checked;
        }

        private void editAutosuggestBands_Click(object sender, EventArgs e)
        {
            var editor = new EventManagerEditor(this, 0);
            editor.ShowDialog();
        }

        private void editAutosuggestSongs_Click(object sender, EventArgs e)
        {
            var editor = new EventManagerEditor(this, 1);
            editor.ShowDialog();
        }

        private void editAutosuggestArtists_Click(object sender, EventArgs e)
        {
            var editor = new EventManagerEditor(this, 2);
            editor.ShowDialog();
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

        private void txtAddSong_KeyUp(object sender, KeyEventArgs e)
        {
            lstSearch.SelectedIndex = -1;
        }

        private void lstSearch_DoubleClick(object sender, EventArgs e)
        {
            if (lstSearch.Items.Count > 0 && lstSearch.SelectedIndex > -1)
            {
                btnSong_Click(sender, e);
            }
        }

        private void lstPrevious_DoubleClick(object sender, EventArgs e)
        {
            if (lstPrevious.Items.Count > 0 && lstPrevious.SelectedIndex > -1)
            {
                btnDetails_Click(sender, e);
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

        private void picRandom_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (lstPrevious.Items.Count == 0)
            {
                MessageBox.Show("How can I choose a winner if nobody has played yet?\nTry again later", AppName,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var random = new Random();
            var winner = random.Next(0, lstPrevious.Items.Count);
            var item = lstPrevious.Items[winner].ToString();
            var index = Convert.ToInt16(item.Substring(0, item.IndexOf(".", StringComparison.Ordinal))) - 1;
            ShowPerformanceDetails(index, true);
        }

        private void lblClock_VisibleChanged(object sender, EventArgs e)
        {
            lblClockDesc.Visible = lblClock.Visible;
        }

        private void lblEndTime_VisibleChanged(object sender, EventArgs e)
        {
            lblEndTimeDesc.Visible = lblEndTime.Visible;
        }

        private void btnLoadSetlist_Click(object sender, EventArgs e)
        {
            selectSetlistFile_Click(sender, e);
        }

        private void cboAddBand_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(cboAddBand.Text);
        }

        private void cboAddBand_Click(object sender, EventArgs e)
        {
            lstNext.SelectedIndex = -1;
            lstPrevious.SelectedIndex = -1;
        }

        private void lstSearch_EnabledChanged(object sender, EventArgs e)
        {
            btnLoadSetlist.Visible = !lstSearch.Enabled;
        }

        private void cboAddBand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || string.IsNullOrWhiteSpace(cboAddBand.Text)) return;
            if (txtSearch.Enabled)
            {
                txtSearch.Focus();
            }
            else
            {
                txtAddSong.Focus();
            }
        }

        private void lstNext_DoubleClick(object sender, EventArgs e)
        {
            if (lstNext.Items.Count > 0 && lstNext.SelectedIndex > -1)
            {
                btnPlay_Click(sender, e);
            }
        }

        private void maximumSecurity_Click(object sender, EventArgs e)
        {
            maximumSecurity.Checked = !maximumSecurity.Checked;
            relaxedSecurity.Checked = !maximumSecurity.Checked;
        }

        private void relaxedSecurity_Click(object sender, EventArgs e)
        {
            relaxedSecurity.Checked = !relaxedSecurity.Checked;
            maximumSecurity.Checked = !relaxedSecurity.Checked;
        }
    }

    public class Performance
    {
        public string BandName { get; set; }
        public string ArtistName { get; set; }
        public string SongName { get; set; }

        public int SetlistIndex { get; set; }
        public int TimesSkipped { get; set; }

        public string AddedTime { get; set; }
        public string PlayTime { get; set; }
        public string StopTime { get; set; }

        public bool Played { get; set; }
        public bool Deleted { get; set; }
        
        public void Initialize()
        {
            BandName = "";
            ArtistName = "";
            SongName = "";
            SetlistIndex = -1;
            TimesSkipped = 0;
            AddedTime = "N/A";
            PlayTime = "N/A";
            StopTime = "N/A";
            Played = false;
            Deleted = false;
        }
    }
}
