using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NAudio.Midi;

namespace Nautilus
{
    public class ChartToRockBandMidi
    {
        class ChartNote
        {
            public int Tick;
            public int Number;
            public int Length;
        }

        class ChartEvent
        {
            public int Tick;
            public string Type;
            public string Value;
        }

        class ChartData
        {
            public int Resolution = 192;
            public List<ChartEvent> SyncTrack = new List<ChartEvent>();
            public List<ChartEvent> Events = new List<ChartEvent>();
            public List<ChartNote> ExpertGuitar = new List<ChartNote>();
            public List<ChartNote> HardGuitar = new List<ChartNote>();
            public List<ChartNote> MediumGuitar = new List<ChartNote>();
            public List<ChartNote> EasyGuitar = new List<ChartNote>();
            public List<ChartNote> ExpertBass = new List<ChartNote>();
            public List<ChartNote> HardBass = new List<ChartNote>();
            public List<ChartNote> MediumBass = new List<ChartNote>();
            public List<ChartNote> EasyBass = new List<ChartNote>();
            public List<ChartNote> ExpertDrums = new List<ChartNote>();
            public List<ChartNote> HardDrums = new List<ChartNote>();
            public List<ChartNote> MediumDrums = new List<ChartNote>();
            public List<ChartNote> EasyDrums = new List<ChartNote>();
            public List<ChartEvent> Lyrics = new List<ChartEvent>();
        }

        public static MidiEventCollection ConvertChartToMidi(string chartPath)
        {
            var chart = ParseChart(chartPath);
            var midi = new MidiEventCollection(1, chart.Resolution);

            double bpm = 120.0; // fallback default
            foreach (var evt in chart.SyncTrack)
            {
                if (evt.Type == "B")
                {
                    int bpmTimes1000 = int.Parse(evt.Value);
                    bpm = bpmTimes1000 / 1000.0;
                    break;
                }
            }
            int offsetTicks = (int)(chart.Resolution * (bpm / 60.0) * 1.85);

            var track0 = new List<MidiEvent>();
            int lastTick = 0;

            foreach (var evt in chart.SyncTrack)
            {
                if (evt.Type == "B")
                {
                    int bpmTimes1000 = int.Parse(evt.Value);
                    bpm = bpmTimes1000 / 1000.0;
                    int microseconds = (int)(60000000 / bpm);
                    track0.Add(new NAudio.Midi.TempoEvent(microseconds, evt.Tick));
                }
                else if (evt.Type == "TS")
                {
                    try
                    {
                        var parts = evt.Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        int numerator = int.Parse(parts[0]);

                        // Denominator exponent is optional; default to 2 (for denominator of 4)
                        int denomExp = (parts.Length > 1) ? int.Parse(parts[1]) : 2;

                        // TimeSignatureEvent constructor: (absoluteTime, numerator, denominator exponent, clocksPerClick, thirtySecondNotesPerQuarterNote)
                        track0.Add(new TimeSignatureEvent(evt.Tick + offsetTicks, numerator, denomExp, 24, 8));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Time Signature Event value: " + evt.Value + "\n\n" + ex.Message + "\n\n" + ex.StackTrace);
                    }
                }
                if (evt.Tick > lastTick)
                    lastTick = evt.Tick;
            }

            lastTick += chart.Resolution;
            track0.Add(new MetaEvent(MetaEventType.EndTrack, 0, lastTick + offsetTicks));
            midi.AddTrack(track0);

            var guitarTrack = CreateInstrumentTrack(chart.ExpertGuitar, chart.HardGuitar, chart.MediumGuitar, chart.EasyGuitar, "PART GUITAR", chart.Resolution, offsetTicks);
            midi.AddTrack(guitarTrack);

            var bassTrack = CreateInstrumentTrack(chart.ExpertBass, chart.HardBass, chart.MediumBass, chart.EasyBass, "PART BASS", chart.Resolution, offsetTicks);
            midi.AddTrack(bassTrack);

            var drumsTrack = new List<MidiEvent>();
            drumsTrack.Add(new TextEvent("PART DRUMS", MetaEventType.SequenceTrackName, 0));

            int drumNoteLength = chart.Resolution / 8;

            for (int i = 0; i < chart.ExpertDrums.Count; i++)
            {
                var note = chart.ExpertDrums[i];
                int tick = note.Tick + offsetTicks;
                int pitch = 96;
                bool isTom = false;

                // Check if this note is followed by a cymbal marker at the same tick
                bool followedByCymbal = false;
                if (i + 1 < chart.ExpertDrums.Count)
                {
                    var next = chart.ExpertDrums[i + 1];
                    if ((next.Number >= 66 && next.Number <= 68) && next.Tick == note.Tick)
                    {
                        followedByCymbal = true;
                    }
                }

                // Handle cymbal modifiers directly
                if (note.Number >= 66 && note.Number <= 68)
                {
                    pitch = 98 + (note.Number - 66); // 98 for yellow, 99 for blue, 100 for green
                    drumsTrack.Add(new NoteOnEvent(tick, 10, pitch, 100, drumNoteLength));
                    drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch, 0));
                    continue;
                }

                // Skip normal note if followed by cymbal modifier
                if (followedByCymbal)
                    continue;

                // Map normal notes 0–4
                switch (note.Number)
                {
                    case 0:
                        pitch = 96; break;
                    case 1:
                        pitch = 97; break;
                    case 2:
                        pitch = 98; isTom = true; break;
                    case 3:
                        pitch = 99; isTom = true; break;
                    case 4:
                        pitch = 100; isTom = true; break;
                    default:
                        continue; // Skip unknown notes
                }

                // Add the regular note
                drumsTrack.Add(new NoteOnEvent(tick, 10, pitch, 100, drumNoteLength));
                drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch, 0));

                // Add tom marker if needed
                if (isTom)
                {
                    drumsTrack.Add(new NoteOnEvent(tick, 10, pitch + 12, 100, drumNoteLength));
                    drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch + 12, 0));
                }
            }

            for (int i = 0; i < chart.HardDrums.Count; i++)
            {
                var note = chart.HardDrums[i];
                int tick = note.Tick + offsetTicks;
                int basepitch = 84;

                // Check if this note is followed by a cymbal marker at the same tick
                bool followedByCymbal = false;
                if (i + 1 < chart.HardDrums.Count)
                {
                    var next = chart.HardDrums[i + 1];
                    if ((next.Number >= 66 && next.Number <= 68) && next.Tick == note.Tick)
                    {
                        followedByCymbal = true;
                    }
                }

                int pitch;
                // Handle cymbal modifiers directly
                if (note.Number >= 66 && note.Number <= 68)
                {
                    pitch = basepitch + 2 + (note.Number - 66);
                    drumsTrack.Add(new NoteOnEvent(tick, 10, pitch, 100, drumNoteLength));
                    drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch, 0));
                    continue;
                }

                // Skip normal note if followed by cymbal modifier
                if (followedByCymbal)
                    continue;

                // Map normal notes 0–4
                switch (note.Number)
                {
                    case 0:                        
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        pitch = basepitch + note.Number;
                        break;
                    default:
                        continue; // Skip unknown notes
                }

                // Add the regular note
                drumsTrack.Add(new NoteOnEvent(tick, 10, pitch, 100, drumNoteLength));
                drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch, 0));
            }

            for (int i = 0; i < chart.MediumDrums.Count; i++)
            {
                var note = chart.MediumDrums[i];
                int tick = note.Tick + offsetTicks;
                int basepitch = 72;

                // Check if this note is followed by a cymbal marker at the same tick
                bool followedByCymbal = false;
                if (i + 1 < chart.MediumDrums.Count)
                {
                    var next = chart.MediumDrums[i + 1];
                    if ((next.Number >= 66 && next.Number <= 68) && next.Tick == note.Tick)
                    {
                        followedByCymbal = true;
                    }
                }

                int pitch;
                // Handle cymbal modifiers directly
                if (note.Number >= 66 && note.Number <= 68)
                {
                    pitch = basepitch + 2 + (note.Number - 66);
                    drumsTrack.Add(new NoteOnEvent(tick, 10, pitch, 100, drumNoteLength));
                    drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch, 0));
                    continue;
                }

                // Skip normal note if followed by cymbal modifier
                if (followedByCymbal)
                    continue;

                // Map normal notes 0–4
                switch (note.Number)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        pitch = basepitch + note.Number;
                        break;
                    default:
                        continue; // Skip unknown notes
                }

                // Add the regular note
                drumsTrack.Add(new NoteOnEvent(tick, 10, pitch, 100, drumNoteLength));
                drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch, 0));
            }

            for (int i = 0; i < chart.EasyDrums.Count; i++)
            {
                var note = chart.EasyDrums[i];
                int tick = note.Tick + offsetTicks;
                int basepitch = 60;

                // Check if this note is followed by a cymbal marker at the same tick
                bool followedByCymbal = false;
                if (i + 1 < chart.EasyDrums.Count)
                {
                    var next = chart.EasyDrums[i + 1];
                    if ((next.Number >= 66 && next.Number <= 68) && next.Tick == note.Tick)
                    {
                        followedByCymbal = true;
                    }
                }

                int pitch;
                // Handle cymbal modifiers directly
                if (note.Number >= 66 && note.Number <= 68)
                {
                    pitch = basepitch + 2 + (note.Number - 66);
                    drumsTrack.Add(new NoteOnEvent(tick, 10, pitch, 100, drumNoteLength));
                    drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch, 0));
                    continue;
                }

                // Skip normal note if followed by cymbal modifier
                if (followedByCymbal)
                    continue;

                // Map normal notes 0–4
                switch (note.Number)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        pitch = basepitch + note.Number;
                        break;
                    default:
                        continue; // Skip unknown notes
                }

                // Add the regular note
                drumsTrack.Add(new NoteOnEvent(tick, 10, pitch, 100, drumNoteLength));
                drumsTrack.Add(new NoteEvent(tick + drumNoteLength, 10, MidiCommandCode.NoteOff, pitch, 0));
            }

            int lastDrumTick = drumsTrack.Count > 0 ? (int)drumsTrack[drumsTrack.Count - 1].AbsoluteTime + 1 : 0;
            drumsTrack.Add(new MetaEvent(MetaEventType.EndTrack, 0, lastDrumTick));
            midi.AddTrack(drumsTrack);

            var vocalTrack = new List<MidiEvent>();
            vocalTrack.Add(new TextEvent("PART VOCALS", MetaEventType.SequenceTrackName, 0));

            int? phraseStart = null;
            foreach (var evt in chart.Events)
            {
                if (evt.Type == "E")
                {
                    string value = evt.Value.Trim('"');
                    if (value == "phrase_start")
                    {
                        phraseStart = evt.Tick + offsetTicks;
                    }
                    else if (value == "phrase_end" && phraseStart.HasValue)
                    {
                        int start = phraseStart.Value;
                        int end = evt.Tick + offsetTicks;
                        vocalTrack.Add(new NoteOnEvent(start, 1, 105, 100, end - start));
                        vocalTrack.Add(new NoteEvent(end, 1, MidiCommandCode.NoteOff, 105, 0));
                        phraseStart = null;
                    }
                }
            }

            for (int i = 0; i < chart.Events.Count; i++)
            {
                var evt = chart.Events[i];
                if (evt.Type == "E" && evt.Value.StartsWith("lyric "))
                {
                    string text = evt.Value.Substring(6).Trim('"');
                    string rbLyric = text + "#";
                    int tick = evt.Tick + offsetTicks;
                    int nextTick = (i + 1 < chart.Events.Count) ? chart.Events[i + 1].Tick + offsetTicks : tick + chart.Resolution / 4;
                    int duration = Math.Max(chart.Resolution / 8, nextTick - tick - chart.Resolution / 8);
                    vocalTrack.Add(new TextEvent(rbLyric, MetaEventType.Lyric, tick));
                    vocalTrack.Add(new NoteOnEvent(tick, 1, 60, 100, duration));
                    vocalTrack.Add(new NoteEvent(tick + duration, 1, MidiCommandCode.NoteOff, 60, 0));
                }
            }
            int lastVocalTick = vocalTrack.Count > 0 ? (int)vocalTrack[vocalTrack.Count - 1].AbsoluteTime + 1 : 0;
            vocalTrack.Add(new MetaEvent(MetaEventType.EndTrack, 0, lastVocalTick));
            midi.AddTrack(vocalTrack);

            return midi;
        }               

        private static List<MidiEvent> CreateInstrumentTrack(List<ChartNote> notesExpert, List<ChartNote> notesHard, List<ChartNote> notesMedium, List<ChartNote> notesEasy, string trackName, int resolution, int offsetTicks)
        {            
            var track = new List<MidiEvent>();
            track.Add(new TextEvent(trackName, MetaEventType.SequenceTrackName, 0));
            int basePitchExpert = 96;
            int basePitchHard = 84;
            int basePitchMedium = 72;
            int basePitchEasy = 60;

            foreach (var note in notesExpert)
            {
                int pitch = basePitchExpert + note.Number;
                int tick = note.Tick + offsetTicks;
                int length = Math.Max(resolution / 8, note.Length);
                track.Add(new NoteOnEvent(tick, 1, pitch, 100, length));
                track.Add(new NoteEvent(tick + length, 1, MidiCommandCode.NoteOff, pitch, 0));
            }
            foreach (var note in notesHard)
            {
                int pitch = basePitchHard + note.Number;
                int tick = note.Tick + offsetTicks;
                int length = Math.Max(resolution / 8, note.Length);
                track.Add(new NoteOnEvent(tick, 1, pitch, 100, length));
                track.Add(new NoteEvent(tick + length, 1, MidiCommandCode.NoteOff, pitch, 0));
            }
            foreach (var note in notesMedium)
            {
                int pitch = basePitchMedium + note.Number;
                int tick = note.Tick + offsetTicks;
                int length = Math.Max(resolution / 8, note.Length);
                track.Add(new NoteOnEvent(tick, 1, pitch, 100, length));
                track.Add(new NoteEvent(tick + length, 1, MidiCommandCode.NoteOff, pitch, 0));
            }
            foreach (var note in notesEasy)
            {
                int pitch = basePitchEasy + note.Number;
                int tick = note.Tick + offsetTicks;
                int length = Math.Max(resolution / 8, note.Length);
                track.Add(new NoteOnEvent(tick, 1, pitch, 100, length));
                track.Add(new NoteEvent(tick + length, 1, MidiCommandCode.NoteOff, pitch, 0));
            }
            int lastTick = track.Count > 0 ? (int)track[track.Count - 1].AbsoluteTime + 1 : 0;
            track.Add(new MetaEvent(MetaEventType.EndTrack, 0, lastTick));
            return track;
        }

        private static ChartData ParseChart(string path)
        {
            var lines = File.ReadAllLines(path);
            var data = new ChartData();
            string currentSection = null;
            var regex = new Regex("(\\d+) = ([A-Z]+) ?(?:\"?([^\"]*)\"?)?");

            foreach (var rawLine in lines)
            {
                try
                {
                    var line = rawLine.Trim();
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        currentSection = line.Trim('[', ']');
                        continue;
                    }

                    if (line.Contains("Resolution"))
                    {
                        var parts = line.Split('=');
                        data.Resolution = int.Parse(parts[1].Trim());
                        continue;
                    }

                    if (line == "{" || line == "}" || currentSection == null)
                        continue;

                    if (regex.IsMatch(line))
                    {
                        var match = regex.Match(line);
                        int tick = int.Parse(match.Groups[1].Value);
                        string type = match.Groups[2].Value;
                        string value = match.Groups[3].Value;

                        switch (currentSection)
                        {
                            case "SyncTrack":
                                data.SyncTrack.Add(new ChartEvent { Tick = tick, Type = type, Value = value });
                                break;
                            case "Events":
                                data.Events.Add(new ChartEvent { Tick = tick, Type = type, Value = value });
                                break;
                            case "Lyrics":
                                data.Lyrics.Add(new ChartEvent { Tick = tick, Type = type, Value = value });
                                break;
                            case "ExpertSingle":
                            case "ExpertDoubleGuitar":
                                if (type == "N") AddNote(data.ExpertGuitar, tick, value);
                                break;
                            case "HardSingle":
                            case "HardDoubleGuitar":
                                if (type == "N") AddNote(data.HardGuitar, tick, value);
                                break;
                            case "MediumSingle":
                            case "MediumDoubleGuitar":
                                if (type == "N") AddNote(data.MediumGuitar, tick, value);
                                break;
                            case "EasySingle":
                            case "EasyDoubleGuitar":
                                if (type == "N") AddNote(data.EasyGuitar, tick, value);
                                break;
                            case "ExpertDoubleBass":
                            case "ExpertDoubleRhythm":
                                if (type == "N") AddNote(data.ExpertBass, tick, value);
                                break;
                            case "HardDoubleBass":
                            case "HardDoubleRhythm":
                                if (type == "N") AddNote(data.HardBass, tick, value);
                                break;
                            case "MediumDoubleBass":
                            case "MediumDoubleRhythm":
                                if (type == "N") AddNote(data.MediumBass, tick, value);
                                break;
                            case "EasyDoubleBass":
                            case "EasyDoubleRhythm":
                                if (type == "N") AddNote(data.EasyBass, tick, value);
                                break;
                            case "ExpertDrums":
                                if (type == "N") AddNote(data.ExpertDrums, tick, value);
                                break;
                            case "HardDrums":
                                if (type == "N") AddNote(data.HardDrums, tick, value);
                                break;
                            case "MediumDrums":
                                if (type == "N") AddNote(data.MediumDrums, tick, value);
                                break;
                            case "EasyDrums":
                                if (type == "N") AddNote(data.EasyDrums, tick, value);
                                break;
                        }
                    }
                }
                catch
                {
                    continue; //don't choke on a problem line
                }
            }
            return data;
        }

        private static void AddNote(List<ChartNote> list, int tick, string value)
        {
            var parts = value.Split(' ');
            int number = int.Parse(parts[0]);
            int length = int.Parse(parts[1]);
            list.Add(new ChartNote { Tick = tick, Number = number, Length = length });
        }
    }
}