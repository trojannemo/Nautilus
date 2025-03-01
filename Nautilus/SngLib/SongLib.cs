using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nautilus.Sng
{
    public class IniFile
    {
        private readonly Dictionary<string, Dictionary<string, string>> sections;

        public IniFile()
        {
            this.sections = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        }

        public void Load(string filePath)
        {
            int currentLineNumber = 0;
            try
            {
                string text = File.ReadAllText(filePath);

                var fileContent = text.AsSpan();
                var currentSection = string.Empty;

                while (!fileContent.IsEmpty)
                {
                    var endOfLineIndex = fileContent.IndexOf('\n');
                    currentLineNumber++;

                    if (endOfLineIndex == -1)
                    {
                        endOfLineIndex = fileContent.Length;
                    }

                    var line = fileContent.Slice(0, endOfLineIndex).Trim();

                    if (endOfLineIndex == fileContent.Length)
                    {
                        fileContent = new Span<char>();
                    }
                    else
                    {
                        fileContent = fileContent.Slice(endOfLineIndex + 1);
                    }

                    // Ignore empty or comment lines
                    if (line.IsEmpty || line[0] == '#' || line[0] == ';')
                    {
                        continue;
                    }

                    ReadOnlySpan<char> open = new char[] { '[' };
                    ReadOnlySpan<char> close = new char[] { ']' };
                    if (line.StartsWith(open) && line.EndsWith(close))
                    {
                        currentSection = line.Slice(1, line.Length - 2).ToString();
                    }
                    else
                    {
                        var separatorIndex = line.IndexOf('=');

                        // Skip lines without proper = character
                        if (separatorIndex == -1)
                        {
                            continue;
                        }

                        var key = line.Slice(0, separatorIndex).Trim();
                        var value = line.Slice(separatorIndex + 1).Trim();

                        if (!this.sections.TryGetValue(currentSection, out var values))
                        {
                            values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            this.sections[currentSection] = values;
                        }

                        values[key.ToString()] = value.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR on line: # {currentLineNumber}: {e}");
                Environment.Exit(1);
            }
        }

        public void Save(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var section in this.sections)
                {
                    writer.WriteLine($"[{section.Key}]");

                    foreach (var keyValue in section.Value)
                    {
                        // skip empty fields
                        if (string.IsNullOrEmpty(keyValue.Value))
                            continue;
                        writer.WriteLine($"{keyValue.Key} = {keyValue.Value}");
                    }

                    writer.WriteLine();
                }
            }
        }

        // handle making section names case-insensitive as some charts use cased section names
        public bool TryGetSection(string sectionName, out Dictionary<string, string> section)
        {
            if (!sections.TryGetValue(sectionName, out var value))
            {
                if (!sections.TryGetValue(sectionName.ToLowerInvariant(), out value))
                {
                    section = null;
                    return false;
                }
                else
                {
                    section = value;
                    return true;
                }
            }
            else
            {
                section = value;
                return true;
            }
        }

        public IEnumerable<string> GetSectionNames()
        {
            return sections.Keys;
        }

        public IEnumerable<string> GetKeyNames(string sectionName)
        {
            if (TryGetSection(sectionName, out var section))
            {
                return section.Keys;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        public bool IsSection(string sectionName)
        {
            if (!sections.ContainsKey(sectionName))
            {
                if (!sections.ContainsKey(sectionName.ToLowerInvariant()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public bool IsKey(string sectionName, string keyName)
        {
            return TryGetSection(sectionName, out var section) && section.ContainsKey(keyName);
        }

        public bool TryGetString(string section, string key, out string value)
        {
            bool rtn;
            string str = null;
            if (rtn = TryGetSection(section, out var sectionValue) && sectionValue.TryGetValue(key, out str))
            {
                value = str;
            }
            else
            {
                value = string.Empty;
            }
            return rtn;
        }

        public bool TryGetBool(string section, string key, out bool value)
        {
            value = default;
            var hasStr = TryGetString(section, key, out var strVal);

            if (!hasStr)
            {
                return false;
            }

            if (int.TryParse(strVal, out var intVal))
            {
                value = intVal != 0;
                return true;
            }

            if (strVal.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                value = true;
                return true;
            }

            if (strVal.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                value = false;
                return true;
            }

            return false;
        }

        public bool TryGetInt(string section, string key, out int value)
        {
            value = default;
            return TryGetString(section, key, out var strVal) && int.TryParse(strVal, out value);
        }

        public bool TryGetFloat(string section, string key, out float value)
        {
            value = default;
            return TryGetString(section, key, out var strVal) && float.TryParse(strVal, out value);
        }

        public string GetString(string section, string key, string defaultValue = "")
        {
            if (TryGetString(section, key, out string val))
            {
                return val;
            }
            else
            {
                return defaultValue;
            }
        }

        public int GetInt(string section, string key, int defaultValue = 0)
        {
            if (TryGetInt(section, key, out var val))
            {
                return val;
            }
            else
            {
                return defaultValue;
            }
        }

        public float GetFloat(string section, string key, float defaultValue = 0f)
        {
            if (TryGetFloat(section, key, out var val))
            {
                return val;
            }
            else
            {
                return defaultValue;
            }
        }

        public bool GetBool(string section, string key, bool defaultValue = false)
        {
            if (TryGetBool(section, key, out var val))
            {
                return val;
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetString(string section, string key, string value)
        {
            if (!this.sections.TryGetValue(section, out var values))
            {
                values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                this.sections[section] = values;
            }

            values[key] = value;
        }

        public void SetInt(string section, string key, int value)
        {
            this.SetString(section, key, value.ToString());
        }

        public void SetFloat(string section, string key, float value)
        {
            this.SetString(section, key, value.ToString());
        }

        public void SetBool(string section, string key, bool value)
        {
            this.SetString(section, key, value.ToString());
        }
    }
}