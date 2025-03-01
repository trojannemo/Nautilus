using System.Collections.Generic;
using Nautilus.Cysharp.Collections;
using System;

namespace Nautilus.Sng
{
    public class SngFile : IDisposable
    {
        public const uint CurrentVersion = 1;
        public uint Version = CurrentVersion;
        public byte[] XorMask = new byte[16];

        public bool metadataAvailable;

        public Dictionary<string, string> Metadata = new Dictionary<string, string>();
        public Dictionary<string, NativeByteArray> Files = new Dictionary<string, NativeByteArray>();

        public void AddFile(string fileName, NativeByteArray data)
        {
            if (!Files.ContainsKey(fileName))
            {
                Files[fileName] = data;
            }
        }

        public bool TryGetString(string key, out string value)
        {
            if (Metadata.TryGetValue(key, out var strVal))
            {
                value = strVal;
                return true;
            }
            else
            {
                value = string.Empty;
                return false;
            }
        }

        public bool TryGetInt(string key, out int value)
        {
            if (Metadata.TryGetValue(key, out var stringValue) && int.TryParse(stringValue, out int intValue))
            {
                value = intValue;
                return true;
            }

            value = 0;
            return false;
        }

        public bool TryGetFloat(string key, out float value)
        {
            if (Metadata.TryGetValue(key, out var stringValue) && float.TryParse(stringValue, out float floatValue))
            {
                value = floatValue;
                return true;
            }

            value = 0;
            return false;
        }

        public bool TryGetBool(string key, out bool value)
        {
            if (Metadata.TryGetValue(key, out var stringValue) && bool.TryParse(stringValue, out bool boolValue))
            {
                value = boolValue;
                return true;
            }

            value = false;
            return false;
        }

        public string GetString(string key, string defaultValue)
        {
            if (TryGetString(key, out var value) && value != null)
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public int GetInt(string key, int defaultValue)
        {
            if (TryGetInt(key, out var value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public float GetFloat(string key, float defaultValue)
        {
            if (TryGetFloat(key, out var value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public bool GetBool(string key, bool defaultValue)
        {
            if (TryGetBool(key, out var value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetString(string key, string value)
        {
            if (!Metadata.ContainsKey(key))
            {
                Metadata[key] = value;
            }
        }

        public void SetInt(string key, int value)
        {
            var newVal = value.ToString();
            if (!Metadata.ContainsKey(key))
            {
                Metadata[key] = newVal;
            }
        }

        public void SetFloat(string key, float value)
        {
            var newVal = value.ToString();
            if (!Metadata.ContainsKey(key))
            {
                Metadata[key] = newVal;
            }
        }

        public void SetBool(string key, bool value)
        {
            var newVal = value.ToString();
            if (!Metadata.ContainsKey(key))
            {
                Metadata[key] = newVal;
            }
        }

        public Dictionary<string, string> GetRawMetadata()
        {
            // Make a copy of the internal dictionary
            return new Dictionary<string, string>(Metadata);
        }

        public void Dispose()
        {
            foreach (var file in Files)
            {
                file.Value?.Dispose();
            }
        }
    }
}