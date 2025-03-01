using Nautilus.Cysharp.Collections;
using Nautilus.Sng;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nautilus
{
    public static class SngDecode
    {
        public static async Task DecodeSng(Stream sngStream, string folderOutPath)
        {
            // ✅ No need to wrap sngStream in MemoryStream—it’s already a stream!
            SngFile sngFile = SngSerializer.LoadSngFile(sngStream);

            if (!Directory.Exists(folderOutPath))
                {
                    Directory.CreateDirectory(folderOutPath);
                }

                // create ini file from metadata
                SerializeMetadata(sngFile, Path.Combine(folderOutPath, "song.ini"));

                // iterate through files and save them to disk
                // Iterate through files and save them to disk
                foreach (KeyValuePair<string, NativeByteArray> pair in sngFile.Files) // ✅ Fix: Use KeyValuePair explicitly
                {
                    string name = pair.Key;   // ✅ Fix: Extract key explicitly
                    NativeByteArray data = pair.Value; // ✅ Fix: Extract value explicitly

                    var filePath = Path.Combine(folderOutPath, Path.Combine(name.Split('/'))); // ✅ Fix: Ensure cross-platform path handling
                    var folder = Path.GetDirectoryName(filePath);

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    await data.WriteToFileAsync(filePath);
                }
            }
        
        private static void SerializeMetadata(SngFile sngFile, string savePath)
        {
            KnownKeys.ValidateKeys(sngFile.Metadata);

            IniFile iniFile = new IniFile();
            foreach (KeyValuePair<string, string> pair in sngFile.Metadata) // ✅ Fix: Explicitly use KeyValuePair
            {
                string key = pair.Key;   // ✅ Fix: Extract key explicitly
                string value = pair.Value; // ✅ Fix: Extract value explicitly

                iniFile.SetString("song", key, value);
            }

            iniFile.Save(savePath);
        }
    }
}
