using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Nautilus
{
    class MIDIStuff
    {
        public int TicksPerQuarter;
        public List<TempoEvent> TempoEvents;
        public List<TimeSignature> TimeSignatures;
        private MidiEventCollection MIDIFile;
        public MIDIChart MIDIInfo;
        public MIDIChart MIDI_Chart;
        public long LengthLong;
        private LyricCollection InternalVocals;
        private LyricCollection InternalHarmonies1;
        private LyricCollection InternalHarmonies2;
        private LyricCollection InternalHarmonies3;
        private List<MIDINote> InternalVocalNotes;
        private List<MIDINote> InternalHarm1Notes;
        private List<MIDINote> InternalHarm2Notes;
        private List<MIDINote> InternalHarm3Notes;
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
        private const int SOLO_MARKER = 103;
        private const int FILL_MARKER = 120;
        private const int OD_MARKER = 116;
        private const int HOPOon = 101;
        private const int HOPOoff = 102;

        public void Initialize(bool doall)
        {
            MIDIInfo = new MIDIChart();
            MIDIInfo.Initialize();
            InternalPhrasesVocals = new PhraseCollection();
            InternalPhrasesHarm1 = new PhraseCollection();
            InternalPhrasesHarm2 = new PhraseCollection();
            InternalPhrasesHarm3 = new PhraseCollection();
            InternalVocalNotes = new List<MIDINote>();
            InternalHarm1Notes = new List<MIDINote>();
            InternalHarm2Notes = new List<MIDINote>();
            InternalHarm3Notes = new List<MIDINote>();
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

        private List<SpecialMarker> GetSpecialMarker(IEnumerable<MidiEvent> track, int marker_note)
        {
            var markers = (from notes in track where notes.CommandCode == MidiCommandCode.NoteOn select (NoteOnEvent)notes into note where note.Velocity > 0 && note.NoteNumber == marker_note let time = GetRealtime(note.AbsoluteTime) let end = GetRealtime(note.AbsoluteTime + note.NoteLength) select new SpecialMarker { MarkerBegin = time, MarkerEnd = end }).ToList();
            return markers;
        }

        public bool ReadMIDIFile(string midi, int HOPOThreshhold, bool output_info = true)
        {
            if (!File.Exists(midi)) return false;
            var Tools = new NemoTools();
            LengthLong = 0;
            MIDIFile = null;
            if (Path.GetExtension(midi).Equals(".chart"))
            {
                MIDIFile = ChartToRockBandMidi.ConvertChartToMidi(midi);
            }
            else
            {
                MIDIFile = Tools.NemoLoadMIDI(midi).Events;
            }
            if (MIDIFile == null) return false;
            try
            {
                TicksPerQuarter = MIDIFile.DeltaTicksPerQuarterNote;
                BuildTempoList();
                BuildTimeSignatureList();
                var didFNFProVocals = false;
                for (var i = 0; i < MIDIFile.Tracks; i++)
                {
                    var trackname = MIDIFile[i][0].ToString();
                    if (trackname.Contains("DRUMS") && !trackname.Contains("FNF") && !trackname.Contains("BAND"))
                    {
                        if (trackname.Contains("PLASTIC")) //for Fortnite Festival
                        {
                            MIDIInfo.Drums = new MIDITrack { Name = "Drums", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
                            MIDIInfo.Drums.Initialize();
                        }
                        GetDiscoFlips(MIDIFile[i]);
                        MIDIInfo.Drums.Toms = GetToms(MIDIFile[i]);
                        MIDIInfo.Drums.Overdrive = GetSpecialMarker(MIDIFile[i], OD_MARKER);
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.Drums, MIDIInfo.Drums.ValidNotes, out toadd, true);
                        if (!output_info) continue;
                        MIDIInfo.Drums.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Drums.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Drums.ChartedNotes);
                        MIDIInfo.Drums.Solos = GetInstrumentSolos(MIDIFile[i], SOLO_MARKER);
                        MIDIInfo.Drums.Fills = GetSpecialMarker(MIDIFile[i], FILL_MARKER);
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
                        MIDIInfo.Bass.Overdrive = GetSpecialMarker(MIDIFile[i], OD_MARKER);
                        MIDIInfo.Bass.HOPOoff = GetSpecialMarker(MIDIFile[i], HOPOoff);
                        MIDIInfo.Bass.HOPOon = GetSpecialMarker(MIDIFile[i], HOPOon);
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.Bass, MIDIInfo.Bass.ValidNotes, out toadd);
                        //if (!output_info) continue;
                        MIDIInfo.Bass.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Bass.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Bass.ChartedNotes);
                        MIDIInfo.Bass.Solos = GetInstrumentSolos(MIDIFile[i], SOLO_MARKER);
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
                            foreach (var hopo in MIDIInfo.Bass.HOPOoff)
                            {
                                if (hopo.MarkerBegin <= note.NoteStart && hopo.MarkerEnd > note.NoteStart)
                                {
                                    note.isHOPOoff = true;
                                    break;
                                }
                            }
                            foreach (var hopo in MIDIInfo.Bass.HOPOon)
                            {
                                if (hopo.MarkerBegin <= note.NoteStart && hopo.MarkerEnd > note.NoteStart)
                                {
                                    note.isHOPOon = true;
                                    note.IsForcedHOPOon = true;
                                    break;
                                }
                            }
                        }
                        // Step 1: Sort by tick time
                        MIDIInfo.Bass.ChartedNotes = MIDIInfo.Bass.ChartedNotes.OrderBy(n => n.Ticks).ToList();

                        // Step 2: Mark chords (same tick time)
                        for (int z = 0; z < MIDIInfo.Bass.ChartedNotes.Count;)
                        {
                            long tick = MIDIInfo.Bass.ChartedNotes[z].Ticks;
                            int count = 1;

                            // Look ahead to count how many notes share the same tick
                            while (z + count < MIDIInfo.Bass.ChartedNotes.Count && MIDIInfo.Bass.ChartedNotes[z + count].Ticks == tick)
                                count++;

                            // Mark as chord if more than one note at the same tick
                            if (count > 1)
                            {
                                for (int j = 0; j < count; j++)
                                {
                                    MIDIInfo.Bass.ChartedNotes[z + j].IsChord = true;
                                    MIDIInfo.Bass.ChartedNotes[z + j].isHOPOon = false; // Enforce: chords cannot be HOPO
                                }
                            }
                            z += count;
                        }

                        // Step 3: Apply HOPO logic (skip chords and forced off)
                        MIDINote lastPlayableNote = null;

                        for (int n = 0; n < MIDIInfo.Bass.ChartedNotes.Count; n++)
                        {
                            var curr = MIDIInfo.Bass.ChartedNotes[n];

                            // Skip if this note is part of a chord or explicitly forced off
                            if (curr.IsChord || curr.isHOPOoff)
                            {
                                lastPlayableNote = curr;
                                continue;
                            }

                            // If there's no valid previous note, can't be HOPO
                            if (lastPlayableNote == null)
                            {
                                lastPlayableNote = curr;
                                continue;
                            }

                            // Rule 1: If same note number as previous and not forced on, HOPO off
                            if (curr.NoteNumber == lastPlayableNote.NoteNumber && !curr.IsForcedHOPOon)
                            {
                                curr.isHOPOon = false;
                                lastPlayableNote = curr;
                                continue;
                            }

                            // Rule 2: If this note is the start of a repeated note group → force HOPO off
                            if (n + 1 < MIDIInfo.Bass.ChartedNotes.Count)
                            {
                                var next = MIDIInfo.Bass.ChartedNotes[n + 1];
                                if (!next.IsChord && next.NoteNumber == curr.NoteNumber)
                                {
                                    curr.isHOPOon = false;
                                    lastPlayableNote = curr;
                                    continue;
                                }
                            }

                            // Rule 3: Timing check
                            long deltaTicks = curr.Ticks - lastPlayableNote.Ticks;
                            if (deltaTicks <= HOPOThreshhold)
                            {
                                curr.isHOPOon = true;
                            }

                            lastPlayableNote = curr;
                        }
                    }
                    else if ((trackname.Contains("GUITAR") && !trackname.Contains("FNF") && !trackname.Contains("REAL") && !trackname.Contains("COOP")) || trackname.Contains("T1 GEMS"))
                    {
                        if (trackname.Contains("PLASTIC")) //for Fortnite Festival
                        {
                            MIDIInfo.Guitar = new MIDITrack { Name = "Guitar", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } };
                            MIDIInfo.Guitar.Initialize();
                        }
                        MIDIInfo.Guitar.Overdrive = GetSpecialMarker(MIDIFile[i], OD_MARKER);
                        MIDIInfo.Bass.HOPOoff = GetSpecialMarker(MIDIFile[i], HOPOoff);
                        MIDIInfo.Bass.HOPOon = GetSpecialMarker(MIDIFile[i], HOPOon);
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.Guitar, MIDIInfo.Guitar.ValidNotes, out toadd);
                        //if (!output_info) continue;
                        MIDIInfo.Guitar.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Guitar.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Guitar.ChartedNotes);
                        MIDIInfo.Guitar.Solos = GetInstrumentSolos(MIDIFile[i], SOLO_MARKER);
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
                            foreach (var hopo in MIDIInfo.Guitar.HOPOoff)
                            {
                                if (hopo.MarkerBegin <= note.NoteStart && hopo.MarkerEnd > note.NoteStart)
                                {
                                    note.isHOPOoff = true;
                                    break;
                                }
                            }
                            foreach (var hopo in MIDIInfo.Guitar.HOPOon)
                            {
                                if (hopo.MarkerBegin <= note.NoteStart && hopo.MarkerEnd > note.NoteStart)
                                {
                                    note.isHOPOon = true;
                                    note.IsForcedHOPOon = true;
                                    break;
                                }
                            }
                        }

                        // Step 1: Sort by tick time
                        MIDIInfo.Guitar.ChartedNotes = MIDIInfo.Guitar.ChartedNotes.OrderBy(n => n.Ticks).ToList();

                        // Step 2: Mark chords (same tick time)
                        for (int z = 0; z < MIDIInfo.Guitar.ChartedNotes.Count;)
                        {
                            long tick = MIDIInfo.Guitar.ChartedNotes[z].Ticks;
                            int count = 1;

                            // Look ahead to count how many notes share the same tick
                            while (z + count < MIDIInfo.Guitar.ChartedNotes.Count && MIDIInfo.Guitar.ChartedNotes[z + count].Ticks == tick)
                                count++;

                            // Mark as chord if more than one note at the same tick
                            if (count > 1)
                            {
                                for (int j = 0; j < count; j++)
                                {
                                    MIDIInfo.Guitar.ChartedNotes[z + j].IsChord = true;
                                    MIDIInfo.Guitar.ChartedNotes[z + j].isHOPOon = false; // Enforce: chords cannot be HOPO
                                }
                            }
                            z += count;
                        }

                        // Step 3: Apply HOPO logic (skip chords and forced off)
                        MIDINote lastPlayableNote = null;

                        for (int n = 0; n < MIDIInfo.Guitar.ChartedNotes.Count; n++)
                        {
                            var curr = MIDIInfo.Guitar.ChartedNotes[n];

                            // Skip if this note is part of a chord or explicitly forced off
                            if (curr.IsChord || curr.isHOPOoff)
                            {
                                lastPlayableNote = curr;
                                continue;
                            }

                            // If there's no valid previous note, can't be HOPO
                            if (lastPlayableNote == null)
                            {
                                lastPlayableNote = curr;
                                continue;
                            }

                            // Rule 1: If same note number as previous and not forced on, HOPO off
                            if (curr.NoteNumber == lastPlayableNote.NoteNumber && !curr.IsForcedHOPOon)
                            {
                                curr.isHOPOon = false;
                                lastPlayableNote = curr;
                                continue;
                            }

                            // Rule 2: If this note is the start of a repeated note group → force HOPO off
                            if (n + 1 < MIDIInfo.Guitar.ChartedNotes.Count)
                            {
                                var next = MIDIInfo.Guitar.ChartedNotes[n + 1];
                                if (!next.IsChord && next.NoteNumber == curr.NoteNumber)
                                {
                                    curr.isHOPOon = false;
                                    lastPlayableNote = curr;
                                    continue;
                                }
                            }

                            // Rule 3: Timing check
                            long deltaTicks = curr.Ticks - lastPlayableNote.Ticks;
                            if (deltaTicks <= HOPOThreshhold)
                            {
                                curr.isHOPOon = true;
                            }

                            lastPlayableNote = curr;
                        }
                    }
                    else if (trackname.Contains("KEYS") && !trackname.Contains("FNF") && !trackname.Contains("REAL") && !trackname.Contains("ANIM"))
                    {
                        if (trackname.Contains("PLASTIC")) //for Fortnite Festival
                        {
                            MIDIInfo.Keys = new MIDITrack { Name = "Keys", ValidNotes = new List<int> { 100, 99, 98, 97, 96 } }; //not supported currently but future proofing
                            MIDIInfo.Keys.Initialize();
                        }
                        MIDIInfo.Keys.Overdrive = GetSpecialMarker(MIDIFile[i], OD_MARKER);
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.Keys, MIDIInfo.Keys.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Keys.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Keys.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Keys.ChartedNotes);
                        MIDIInfo.Keys.Solos = GetInstrumentSolos(MIDIFile[i], SOLO_MARKER);
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
                        MIDIInfo.ProKeys.Overdrive = GetSpecialMarker(MIDIFile[i], OD_MARKER);
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.ProKeys, MIDIInfo.ProKeys.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.ProKeys.ChartedNotes.AddRange(toadd);
                        MIDIInfo.ProKeys.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.ProKeys.ChartedNotes);
                        MIDIInfo.ProKeys.Solos = GetInstrumentSolos(MIDIFile[i], 115);
                    }
                    else if (trackname.Contains("VOCALS") && !trackname.Contains("FNF") && !didFNFProVocals)
                    {
                        if (trackname.Contains("PRO VOCALS"))
                        {
                            didFNFProVocals = true;
                        }
                        List<MIDINote> toAdd;
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.Vocals, MIDIInfo.Vocals.ValidNotes, out toAdd);
                        if (!output_info) continue;
                        MIDIInfo.Vocals.ChartedNotes.AddRange(toAdd);
                        MIDIInfo.Vocals.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Vocals.ChartedNotes);
                        GetPhraseMarkers(MIDIFile[i], InternalPhrasesVocals);
                        GetInternalLyrics(MIDIFile[i], 0, InternalPhrasesVocals);
                        MIDIInfo.UsesPercussion = SongUsesPercussion(MIDIFile[i]);

                        //only do this if we have exact match of lyrics and vocal notes
                        var ChartedNotesNoPercussion = new List<MIDINote>();
                        for (var c = 0; c < MIDIInfo.Vocals.ChartedNotes.Count; c++)
                        {
                            if ((MIDIInfo.Vocals.ChartedNotes[c].NoteNumber < 96)) //avoid percussion
                            {
                                ChartedNotesNoPercussion.Add(MIDIInfo.Vocals.ChartedNotes[c]);
                            }
                        }
                        if (ChartedNotesNoPercussion.Count == InternalVocals.Lyrics.Count)
                        {
                            ChartedNotesNoPercussion.Sort((a, b) => a.NoteStart.CompareTo(b.NoteStart));

                            for (int z = 0; z < ChartedNotesNoPercussion.Count; z++)
                            {
                                InternalVocals.Lyrics[z].Duration = ChartedNotesNoPercussion[z].NoteLength;
                            }
                        }
                    }
                    else if (trackname.Contains("HARM1"))
                    {
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.Harm1, MIDIInfo.Harm1.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Harm1.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Harm1.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Harm1.ChartedNotes);
                        GetPhraseMarkers(MIDIFile[i], InternalPhrasesHarm1);
                        GetInternalLyrics(MIDIFile[i], 1, InternalPhrasesHarm1);
                    }
                    else if (trackname.Contains("HARM2"))
                    {
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.Harm2, MIDIInfo.Harm2.ValidNotes, out toadd);
                        if (!output_info) continue;
                        MIDIInfo.Harm2.ChartedNotes.AddRange(toadd);
                        MIDIInfo.Harm1.NoteRange = MIDIInfo.GetNoteVariety(MIDIInfo.Harm2.ChartedNotes, MIDIInfo.Harm1.NoteRange);
                        GetPhraseMarkers(MIDIFile[i], InternalPhrasesHarm2);
                        GetInternalLyrics(MIDIFile[i], 2, InternalPhrasesHarm2);
                    }
                    else if (trackname.Contains("HARM3"))
                    {
                        List<MIDINote> toadd;
                        CheckMIDITrack(MIDIFile[i], MIDIInfo.Harm3, MIDIInfo.Harm3.ValidNotes, out toadd);
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
                        GetInternalLyrics(MIDIFile[i], 3, InternalPhrasesHarm3);
                    }
                    else if (trackname.Contains("EVENTS") && output_info)
                    {
                        foreach (var note in MIDIFile[i])
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
            catch// (Exception ex)
            {
                //Clipboard.SetText(ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.StackTrace);
                //MessageBox.Show("Error when loading with MidiProcessor:\n" + ex.Message);
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

        private List<MIDINote> GetToms(IList<MidiEvent> track)
        {
            var ValidToms = new List<int> { 110, 111, 112 };
            var toms = new List<MIDINote>();
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
                    toms.Add(n);
                }
                catch (Exception)
                { }
            }
            return toms;
        }

        private List<SpecialMarker> GetInstrumentSolos(IEnumerable<MidiEvent> track, int solo_note)
        {
            return (from notes in track where notes.CommandCode == MidiCommandCode.NoteOn select (NoteOnEvent)notes into note where note.Velocity > 0 && note.NoteNumber == solo_note let time = GetRealtime(note.AbsoluteTime) let end = GetRealtime(note.AbsoluteTime + note.NoteLength) select new SpecialMarker { MarkerBegin = time, MarkerEnd = end }).ToList();
        }

        private static bool SongUsesPercussion(IEnumerable<MidiEvent> track)
        {
            return track.Any(midiEvent => midiEvent.CommandCode == MidiCommandCode.MetaEvent && (midiEvent.ToString().Contains("[cowbell_")
            || midiEvent.ToString().Contains("[clap_") || midiEvent.ToString().Contains("[tambourine_")));
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

        private void GetPhraseMarkers(IEnumerable<MidiEvent> track, PhraseCollection collection)
        {
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
                            if (string.IsNullOrEmpty(lyric)) continue;
                            var time = GetRealtime(vocal_event.AbsoluteTime);
                            var index = 0;
                            for (var i = 0; i < collection.Phrases.Count; i++)
                            {
                                if (collection.Phrases[i].PhraseStart > time) break;
                                index = i;
                            }
                            collection.Phrases[index].PhraseText = collection.Phrases[index].PhraseText + " " + lyric;
                            var l = new Lyric { Text = lyric, Start = time, Ticks = vocal_event.AbsoluteTime, DisplayText = ProcessLine(lyric, true) };
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
                            switch (type)
                            {
                                case 1:
                                    InternalHarm1Notes.Add(n);
                                    break;
                                case 2:
                                    InternalHarm2Notes.Add(n);
                                    break;
                                case 3:
                                    InternalHarm3Notes.Add(n);
                                    break;
                                default:
                                    InternalVocalNotes.Add(n);
                                    break;
                            }
                        }
                        break;
                }
            }

            switch (type)
            {
                case 0:
                default:
                    foreach (var lyric in InternalVocals.Lyrics)
                    {
                        foreach (var note in InternalVocalNotes)
                        {
                            if (note.Ticks <= lyric.Ticks && note.Ticks >= lyric.Ticks)
                            {
                                lyric.End = note.NoteEnd;
                                lyric.Duration = lyric.End - lyric.Start;
                                break;
                            }
                        }
                    }
                    break;
                case 1:
                    foreach (var lyric in InternalHarmonies1.Lyrics)
                    {
                        foreach (var note in InternalHarm1Notes)
                        {
                            if (note.Ticks <= lyric.Ticks && note.Ticks >= lyric.Ticks)
                            {
                                lyric.End = note.NoteEnd;
                                lyric.Duration = lyric.End - lyric.Start;
                                break;
                            }
                        }
                    }
                    break;
                case 2:
                    foreach (var lyric in InternalHarmonies2.Lyrics)
                    {
                        foreach (var note in InternalHarm2Notes)
                        {
                            if (note.Ticks <= lyric.Ticks && note.Ticks >= lyric.Ticks)
                            {
                                lyric.End = note.NoteEnd;
                                lyric.Duration = lyric.End - lyric.Start;
                                break;
                            }
                        }
                    }
                    break;
                case 3:
                    foreach (var lyric in InternalHarmonies3.Lyrics)
                    {
                        foreach (var note in InternalHarm3Notes)
                        {
                            if (note.Ticks <= lyric.Ticks && note.Ticks >= lyric.Ticks)
                            {
                                lyric.End = note.NoteEnd;
                                lyric.Duration = lyric.End - lyric.Start;
                                break;
                            }
                        }
                    }
                    break;
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
                return lyric;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public string ProcessLine(string line, bool clean)
        {
            if (line == null) return "";
            string newline;
            if (clean)
            {
                newline = line.Replace("$", "");
                newline = newline.Replace("%", "");
                newline = newline.Replace("#", "");
                newline = newline.Replace("^", "");
                newline = newline.Replace("- + ", "");
                newline = newline.Replace("+- ", "");
                newline = newline.Replace("- ", "");
                newline = newline.Replace(" + ", " ");
                newline = newline.Replace(" +", "");
                newline = newline.Replace("+ ", "");
                newline = newline.Replace("+-", "");
                newline = newline.Replace("=", "-");
                newline = newline.Replace("§", "‿");
                newline = newline.Replace("- ", "-").Trim();
                if (newline.EndsWith("+", StringComparison.Ordinal))
                {
                    newline = newline.Substring(0, newline.Length - 1).Trim();
                }
                if (newline.EndsWith("-", StringComparison.Ordinal))
                {
                    newline = newline.Substring(0, newline.Length - 1);
                }
            }
            else
            {
                newline = line;
            }
            return newline.Replace("/", "").Trim();
        }

        private void CheckMIDITrack(IList<MidiEvent> track, MIDITrack instrument, ICollection<int> valid_notes, out List<MIDINote> output, bool isDrums = false)
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
                    /*var time = GetRealtime(note.AbsoluteTime);
                    var length = GetRealtime(note.NoteLength);
                    var end = Math.Round(time + length, 5);*/
                    var time = GetRealtime(note.AbsoluteTime);
                    var end = GetRealtime(note.AbsoluteTime + note.NoteLength);
                    var length = Math.Round(end - time, 5);
                    end = Math.Round(end, 5);
                    if (isDrums && MIDIInfo.DiscoFlips.Any() && (note.NoteNumber == 97 || note.NoteNumber == 98))
                    {
                        if (MIDIInfo.DiscoFlips.Where(flip => flip.MarkerBegin <= time).Any(flip => flip.MarkerEnd > time || flip.MarkerEnd == -1.0))
                        {
                            note.NoteNumber = note.NoteNumber == 97 ? 98 : 97;
                        }
                    }
                    var isTom = instrument.Toms.Where(tom => tom.NoteStart <= time).Where(tom => tom.NoteEnd >= time).Any(tom => tom.NoteNumber - note.NoteNumber == 12);
                    var hasOD = instrument.Overdrive.Where(OD => OD.MarkerBegin <= time).Any(OD => OD.MarkerEnd >= end);
                    var n = new MIDINote
                    {
                        NoteStart = time,
                        NoteLength = length,
                        NoteEnd = end,
                        NoteNumber = note.NoteNumber,
                        NoteName = note.NoteName,
                        isTom = isTom,
                        hasOD = hasOD,
                        Ticks = notes.AbsoluteTime,
                    };
                    output.Add(n);
                }
                catch (Exception)
                { }
            }
        }

        public double GetRealtime(long absdelta)
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
            foreach (var ev in MIDIFile[0])
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

        private void BuildTempoList()
        {
            TempoEvents = new List<TempoEvent>();

            double currentBpm = 120.0;
            double realTimeMs = 0.0;
            int relDeltaTicks = 0;

            foreach (var ev in MIDIFile[0])
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

        public double AverageBPM()
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
        public List<MIDINote> Toms { get; set; }
        public List<SpecialMarker> Solos { get; set; }
        public List<SpecialMarker> Overdrive { get; set; }
        public List<SpecialMarker> Fills { get; set; }
        public List<SpecialMarker> HOPOon { get; set; }
        public List<SpecialMarker> HOPOoff { get; set; }

        public void Sort()
        {
            ChartedNotes.Sort((a, b) => a.NoteStart.CompareTo(b.NoteStart));
            Solos.Sort((a, b) => a.MarkerBegin.CompareTo(b.MarkerBegin));
            Overdrive.Sort((a, b) => a.MarkerBegin.CompareTo(b.MarkerBegin));
            Fills.Sort((a, b) => a.MarkerBegin.CompareTo(b.MarkerBegin));
            HOPOoff.Sort((a, b) => a.MarkerBegin.CompareTo(b.MarkerBegin));
            HOPOon.Sort((a, b) => a.MarkerBegin.CompareTo(b.MarkerBegin));
        }

        public void Initialize()
        {
            ChartedNotes = new List<MIDINote>();
            NoteRange = new List<int>();
            Solos = new List<SpecialMarker>();
            Toms = new List<MIDINote>();
            Fills = new List<SpecialMarker>();
            Overdrive = new List<SpecialMarker>();
            HOPOoff = new List<SpecialMarker>();
            HOPOon = new List<SpecialMarker>();
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
        public bool UsesPercussion { get; set; }

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
            AverageBPM = 0.0;
            UsesPercussion = false;
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
            Lyrics.Sort((a, b) => a.Start.CompareTo(b.Start));
        }
        public void Initialize()
        {
            Lyrics = new List<Lyric>();
        }
    }

    public class Lyric
    {
        public string Text { get; set; }
        public double Start { get; set; }
        public double Duration { get; set; }
        public double End { get; set; }
        public long Ticks { get; set; }
        public string DisplayText { get; set; }
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
        public bool hasOD { get; set; }
        public bool isHOPOon { get; set; }
        public bool IsForcedHOPOon { get; set; }
        public bool isHOPOoff { get; set; }

        public long Ticks { get; set; }

        public bool IsChord { get; set; }

        public bool SkTriggered { get; set; }
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