using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Midi;

namespace Nautilus
{
    class MIDIStuff
    {
        private int TicksPerQuarter;
        private List<TempoEvent> TempoEvents;
        private List<TimeSignature> TimeSignatures;
        private MidiFile MIDIFile;
        public MIDIChart MIDIInfo;
        public MIDIChart MIDI_Chart;
        private long LengthLong;
        private LyricCollection InternalVocals;
        private LyricCollection InternalHarmonies1;
        private LyricCollection InternalHarmonies2;
        private LyricCollection InternalHarmonies3;
        public LyricCollection LyricsVocals;
        public LyricCollection LyricsHarm1;
        public LyricCollection LyricsHarm2;
        public LyricCollection LyricsHarm3;
        private PhraseCollection InternalPhrasesVocals;
        private PhraseCollection InternalPhrasesHarm1;
        private PhraseCollection InternalPhrasesHarm2;
        private PhraseCollection InternalPhrasesHarm3;
        public PhraseCollection PhrasesVocals;
        public PhraseCollection PhrasesHarm1;
        public PhraseCollection PhrasesHarm2;
        public PhraseCollection PhrasesHarm3;
        private List<PracticeSection> InternalPracticeSessions;
        public List<PracticeSection> PracticeSessions;

        public void Initialize(bool doall)
        {
            MIDIInfo = new MIDIChart();
            MIDIInfo.Initialize();
            InternalPhrasesVocals = new PhraseCollection();
            InternalPhrasesHarm1 = new PhraseCollection();
            InternalPhrasesHarm2 = new PhraseCollection();
            InternalPhrasesHarm3 = new PhraseCollection();
            InternalPhrasesVocals.Initialize();
            InternalPhrasesHarm1.Initialize();
            InternalPhrasesHarm2.Initialize();
            InternalPhrasesHarm3.Initialize();
            InternalPracticeSessions = new List<PracticeSection>();
            InternalVocals = new LyricCollection();
            InternalHarmonies1 = new LyricCollection();
            InternalHarmonies2 = new LyricCollection();
            InternalHarmonies3 = new LyricCollection();
            InternalVocals.Initialize();
            InternalHarmonies1.Initialize();
            InternalHarmonies2.Initialize();
            InternalHarmonies3.Initialize();
            if (doall)
            {
                InternalsToPublics();
            }
        }

        public bool ReadMIDIFile(string midi, bool output_info = true)
        {
            if (!File.Exists(midi)) return false;
            var Tools = new NemoTools();
            LengthLong = 0;
            MIDIFile = null;
            MIDIFile = Tools.NemoLoadMIDI(midi);
            if (MIDIFile == null) return false;
            try
            {
                TicksPerQuarter = MIDIFile.DeltaTicksPerQuarterNote;
                BuildTempoList();
                BuildTimeSignatureList();
                for (var i = 0; i < MIDIFile.Events.Tracks; i++)
                {
                    var trackname = MIDIFile.Events[i][0].ToString();
                    if (trackname.Contains("DRUMS") && !trackname.Contains("FNF") && !trackname.Contains("BAND"))
                    {
                        if (trackname.Contains("PLASTIC")) //for Fortnite Festival
                        {
                            MIDIInfo.Drums = new MIDITrack { Name = "Drums", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
                            MIDIInfo.Drums.Initialize();
                        }
                        GetDiscoFlips(MIDIFile.Events[i]);
                        GetToms(MIDIFile.Events[i]);
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.Drums.ValidNotes, out toadd, true);
                        if (!output_info) continue;
                        MIDIInfo.Drums.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Drums.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Drums.ChartedNotes);
                        MIDIInfo.Drums.Solos = GetInstrumentSolos(MIDIFile.Events[i], 103);
                        foreach (var note in MIDIInfo.Drums.ChartedNotes)
                        {
                            switch (note.NoteNumber)
                            {
                                case 96:
                                    note.NoteName = "KICK";
                                    break;
                                case 97:
                                    note.NoteName = "SNARE";
                                    break;
                                case 98:
                                    note.NoteName = "CYMBAL";
                                    break;
                                case 99:
                                    note.NoteName = "CYMBAL";
                                    break;
                                case 100:
                                    note.NoteName = "CYMBAL";
                                    break;
                                case 110:
                                    note.NoteName = "TOM";
                                    break;
                                case 111:
                                    note.NoteName = "TOM";
                                    break;
                                case 112:
                                    note.NoteName = "TOM";
                                    break;
                            }
                        }
                    }
                    else if (trackname.Contains("BASS") && !trackname.Contains("FNF") && !trackname.Contains("REAL") && !trackname.Contains("BAND") && !trackname.Contains("COOP"))
                    {
                        if (trackname.Contains("PLASTIC")) //for Fortnite Festival
                        {
                            MIDIInfo.Bass = new MIDITrack { Name = "Bass", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
                            MIDIInfo.Bass.Initialize();
                        }
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.Bass.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Bass.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Bass.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Bass.ChartedNotes);
                        MIDIInfo.Bass.Solos = GetInstrumentSolos(MIDIFile.Events[i], 103);
                        foreach (var note in MIDIInfo.Bass.ChartedNotes)
                        {
                            switch (note.NoteNumber)
                            {
                                case 96:
                                    note.NoteName = "G";
                                    break;
                                case 97:
                                    note.NoteName = "R";
                                    break;
                                case 98:
                                    note.NoteName = "Y";
                                    break;
                                case 99:
                                    note.NoteName = "B";
                                    break;
                                case 100:
                                    note.NoteName = "O";
                                    break;
                            }
                        }
                    }
                    else if ((trackname.Contains("GUITAR") && !trackname.Contains("FNF") && !trackname.Contains("REAL") && !trackname.Contains("COOP")) || trackname.Contains("T1 GEMS"))
                    {
                        if (trackname.Contains("PLASTIC")) //for Fortnite Festival
                        {
                            MIDIInfo.Guitar = new MIDITrack { Name = "Guitar", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
                            MIDIInfo.Guitar.Initialize();
                        }
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.Guitar.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Guitar.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Guitar.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Guitar.ChartedNotes);
                        MIDIInfo.Guitar.Solos = GetInstrumentSolos(MIDIFile.Events[i], 103);
                        foreach (var note in MIDIInfo.Guitar.ChartedNotes)
                        {
                            switch (note.NoteNumber)
                            {
                                case 96:
                                    note.NoteName = "G";
                                    break;
                                case 97:
                                    note.NoteName = "R";
                                    break;
                                case 98:
                                    note.NoteName = "Y";
                                    break;
                                case 99:
                                    note.NoteName = "B";
                                    break;
                                case 100:
                                    note.NoteName = "O";
                                    break;
                            }
                        }
                    }
                    else if (trackname.Contains("KEYS") && !trackname.Contains("FNF") && !trackname.Contains("REAL") && !trackname.Contains("ANIM"))
                    {
                        if (trackname.Contains("PLASTIC")) //for Fortnite Festival
                        {
                            MIDIInfo.Keys = new MIDITrack { Name = "Keys", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } }; //not supported currently but future proofing
                            MIDIInfo.Keys.Initialize();
                        }
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.Keys.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Keys.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Keys.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Keys.ChartedNotes);
                        MIDIInfo.Keys.Solos = GetInstrumentSolos(MIDIFile.Events[i], 103);
                        foreach (var note in MIDIInfo.Keys.ChartedNotes)
                        {
                            switch (note.NoteNumber)
                            {
                                case 96:
                                    note.NoteName = "G";
                                    break;
                                case 97:
                                    note.NoteName = "R";
                                    break;
                                case 98:
                                    note.NoteName = "Y";
                                    break;
                                case 99:
                                    note.NoteName = "B";
                                    break;
                                case 100:
                                    note.NoteName = "O";
                                    break;
                            }
                        }
                    }
                    else if (trackname.Contains("REAL_KEYS_X"))
                    {
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.ProKeys.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.ProKeys.ChartedNotes.AddRange(toadd);
                        MIDIInfo.ProKeys.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.ProKeys.ChartedNotes);
                        MIDIInfo.ProKeys.Solos = GetInstrumentSolos(MIDIFile.Events[i], 115);
                    }
                    else if (trackname.Contains("VOCALS") || trackname.Contains("vocals_1_expert")) //PowerGig
                    {
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.Vocals.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Vocals.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Vocals.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Vocals.ChartedNotes);
                        GetPhraseMarkers(MIDIFile.Events[i], InternalPhrasesVocals, trackname.Contains("vocals_1_expert"));
                        GetInternalLyrics(MIDIFile.Events[i], 0, InternalPhrasesVocals);
                        MIDIInfo.UsesCowbell = SongUsesCowbell(MIDIFile.Events[i]);
                    }
                    else if (trackname.Contains("HARM1"))
                    {
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.Harm1.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Harm1.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Harm1.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Harm1.ChartedNotes);
                        GetPhraseMarkers(MIDIFile.Events[i], InternalPhrasesHarm1);
                        GetInternalLyrics(MIDIFile.Events[i], 1, InternalPhrasesHarm1);
                    }
                    else if (trackname.Contains("HARM2"))
                    {
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.Harm2.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Harm2.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Harm1.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Harm2.ChartedNotes, MIDIInfo.Harm1.NoteRange);
                        GetPhraseMarkers(MIDIFile.Events[i], InternalPhrasesHarm2);
                        GetInternalLyrics(MIDIFile.Events[i], 2, InternalPhrasesHarm2);
                    }
                    else if (trackname.Contains("HARM3"))
                    {
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile.Events[i], MIDIInfo.Harm3.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Harm3.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Harm1.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Harm3.ChartedNotes, MIDIInfo.Harm1.NoteRange);
                        //this purposefully uses markers from harm1
                        //but we have to clear out the text, we just want the phrase markers
                        foreach (var phrase in InternalPhrasesHarm1.Phrases.Select(phrases => new LyricPhrase
                        {
                            PhraseStart = phrases.PhraseStart,
                            PhraseEnd = phrases.PhraseEnd,
                            PhraseText = ""
                        }))
                        {
                            InternalPhrasesHarm3.Phrases.Add(phrase);
                        }
                        GetInternalLyrics(MIDIFile.Events[i], 3, InternalPhrasesHarm3);
                    }
                    else if (trackname.Contains("EVENTS") && output_info)
                    {
                        foreach (var note in MIDIFile.Events[i])
                        {
                            switch (note.CommandCode)
                            {
                                case MidiCommandCode.MetaEvent:
                                    var section_event = (MetaEvent)note;
                                    if (section_event.MetaEventType != MetaEventType.Lyric &&
                                        section_event.MetaEventType != MetaEventType.TextEvent)
                                    {
                                        continue;
                                    }
                                    if (section_event.ToString().Contains("[section "))
                                    {
                                        var index = section_event.ToString().IndexOf("[", StringComparison.Ordinal);
                                        var new_section = section_event.ToString().Substring(index, section_event.ToString().Length - index);
                                        new_section = new_section.Replace("section ", "prc_");
                                        new_section = new_section.Replace("guitar", "gtr");
                                        new_section = new_section.Replace("practice_outro", "outro");
                                        new_section = new_section.Replace("big_rock_ending", "bre");
                                        new_section = new_section.Replace(" ", "_").Replace("-", "").Replace("!", "").Replace("?", "");
                                        GetPracticeSession(new_section, section_event.AbsoluteTime);
                                    }
                                    else if (section_event.ToString().Contains("[prc_"))
                                    {
                                        GetPracticeSession(section_event.ToString(), section_event.AbsoluteTime);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            MIDIInfo.AverageBPM = AverageBPM();
            if (!output_info) return true;
            InternalsToPublics();
            PhrasesVocals.Sort();
            PhrasesHarm1.Sort();
            PhrasesHarm2.Sort();
            PhrasesHarm3.Sort();
            LyricsVocals.Sort();
            LyricsHarm1.Sort();
            LyricsHarm2.Sort();
            LyricsHarm3.Sort();
            PracticeSessions.Sort((a, b) => a.SectionStart.CompareTo(b.SectionStart));
            MIDI_Chart.Drums.Sort();
            MIDI_Chart.Bass.Sort();
            MIDI_Chart.Guitar.Sort();
            MIDI_Chart.ProKeys.Sort();
            MIDI_Chart.Keys.Sort();
            MIDI_Chart.Vocals.Sort();
            MIDI_Chart.Harm1.Sort();
            MIDI_Chart.Harm2.Sort();
            MIDI_Chart.Harm3.Sort();
            MIDI_Chart.Harm2.NoteRange = MIDI_Chart.Harm1.NoteRange;
            MIDI_Chart.Harm3.NoteRange = MIDI_Chart.Harm1.NoteRange;
            return true;
        }

        private void GetDiscoFlips(IEnumerable<MidiEvent> track)
        {
            foreach (var midiEvent in track.Where(midiEvent => midiEvent.CommandCode == MidiCommandCode.MetaEvent).Where(midiEvent => midiEvent.ToString().Contains("mix 3 drums")))
            {
                if (midiEvent.ToString().Contains("d]"))
                {
                    MIDIInfo.DiscoFlips.Add(new SpecialMarker { MarkerBegin = GetRealtime(midiEvent.AbsoluteTime), MarkerEnd = -1.0 });
                }
                else
                {
                    if (MIDIInfo.DiscoFlips.Any() && MIDIInfo.DiscoFlips[MIDIInfo.DiscoFlips.Count - 1].MarkerEnd == -1.0)
                    {
                        MIDIInfo.DiscoFlips[MIDIInfo.DiscoFlips.Count - 1].MarkerEnd = GetRealtime(midiEvent.AbsoluteTime);
                    }
                }
            }
        }

        private void GetToms(IList<MidiEvent> track)
        {
            var ValidToms = new List<int> { 110, 111, 112 };
            for (var z = 0; z < track.Count(); z++)
            {
                try
                {
                    var notes = track[z];
                    if (notes.AbsoluteTime > LengthLong)
                    {
                        LengthLong = notes.AbsoluteTime;
                    }
                    if (notes.CommandCode != MidiCommandCode.NoteOn) continue;
                    var note = (NoteOnEvent)notes;
                    if (note.Velocity <= 0 || !ValidToms.Contains(note.NoteNumber)) continue;
                    var time = GetRealtime(note.AbsoluteTime);
                    var length = GetRealtime(note.NoteLength);
                    var end = Math.Round(time + length, 5);
                    var n = new MIDINote
                    {
                        NoteStart = time,
                        NoteLength = length,
                        NoteEnd = end,
                        NoteNumber = note.NoteNumber,
                        isTom = true
                    };
                    MIDIInfo.Toms.Add(n);
                }
                catch (Exception)
                { }
            }
        }

        private List<SpecialMarker> GetInstrumentSolos(IEnumerable<MidiEvent> track, int solo_note)
        {
            return (from notes in track where notes.CommandCode == MidiCommandCode.NoteOn select (NoteOnEvent)notes into note where note.Velocity > 0 && note.NoteNumber == solo_note let time = GetRealtime(note.AbsoluteTime) let end = GetRealtime(note.AbsoluteTime + note.NoteLength) select new SpecialMarker { MarkerBegin = time, MarkerEnd = end }).ToList();
        }

        private static bool SongUsesCowbell(IEnumerable<MidiEvent> track)
        {
            return track.Any(midiEvent => midiEvent.CommandCode == MidiCommandCode.MetaEvent && midiEvent.ToString().Contains("[cowbell"));
        }

        private void InternalsToPublics()
        {
            MIDI_Chart = MIDIInfo;
            PhrasesVocals = InternalPhrasesVocals;
            PhrasesHarm1 = InternalPhrasesHarm1;
            PhrasesHarm2 = InternalPhrasesHarm2;
            PhrasesHarm3 = InternalPhrasesHarm3;
            LyricsVocals = InternalVocals;
            LyricsHarm1 = InternalHarmonies1;
            LyricsHarm2 = InternalHarmonies2;
            LyricsHarm3 = InternalHarmonies3;
            PracticeSessions = InternalPracticeSessions;
        }

        private void GetPracticeSession(string session, long start_time)
        {
            var index = session.IndexOf("[", StringComparison.Ordinal);
            session = session.Substring(index, session.Length - index).Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Trim();
            if (File.Exists(Application.StartupPath + "\\bin\\sections"))
            {
                var sr = new StreamReader(Application.StartupPath + "\\bin\\sections");
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    line = line.Replace("(", "").Replace(")", "");
                    var i = line.IndexOf("\"", StringComparison.Ordinal);
                    var prc = line.Substring(0, i).Trim();
                    if (prc != session) continue;
                    session = line.Substring(i, line.Length - i).Replace("\"", "").Trim();
                    session = session.Replace("Gtr", "Guitar");
                    var myTI = new CultureInfo("en-US", false).TextInfo;
                    session = myTI.ToTitleCase(session);
                    break;
                }
                sr.Dispose();
            }
            var practice = new PracticeSection
            {
                SectionStart = GetRealtime(start_time),
                SectionName = "[" + session.Replace("prc", "").Replace("_", " ").Trim() + "]"
            };
            InternalPracticeSessions.Add(practice);
        }

        private void GetPhraseMarkers(IEnumerable<MidiEvent> track, PhraseCollection collection, bool isPowerGig = false)
        {
            double lastStartTime = 0.0;
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
                                if (collection.Phrases.Any())
                                {
                                    var index = collection.Phrases.Count - 1;
                                    if (collection.Phrases[index].PhraseStart == time)
                                    {
                                        continue; //for old double-stacked markers
                                    }
                                }
                                var phrase = new LyricPhrase
                                {
                                    PhraseStart = time,
                                    PhraseEnd = end,
                                };
                                collection.Phrases.Add(phrase); //new line
                                break;
                        }
                        break;
                    case MidiCommandCode.MetaEvent:
                        if (!isPowerGig) continue;
                        var vocal_event = (MetaEvent)notes;
                        if ((vocal_event.MetaEventType == MetaEventType.Lyric ||
                             vocal_event.MetaEventType == MetaEventType.TextEvent) &&
                            !vocal_event.ToString().Contains("["))
                        {
                            var lyric = GetCleanMIDILyric(vocal_event.ToString());
                            if (string.IsNullOrEmpty(lyric)) continue;
                            if (lyric.Equals("\\r")) //as best as I can guess, this is their phrase marker
                            {
                                var endTime = GetRealtime(vocal_event.AbsoluteTime);
                                var phrase = new LyricPhrase
                                {
                                    PhraseStart = lastStartTime,
                                    PhraseEnd = endTime,//we have no end time so just use the same start and end
                                };
                                collection.Phrases.Add(phrase); //new line
                                lastStartTime = endTime;
                                break;
                            }                            
                        }
                        break;
                }
            }
        }

        private void GetInternalLyrics(IEnumerable<MidiEvent> track, int type, PhraseCollection collection)
        {
            if (!collection.Phrases.Any()) return;            
            foreach (var notes in track)
            {
                switch (notes.CommandCode)
                {
                    case MidiCommandCode.MetaEvent:
                        var vocal_event = (MetaEvent)notes;
                        if ((vocal_event.MetaEventType == MetaEventType.Lyric ||
                             vocal_event.MetaEventType == MetaEventType.TextEvent) &&
                            !vocal_event.ToString().Contains("["))
                        {
                            var lyric = GetCleanMIDILyric(vocal_event.ToString());
                            if (string.IsNullOrEmpty(lyric.Replace("\\r",""))) continue;
                            var time = GetRealtime(vocal_event.AbsoluteTime);
                            var index = 0;
                            for (var i = 0; i < collection.Phrases.Count; i++)
                            {
                                if (collection.Phrases[i].PhraseStart > time) break;
                                index = i;
                            }
                            collection.Phrases[index].PhraseText = collection.Phrases[index].PhraseText + " " + lyric;
                            var l = new Lyric { LyricText = lyric, LyricStart = time };
                            switch (type)
                            {
                                case 0:
                                    InternalVocals.Lyrics.Add(l);
                                    break;
                                case 1:
                                    InternalHarmonies1.Lyrics.Add(l);
                                    break;
                                case 2:
                                    InternalHarmonies2.Lyrics.Add(l);
                                    break;
                                case 3:
                                    InternalHarmonies3.Lyrics.Add(l);
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
                var lyric = raw_event.Substring(index + 1, raw_event.Length - index - 1);
                return lyric.Replace("\\n", "").Replace("*", ""); //clean up weird PowerGig lyric symbols
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void CheckMIDITrack(IList<MidiEvent> track, ICollection<int> valid_notes, out List<MIDINote> output, bool isDrums = false)
        {
            output = new List<MIDINote>();
            for (var z = 0; z < track.Count(); z++)
            {
                try
                {
                    var notes = track[z];
                    if (notes.AbsoluteTime > LengthLong)
                    {
                        LengthLong = notes.AbsoluteTime;
                    }
                    if (notes.CommandCode != MidiCommandCode.NoteOn) continue;
                    var note = (NoteOnEvent)notes;
                    if (note.Velocity <= 0) continue;
                    if (!valid_notes.Contains(note.NoteNumber)) continue;
                    var time = GetRealtime(note.AbsoluteTime);
                    var length = GetRealtime(note.NoteLength);
                    var end = Math.Round(time + length, 5);
                    if (isDrums && MIDIInfo.DiscoFlips.Any() && (note.NoteNumber == 97 || note.NoteNumber == 98))
                    {
                        if (MIDIInfo.DiscoFlips.Where(flip => flip.MarkerBegin <= time).Any(flip => flip.MarkerEnd > time || flip.MarkerEnd == -1.0))
                        {
                            note.NoteNumber = note.NoteNumber == 97 ? 98 : 97;
                        }
                    }
                    var isTom = MIDIInfo.Toms.Where(tom => tom.NoteStart <= time).Where(tom => tom.NoteEnd >= time).Any(tom => tom.NoteNumber - note.NoteNumber == 12);
                    var n = new MIDINote
                    {
                        NoteStart = time,
                        NoteLength = length,
                        NoteEnd = end,
                        NoteNumber = note.NoteNumber,
                        NoteName = note.NoteName,
                        isTom = isTom
                    };
                    output.Add(n);
                }
                catch (Exception)
                { }
            }
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
            return Math.Round(time / 1000, 5);
        }


        private void BuildTimeSignatureList()
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

        private void BuildTempoList()
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

        private double AverageBPM()
        {
            var total_bpm = 0.0;
            var last = 0.0;
            var bpm = 120.0;
            double difference;
            var LengthSeconds = GetRealtime(LengthLong);
            if (LengthSeconds <= 0.0)
            {
                var count = TempoEvents.Sum(tempo => tempo.BPM);
                return Math.Round(count / TempoEvents.Count, 2);
            }
            foreach (var tempo in TempoEvents)
            {
                var current = GetRealtime(tempo.AbsoluteTime);
                difference = current - last;
                last = GetRealtime(tempo.AbsoluteTime);
                if (difference <= 0.0)
                {
                    bpm = tempo.BPM;
                    continue;
                }
                total_bpm += bpm * (difference / LengthSeconds);
                bpm = tempo.BPM;
            }
            difference = LengthSeconds - last;
            total_bpm += bpm * (difference / LengthSeconds);
            if (total_bpm == 0)
            {
                total_bpm = bpm;
            }
            return Math.Round(total_bpm, 2);
        }
    }

    public class MIDITrack
    {
        public string Name { get; set; }
        public List<MIDINote> ChartedNotes { get; set; }
        public List<int> ValidNotes { get; set; }
        public List<int> NoteRange { get; set; }
        public int ActiveIndex { get; set; }
        public List<SpecialMarker> Solos { get; set; }
        public void Sort()
        {
            ChartedNotes.Sort((a, b) => a.NoteStart.CompareTo(b.NoteStart));
            Solos.Sort((a, b) => a.MarkerBegin.CompareTo(b.MarkerBegin));
        }
        public void Initialize()
        {
            ChartedNotes = new List<MIDINote>();
            NoteRange = new List<int>();
            Solos = new List<SpecialMarker>();
        }
    }

    public class MIDIChart
    {
        public MIDITrack Drums { get; set; }
        public MIDITrack Bass { get; set; }
        public MIDITrack Guitar { get; set; }
        public MIDITrack ProKeys { get; set; }
        public MIDITrack Keys { get; set; }
        public MIDITrack Vocals { get; set; }
        public MIDITrack Harm1 { get; set; }
        public MIDITrack Harm2 { get; set; }
        public MIDITrack Harm3 { get; set; }
        public double AverageBPM { get; set; }
        public bool UsesCowbell { get; set; }
        public List<MIDINote> Toms { get; set; }
        public List<SpecialMarker> DiscoFlips;

        public void Initialize()
        {
            Drums = new MIDITrack { Name = "Drums", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
            Bass = new MIDITrack { Name = "Bass", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
            Guitar = new MIDITrack { Name = "Guitar", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
            ProKeys = new MIDITrack { Name = "ProKeys", ValidNotes = new List<int> { 72, 71, 70, 69, 68, 67, 66, 65, 64, 63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48 } };
            Keys = new MIDITrack { Name = "Keys", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
            Vocals = new MIDITrack
            {
                Name = "Vocals",
                ValidNotes = new List<int>{ 97,96,84,83,82,81,80,79,78,77,76,75,74,73,72,71,70,
                            69,68,67,66,65,64,63,62,61,60,59,58,57,56,55,54,53,52,51,50,49,48,47,46,45,44,43,42,41,40,39,38,37,36 }
            };
            Harm1 = new MIDITrack { Name = "Harm1", ValidNotes = Vocals.ValidNotes };
            Harm2 = new MIDITrack { Name = "Harm2", ValidNotes = Vocals.ValidNotes };
            Harm3 = new MIDITrack { Name = "Harm3", ValidNotes = Vocals.ValidNotes };
            Drums.Initialize();
            Bass.Initialize();
            Guitar.Initialize();
            ProKeys.Initialize();
            Keys.Initialize();
            Vocals.Initialize();
            Harm1.Initialize();
            Harm2.Initialize();
            Harm3.Initialize();
            DiscoFlips = new List<SpecialMarker>();
            Toms = new List<MIDINote>();
            AverageBPM = 0.0;
            UsesCowbell = false;
        }

        public int GetTrackCount()
        {
            const int tall = 2;
            var tracks = 0;
            if (Drums.ChartedNotes.Any())
            {
                tracks++;
            }
            if (Bass.ChartedNotes.Any())
            {
                tracks++;
            }
            if (Guitar.ChartedNotes.Any())
            {
                tracks++;
            }
            if (ProKeys.ChartedNotes.Any())
            {
                tracks += tall;//only count if there's no keys chart, since I'll only display one of the two
            }
            else if (Keys.ChartedNotes.Any())
            {
                tracks++;
            }
            if (Vocals.ChartedNotes.Any())
            {
                tracks += tall;
            }
            return tracks;
        }

        public List<int> GetNoteVariety(List<MIDINote> track, List<int> input = null)
        {
            var variety = new List<int>();
            if (input != null)
            {
                variety = input;
            }
            foreach (var note in track.Where(note => !variety.Contains(note.NoteNumber)))
            {
                variety.Add(note.NoteNumber);
            }
            variety.Sort();
            variety.Reverse();
            return variety;
        }
    }

    public class PhraseCollection
    {
        public List<LyricPhrase> Phrases { get; set; }
        public void Sort()
        {
            Phrases.Sort((a, b) => a.PhraseStart.CompareTo(b.PhraseStart));
        }
        public void Initialize()
        {
            Phrases = new List<LyricPhrase>();
        }
    }

    public class LyricPhrase
    {
        public string PhraseText { get; set; }
        public double PhraseStart { get; set; }
        public double PhraseEnd { get; set; }
    }

    public class LyricCollection
    {
        public List<Lyric> Lyrics { get; set; }
        public void Sort()
        {
            Lyrics.Sort((a, b) => a.LyricStart.CompareTo(b.LyricStart));
        }
        public void Initialize()
        {
            Lyrics = new List<Lyric>();
        }
    }

    public class Lyric
    {
        public string LyricText { get; set; }
        public double LyricStart { get; set; }
    }

    public class MIDINote
    {
        public int NoteNumber { get; set; }
        public double NoteStart { get; set; }
        public double NoteEnd { get; set; }
        public double NoteLength { get; set; }
        public string NoteName { get; set; }
        public Color NoteColor { get; set; }
        public bool isTom { get; set; }

        public long Ticks { get; set; }
    }

    public class TempoEvent
    {
        public long AbsoluteTime { get; set; }
        public double RealTime { get; set; }
        public double BPM { get; set; }
    }

    public class TimeSignature
    {
        public long AbsoluteTime { get; set; }
        public int Numerator { get; set; }
        public int Denominator { get; set; }
    }

    public class PracticeSection
    {
        public double SectionStart { get; set; }
        public string SectionName { get; set; }
    }

    public class SpecialMarker
    {
        public double MarkerBegin { get; set; }
        public double MarkerEnd { get; set; }
    }
}