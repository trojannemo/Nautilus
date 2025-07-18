using NAudio.Midi;
using Nautilus.x360;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;

namespace Nautilus
{
    public partial class CDGConverter : Form
    {
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private MoggSplitter moggSplitter;
        private List<TempoEvent> TempoEvents;
        private List<TimeSignature> TimeSignatures;
        private int TicksPerQuarter;
        private List<KaraokeLyric> VocalLyrics;
        private List<KaraokeLyric> Harm1Lyrics;
        private List<KaraokeLyric> Harm2Lyrics;
        private List<KaraokeLyric> Harm3Lyrics;
        private List<MIDINote> Harm1Notes;
        private List<MIDINote> Harm2Notes;
        private List<MIDINote> Harm3Notes;
        private List<MIDINote> VocalNotes;
        private List<LyricPhrase> VocalPhrases;
        private List<string> inputFiles;
        private string backgroundColor = "";
        private string textColor1 = "";
        private string highlightColor1 = "";
        private string textColor2 = "";
        private string highlightColor2 = "";
        private string textColor3 = "";
        private string highlightColor3 = "";
        private int fileCounter = 0;
        private int successCounter = 0;
        private int failureCounter = 0;
        private string failedSongs = "";
        private string vocalOption = "";
        private bool doKeepTOML;
        private string cdgOutput = "";
        private bool workSilently = true;
        private bool doHarmonies = true;
        private readonly string configFile;
        private PrivateFontCollection pfc = new PrivateFontCollection();
        private string ActiveFont;
        private int trackLength = 0;

        List<string> karaokeColorHexes = new List<string>
        {
            "#000000", //Black
            "#FFFFFF", //White
            "#FF0000", //Red
            "#00FF00", //Green
            "#2299FF", //Approximate DodgerBlue
            "#FFFF00", //Yellow
            "#00FFFF", //Cyan
            "#FF00FF", //Magenta
            "#888888", //Gray
            "#FF8800", //Orange
            "#FF88FF", //Pink
            "#8800FF", //Purple
            "#008888", //Teal
            "#88FF00", //Lime
            "#000088", //Navy
            "#884400"  //Brown
        };

        public CDGConverter()
        {
            InitializeComponent();

            Tools = new NemoTools();
            Parser = new DTAParser();
            moggSplitter = new MoggSplitter();
            inputFiles = new List<string>();
            NewFile();
            configFile = Application.StartupPath + "\\bin\\config\\karaoke.config";
        }

        private void NewFile()
        {
            VocalLyrics = new List<KaraokeLyric>();
            VocalNotes = new List<MIDINote>();
            VocalPhrases = new List<LyricPhrase>();
            Harm1Lyrics = new List<KaraokeLyric>();
            Harm2Lyrics = new List<KaraokeLyric>();
            Harm3Lyrics = new List<KaraokeLyric>();            
            Harm1Notes = new List<MIDINote>();
            Harm2Notes = new List<MIDINote>();
            Harm3Notes = new List<MIDINote>();
        }
        
        private void SaveConfig()
        {
            var sw = new StreamWriter(configFile, false);
            sw.WriteLine(cboBackground.SelectedIndex.ToString());
            sw.WriteLine(cboTextColor1.SelectedIndex.ToString());
            sw.WriteLine(cboTextHighlight1.SelectedIndex.ToString());
            sw.WriteLine(cboTextColor2.SelectedIndex.ToString());
            sw.WriteLine(cboTextHighlight2.SelectedIndex.ToString());
            sw.WriteLine(cboTextColor3.SelectedIndex.ToString());
            sw.WriteLine(cboTextHighlight3.SelectedIndex.ToString());
            sw.WriteLine(cboFont.SelectedIndex.ToString());
            sw.WriteLine(radioRemove.Checked ? "True" : "False");
            sw.WriteLine(radioKeep.Checked ? "True" : "False");
            sw.WriteLine(chkKeepTOML.Checked ? "True" : "False");
            sw.WriteLine(chkHarmonies.Checked ? "True" : "False");
            sw.Dispose();
        }

        private void LoadConfig()
        {
            if (!File.Exists(configFile)) return;
            var sr = new StreamReader(configFile);
            try
            {
                cboBackground.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboTextColor1.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboTextHighlight1.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboTextColor2.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboTextHighlight2.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboTextColor3.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboTextHighlight3.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboFont.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                var remove = sr.ReadLine().Contains("True");
                if (remove)
                {
                    radioRemove.Checked = true;
                    radioKeep.Checked = false;
                }
                else
                {
                    radioRemove.Checked = false;
                    radioKeep.Checked = true;
                }
                sr.ReadLine();
                chkKeepTOML.Checked = sr.ReadLine().Contains("True");
                chkHarmonies.Checked = sr.ReadLine().Contains("True");
            }
            catch 
            {
                sr.Dispose();
                Tools.DeleteFile(configFile);
                return;
            }
            sr.Dispose();
        }

        private void CDGConverter_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void CDGConverter_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (VariousFunctions.ReadFileType(files[0]) != XboxFileType.STFS)
            {
                MessageBox.Show("That's not a valid file to drop here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            inputFiles = files.ToList();
            backgroundColor = karaokeColorHexes[cboBackground.SelectedIndex];
            textColor1 = karaokeColorHexes[cboTextColor1.SelectedIndex];
            highlightColor1 = karaokeColorHexes[cboTextHighlight1.SelectedIndex];
            textColor2 = karaokeColorHexes[cboTextColor2.SelectedIndex];
            highlightColor2 = karaokeColorHexes[cboTextHighlight2.SelectedIndex];
            textColor3 = karaokeColorHexes[cboTextColor3.SelectedIndex];
            highlightColor3 = karaokeColorHexes[cboTextHighlight3.SelectedIndex];
            doKeepTOML = chkKeepTOML.Checked;
            doHarmonies = chkHarmonies.Checked;
            ActiveFont = cboFont.Text.Split('|')[1].Trim();
            var remove = "drums|bass|guitar|keys|backing|NOcrowd";
            var keep = "allstems|NOcrowd";
            vocalOption = radioRemove.Checked ? remove : keep;
            EnableDisable(false);
            backgroundWorker1.RunWorkerAsync();
        }

        private void EnableDisable(bool enable)
        {
            cboBackground.Enabled = enable;
            cboFont.Enabled = enable;
            cboTextColor1.Enabled = enable;
            cboTextColor2.Enabled = enable;
            cboTextColor3.Enabled = enable;
            cboTextHighlight1.Enabled = enable;
            cboTextHighlight2.Enabled = enable;
            cboTextHighlight3.Enabled = enable;
            btnHelp.Enabled = enable;
            radioRemove.Enabled = enable;
            radioKeep.Enabled = enable;
            chkKeepTOML.Enabled = enable;
            picWorking.Visible = !enable;
            chkHarmonies.Enabled = enable;
        }

        private string CleanString(string str)
        {
            return str.Replace("#", "").Replace("^", "").Replace("\"", "").Replace("§", "_").Replace(",", "").Replace("$", "");
        }

        private void ProcessFile(string file)
        {
            NewFile();
            Parser.ExtractDTA(file);
            Parser.ReadDTA(Parser.DTA);

            if (Parser.Songs.Count > 1)
            {
                MessageBox.Show("This is a pack, dePACK first then try again","Uhhh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Parser.Songs.Any())
            {
                MessageBox.Show("Couldn't find any songs in that file","Uhhh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Parser.Songs[0].VocalParts == 0)
            {
                MessageBox.Show("That song has no vocals", "Uhhh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var title = CleanString(Parser.Songs[0].Artist) + " - " + CleanString(Parser.Songs[0].Name);
            trackLength = Parser.Songs[0].Length;

            var folder = Path.GetDirectoryName(file) + "\\Karaoke\\" + title + "\\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var wav = folder + title + ".wav";
            var mp3 = folder + title + ".mp3";
            var cdg = folder + title + ".cdg";
            var toml = folder + title + ".toml";

            moggSplitter.DownmixMogg(file, wav, MoggSplitter.MoggSplitFormat.WAV, vocalOption);

            var xPackage = new STFSPackage(file);

            if (!xPackage.ParseSuccess)
            {
                MessageBox.Show("Could not parse that file");
                return;
            }

            var xFile = xPackage.GetFile("songs/" + Parser.Songs[0].InternalName + "/" + Parser.Songs[0].InternalName + ".mid");
            if (xFile == null)
            {
                MessageBox.Show("Could not find MIDI file");
                xPackage.CloseIO();
                return;
            }
            var midi = Path.GetTempPath() + Parser.Songs[0].InternalName + ".mid";
            Tools.DeleteFile(midi);
            if (!xFile.ExtractToFile(midi))
            {
                MessageBox.Show("Could not extract MIDI file");
                xPackage.CloseIO();
                return;
            }

            xPackage.CloseIO();

            var MIDIFile = Tools.NemoLoadMIDI(midi);
            if (MIDIFile == null)
            {
                MessageBox.Show("Unable to load MIDI file '" + Path.GetFileName(midi) + "'");
                return;
            }

            TicksPerQuarter = MIDIFile.DeltaTicksPerQuarterNote;
            BuildTempoList(MIDIFile);
            BuildTimeSignatureList(MIDIFile);

            for (var i = 0; i < MIDIFile.Events.Tracks; i++)
            {
                var trackname = MIDIFile.Events[i][0].ToString();
                if (trackname.Contains("PART VOCALS"))
                {
                    GetPhraseMarkers(MIDIFile.Events[i]);
                    AnalyzeVocals(MIDIFile.Events[i], 0);
                }
                else if (trackname.Contains("HARM1"))
                {
                    AnalyzeVocals(MIDIFile.Events[i], 1);
                }
                else if (trackname.Contains("HARM2"))
                {
                    AnalyzeVocals(MIDIFile.Events[i], 2);
                }
                else if (trackname.Contains("HARM3"))
                {
                    AnalyzeVocals(MIDIFile.Events[i], 3);
                }
            }

            foreach (var lyric in VocalLyrics)
            {
                foreach (var note in VocalNotes)
                {
                    if (note.Ticks <= lyric.Ticks && note.Ticks >= lyric.Ticks)
                    {
                        lyric.End = note.NoteEnd;
                        break;
                    }
                }
            }
            if (Parser.Songs[0].VocalParts > 1)
            {
                foreach (var lyric in Harm1Lyrics)
                {
                    foreach (var note in Harm1Notes)
                    {
                        if (note.Ticks <= lyric.Ticks && note.Ticks >= lyric.Ticks)
                        {
                            lyric.End = note.NoteEnd;
                            break;
                        }
                    }
                }
                foreach (var lyric in Harm2Lyrics)
                {
                    foreach (var note in Harm2Notes)
                    {
                        if (note.Ticks <= lyric.Ticks && note.Ticks >= lyric.Ticks)
                        {
                            lyric.End = note.NoteEnd;
                            break;
                        }
                    }
                }
                foreach (var lyric in Harm3Lyrics)
                {
                    foreach (var note in Harm3Notes)
                    {
                        if (note.Ticks <= lyric.Ticks && note.Ticks >= lyric.Ticks)
                        {
                            lyric.End = note.NoteEnd;
                            break;
                        }
                    }
                }
            }

            WriteTOML(toml, wav, cdg);
            CreateCDG(toml);                      

            if (!workSilently)
            {
                var message = "File " + fileCounter + " of " + inputFiles.Count + ":\n";
                message += Parser.Songs[0].Artist + " - " + Parser.Songs[0].Name;
                message += "\nMP3: " + (File.Exists(mp3) ? "Success" : "Failed");
                message += "\nCDG: " + (File.Exists(cdg) ? "Success" : "Failed");
                MessageBox.Show(message, "Progress Update", MessageBoxButtons.OK, MessageBoxIcon.Information);                
            }
            Tools.DeleteFile(wav);
            if (!doKeepTOML)
            {
                Tools.DeleteFile(toml);
            }
            if (File.Exists(mp3) && File.Exists(cdg))
            {
                successCounter++;
            }
            else
            {
                failureCounter++;
                failedSongs += Parser.Songs[0].Artist + " - " + Parser.Songs[0].Name + "\n";
            }
        }

        private string EscapeTomlString(string input)
        {
            if (input.Contains('"'))
                return "\"\"\"" + input + "\"\"\""; // wrap in triple quotes
            return "\"" + input + "\"";

        }

        public void InsertLoadingBars(ref List<TimedLyricLine> lines, List<TimedLyricLine> lyricsVocals, int vocal_parts)
        {
            const int minGapCs = 500; // 5 seconds
            const int paddingCs = 100; // 1 second
            const string loadingBar = "████████████████";

            var loadingLines = new List<(int InsertIndex, List<TimedLyricLine> Lines)>();

            // Gather all actual lyric timings
            var lyricTimings = lyricsVocals
                .SelectMany(line => line.StartTimes.Zip(line.EndTimes, (start, end) => new { Start = start, End = end }))
                .OrderBy(t => t.Start)
                .ToList();

            if (lyricTimings.Count == 0)
                return; // no sung content at all

            // Handle gap before first sung line
            if (lyricTimings[0].Start >= minGapCs)
            {
                int start = paddingCs;
                int end = lyricTimings[0].Start - paddingCs;
                loadingLines.Add((0, BuildLoadingBlock(start, end, loadingBar, vocal_parts)));
            }

            // Handle gaps between sung syllables
            for (int i = 0; i < lyricTimings.Count - 1; i++)
            {
                int endCurrent = lyricTimings[i].End;
                int startNext = lyricTimings[i + 1].Start;
                int gap = startNext - endCurrent;

                if (gap >= minGapCs)
                {
                    int start = endCurrent + paddingCs;
                    int end = startNext - paddingCs;

                    // Find insertion index in lines
                    int insertIndex = lines.FindIndex(l =>
                        l.StartTimes.Count > 0 && l.StartTimes[0] > start);

                    if (insertIndex == -1)
                        insertIndex = lines.Count;

                    loadingLines.Add((insertIndex, BuildLoadingBlock(start, end, loadingBar, vocal_parts)));
                }
            }

            if (trackLength > 0)
            {
                // Handle gap after last sung lyric to near end of track
                int lastEnd = lyricTimings.Last().End;
                int trackEnd = (int)(trackLength * 0.1);
                int outroEnd = trackEnd - minGapCs;

                if (lastEnd < outroEnd)
                {
                    int start = lastEnd + paddingCs;
                    int end = outroEnd;

                    int insertIndex = lines.Count; // append at the end
                    loadingLines.Add((insertIndex, BuildLoadingBlock(start, end, loadingBar, vocal_parts)));
                }
            }

            // Insert all loading bar blocks in reverse to preserve indices
            foreach (var (insertIndex, barLines) in loadingLines.OrderByDescending(l => l.InsertIndex))
            {
                lines.InsertRange(insertIndex, barLines);
            }
        }


        public static void InsertLoadingBars2(ref List<TimedLyricLine> lines, List<LyricPhrase> phrases, int vocal_parts)
        {
            const int minGapCs = 500; // 5 seconds
            const int paddingCs = 100; // 1 second
            const string loadingBar = "████████████████";

            var loadingLines = new List<(int InsertIndex, List<TimedLyricLine> Lines)>();

            // Handle gap before first phrase
            if (phrases.Count > 0)
            {
                int firstStart = (int)(phrases[0].PhraseStart / 1000 * 100);
                if (firstStart >= minGapCs)
                {
                    int start = paddingCs;
                    int end = firstStart - paddingCs;
                    loadingLines.Add((0, BuildLoadingBlock(start, end, loadingBar, vocal_parts)));
                }
            }

            // Handle gaps between phrases
            for (int i = 0; i < phrases.Count - 1; i++)
            {
                int endCurrent = (int)(phrases[i].PhraseEnd / 1000 * 100);
                int startNext = (int)(phrases[i + 1].PhraseStart / 1000 * 100);
                int gap = startNext - endCurrent;

                if (gap >= minGapCs)
                {
                    int start = endCurrent + paddingCs;
                    int end = startNext - paddingCs;

                    // Find insertion index in lines
                    int insertIndex = lines.FindIndex(l =>
                        l.StartTimes.Count > 0 && l.StartTimes[0] > start);

                    if (insertIndex == -1)
                        insertIndex = lines.Count;

                    loadingLines.Add((insertIndex, BuildLoadingBlock(start, end, loadingBar, vocal_parts)));
                }
            }

            // Insert all loading bar blocks in reverse to preserve indices
            foreach (var (insertIndex, barLines) in loadingLines.OrderByDescending(l => l.InsertIndex))
            {
                lines.InsertRange(insertIndex, barLines);
            }
        }

        private static List<TimedLyricLine> BuildLoadingBlock(int start, int end, string loadingBar, int vocal_parts)
        {
            var lines = new List<TimedLyricLine>();

            if (vocal_parts == 1)
            {// Empty line (~)
                lines.Add(new TimedLyricLine { Syllables = new List<string> { "~" }, FormattedLine = "~" });
            }

            // Loading bar line with timing
            lines.Add(new TimedLyricLine
            {
                Syllables = new List<string> { loadingBar, "^" },
                StartTimes = new List<int> { start, end },
                EndTimes = new List<int> { end, end },
                FormattedLine = $"{loadingBar}/^"
            });

            // Empty line (~)
            lines.Add(new TimedLyricLine { Syllables = new List<string> { "~" }, FormattedLine = "~" });
            lines.Add(new TimedLyricLine { Syllables = new List<string> { "~" }, FormattedLine = "~" });
            return lines;
        }

        private void WriteTOML(string toml, string wav, string cdg)
        {
            var vocal_parts = !doHarmonies || !Harm2Lyrics.Any() ? 1 : (Harm3Lyrics.Any() ? 3 : 2);
            int harm1Top = vocal_parts == 1 ? 5 : (Harm3Lyrics.Any() ? 1 : 2);
            int harm1Parts = vocal_parts == 1 ? 4 : (Harm3Lyrics.Any() ? 2 : 3);
            int harm2Top = vocal_parts == 2 ? 11 : 7;
            int harm2Parts = vocal_parts == 2 ? 3 : 2;
            const int harm3Top = 13;
            const int harm3Parts = 2;
            var fontPath = Application.StartupPath + "\\bin\\fonts\\" + ActiveFont;

            var sw = new StreamWriter(toml, false);
            sw.WriteLine("title = " + EscapeTomlString(Parser.Songs[0].Name));
            sw.WriteLine("artist = " + EscapeTomlString(Parser.Songs[0].Artist));
            sw.WriteLine("file = \"" + wav.Replace("\\", "\\\\") + "\"");
            sw.WriteLine("outname = \"" + cdg.Replace("\\", "\\\\") + "\"");
            sw.WriteLine("");
            sw.WriteLine("clear_mode = \"" + (vocal_parts == 1 ? "page" : "delayed") + "\"");
            sw.WriteLine("");
            sw.WriteLine("background = \"" + backgroundColor + "\"");
            sw.WriteLine("border = \"" + backgroundColor + "\""); //match the background for now
            sw.WriteLine("");
            sw.WriteLine("font = '" + ActiveFont + "'");//"" + fontPath.Replace("\\", "\\\\") + "\"");
            sw.WriteLine("font_size = 18"); //hardcoded
            sw.WriteLine("");
            sw.WriteLine("[[singers]]"); //always must have atleast one, this is lead/harm1
            sw.WriteLine("active_fill = \"" + highlightColor1 + "\"");
            sw.WriteLine("active_stroke = \"" + highlightColor1 + "\""); //match the highlight color
            sw.WriteLine("inactive_fill = \"" + textColor1 + "\""); //
            sw.WriteLine("inactive_stroke = \"" + textColor1 + "\""); //match the inactive color
            if (Harm2Lyrics.Any())
            {
                sw.WriteLine("");
                sw.WriteLine("[[singers]]"); //harm2
                sw.WriteLine("active_fill = \"" + highlightColor2 + "\"");
                sw.WriteLine("active_stroke = \"" + highlightColor2 + "\""); //match the highlight color
                sw.WriteLine("inactive_fill = \"" + textColor2 + "\""); //
                sw.WriteLine("inactive_stroke = \"" + textColor2 + "\""); //match the inactive color
            }
            if (Harm3Lyrics.Any())
            {
                sw.WriteLine("");
                sw.WriteLine("[[singers]]"); //harm3
                sw.WriteLine("active_fill = \"" + highlightColor3 + "\"");
                sw.WriteLine("active_stroke = \"" + highlightColor3 + "\""); //match the highlight color
                sw.WriteLine("inactive_fill = \"" + textColor3 + "\""); //
                sw.WriteLine("inactive_stroke = \"" + textColor3 + "\""); //match the inactive color
            }
            sw.WriteLine("");
            sw.WriteLine("[[lyrics]]"); //must always have one, this is lead/harm1
            sw.WriteLine("singer = 1");
            sw.WriteLine("sync = [");
            var lyricsVocals = GetTimedLyricsForTOML(vocal_parts > 1 ? 1 : 0);
            if (vocal_parts == 1)
            {
                InsertLoadingBars(ref lyricsVocals, lyricsVocals, vocal_parts);
            }
            foreach (var lyricLine in lyricsVocals)
            {
                for (int i = 0; i < lyricLine.Syllables.Count; i++)
                {
                    if (lyricLine.StartTimes.Count == 0) continue;
                    sw.Write(lyricLine.StartTimes[i] + ",");
                }
            }
            sw.Write("\n");
            sw.WriteLine("]");
            sw.WriteLine("row = " + harm1Top);
            sw.WriteLine("line_tile_height = 2");
            sw.WriteLine("lines_per_page = " + harm1Parts);
            sw.WriteLine("text = '''");
            foreach (var line in lyricsVocals)
            {
                sw.WriteLine(line.FormattedLine);
            }
            sw.WriteLine("'''");
            if (Harm2Lyrics.Any() && doHarmonies)
            {
                sw.WriteLine("");
                sw.WriteLine("[[lyrics]]");
                sw.WriteLine("singer = 2");
                sw.WriteLine("sync = [");                
                var lyricsHarm2 = GetTimedLyricsForTOML(2);
                if (vocal_parts > 1)
                {
                    InsertLoadingBars(ref lyricsHarm2, lyricsVocals, vocal_parts);
                }
                foreach (var lyricLine in lyricsHarm2)
                {
                    for (int i = 0; i < lyricLine.Syllables.Count; i++)
                    {
                        if (lyricLine.StartTimes.Count == 0) continue;
                        sw.Write(lyricLine.StartTimes[i] + ",");
                    }
                }
                sw.WriteLine("]");
                sw.WriteLine("row = " + harm2Top);
                sw.WriteLine("line_tile_height = 2");
                sw.WriteLine("lines_per_page = " + harm2Parts);
                sw.WriteLine("text = '''");
                foreach (var line in lyricsHarm2)
                {
                    sw.WriteLine(line.FormattedLine);
                }
                sw.WriteLine("'''");
                if (Harm3Lyrics.Any())
                {
                    sw.WriteLine("");
                    sw.WriteLine("[[lyrics]]");
                    sw.WriteLine("singer = 3");
                    sw.WriteLine("sync = [");
                    var lyricsHarm3 = GetTimedLyricsForTOML(3);
                    foreach (var lyricLine in lyricsHarm3)
                    {
                        for (int i = 0; i < lyricLine.Syllables.Count; i++)
                        {
                            if (lyricLine.StartTimes.Count == 0) continue;
                            sw.Write(lyricLine.StartTimes[i] + ",");
                        }
                    }
                    sw.WriteLine("]");
                    sw.WriteLine("row = " + harm3Top);
                    sw.WriteLine("line_tile_height = 2");
                    sw.WriteLine("lines_per_page = " + harm3Parts);
                    sw.WriteLine("text = '''");
                    foreach (var line in lyricsHarm3)
                    {
                        sw.WriteLine(line.FormattedLine);
                    }
                    sw.WriteLine("'''");
                }
            }
            sw.Dispose();
        }

        private void CreateCDG(string toml)
        {
            var folderPath = Application.StartupPath + "\\bin\\";
            var exePath = Path.Combine(folderPath, "cdgmaker.exe");
            if (!File.Exists(exePath))
            {
                MessageBox.Show("cdgmaker.exe is missing and I can't continue without it", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var arg = "\"" + toml + "\"";

            var app = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arg,
                WorkingDirectory = folderPath,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(app);
            cdgOutput += $"Begin log for song '{Parser.Songs[0].Artist} - {Parser.Songs[0].Name}'{process.StandardError.ReadToEnd()}" +
                $"End log for song '{Parser.Songs[0].Artist} - {Parser.Songs[0].Name}'\n\n";
            process.WaitForExit();
            process.Dispose();
        }
                       
        private List<TimedLyricLine> GetTimedLyricsForTOML(int part)
        {
            var result = new List<TimedLyricLine>();
            int maxLineLength = 24;

            var allLyrics = (part == 0 ? VocalLyrics :
                            (part == 1 ? Harm1Lyrics :
                            (part == 2 ? Harm2Lyrics : Harm3Lyrics)));

            foreach (var phrase in VocalPhrases)
            {
                var phraseLyrics = allLyrics
                    .Where(l => l.Start >= phrase.PhraseStart && l.Start <= phrase.PhraseEnd)
                    .ToList();

                if (phraseLyrics.Count == 0)
                {
                    result.Add(new TimedLyricLine { Syllables = new List<string> { "~" }, FormattedLine = "~" });
                    continue;
                }

                var currentLine = new TimedLyricLine();
                int visibleCharCount = 0;
                var formattedLineBuilder = new List<string>();

                int i = 0;
                while (i < phraseLyrics.Count)
                {
                    var lyric = phraseLyrics[i];
                    string raw = lyric.Lyric.Trim().Replace("^", "").Replace("$", "").Replace("#", "");

                    // Handle hold syllables
                    if (raw == "+")
                    {
                        int holdStart = (int)Math.Round(lyric.Start * 0.1);
                        int holdEnd = (int)Math.Round(lyric.End * 0.1);
                        
                        currentLine.Syllables.Add("^");
                        currentLine.StartTimes.Add(holdStart);
                        currentLine.EndTimes.Add(holdEnd);
                        formattedLineBuilder.Add("/^");
                        i++;
                        continue;
                    }

                    // Rebuild full word from syllables
                    var syllables = new List<(string Text, int Start, int End)>();
                    bool endsWithDash = EndsWithDash(raw);
                    string clean = CleanSyllable(raw);

                    if (!string.IsNullOrWhiteSpace(clean))
                        syllables.Add((clean, (int)Math.Round(lyric.Start * 0.1), (int)Math.Round(lyric.End * 0.1)));

                    int j = i + 1;
                    while (endsWithDash && j < phraseLyrics.Count)
                    {
                        var next = phraseLyrics[j];
                        string nextRaw = next.Lyric.Trim();
                        endsWithDash = EndsWithDash(nextRaw) || nextRaw == "+";
                        string nextClean = CleanSyllable(nextRaw);
                        if (!string.IsNullOrWhiteSpace(nextClean))
                            syllables.Add((nextClean, (int)Math.Round(next.Start * 0.1), (int)Math.Round(next.End * 0.1)));

                        j++;
                    }

                    int visibleLen = syllables.Sum(s => s.Text.Length);

                    // Wrap if too long
                    if (visibleCharCount + (visibleCharCount > 0 ? 1 : 0) + visibleLen > maxLineLength)
                    {
                        if (currentLine.Syllables.Count > 0)
                        {
                            currentLine.FormattedLine = string.Join("", formattedLineBuilder);
                            result.Add(currentLine);
                        }
                        currentLine = new TimedLyricLine();
                        formattedLineBuilder.Clear();
                        visibleCharCount = 0;
                    }

                    // Add space before word
                    if (visibleCharCount > 0)
                    {
                        currentLine.Syllables.Add(" ");
                        currentLine.StartTimes.Add(syllables[0].Start);
                        currentLine.EndTimes.Add(syllables[0].Start);
                        formattedLineBuilder.Add(" ");
                        visibleCharCount++;
                    }

                    for (int s = 0; s < syllables.Count; s++)
                    {
                        var syll = syllables[s];

                        int highlightStart = syll.Start;
                        int highlightEnd = syll.End;

                        currentLine.Syllables.Add(syll.Text);
                        currentLine.StartTimes.Add(highlightStart);
                        currentLine.EndTimes.Add(highlightEnd);
                        formattedLineBuilder.Add(syll.Text);
                        visibleCharCount += syll.Text.Length;

                        currentLine.Syllables.Add("^");
                        currentLine.StartTimes.Add(highlightEnd);
                        currentLine.EndTimes.Add(highlightEnd);

                        if (s < syllables.Count - 1)
                            formattedLineBuilder.Add("/^/");
                        else
                            formattedLineBuilder.Add("/^");
                    }

                    i = j;
                }

                if (currentLine.Syllables.Count > 0)
                {
                    currentLine.FormattedLine = string.Join("", formattedLineBuilder);
                    result.Add(currentLine);
                }
            }

            return result;
        }

        public class TimedLyricLine
        {
            public List<string> Syllables { get; set; } = new List<string>();
            public List<int> StartTimes { get; set; } = new List<int>();
            public List<int> EndTimes { get; set; } = new List<int>();

            public string FormattedLine { get; set; } = "";
        }

        private bool EndsWithDash(string s)
        {
            return s.Replace("#", "").Replace("^", "")
                    .TrimEnd('.', ',', '!', '?', '…', ';', ':')
                    .Trim()
                    .EndsWith("-");
        }

        private string CleanSyllable(string s)
        {
            return CleanString(s)                
                .Replace("-", "")
                .Replace("=", "-")
                .Replace("#", "")
                .Replace("^", "")
                .Replace("$", "")
                .Replace("+", "^")
                .Trim();
        }
        
        private void AnalyzeVocals(IEnumerable<MidiEvent> track, int part)
        {
            foreach (var notes in track)
            {
                switch (notes.CommandCode)
                {
                    case MidiCommandCode.MetaEvent:
                        var vocal_event = (MetaEvent)notes;
                        if ((vocal_event.MetaEventType == MetaEventType.Lyric || vocal_event.MetaEventType == MetaEventType.TextEvent) &&
                            !vocal_event.ToString().Contains("["))
                        {
                            var lyric = GetCleanMIDILyric(vocal_event.ToString());
                            if (string.IsNullOrWhiteSpace(lyric)) continue;                            
                            var k = new KaraokeLyric()
                            {
                                Lyric = lyric,
                                Ticks = vocal_event.AbsoluteTime,
                                Start = GetRealtime(vocal_event.AbsoluteTime)
                            };
                            switch (part)
                            {
                                case 1:
                                    Harm1Lyrics.Add(k);
                                    break;
                                case 2:
                                    Harm2Lyrics.Add(k);
                                    break;
                                case 3:
                                    Harm3Lyrics.Add(k);
                                    break;
                                default:
                                    VocalLyrics.Add(k);
                                    break;
                            }                            
                        }
                        break;
                    case MidiCommandCode.NoteOn:
                        var note = (NoteOnEvent)notes;
                        if (note.Velocity <= 0) continue;

                        if (note.NoteNumber <= 84 && note.NoteNumber >= 36)
                        {
                            var n = new MIDINote()
                            {
                                NoteNumber = note.NoteNumber,
                                NoteStart = GetRealtime(note.AbsoluteTime),
                                Ticks = note.AbsoluteTime,
                                NoteEnd = GetRealtime(note.AbsoluteTime + note.NoteLength)
                            };
                            switch (part)
                            {
                                case 1:
                                    Harm1Notes.Add(n);
                                    break;
                                case 2:
                                    Harm2Notes.Add(n);
                                    break;
                                case 3:
                                    Harm3Notes.Add(n);
                                    break;
                                default:
                                    VocalNotes.Add(n);
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        private static string GetCleanMIDILyric(string raw_event)
        {
            try
            {
                raw_event = raw_event.Trim();
                int index;
                if (raw_event.Contains("Lyric"))
                {
                    index = raw_event.IndexOf("Lyric", StringComparison.Ordinal) + 5;
                }
                else if (raw_event.Contains("TextEvent"))
                {
                    index = raw_event.IndexOf("TextEvent", StringComparison.Ordinal) + 9;
                }
                else
                {
                    index = raw_event.LastIndexOf(" ", StringComparison.Ordinal);
                }
                if (index == -1) return "";

                var old_lyric = raw_event.Substring(index + 1, raw_event.Length - index - 1);
                return old_lyric;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void GetPhraseMarkers(IEnumerable<MidiEvent> track)
        {
            var Lyrics = new List<LyricPhrase>();
            foreach (var notes in track)
            {
                switch (notes.CommandCode)
                {
                    case MidiCommandCode.NoteOn:
                        var note = (NoteOnEvent)notes;
                        if (note.Velocity <= 0) continue;
                        switch (note.NoteNumber)
                        {
                            case 106: //older style part vocals had both 106 and 105
                            case 105: //typical phrase marker
                                var time = GetRealtime(note.AbsoluteTime);
                                var end = GetRealtime(note.AbsoluteTime + note.NoteLength);
                                if (Lyrics.Any())
                                {
                                    var index = Lyrics.Count - 1;
                                    if (Lyrics[index].PhraseStart == time)
                                    {
                                        continue; //for old double-stacked markers
                                    }
                                }
                                var phrase = new LyricPhrase
                                {
                                    PhraseStart = time,
                                    PhraseEnd = end
                                };
                                Lyrics.Add(phrase); //new line
                                break;
                        }
                        break;
                }
            }
            VocalPhrases = Lyrics;
        }

        private double GetRealtime(long absdelta)
        {
            //code by raynebc
            var BPM = 120.0;   //As per the MIDI specification, until a tempo change is reached, 120BPM is assumed
            var reldelta = absdelta;   //The number of delta ticks between the delta time being converted and the tempo change immediately at or before it
            var time = 0.0;   //The real time position of the tempo change immediately at or before the delta time being converted
            foreach (var tempo in TempoEvents.Where(tempo => tempo.AbsoluteTime <= absdelta))
            {
                BPM = tempo.BPM;
                time = tempo.RealTime;
                reldelta = absdelta - tempo.AbsoluteTime;
            }
            time += (double)reldelta / TicksPerQuarter * (60000.0 / BPM);
            return time;
        }

        private void BuildTimeSignatureList(MidiFile MIDIFile)
        {
            TimeSignatures = new List<TimeSignature>();
            foreach (var ev in MIDIFile.Events[0])
            {
                if (ev.CommandCode != MidiCommandCode.MetaEvent) continue;
                var meta = ev as MetaEvent;
                if (meta?.MetaEventType != MetaEventType.TimeSignature) continue;

                var ts = meta as TimeSignatureEvent;
                if (ts == null) continue;

                // The actual denominator is 2 raised to the power of denominator exponent
                int numerator = ts.Numerator;
                int denominator = (int)Math.Pow(2, ts.Denominator);

                var timeSig = new TimeSignature
                {
                    AbsoluteTime = ev.AbsoluteTime,
                    Numerator = numerator,
                    Denominator = denominator
                };

                TimeSignatures.Add(timeSig);
            }
        }

        private void BuildTempoList(MidiFile MIDIFile)
        {
            TempoEvents = new List<TempoEvent>();

            double currentBpm = 120.0;
            double realTimeMs = 0.0;
            int relDeltaTicks = 0;

            foreach (var ev in MIDIFile.Events[0])
            {
                relDeltaTicks += ev.DeltaTime;

                if (ev.CommandCode != MidiCommandCode.MetaEvent) continue;

                var meta = ev as MetaEvent;
                if (meta?.MetaEventType != MetaEventType.SetTempo) continue;

                var tempoEvent = meta as NAudio.Midi.TempoEvent;
                if (tempoEvent == null) continue;

                // Convert ticks to ms using current tempo
                double deltaMs = relDeltaTicks / (double)TicksPerQuarter * (60000.0 / currentBpm);
                realTimeMs += deltaMs;

                // Convert microseconds/quarter note to BPM
                currentBpm = 60000000.0 / tempoEvent.MicrosecondsPerQuarterNote;
                relDeltaTicks = 0;

                TempoEvents.Add(new TempoEvent
                {
                    AbsoluteTime = ev.AbsoluteTime,
                    RealTime = realTimeMs,
                    BPM = currentBpm
                });
            }
        }

        private void cboBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            Color backColor = GetColorFromIndex(cboBackground.SelectedIndex);
            picBackground1.BackColor = backColor;
            lblTextHighlight1.BackColor = backColor;
            lblTextColor1.BackColor = backColor;
            
            picBackground2.BackColor = backColor;
            lblTextHighlight2.BackColor = backColor;
            lblTextColor2.BackColor = backColor;
            
            picBackground3.BackColor = backColor;
            lblTextHighlight3.BackColor = backColor;
            lblTextColor3.BackColor = backColor;
        }

        private System.Drawing.Color GetColorFromIndex(int index)
        {
            try
            {
                return ColorTranslator.FromHtml(karaokeColorHexes[index]);
            }
            catch
            {
                return Color.Black;
            }            
        }        

        private void cboFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFont();
        }

        private void UpdateFont()
        {
            if (!cboFont.Text.Contains(".ttf")) return;
            try
            {
                var fontFile = cboFont.Text.Split('|')[1].Trim();
                string fontPath = Path.Combine(Application.StartupPath, "bin", "fonts", fontFile);
                pfc = new PrivateFontCollection();
                pfc.AddFontFile(fontPath);               
                FontFamily ff = pfc.Families[0]; // assuming the TTF file only contains one family
                             
                var font = new Font(ff, 16f, FontStyle.Bold);
                lblTextColor1.Font = font;
                lblTextHighlight1.Font = font;
                lblTextColor2.Font = font;
                lblTextHighlight2.Font = font;
                lblTextColor3.Font = font;
                lblTextHighlight3.Font = font;
            }
            catch
            {
                MessageBox.Show("Error trying to change font, are you sure it's a valid selection?", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            cdgOutput = "";
            var log = Application.StartupPath + "\\bin\\karaoke.log.txt";
            Tools.DeleteFile(log);
            foreach (var file in inputFiles)
            {
                fileCounter++;
                ProcessFile(file);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            EnableDisable(true);
            inputFiles = new List<string>();
            var log = Application.StartupPath + "\\bin\\karaoke.log.txt";

            if (failureCounter > 0 && !string.IsNullOrEmpty(cdgOutput))
            {
                File.WriteAllText(log, cdgOutput);
            }

            var message = $"Processed {fileCounter} song file(s):\n{successCounter} file(s) were converted successfully\n{failureCounter}" +
                $" file(s) failed to convert" + (failureCounter > 0 ? $"\n\nThe failed song file(s) are:\n{failedSongs}\n\nDo you want to " +
                $"open the log file to view the error(s)?" : "");
            if (failureCounter > 0)
            {
                if (MessageBox.Show(message, "Process Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (File.Exists(log))
                    {
                        Process.Start(log);
                    }
                }
            }
            else
            {
                MessageBox.Show(message, "Process Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            fileCounter = 0;
            failureCounter = 0;
            successCounter = 0;
            failedSongs = "";
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            var message = "This tool is designed to take any Rock Band song file (official or custom, encrypted or not) and convert it to matched " +
                ".mp3 and .cdg files that are used by most karaoke emulators and professional karaoke players. In essence, as the title says " +
                "it is a Rock Band to Karaoke Converter\r\n\r\nIt is limited in what it can do because the .cdg format is very limited itself, but " +
                "the goal is compatibility with as many karaoke players as possible\r\n\r\nYou can drag and drop one or multiple files and it'll batch " +
                " process them\r\n\r\nOutput files are in a 'Karaoke' folder in the same directory as the source file(s)\r\n\r\nThis tool wraps around the" +
                " cdgmaker Python application by Josiah Winslow (https://github.com/WinslowJosiah/cdgmaker) made to work as an executable on Windows after " +
                "many modifications by me\r\n\r\nI'm still learning all the ins and outs of the original Python application so if I missed " +
                "something let me know so I can try to address it in a future update" + "\r\n\r\nOptions are limited but also self explanatory\r\n\r\nEnjoy!";
            var helper = new HelpForm(Text + " - Help", message, false, false);
            helper.ShowDialog();
        }

        private void CDGConverter_Shown(object sender, EventArgs e)
        {
            LoadConfig();
            var folder = Application.StartupPath + "\\bin\\fonts\\";
            var fonts = Directory.GetFiles(folder, "*.ttf", SearchOption.TopDirectoryOnly);
            if (fonts.Any())
            {
                cboFont.Items.Clear();
                foreach (var font in fonts)
                {
                    pfc = new PrivateFontCollection();
                    pfc.AddFontFile(font);
                    FontFamily ff = pfc.Families[0];
                    cboFont.Items.Add(ff.Name + " | " + Path.GetFileName(font));
                }
                if (cboFont.Items.Count > 0)
                {
                    cboFont.SelectedIndex = 0;
                }
            }
            UpdateFont();
        }

        private void CDGConverter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (picWorking.Visible)
            {
                MessageBox.Show("Please wait until the current process finishes", "Working...", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                e.Cancel = true;
                return;
            }
            SaveConfig();
        }

        private void cboTextColor1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTextColor1.ForeColor = GetColorFromIndex(cboTextColor1.SelectedIndex);
        }

        private void cboTextHighlight1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTextHighlight1.ForeColor = GetColorFromIndex(cboTextHighlight1.SelectedIndex);
        }

        private void cboTextColor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTextColor2.ForeColor = GetColorFromIndex(cboTextColor2.SelectedIndex);
        }

        private void cboTextColor3_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTextColor3.ForeColor = GetColorFromIndex(cboTextColor3.SelectedIndex);
        }

        private void cboTextHighlight2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTextHighlight2.ForeColor = GetColorFromIndex(cboTextHighlight2.SelectedIndex);
        }

        private void cboTextHighlight3_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTextHighlight3.ForeColor = GetColorFromIndex(cboTextHighlight3.SelectedIndex);
        }

        private void lblFontQuestion_MouseClick(object sender, MouseEventArgs e)
        {
            const string message = "You can customize the font that the converter will use\n\nPut your preferred fonts in the \\bin\\fonts\\ folder and they will be displayed here\n\n"
                + "Please note that only .ttf font files are accepted";
            MessageBox.Show(message, "Fonts", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class KaraokeLyric
    {
        public string Lyric { get; set; }
        public double Start { get; set; }
        public double End { get; set; }
        public long Ticks { get; set; }
    }    
}
