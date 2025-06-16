using NAudio.Midi;
using Nautilus.x360;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;

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
        private List<MIDINote> Harm1Notes;
        private List<MIDINote> Harm2Notes;
        private List<MIDINote> VocalNotes;
        private List<LyricPhrase> VocalPhrases;
        private List<string> inputFiles;
        private string backgroundColor = "";
        private string textColor1 = "";
        private string sungColor1 = "";
        private string textColor2 = "#0FF";
        private string sungColor2 = "#F8F";
        private string fontName = "";
        private string fontStyle = "";
        private int fileCounter = 0;
        private int successCounter = 0;
        private int failureCounter = 0;
        private string failedSongs = "";
        private string vocalOption = "";
        private bool doKeepLRC;
        private string cdgOutput = "";
        private bool workSilently = true;
        private bool doHarmonies = true;
        private readonly string configFile;
        
        List<string> karaokeColorHexes = new List<string>
{
    "#000", // Black
    "#FFF", // White
    "#F00", // Red
    "#0F0", // Green
    //"#00F", // Blue
    "#29F", // Approximate DodgerBlue
    "#FF0", // Yellow
    "#0FF", // Cyan
    "#F0F", // Magenta
    "#888", // Gray
    "#F80", // Orange
    "#F8F", // Pink
    "#80F", // Purple
    "#088", // Teal
    "#8F0", // Lime
    "#008", // Navy
    "#840"  // Brown
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
            Harm2Notes = new List<MIDINote>();
            Harm1Notes = new List<MIDINote>();
        }
        
        private void SaveConfig()
        {
            var sw = new StreamWriter(configFile, false);
            sw.WriteLine(cboBackground.SelectedIndex.ToString());
            sw.WriteLine(cboText.SelectedIndex.ToString());
            sw.WriteLine(cboSung.SelectedIndex.ToString());
            sw.WriteLine(cboFont.SelectedIndex.ToString());
            sw.WriteLine(cboType.SelectedIndex.ToString());
            sw.WriteLine(radioRemove.Checked ? "True" : "False");
            sw.WriteLine(radioKeep.Checked ? "True" : "False");
            sw.WriteLine(chkKeepLRC.Checked ? "True" : "False");
            sw.WriteLine(chkSilent.Checked ? "True" : "False");
            sw.Dispose();
        }

        private void LoadConfig()
        {
            if (!File.Exists(configFile)) return;
            var sr = new StreamReader(configFile);
            try
            {
                cboBackground.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboText.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboSung.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboFont.SelectedIndex = Convert.ToInt16(sr.ReadLine());
                cboType.SelectedIndex = Convert.ToInt16(sr.ReadLine());
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
                chkKeepLRC.Checked = sr.ReadLine().Contains("True");
                chkSilent.Checked = sr.ReadLine().Contains("True");
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
            textColor1 = karaokeColorHexes[cboText.SelectedIndex];
            sungColor1 = karaokeColorHexes[cboSung.SelectedIndex];
            fontName = cboFont.Text;
            fontStyle = cboType.Text;
            doKeepLRC = chkKeepLRC.Checked;
            workSilently = chkSilent.Checked;
            doHarmonies = false;//disable for now while I work on the library backend
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
            cboSung.Enabled = enable;
            cboText.Enabled = enable;
            cboType.Enabled = enable;
            btnHelp.Enabled = enable;
            radioRemove.Enabled = enable;
            radioKeep.Enabled = enable;
            chkKeepLRC.Enabled = enable;
            picWorking.Visible = !enable;
            chkSilent.Enabled = enable;
        }

        private string CleanString(string str)
        {
            return str.Replace("#", "").Replace("^", "").Replace("\"", "").Replace("§", " ").Replace(",", "");
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

            var folder = Path.GetDirectoryName(file) + "\\Karaoke\\" + title + "\\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var mp3 = folder + title + ".mp3";
            var cdg = folder + title + ".cdg";
            var vocals = folder + title + ".lrc";
            var harmonies = folder + title + "_harm2.lrc";

            moggSplitter.DownmixMogg(file, mp3, MoggSplitter.MoggSplitFormat.MP3, vocalOption);

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
            }
            
            if (doHarmonies && Harm2Lyrics != null && Harm2Lyrics.Count > 0)
            {
                WriteLRC(vocals, 1);
                WriteLRC(harmonies, 2);
                CreateCDG(cdg, vocals, harmonies);
            }
            else
            {
                WriteLRC(vocals, 0);
                CreateCDG(cdg, vocals);
            }            

            if (!workSilently)
            {
                var message = "File " + fileCounter + " of " + inputFiles.Count + ":\n";
                message += Parser.Songs[0].Artist + " - " + Parser.Songs[0].Name;
                message += "\nMP3: " + (File.Exists(mp3) ? "Success" : "Failed");
                message += "\nCDG: " + (File.Exists(cdg) ? "Success" : "Failed");
                MessageBox.Show(message, "Progress Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void CreateCDG(string cdg, string lrc, string lrc2 = "")
        {
            var folderPath = Application.StartupPath + "\\bin\\cdg\\";
            var exePath = Path.Combine(folderPath, "CDGSharp.CLI.exe");
            if (!File.Exists(exePath))
            {
                MessageBox.Show("CDGSharp.CLI.exe is missing and I can't continue without it", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Tools.DeleteFile(cdg);

            var arg = "";
            if (!string.IsNullOrEmpty(lrc2))
            {
                arg = $"convert-lrc --file-path \"{lrc}\" --file-path2 \"{lrc2}\" --bg-color {backgroundColor} --text-color {textColor1} --sung-text-color {sungColor1} --text-color2 {textColor2} --sung-text-color2 {sungColor2} --font \"{fontName}\" --font-size 20 --font-style {fontStyle}";

            }
            else
            {
                arg = $"convert-lrc --file-path \"{lrc}\" --bg-color {backgroundColor} --text-color {textColor1} --sung-text-color {sungColor1} --font \"{fontName}\" --font-size 20 --font-style {fontStyle}";
            }

            var app = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arg,
                WorkingDirectory = folderPath,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };

            var process = Process.Start(app);
            cdgOutput += $"Begin log for song '{Parser.Songs[0].Artist} - {Parser.Songs[0].Name}'\n{process.StandardOutput.ReadToEnd()}" +
                $"End log for song '{Parser.Songs[0].Artist} - {Parser.Songs[0].Name}'\n\n";
            process.WaitForExit();
            process.Dispose();

            if (File.Exists(cdg) && !doKeepLRC)
            {
                Tools.DeleteFile(lrc);
                Tools.DeleteFile(lrc2);
            }
        }

        public static string TruncateLine(string input, int maxLength = 24)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            if (input.Length <= maxLength) return input;

            return input.Substring(0, maxLength - 3).TrimEnd() + "...";
        }

        private void WriteLRC(string lrc, int part)
        {
            if (VocalPhrases.Count == 0) return;

            string FormatTime(double ms)
            {
                var ts = TimeSpan.FromMilliseconds(ms);
                return $"{ts.Minutes:D2}:{ts.Seconds:D2}:{ts.Milliseconds / 10:D2}";
            }

            var sb = new StringBuilder();

            // Prepend metadata
            sb.Append("[ti:" + TruncateLine(Parser.Songs[0].Name) + "]\n"); //biggest font
            sb.Append("[ar:" + TruncateLine(Parser.Songs[0].Artist, 26) + "]\n"); //slightly smaller font   
            sb.Append("[al:" + TruncateLine(Parser.Songs[0].Album, 36) + "]\n\n"); //this was not supported in the original CDGSharp.CLI library but I added it

            int phrase = 0;
            const string loadingBar = "████████████████";
            const int loadingBarThreshold = 5000;

            if (part < 2) //don't do double loading bar for harm 1 and harm 2, only part vocals or harm 1
            {
                //intro loading bar where applicable
                double firstPhraseStart = VocalPhrases[0].PhraseStart;
                double introStart = firstPhraseStart - 6000.0;
                double introEnd = firstPhraseStart - 1000.0; // 1.0s before lyrics

                if (introStart >= loadingBarThreshold && introEnd > introStart)
                {
                    sb.AppendLine($"[{FormatTime(introStart)}]{loadingBar}[{FormatTime(introEnd)}]\n");
                }
            }

            //display song lyrics
            for (phrase = 0; phrase < VocalPhrases.Count; phrase++)
            {
                var phraseInfo = VocalPhrases[phrase];
                var phraseLyrics = (part == 0 ? VocalLyrics : (part == 1 ? Harm1Lyrics : Harm2Lyrics))
                    .Where(l => l.Start >= phraseInfo.PhraseStart && l.Start <= phraseInfo.PhraseEnd)
                    .ToList();

                if (phraseLyrics.Count == 0)
                    continue;

                // STEP 1: Convert syllables into full words
                List<KaraokeLyric> wordList = new List<KaraokeLyric>();
                int i = 0;

                while (i < phraseLyrics.Count)
                {
                    var first = phraseLyrics[i];
                    if (first.Lyric.Trim() == "+")
                    {
                        i++;
                        continue;
                    }

                    string wordText = CleanString(first.Lyric);
                    double wordStart = first.Start;
                    double wordEnd = first.End;
                    bool lastNonPlusEndedWithDash = wordText.Trim().EndsWith("-");
                    int j = i + 1;

                    while (j < phraseLyrics.Count)
                    {
                        var curr = phraseLyrics[j];
                        string currText = curr.Lyric.Trim();

                        if (currText == "+")
                        {
                            wordText += curr.Lyric;
                            wordEnd = curr.End;
                            j++;
                            continue;
                        }

                        if (!lastNonPlusEndedWithDash)
                            break;

                        wordText += curr.Lyric;
                        wordEnd = curr.End;
                        lastNonPlusEndedWithDash = CleanString(currText).EndsWith("-");
                        j++;
                    }

                    string cleanWord = CleanString(wordText).Replace("+", "").Replace("-", "").Replace("=", "-");
                    if (!string.IsNullOrWhiteSpace(cleanWord))
                    {
                        wordList.Add(new KaraokeLyric
                        {
                            Lyric = cleanWord,
                            Start = wordStart,
                            End = wordEnd
                        });
                    }

                    i = j;
                }
                                
                // STEP 2: Render full phrase with wrapping
                string currentLineText = "";
                double? lineStart = null;
                double? lineEnd = null;
                int charCount = 0;

                foreach (var word in wordList.OrderBy(w => w.Start))
                {
                    string w = word.Lyric.Trim();
                    int wordLen = w.Length;
                    int spaceLen = charCount > 0 ? 1 : 0;
                    int projectedLen = charCount + spaceLen + wordLen;

                    if (projectedLen > 24)
                    {
                        if (!string.IsNullOrWhiteSpace(currentLineText))
                        {
                            sb.AppendLine(currentLineText.TrimEnd());
                        }

                        currentLineText = "";
                        lineStart = null;
                        lineEnd = null;
                        charCount = 0;
                        spaceLen = 0;
                    }

                    if (lineStart == null)
                        lineStart = word.Start;
                    lineEnd = word.End;

                    if (spaceLen > 0)
                        currentLineText += " ";

                    currentLineText += $"[{FormatTime(word.Start)}]{w}[{FormatTime(word.End)}]";
                    charCount += spaceLen + wordLen;
                }

                // Final flush
                if (!string.IsNullOrWhiteSpace(currentLineText))
                {
                    sb.AppendLine(currentLineText.TrimEnd());
                }

                // STEP 3: Insert loading bar based on gap between phrases
                if (phrase < VocalPhrases.Count - 1)
                {
                    sb.AppendLine(); // spacing between phrases

                    double currentEnd = VocalPhrases[phrase].PhraseEnd;
                    double nextStart = VocalPhrases[phrase + 1].PhraseStart;
                    double gap = nextStart - currentEnd;

                    double startCountdown = currentEnd + 1000; // 1.0s after current phrase
                    double endCountdown = nextStart - 1000;    // 1.0s before next phrase

                    double countdownSpan = endCountdown - startCountdown;
                    
                    if (countdownSpan >= loadingBarThreshold && startCountdown < endCountdown)
                    {
                        sb.AppendLine($"[{FormatTime(startCountdown)}]{loadingBar}[{FormatTime(endCountdown)}]\n");
                        sb.AppendLine();
                    }                    
                }
            }

            /*double nemoStart = VocalPhrases[VocalPhrases.Count - 1].PhraseEnd + 1000;
            double nemoEnd = Parser.Songs[0].Length;
            //sb.Append($"[{FormatTime(nemoStart)}].:MADE.WITH.NAUTILUS:.[{FormatTime(nemoEnd)}]");
            if (nemoEnd > nemoStart && (nemoEnd - nemoStart >= loadingBarThreshold))
            {
                sb.AppendLine();
                sb.AppendLine($"[{FormatTime(nemoStart)}]{loadingBar}[{FormatTime(nemoEnd)}]\n");                
            }
            */
            // Write to file
            File.WriteAllText(lrc, sb.ToString().Replace("\r\n", "\n"), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
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
                                default:
                                    VocalLyrics.Add(k);
                                    break;
                            }                            
                        }
                        break;
                    case MidiCommandCode.NoteOn:
                        var note = (NoteOnEvent)notes;
                        if (note.Velocity <= 0) continue;

                        switch (note.NoteNumber)
                        {
                            default:
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
                                        default:
                                            VocalNotes.Add(n);
                                            break;
                                    }                                    
                                }
                                break;
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
            return time;// Math.Round(time / 1000, 3);
        }

        private void BuildTimeSignatureList(MidiFile MIDIFile)
        {
            TimeSignatures = new List<TimeSignature>();
            foreach (var ev in MIDIFile.Events[0])
            {
                if (ev.CommandCode != MidiCommandCode.MetaEvent) continue;
                var signature = (MetaEvent)ev;
                if (signature.MetaEventType != MetaEventType.TimeSignature) continue;
                //Track the time signature change
                var index1 = signature.ToString().IndexOf(" ", signature.ToString().IndexOf("TimeSignature", StringComparison.Ordinal), StringComparison.Ordinal) + 1;
                var index2 = signature.ToString().IndexOf("/", StringComparison.Ordinal);
                var numerator = Convert.ToInt16(signature.ToString().Substring(index1, index2 - index1));
                //Track the time signature change
                index1 = signature.ToString().IndexOf("/", StringComparison.Ordinal) + 1;
                index2 = signature.ToString().IndexOf(" ", signature.ToString().IndexOf("/", StringComparison.Ordinal), StringComparison.Ordinal);
                var denominator = Convert.ToInt16(signature.ToString().Substring(index1, index2 - index1));
                var time_sig = new TimeSignature
                {
                    AbsoluteTime = ev.AbsoluteTime,
                    Numerator = numerator,
                    Denominator = denominator
                };
                TimeSignatures.Add(time_sig);
            }
        }

        private void BuildTempoList(MidiFile MIDIFile)
        {
            //code provided by raynebc
            //Build tempo list
            var currentbpm = 120.00;
            var realtime = 0.0;
            var reldelta = 0;   //The number of delta ticks since the last tempo change
            TempoEvents = new List<TempoEvent>();
            foreach (var ev in MIDIFile.Events[0])
            {
                reldelta += ev.DeltaTime;
                if (ev.CommandCode != MidiCommandCode.MetaEvent) continue;
                var tempo = (MetaEvent)ev;
                if (tempo.MetaEventType != MetaEventType.SetTempo) continue;
                var relativetime = (double)reldelta / TicksPerQuarter * (60000.0 / currentbpm);
                var index1 = tempo.ToString().IndexOf("SetTempo", StringComparison.Ordinal) + 9;
                var index2 = tempo.ToString().IndexOf("bpm", StringComparison.Ordinal);
                var bpm = tempo.ToString().Substring(index1, index2 - index1);
                currentbpm = Convert.ToDouble(bpm);   //As per the MIDI specification, until a tempo change is reached, 120BPM is assumed
                realtime += relativetime;   //Add that to the ongoing current real time of the MIDI
                reldelta = 0;
                var tempo_event = new TempoEvent
                {
                    AbsoluteTime = tempo.AbsoluteTime,
                    RealTime = realtime,
                    BPM = currentbpm
                };
                TempoEvents.Add(tempo_event);
            }
        }

        private void cboBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            picSample.BackColor = GetColorFromIndex(cboBackground.SelectedIndex);
            lblHighlight.BackColor = picSample.BackColor;
            lblText.BackColor = picSample.BackColor;
        }

        private System.Drawing.Color GetColorFromIndex(int index)
        {
            var color = System.Drawing.Color.Black;
            switch (index)
            {
                case 0: color = System.Drawing.Color.Black; break;
                case 1: color = System.Drawing.Color.White; break;
                case 2: color = System.Drawing.Color.Red; break;
                case 3: color = System.Drawing.Color.Lime; break;      // #0F0
                case 4: color = System.Drawing.Color.DodgerBlue; break;
                case 5: color = System.Drawing.Color.Yellow; break;
                case 6: color = System.Drawing.Color.Cyan; break;
                case 7: color = System.Drawing.Color.Magenta; break;
                case 8: color = System.Drawing.Color.Gray; break;
                case 9: color = System.Drawing.Color.Orange; break;
                case 10: color = System.Drawing.Color.HotPink; break;  // #F8F approx
                case 11: color = System.Drawing.Color.MediumPurple; break; // #80F approx
                case 12: color = System.Drawing.Color.Teal; break;
                case 13: color = System.Drawing.Color.YellowGreen; break;  // #8F0 approx
                case 14: color = System.Drawing.Color.Navy; break;
                case 15: color = System.Drawing.Color.SaddleBrown; break; // #840 approx
            }
            return color;
        }

        private void cboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblText.ForeColor = GetColorFromIndex(cboText.SelectedIndex);
        }

        private void cboSung_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblHighlight.ForeColor = GetColorFromIndex(cboSung.SelectedIndex);
        }

        private void cboFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFont();
        }

        private void UpdateFont()
        {
            try
            {
                var font = new System.Drawing.Font(cboFont.Text, 16f, cboType.SelectedIndex == 0 ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular);
                lblText.Font = font;
                lblHighlight.Font = font;
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
                " process them\r\n\r\nOutput files are in a 'Karaoke' folder in the same directory as the source file(s)\r\n\r\nI rely on the CDGSharp.CLI library" +
                " which at the moment does not support harmonies, so this version only works with PART VOCALS and therefore only supports one singer\r\n\r\n" +
                "Another unfortunate problem with this library is that it struggles with songs with very fast syllables - \"Weird Al\" Yankovic's Polka Power!" +
                " is an example of a song that has some visual glitches as a result of the very fast syllables in the song - if I find a solution I will update" +
                " the tool\r\n\r\nOptions are pretty limited but also self explanatory\r\n\r\nEnjoy!";
            var helper = new HelpForm(Text + " - Help", message, false, false);
            helper.ShowDialog();
        }

        private void CDGConverter_Shown(object sender, EventArgs e)
        {
            LoadConfig();
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
    }

    public class KaraokeLyric
    {
        public string Lyric { get; set; }
        public double Start { get; set; }
        public double End { get; set; }
        public long Ticks { get; set; }
    }    
}
