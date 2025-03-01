using Nautilus.Cysharp.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Un4seen.Bass.Misc;

namespace Nautilus.Sng
{
    public static class SngEncode
    {
        private static bool IsValidSongFolder(string songFolder)
        {
            string[] files = Directory.GetFiles(songFolder);
            bool hasChart = files.Any(f => f.EndsWith(".chart", StringComparison.OrdinalIgnoreCase));
            bool hasMidi = files.Any(f => f.EndsWith(".mid", StringComparison.OrdinalIgnoreCase));
            bool hasAudioFile = files.Any(f => f.EndsWith(".wav", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".opus", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase));
            bool hasSongIni = files.Any(f => f.EndsWith("song.ini", StringComparison.OrdinalIgnoreCase));

            return (hasChart || hasMidi) && hasAudioFile && (hasSongIni || hasChart);
        }

        public static void ReadFeedbackChartMetadata(SngFile sngFile, string chartPath)
        {
            // Already parsed metadata
            if (sngFile.metadataAvailable)
            {
                return;
            }
            using (Stream stream = File.Open(chartPath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string lineFull = reader.ReadLine();

                        if (lineFull == null)
                        {
                            break;
                        }
                        ReadOnlySpan<char> trimString = lineFull.AsSpan(); // ✅ Convert string to ReadOnlySpan<char>
                        trimString = trimString.Trim();

                        // Quit reading once we hit the end of the first .chart section
                        if (trimString.EndsWith("}".AsSpan())) // ✅ Fix: Convert string to ReadOnlySpan<char>
                            break;

                        var sepPos = trimString.IndexOf('='); // ✅ Fix: Use `char`, not `string`

                        // Skip any lines that don't have a key value pair
                        if (sepPos == -1)
                        {
                            continue;
                        }

                        // parse key
                        var keySpan = trimString.Slice(0, sepPos).Trim();

                        // Parse value
                        var valueSpan = trimString.Slice(sepPos + 1).Trim();
                        int quotePosStart = valueSpan.IndexOf('"'); // ✅ Fix: Use `char` instead of `string`

                        // Remove any quotes around values
                        // CH removes any quotes within the value, however I've
                        // opted to only remove the outer set of quotes if they
                        // exist, any quotes within are not touched.
                        if (quotePosStart != -1)
                        {
                            int quotePosEnd = valueSpan.LastIndexOf('"'); // ✅ Fix: Use `char` instead of `string`

                            // We only have a single quote in this string
                            // some older malformed charts do have this unfortunately
                            if (quotePosStart == quotePosEnd)
                            {
                                if (quotePosStart == 0)
                                {
                                    // remove the first character
                                    valueSpan = valueSpan.Slice(1);
                                }
                                else if (quotePosStart == (valueSpan.Length - 1))
                                {
                                    // remove the last character
                                    valueSpan = valueSpan.Slice(0, valueSpan.Length - 1);
                                }
                            }
                            // This means we should have atleast 2 different quotes
                            else
                            {
                                // only remove the first and last quote don't touch others
                                if (quotePosStart == 0)
                                {
                                    // remove the first character
                                    valueSpan = valueSpan.Slice(1);
                                    quotePosEnd--; // offset end position
                                }

                                if (quotePosEnd == (valueSpan.Length - 1))
                                {
                                    // remove the last character
                                    valueSpan = valueSpan.Slice(0, valueSpan.Length - 1);
                                }
                            }
                        }

                        valueSpan = valueSpan.Trim();

                        // skip any empty keys
                        if (valueSpan.IsEmpty || valueSpan.IsWhiteSpace())
                        {
                            continue;
                        }

                        switch (keySpan.ToString()) // ✅ Fix: Convert ReadOnlySpan<char> to string
                        {
                            case "Name":
                                sngFile.SetString("name", valueSpan.ToString());
                                break;
                            case "Artist":
                                sngFile.SetString("artist", valueSpan.ToString());
                                break;
                            case "Genre":
                                sngFile.SetString("genre", valueSpan.ToString());
                                break;
                            case "Charter":
                                sngFile.SetString("charter", valueSpan.ToString());
                                break;
                            case "Year":
                                // Some older charts have a comma in front of the year
                                if (valueSpan.StartsWith(", ".AsSpan())) // ✅ Fix: Convert string to ReadOnlySpan<char>
                                {
                                    valueSpan = valueSpan.Slice(2);
                                }
                                sngFile.SetString("year", valueSpan.ToString());
                                break;
                            case "Album":
                                sngFile.SetString("album", valueSpan.ToString());
                                break;
                            case "Offset":
                                sngFile.SetInt("delay", (int)Math.Ceiling(float.Parse(valueSpan.ToString()) * 1000));
                                break;
                            case "PreviewStart":
                                sngFile.SetInt("preview_start_time", (int)Math.Ceiling(float.Parse(valueSpan.ToString()) * 1000));
                                break;
                        }
                    }
                }
            }
            sngFile.metadataAvailable = true;
        }

        private static bool ParseMetadata(SngFile sngFile, string iniPath)
        {
            // Already parsed metadata
            if (sngFile.metadataAvailable)
            {
                return false;
            }

            IniFile iniFile = new IniFile();
            iniFile.Load(iniPath);
            if (!iniFile.TryGetSection("song", out var section))
                return false;

            KnownKeys.ValidateKeys(section);

            foreach (KeyValuePair<string, string> pair in section) // ✅ Fix: Explicitly use KeyValuePair
            {
                string key = pair.Key;   // ✅ Fix: Extract key explicitly
                string value = pair.Value; // ✅ Fix: Extract value explicitly

                sngFile.SetString(key, value);
            }

            sngFile.metadataAvailable = true;
            return true;
        }

        private static readonly string videoPattern = @"(?i).*\.(mp4|avi|webm|vp8|ogv|mpeg)$";
        private static Regex videoRegex = new Regex(videoPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly string imagePattern = @"(?i).*\.(png|jpg|jpeg)$";
        private static Regex imageRegex = new Regex(imagePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly string audioPattern = @"(?i).*\.(wav|opus|ogg|mp3)$";
        private static Regex audioRegex = new Regex(audioPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly string sngPattern = @"(?i).*\.sng$";
        private static Regex sngRegex = new Regex(sngPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly string[] supportedImageNames = { "background", "highway", "album" };

        private static readonly string[] supportedAudioNames =
        {
            "guitar",
            "bass",
            "rhythm",
            "vocals",
            "vocals_1",
            "vocals_2",
            "drums",
            "drums_1",
            "drums_2",
            "drums_3",
            "drums_4",
            "keys",
            "song",
            "crowd",
            "preview"
        };

        private static readonly string[] excludeFiles =
        {
            "desktop.ini",
            ".DS_Store",
            "ps.dat",
            "ch.dat"
        };

        private static readonly string[] excludeFolders =
        {
            "__MACOSX"
        };

        private static bool MatchesNames(string fileName, string[] names)
        {
            ReadOnlySpan<char> spanFile = fileName.AsSpan(); // ✅ Use .AsSpan() to convert string to ReadOnlySpan<char>
            spanFile = spanFile.Slice(0, spanFile.LastIndexOf('.'));

            Span<char> strSpan = stackalloc char[spanFile.Length];
            spanFile.ToLowerInvariant(strSpan); // ✅ Corrected case conversion

            return names.Contains(strSpan.ToString()); // ✅ Convert to string for comparison
        }

        private static bool PathsHasFileName(string fileName, string[] filePaths)
        {
            foreach (var path in filePaths)
            {
                if (path.EndsWith(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static async Task<(long startingSize, long encodedSize)> EncodeFolder(SngFile sngFile, string songFolder)
        {
            string MakeFileName(string fileName, bool knownFile)
            {
                return knownFile ? fileName.ToLowerInvariant() : fileName;
            }

            (string name, NativeByteArray data) fileData = ("", null);

            var fileList = Directory.GetFiles(songFolder);
            bool hasIniFile = PathsHasFileName("song.ini", fileList);

            long startingSize = 0;
            long endSize = 0;

            foreach (var file in fileList)
            {
                FileInfo fileInfo = new FileInfo(file);
                startingSize += fileInfo.Length;
                var fileName = Path.GetFileName(file);

                // Skip any SNG files found
                if (sngRegex.IsMatch(file))
                {
                    continue;
                }

                NativeByteArray fileDataArray = new NativeByteArray(fileInfo.Length, skipZeroClear: true);

                if (audioRegex.IsMatch(file))
                {
                    var knownAudio = MatchesNames(fileName, supportedAudioNames);
                    if (knownAudio)
                    {
                        await LargeFile.ReadAllBytesAsync(file, fileDataArray);
                        fileData = (MakeFileName(fileName, true), fileDataArray);
                    }
                }
                else if (string.Equals(fileName, "song.ini", StringComparison.OrdinalIgnoreCase))
                {
                    if (!ParseMetadata(sngFile, file))
                    {
                        return (-1, -1);
                    }
                    continue;
                }
                else if (string.Equals(fileName, "notes.mid", StringComparison.OrdinalIgnoreCase))
                {
                    await LargeFile.ReadAllBytesAsync(file, fileDataArray);
                    fileData = ("notes.mid", fileDataArray);
                }
                else if (string.Equals(fileName, "notes.chart", StringComparison.OrdinalIgnoreCase))
                {
                    if (!hasIniFile)
                    {
                        ReadFeedbackChartMetadata(sngFile, file);
                    }
                    await LargeFile.ReadAllBytesAsync(file, fileDataArray);
                    fileData = ("notes.chart", fileDataArray);
                }
                else if (imageRegex.IsMatch(file))
                {
                    var knownImage = MatchesNames(fileName, supportedImageNames);
                    if (knownImage)
                    {
                        await LargeFile.ReadAllBytesAsync(file, fileDataArray);
                        fileData = (MakeFileName(fileName, true), fileDataArray);
                    }
                }
                else if (videoRegex.IsMatch(file) && fileName.StartsWith("video", StringComparison.OrdinalIgnoreCase))
                {
                    await LargeFile.ReadAllBytesAsync(file, fileDataArray);
                    fileData = (MakeFileName(fileName, true), fileDataArray);
                }
                else // Include other unknown files
                {
                    await LargeFile.ReadAllBytesAsync(file, fileDataArray);
                    fileData = (fileName, fileDataArray);
                }

                if (fileData.data != null)
                {
                    endSize += fileData.data.Length;
                    sngFile.AddFile(fileData.name, fileData.data);
                }
            }

            return (startingSize, endSize);
        }


        private static readonly Random _random = new Random();
        public static async Task EncodeSng(string songFolder, string outFolder)
        {    
            var saveFile = $"{Path.GetFileName(songFolder)}.sng";
            var fullPath = Path.Combine(outFolder, saveFile);
                        
            using (SngFile sngFile = new SngFile())
            {
                _random.NextBytes(sngFile.XorMask);

                (long startingSize, long encodedSize) = await EncodeFolder(sngFile, songFolder);

                SngSerializer.SaveSngFile(sngFile, fullPath);
            }
        }
    }
}