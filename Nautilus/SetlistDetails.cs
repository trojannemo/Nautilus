using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Nautilus
{
    public partial class SetlistDetails : Form
    {
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        public List<SongData> Songs;
        public string ActiveConsole;
        public string ActiveSetlist;
        public int CachePackages;
        public List<ListViewItem> Stats;
        public bool isBlitzCache;

        public SetlistDetails(Form xParent, Color ButtonBackColor, Color ButtonTextColor)
        {
            var setlist = xParent;
            InitializeComponent();

            Tools = new NemoTools();
            Parser = new DTAParser();
            Stats = new List<ListViewItem>();
            
            Left = setlist.Left + ((setlist.Width - Width) / 2);
            Top = setlist.Top + ((setlist.Height - Height) / 2);
            ControlBox = false;

            var formButtons = new List<Button> { btnExport, btnClose };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        
        private void AddInfo(string name, string value)
        {
            var entry = new ListViewItem(name);
            entry.SubItems.Add(value);
            Stats.Add(entry);
        }
        
        private string CleanName(string name, bool keep_feat = false)
        {
            var clean = name;
            clean = Tools.FixFeaturedArtist(clean);
            var index = -1;
            if (!keep_feat)
            {
                if (clean.Contains("(ft."))
                {
                    index = clean.IndexOf("(ft.", StringComparison.Ordinal);
                }
                if (clean.Contains("[ft."))
                {
                    index = clean.IndexOf("[ft.", StringComparison.Ordinal);
                }
                else if (clean.Contains(" ft."))
                {
                    index = clean.IndexOf(" ft.", StringComparison.Ordinal);
                }
                if (index > -1)
                {
                    clean = clean.Substring(0, index).Trim();
                }
            }
            clean = clean.Replace("(Live)", "").Trim();
            clean = clean.Replace("(live)", "").Trim();
            clean = clean.Replace("(2X Bass Pedal)", "").Trim();
            clean = clean.Replace("(2x Bass Pedal)", "").Trim();
            clean = clean.Replace("(2X Bass)", "").Trim();
            clean = clean.Replace("(2x Bass)", "").Trim();
            clean = clean.Replace("(2X Pedal)", "").Trim();
            clean = clean.Replace("(2x Pedal)", "").Trim();
            clean = clean.Replace("(RB3 Version)", "").Trim();
            clean = clean.Replace("(RB3 version)", "").Trim();
            clean = clean.Replace("(Rhythm Version)", "").Trim();
            clean = clean.Replace("(rhythm version)", "").Trim();
            clean = clean.Replace("(Rhythm Guitar Version)", "").Trim();
            clean = clean.Replace("(rhythm guitar version)", "").Trim();
            clean = clean.Replace("(Rhythm Guitar)", "").Trim();
            clean = clean.Replace("(rhythm guitar)", "").Trim();
            return clean;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var sfd = new SaveFileDialog
                {
                    Title = "Select location to export to",
                    FileName = Tools.CleanString(ActiveSetlist, true).Replace("'", "").Replace(" ", "") + "_details.txt",
                    OverwritePrompt = true,
                    AddExtension = true,
                    InitialDirectory = desktop,
                    Filter = "Text Files (*.txt)|*txt"
                };

            if (sfd.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(sfd.FileName)) return;
            
            Tools.DeleteFile(sfd.FileName);

            var sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8);
            try
            {
                for (var i = 0; i < lstStats.Items.Count; i++)
                {
                    sw.WriteLine(FormatText(lstStats.Items[i].SubItems[0].Text, lstStats.Items[i].SubItems[1].Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error exporting the Setlist Details\nThe error says: " + ex.Message, Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                sw.Dispose();
                return;
            }
            sw.Dispose();

            if (MessageBox.Show("Setlist Details exported successfully\nDo you want to open the file now?", Text,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            Process.Start(sfd.FileName);
        }

        private static string FormatText(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(value)) return "";
            if (string.IsNullOrWhiteSpace(value)) return name.Trim();

            var spacer = "..............................";
            spacer = name.Trim().Length > spacer.Length ? "" : spacer.Substring(0, spacer.Length - name.Trim().Length);

            return name.Trim() + ":   " + spacer + "   " + value.Trim();
        }

        private void DoWait(bool wait)
        {
            var cursor = Cursors.Default;
            if (wait)
            {
                cursor = Cursors.WaitCursor;
            }

            lstStats.Cursor = cursor;
            Cursor = cursor;
            picloading.Cursor = cursor;

            btnClose.Enabled = !wait;
            btnExport.Enabled = !wait;
            chkPercent.Enabled = !wait;
            picloading.Visible = wait;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var rb1 = 0;
            var rb2 = 0;
            var rb3 = 0;
            var rb4 = 0;
            var rb4_dlc = 0;
            var dlc = 0;
            var rbn1 = 0;
            var rbn2 = 0;
            var greenday = 0;
            var beatles = 0;
            var acdc = 0;
            var customs = 0;
            var lego = 0;
            var ratingff = 0;
            var ratingm = 0;
            var ratingsr = 0;
            var male = 0;
            var female = 0;
            var master = 0;
            var live = 0;
            var rhythm = 0;
            var doublebass = 0;
            var rb3vers = 0;
            var drums = 0;
            var guitar = 0;
            var proguitar = 0;
            var bass = 0;
            var probass = 0;
            var keys = 0;
            var prokeys = 0;
            var vocals = 0;
            var harm2 = 0;
            var harm3 = 0;
            var min2 = 0;
            var min2to4 = 0;
            var min4to6 = 0;
            var min6to8 = 0;
            var min8to10 = 0;
            var min10plus = 0;
            long shortest = 0;
            var shortestsong = "";
            long longest = 0;
            var longestsong = "";
            var topartists = new List<string>();
            var featured = 0;
            var rerecord = 0;
            var before50s = 0;
            var the50s = 0;
            var the60s = 0;
            var the70s = 0;
            var the80s = 0;
            var the90s = 0;
            var the00s = 0;
            var the10s = 0;
            var oldest = 0;
            var newest = 0;
            var singles = 0;
            var warmup = 0;
            var apprentice = 0;
            var solid = 0;
            var moderate = 0;
            var challenging = 0;
            var nightmare = 0;
            var impossible = 0;
            var artists = new List<string>();
            var albums = new List<string>();
            var all_artists = new List<string>();
            var proguitartuning = 0;
            var probasstuning = 0;

            foreach (var song in Songs)
            {
                if (!string.IsNullOrWhiteSpace(song.ProGuitarTuning) && song.ProGuitarDiff > 0)
                {
                    proguitartuning++;
                }
                if (!string.IsNullOrWhiteSpace(song.ProBassTuning) && song.ProBassDiff > 0)
                {
                    probasstuning++;
                }
                switch (song.BandDiff)
                {
                    case 1:
                        warmup++;
                        break;
                    case 2:
                        apprentice++;
                        break;
                    case 3:
                        solid++;
                        break;
                    case 4:
                        moderate++;
                        break;
                    case 5:
                        challenging++;
                        break;
                    case 6:
                        nightmare++;
                        break;
                    case 7:
                        impossible++;
                        break;
                }
                switch (song.GetSource())
                {
                    case "RB1":
                        rb1++;
                        break;
                    case "RB2":
                        rb2++;
                        break;
                    case "RB3":
                        rb3++;
                        break;
                    case "RB4":
                        rb4++;
                        break;
                    case "RBN1":
                        rbn1++;
                        break;
                    case "RBN2":
                        rbn2++;
                        break;
                    case "DLC":
                        if (song.Source == "rb4_dlc")
                        {
                            rb4_dlc++;
                        }
                        else
                        {
                            dlc++; 
                        }
                        break;
                    case "TBRB":
                        beatles++;
                        break;
                    case "GDRB":
                        greenday++;
                        break;
                    case "Lego":
                        lego++;
                        break;
                    case "AC/DC":
                        acdc++;
                        break;
                    case "Custom":
                        customs++;
                        break;
                }
                switch (song.Rating)
                {
                    case 1:
                        ratingff++;
                        break;
                    case 2:
                        ratingsr++;
                        break;
                    case 3:
                        ratingm++;
                        break;
                }
                switch (song.GetGender())
                {
                    case "Female":
                        female++;
                        break;
                    case "Male":
                        male++;
                        break;
                }
                if (song.Master)
                {
                    master++;
                }
                var name = song.Name.ToLowerInvariant();
                if (name.Contains("rhythm") && (name.Contains("guitar") || name.Contains("version")))
                {
                    rhythm++;
                }
                if (name.Contains("(live)"))
                {
                    live++;
                }
                if (name.Contains("rb3 version"))
                {
                    rb3vers++;
                }
                if (name.Contains("2x") && (name.Contains("bass") || name.Contains("pedal")))
                {
                    doublebass++;
                }
                if (song.DrumsDiff > 0)
                {
                    drums++;
                }
                if (song.BassDiff > 0)
                {
                    bass++;
                }
                if (song.ProBassDiff > 0)
                {
                    probass++;
                }
                if (song.GuitarDiff > 0)
                {
                    guitar++;
                }
                if (song.ProGuitarDiff > 0)
                {
                    proguitar++;
                }
                if ((song.KeysDiff > 0 && song.ProKeysDiff > 0) || (song.KeysDiff > 0 && (!name.Contains("rhythm"))))
                {
                    keys++; //don't count rhythm on keys as "keys"
                }
                if (song.ProKeysDiff > 0)
                {
                    prokeys++;
                }
                if (song.VocalsDiff > 0)
                {
                    switch (song.VocalParts)
                    {
                        case 1:
                            vocals++;
                            break;
                        case 2:
                            harm2++;
                            break;
                        case 3:
                            harm3++;
                            break;
                    }
                }
                if (song.Length < shortest || shortest == 0)
                {
                    shortest = song.Length;
                    shortestsong = song.Artist + " - " + song.Name;
                }
                else if (song.Length > longest)
                {
                    longest = song.Length;
                    longestsong = song.Artist + " - " + song.Name;
                }
                if (song.Length <= 120000) //2 min
                {
                    min2++;
                }
                else if (song.Length > 120000 && song.Length <= 240000) //2 min to 4 min
                {
                    min2to4++;
                }
                else if (song.Length > 240000 && song.Length < 360000) //4 min to 6 min
                {
                    min4to6++;
                }
                else if (song.Length > 360000 && song.Length <= 480000) //6 min to 8 min
                {
                    min6to8++;
                }
                else if (song.Length > 480000 && song.Length <= 600000) //8 min to 10 min
                {
                    min8to10++;
                }
                else if (song.Length > 600000) //over 10 min
                {
                    min10plus++;
                }
                if (CleanName(song.Name,true).Contains("ft.") || CleanName(song.Artist,true).Contains("ft."))
                {
                    featured++;
                }
                if (song.YearRecorded > song.YearReleased)
                {
                    rerecord++;
                }
                var year = song.YearRecorded == 0 ? song.YearReleased : song.YearRecorded;
                if (year < 1950)
                {
                    before50s++;
                }
                else if (year >= 1950 && year < 1960)
                {
                    the50s++;
                }
                else if (year >= 1960 && year < 1970)
                {
                    the60s++;
                }
                else if (year >= 1970 && year < 1980)
                {
                    the70s++;
                }
                else if (year >= 1980 && year < 1990)
                {
                    the80s++;
                }
                else if (year >= 1990 && year < 2000)
                {
                    the90s++;
                }
                else if (year >= 2000 && year < 2010)
                {
                    the00s++;
                }
                else if (year > 2010)
                {
                    the10s++;
                }
                if (year > newest)
                {
                    newest = year;
                }
                if (year < oldest || oldest == 0)
                {
                    oldest = year;
                }
                if (string.IsNullOrWhiteSpace(song.Album))
                {
                    singles++;
                }
                //unique artists
                if (!artists.Contains(CleanName(song.Artist).ToLowerInvariant()))
                {
                    artists.Add(CleanName(song.Artist).ToLowerInvariant());
                }
                //unique albums
                if (!albums.Contains(song.Album.ToLowerInvariant()))
                {
                    albums.Add(song.Album.ToLowerInvariant());
                }
                //all artists for counting later
                all_artists.Add(CleanName(song.Artist));
            }
            
            var mostsongs = 0;
            foreach (var artist in all_artists)
            {
                var count = all_artists.Count(s => artist.ToLowerInvariant().Contains(s.ToLowerInvariant()));

                if (count == mostsongs && !topartists.Contains(artist))
                {
                    topartists.Add(artist);
                    mostsongs = count;
                }
                else if (count > mostsongs)
                {
                    topartists.Clear();
                    topartists.Add(artist);
                    mostsongs = count;
                }
            }

            //dynamically grab only the genres that are present and count them
            //rather than hardcoding all 30 possible Genres
            var genres = (from song in Songs where song.Genre != "" select song.Genre).ToList();
            genres.Sort();
            var Genres = new List<Genres>(); 
            foreach (var genre in genres)
            {
                var exists = false;
                var genre1 = genre;
                foreach (var gen in Genres.Where(gen => gen.Name == genre1))
                {
                    exists = true;
                    gen.Count++;
                    break;
                }
                if (exists || string.IsNullOrWhiteSpace(genre)) continue;
                Genres.Add(new Genres());
                Genres[Genres.Count - 1].Count = 1;
                Genres[Genres.Count - 1].Name = genre;
            }

            var limit = ActiveConsole == "Wii" ? "952" : (ActiveConsole == "Xbox 360" || ActiveConsole == "PS3" ? "2952" : "Unknown");
            var isRB4 = ActiveConsole == "PS4" || ActiveConsole == "Xbox One";

            var Duplicates = FindDuplicates();
            //duplicates include all instances, for every two songs one is unique, one is not
            Duplicates = Duplicates % 2 == 0 ? (Duplicates / 2) : ((Duplicates / 2) + 1);

            string game;
            switch (ActiveConsole)
            {
                case "Wii":
                    game = "Rock Band 3";                                    
                    break;
                case "YARG":
                    game = "YARG";
                    ActiveConsole = "PC";
                    break;
                case "Clone Hero":
                    game = "Clone Hero";
                    ActiveConsole = "PC";
                    break;
                default:
                    game = isBlitzCache ? "Rock Band: Blitz" : "Rock Band 3";
                    break;
            }

            AddInfo("Setlist Name", ActiveSetlist);
            AddInfo("Console", ActiveConsole);
            AddInfo("Game", game);
            if (!isBlitzCache && !isRB4)
            {
                AddInfo("Song Limit", limit.ToString(CultureInfo.InvariantCulture));
            }
            AddInfo("Songs in Setlist", Songs.Count.ToString(CultureInfo.InvariantCulture));
            if (ActiveConsole != "PS3" && !isRB4 && ActiveConsole != "PC")
            {
                AddInfo("Packages in Cache", CachePackages.ToString(CultureInfo.InvariantCulture));
            }
            AddInfo("Unique Artists", artists.Count.ToString(CultureInfo.InvariantCulture));
            AddInfo("Unique Songs", (Songs.Count - Duplicates).ToString(CultureInfo.InvariantCulture));
            AddInfo("Duplicate Songs", Duplicates.ToString(CultureInfo.InvariantCulture));

            AddInfo("", "");
            AddInfo("Songs by Source", "");
            AddInfo("Rock Band 1", rb1 + GetPercent(rb1));
            AddInfo("Rock Band 2", rb2 + GetPercent(rb2));
            AddInfo("Rock Band 3", rb3 + GetPercent(rb3));
            if (isRB4)
            {
                AddInfo("Rock Band 4", rb4 + GetPercent(rb4));
                AddInfo("Pre-RB4 DLC", dlc + GetPercent(dlc));
                AddInfo("RB4 DLC", rb4_dlc + GetPercent(rb4_dlc));
            }
            else
            {
                AddInfo("Official DLC", dlc + GetPercent(dlc));
            }
            AddInfo("Rock Band Network 1", rbn1 + GetPercent(rbn1));
            AddInfo("Rock Band Network 2", rbn2 + GetPercent(rbn2));
            AddInfo("Lego Rock Band", lego + GetPercent(lego));
            AddInfo("AC/DC Track Pack", acdc + GetPercent(acdc));
            AddInfo("Green Day: Rock Band", greenday + GetPercent(greenday));
            if (!isRB4)
            {
                AddInfo("The Beatles: Rock Band", beatles + GetPercent(beatles));
                AddInfo("Customs", customs + GetPercent(customs));
            }

            AddInfo("", "");
            AddInfo("Songs by Instruments", "");
            AddInfo("Guitar", guitar + GetPercent(guitar));
            if (!isRB4)
            {
                AddInfo("Pro Guitar", proguitar + GetPercent(proguitar));
            }
            AddInfo("Bass", bass + GetPercent(bass));
            if (!isRB4)
            {
                AddInfo("Pro Bass", probass + GetPercent(probass));
            }
            AddInfo("Drums", drums + GetPercent(drums));
            if (!isRB4)
            {
                AddInfo("Keys", keys + GetPercent(keys));
                AddInfo("Pro Keys", prokeys + GetPercent(prokeys));
            }
            AddInfo("1 Part Vocals", vocals + GetPercent(vocals));
            AddInfo("2 Part Harmonies", harm2 + GetPercent(harm2));
            AddInfo("3 Part Harmonies", harm3 + GetPercent(harm3));

            AddInfo("", "");
            AddInfo("Songs by Difficulty", "");
            AddInfo("Warmup", warmup + GetPercent(warmup));
            AddInfo("Apprentice", apprentice + GetPercent(apprentice));
            AddInfo("Solid", solid + GetPercent(solid));
            AddInfo("Moderate", moderate + GetPercent(moderate));
            AddInfo("Challenging", challenging + GetPercent(challenging));
            AddInfo("Nightmare", nightmare + GetPercent(nightmare));
            AddInfo("Impossible", impossible + GetPercent(impossible));

            AddInfo("", "");
            AddInfo("Songs by Genre", "");
            foreach (var genre in Genres)
            {
                AddInfo(genre.Name, genre.Count + GetPercent(genre.Count));
            }

            //following metrics are not available in Blitz cache files
            if (!isBlitzCache)
            {
                AddInfo("", "");
                AddInfo("Songs by Duration", "");
                AddInfo("Shortest Song", shortestsong);
                AddInfo("Shortest Song", Parser.GetSongDuration(shortest.ToString(CultureInfo.InvariantCulture)));
                AddInfo("Longest Song", longestsong);
                AddInfo("Longest Song", Parser.GetSongDuration(longest.ToString(CultureInfo.InvariantCulture)));
                AddInfo("2:00 or Less", min2 + GetPercent(min2));
                AddInfo("2:01 to 4:00", min2to4 + GetPercent(min2to4));
                AddInfo("4:01 to 6:00", min4to6 + GetPercent(min4to6));
                AddInfo("6:01 to 8:00", min6to8 + GetPercent(min6to8));
                AddInfo("8:01 to 10:00", min8to10 + GetPercent(min8to10));
                AddInfo("10:01 or More", min10plus + GetPercent(min10plus));
            
                AddInfo("", "");
                AddInfo("Songs by Rating", "");
                AddInfo("Family Friendly", ratingff + GetPercent(ratingff));
                AddInfo("Supervision Recommended", ratingsr + GetPercent(ratingsr));
                AddInfo("Mature", ratingm + GetPercent(ratingm));
                AddInfo("Not Rated",(Songs.Count - ratingff - ratingm - ratingsr) + GetPercent(Songs.Count - ratingff - ratingm - ratingsr));

                AddInfo("", "");
                AddInfo("Songs by Lead Singer", "");
                AddInfo("Feminine", female + GetPercent(female));
                AddInfo("Masculine", male + GetPercent(male));
                AddInfo("N/A (Instrumental)", (Songs.Count - male - female) + GetPercent(Songs.Count - male - female));
            }

            AddInfo("", "");
            AddInfo("Songs by Type", "");
            if (!isBlitzCache)
            {
                AddInfo("Master", master + GetPercent(master));
                AddInfo("Cover", (Songs.Count - master) + GetPercent(Songs.Count - master));
            }
            AddInfo("Single",singles + GetPercent(singles));
            AddInfo("Re-Record", rerecord + GetPercent(rerecord));
            AddInfo("Live", live + GetPercent(live));
            AddInfo("RB3 Version", rb3vers + GetPercent(rb3vers));
            if (!isRB4)
            {
                AddInfo("2X Bass Pedal", doublebass + GetPercent(doublebass));
                AddInfo("Rhythm Version", rhythm + GetPercent(rhythm));
            }

            AddInfo("", "");
            AddInfo("Songs by Decade", "");
            AddInfo("Oldest Song",oldest.ToString(CultureInfo.InvariantCulture));
            AddInfo("Newest Song", newest.ToString(CultureInfo.InvariantCulture));
            AddInfo("Before 1950", before50s + GetPercent(before50s));
            AddInfo("The 50s (1950-1959)", the50s + GetPercent(the50s));
            AddInfo("The 60s (1960-1969)", the60s + GetPercent(the60s));
            AddInfo("The 70s (1970-1979)", the70s + GetPercent(the70s));
            AddInfo("The 80s (1980-1989)", the80s + GetPercent(the80s));
            AddInfo("The 90s (1990-1999)", the90s + GetPercent(the90s));
            AddInfo("The 00s (2000-2009)", the00s + GetPercent(the00s));
            AddInfo("The 10s (2010+)", the10s + GetPercent(the10s));

            AddInfo("", "");
            AddInfo("Other Metrics", "");
            AddInfo("Most Songs by an Artist", mostsongs + GetPercent(mostsongs));
            foreach (var artist in topartists)
            {
                AddInfo(topartists.Count > 1? "Artists" : "Artist" + " With Most Songs", artist);
            }
            AddInfo("Unique Albums", albums.Count + GetPercent(albums.Count));
            AddInfo("Songs With Featured Artists",featured + GetPercent(featured));
            if (!isRB4)
            {
                AddInfo("Songs With Pro Guitar Tuning", proguitartuning + GetPercent(proguitartuning));
                AddInfo("Songs With Pro Bass Tuning", probasstuning + GetPercent(probasstuning));
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lstStats.BeginUpdate();
            foreach (var stat in Stats)
            {
                lstStats.Items.Add(stat);
            }
            lstStats.EndUpdate();
            DoWait(false);
        }

        private void SetlistStats_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private string GetPercent(int num1)
        {
            if (!chkPercent.Checked) return "";
            if (num1 == 0) return " (0 %)";

            var value = Math.Round((decimal)num1/Songs.Count*100, 2);

            return " (" + value + " %)";
        }

        private void chkPercent_CheckedChanged(object sender, EventArgs e)
        {
            lstStats.Items.Clear();
            Stats.Clear();
            DoWait(true);
            backgroundWorker1.RunWorkerAsync();
        }

        private int FindDuplicates()
        {
            var songs = Songs.Select(t => CleanName(t.Name).ToLowerInvariant()).ToList();
            var artists = Songs.Select(t => CleanName(t.Artist).ToLowerInvariant()).ToList();
            var duplicates = new List<int>();

            for (var i = 0; i < Songs.Count; i++)
            {
                var name = CleanName(Songs[i].Name).ToLowerInvariant();
                var artist = CleanName(Songs[i].Artist).ToLowerInvariant();
                if (!songs.Contains(name)) continue;
                if (artists[songs.IndexOf(name)] != artist) continue;
                if (songs.IndexOf(name) == i) continue;
                if (!duplicates.Contains(i))
                {
                    duplicates.Add(i);
                }
                if (!duplicates.Contains(songs.IndexOf(name)))
                {
                    duplicates.Add(songs.IndexOf(name));
                }
            }
            return duplicates.Count;
        }

        private void SetlistDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picloading.Visible) return;
            backgroundWorker1.CancelAsync();
            backgroundWorker1.Dispose();
        }
    }

    public class Genres
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public void Initiliaze()
        {
            Name = "";
            Count = 0;
        }
    }
}
