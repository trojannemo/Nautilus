using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Nautilus
{
    public class Tools
    {
        /// <summary>
        /// Writes the small Byte Array into the big one at the given offset
        /// </summary>
        /// <param name="big"></param>
        /// <param name="small"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static byte[] InsertByteArray(byte[] big, byte[] small, int offset)
        {
            for (var i = 0; i < small.Length; i++)
                big[offset + i] = small[i];
            return big;
        }

        /// <summary>
        /// Returns the current UTC Unix Timestamp as a Byte Array
        /// </summary>
        /// <returns></returns>
        public static byte[] GetTimestamp()
        {
            var dtNow = DateTime.UtcNow;
            var tsTimestamp = (dtNow - new DateTime(1970, 1, 1, 0, 0, 0));

            var timestamp = (int)tsTimestamp.TotalSeconds;
            var enc = new ASCIIEncoding();
            var timestampBytes = enc.GetBytes("CMiiUT" + timestamp);
            return timestampBytes;
        }

        /// <summary>
        /// Creates a new Byte Array out of the given one
        /// from the given offset with the specified length
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetPartOfByteArray(byte[] array, int offset, int length)
        {
            var ret = new byte[length];
            for (var i = 0; i < length; i++)
                ret[i] = array[offset + i];
            return ret;
        }

        /// <summary>
        /// Converts UInt16 Array into Byte Array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static byte[] UIntArrayToByteArray(UInt16[] array)
        {
            var results = new List<byte>();
            foreach (var converted in array.Select(BitConverter.GetBytes))
            {
                results.AddRange(converted);
            }
            return results.ToArray();
        }

        /// <summary>
        /// Converts UInt16 Array into Byte Array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static byte[] UIntArrayToByteArray(uint[] array)
        {
            var results = new List<byte>();
            foreach (var converted in array.Select(BitConverter.GetBytes))
            {
                results.AddRange(converted);
            }
            return results.ToArray();
        }

        /// <summary>
        /// Converts Byte Array into UInt16 Array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static UInt32[] ByteArrayToUInt32Array(byte[] array)
        {
            var converted = new UInt32[array.Length / 2];
            var j = 0;
            for (var i = 0; i < array.Length; i += 4)
            {
                converted[j] = BitConverter.ToUInt32(array, i);
                j++;
            }
            return converted;
        }

        /// <summary>
        /// Converts Byte Array into UInt16 Array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static UInt16[] ByteArrayToUInt16Array(byte[] array)
        {
            var converted = new UInt16[array.Length / 2];
            var j = 0;
            for (var i = 0; i < array.Length; i += 2)
            {
                converted[j] = BitConverter.ToUInt16(array, i);
                j++;
            }
            return converted;
        }

        /// <summary>
        /// Returns the file length as a Byte Array
        /// </summary>
        /// <param name="filelength"></param>
        /// <returns></returns>
        public static byte[] FileLengthToByteArray(int filelength)
        {
            var length = BitConverter.GetBytes(filelength);
            Array.Reverse(length);
            return length;
        }

        /// <summary>
        /// Adds a padding to the next 64 bytes, if necessary
        /// </summary>
        /// <returns></returns>
        public static int AddPadding(int value)
        {
            return AddPadding(value, 64);
        }

        /// <summary>
        /// Adds a padding to the given value, if necessary
        /// </summary>
        /// <param name="value"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static int AddPadding(int value, int padding)
        {
            if (value % padding != 0)
            {
                value = value + (padding - (value % padding));
            }

            return value;
        }

        /// <summary>
        /// Converts a Hex-String to Int
        /// </summary>
        /// <param name="hexstring"></param>
        /// <returns></returns>
        public static int HexStringToInt(string hexstring)
        {
            try { return int.Parse(hexstring, NumberStyles.HexNumber); }
            catch { throw new Exception("An Error occured, maybe the Wad file is corrupt!"); }
        }

        /// <summary>
        /// Converts a Hex-String to Long
        /// </summary>
        /// <param name="hexstring"></param>
        /// <returns></returns>
        public static long HexStringToLong(string hexstring)
        {
            try { return long.Parse(hexstring, NumberStyles.HexNumber); }
            catch { throw new Exception("An Error occured, maybe the Wad file is corrupt!"); }
        }

        /// <summary>
        /// Writes a Byte Array to a file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="destination"></param>
        public static void SaveFileFromByteArray(byte[] file, string destination)
        {
            using (var fs = new FileStream(destination, FileMode.Create))
            {
                fs.Write(file, 0, file.Length);
                fs.Dispose();
            }
        }

        /// <summary>
        /// Loads a file into a Byte Array
        /// </summary>
        /// <param name="sourcefile"></param>
        /// <returns></returns>
        public static byte[] LoadFileToByteArray(string sourcefile)
        {
            if (!File.Exists(sourcefile)) throw new FileNotFoundException("File couldn't be found:\r\n" + sourcefile);
            using (var fs = new FileStream(sourcefile, FileMode.Open))
            {
                var filearray = new byte[fs.Length];
                fs.Read(filearray, 0, filearray.Length);
                fs.Dispose();
                return filearray;
            }
        }

        /// <summary>
        /// Loads a file into a Byte Array
        /// </summary>
        /// <param name="sourcefile"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] LoadFileToByteArray(string sourcefile, int offset, int length)
        {
            if (!File.Exists(sourcefile)) throw new FileNotFoundException("File couldn't be found:\r\n" + sourcefile);
            using (var fs = new FileStream(sourcefile, FileMode.Open))
            {
                if (fs.Length < length) length = (int)fs.Length;
                var filearray = new byte[length];
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Read(filearray, 0, length);
                fs.Dispose();
                return filearray;
            }
        }

        /// <summary>
        /// Checks the SHA1 of the Common-Key
        /// </summary>
        /// <param name="pathtocommonkey"></param>
        /// <returns></returns>
        public static bool CheckCommonKey(string pathtocommonkey)
        {
            var sum = new byte[] { 0xEB, 0xEA, 0xE6, 0xD2, 0x76, 0x2D, 0x4D, 0x3E, 0xA1, 0x60, 0xA6, 0xD8, 0x32, 0x7F, 0xAC, 0x9A, 0x25, 0xF8, 0x06, 0x2B };
            
            var fi = new FileInfo(pathtocommonkey);
            if (fi.Length != 16) return false;
            var ckey = LoadFileToByteArray(pathtocommonkey);

            var sha1 = new SHA1Managed();
            var newsum = sha1.ComputeHash(ckey);

            return CompareByteArrays(sum, newsum);
        }

        /// <summary>
        /// Creates the Common Key
        /// </summary>
        /// <param name="fat">Must be "45e"</param>
        /// <param name="destinationpath"></param>
        public static void CreateCommonKey(string fat, string destinationpath)
        {
            //What an effort, lol
            var encryptedwater = new byte[] { 0x4d, 0x89, 0x21, 0x34, 0x62, 0x81, 0xe4, 0x02, 0x37, 0x36, 0xc4, 0xb4, 0xde, 0x40, 0x32, 0xab };
            var key = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, byte.Parse(fat.Remove(2), NumberStyles.HexNumber), byte.Parse(fat.Remove(0, 2) + "0", NumberStyles.HexNumber) };
            var decryptedwater = new byte[10];

            var decryptkey = new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.None,
                    KeySize = 128,
                    BlockSize = 128,
                    Key = key
                };
            Array.Reverse(key);
            decryptkey.IV = key;

            var cryptor = decryptkey.CreateDecryptor();

            using (var memory = new MemoryStream(encryptedwater))
            {
                using (var crypto = new CryptoStream(memory, cryptor, CryptoStreamMode.Read))
                {
                    crypto.Read(decryptedwater, 0, 10);
                    crypto.Dispose();
                }
                memory.Dispose();
            }

            var water = BitConverter.ToString(decryptedwater).Replace("-", "").ToLowerInvariant() + " ";

            water = water.Insert(0, fat[2].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(2, fat[2].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(7, fat[2].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(11, fat[2].ToString(CultureInfo.InvariantCulture));

            water = water.Insert(7, fat[1].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(10, fat[1].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(18, fat[1].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(19, fat[1].ToString(CultureInfo.InvariantCulture));

            water = water.Insert(3, fat[0].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(15, fat[0].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(16, fat[0].ToString(CultureInfo.InvariantCulture));
            water = water.Insert(22, fat[0].ToString(CultureInfo.InvariantCulture));

            var cheese = new byte[16];
            var count = -1;

            for (var i = 0; i < 32; i += 2)
                cheese[++count] = byte.Parse(water.Remove(0, i).Remove(2), NumberStyles.HexNumber);

            if (destinationpath[destinationpath.Length - 1] != '\\') destinationpath = destinationpath + "\\";
            using (var keystream = new FileStream(destinationpath + "\\common-key.bin", FileMode.Create))
            {
                keystream.Write(cheese, 0, cheese.Length);
                keystream.Dispose();
            }
        }

        /// <summary>
        /// Counts the appearance of a specific character in a string
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="theChar"></param>
        /// <returns></returns>
        public static int CountCharsInString(string theString, char theChar)
        {
            return theString.Count(thisChar => thisChar == theChar);
        }

        /// <summary>
        /// Compares two Byte Arrays and returns true, if they match
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool CompareByteArrays(byte[] first, byte[] second)
        {
            if (first.Length != second.Length) return false;
            return !first.Where((t, i) => t != second[i]).Any();
        }

        /// <summary>
        /// Converts a Hex String to a Byte Array
        /// </summary>
        /// <param name="hexstring"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hexstring)
        {
            var ba = new byte[hexstring.Length / 2];

            for (var i = 0; i < hexstring.Length / 2; i++)
            {
                ba[i] = byte.Parse(hexstring.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            return ba;
        }
        
        /// <summary>
        /// Checks, if the given string does exist in the string Array
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="theStringArray"></param>
        /// <returns></returns>
        public static bool StringExistsInStringArray(string theString, string[] theStringArray)
        {
            return Array.Exists(theStringArray, thisString => thisString == theString);
        }

        /// <summary>
        /// Copies an entire Directoy
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyDirectory(string source, string destination)
        {
            var subdirs = Directory.GetDirectories(source);
            var files = Directory.GetFiles(source);

            foreach (var thisFile in files)
            {
                if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
                if (File.Exists(destination + "\\" + Path.GetFileName(thisFile))) File.Delete(destination + "\\" + Path.GetFileName(thisFile));
                File.Copy(thisFile, destination + "\\" + Path.GetFileName(thisFile));
            }

            foreach (var thisDir in subdirs)
            {
                CopyDirectory(thisDir, destination + "\\" + thisDir.Remove(0, thisDir.LastIndexOf('\\') + 1));
            }
        }
    }

    public class TPL
    {
        /// <summary>
        /// Fixes rough edges (artifacts), if necessary
        /// </summary>
        /// <param name="tplFile"></param>
        public static void FixFilter(string tplFile)
        {
            using (var fs = new FileStream(tplFile, FileMode.Open))
            {
                fs.Seek(41, SeekOrigin.Begin);
                if (fs.ReadByte() == 0x01)
                {
                    fs.Seek(-1, SeekOrigin.Current);
                    fs.Write(new byte[] { 0x00, 0x00, 0x01 }, 0, 3);
                }

                fs.Seek(45, SeekOrigin.Begin);
                if (fs.ReadByte() == 0x01)
                {
                    fs.Seek(-1, SeekOrigin.Current);
                    fs.Write(new byte[] { 0x00, 0x00, 0x01 }, 0, 3);
                }
                fs.Dispose();
            }
        }

        /// <summary>
        /// Converts a Tpl to a Bitmap
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static Bitmap ConvertFromTPL(string tpl)
        {
            var tplarray = Tools.LoadFileToByteArray(tpl);
            return ConvertFromTPL(tplarray);
        }

        /// <summary>
        /// Converts a Tpl to a Bitmap
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static Bitmap ConvertFromTPL(byte[] tpl)
        {
            if (GetTextureCount(tpl) > 1) throw new Exception("Tpl files containing more than one texture are not supported!");

            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var format = GetTextureFormat(tpl);
            if (format == -1) throw new Exception("The texture has an unsupported format!");

            switch (format)
            {
                case 0:
                    var temp0 = FromI4(tpl);
                    return ConvertPixelToBitmap(temp0, width, height);
                case 1:
                    var temp1 = FromI8(tpl);
                    return ConvertPixelToBitmap(temp1, width, height);
                case 2:
                    var temp2 = FromIA4(tpl);
                    return ConvertPixelToBitmap(temp2, width, height);
                case 3:
                    var temp3 = FromIA8(tpl);
                    return ConvertPixelToBitmap(temp3, width, height);
                case 4:
                    var temp4 = FromRGB565(tpl);
                    return ConvertPixelToBitmap(temp4, width, height);
                case 5:
                    var temp5 = FromRGB5A3(tpl);
                    return ConvertPixelToBitmap(temp5, width, height);
                case 6:
                    var temp6 = FromRGBA8(tpl);
                    return ConvertPixelToBitmap(temp6, width, height);
                case 8:
                    var temp8 = FromCI4(tpl);
                    return ConvertPixelToBitmap(temp8, width, height);
                case 9:
                    var temp9 = FromCI8(tpl);
                    return ConvertPixelToBitmap(temp9, width, height);
                case 10:
                    var temp10 = FromCI14X2(tpl);
                    return ConvertPixelToBitmap(temp10, width, height);
                case 14:
                    var temp14 = FromCMP(tpl);
                    return ConvertPixelToBitmap(temp14, width, height);
                default:
                    throw new Exception("The Texture has an unsupported format!");
            }
        }

        /// <summary>
        /// Converts the Pixel Data into a Png Image
        /// </summary>
        /// <param name="data">Byte array with pixel data</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static Bitmap ConvertPixelToBitmap(byte[] data, int width, int height)
        {
            if (width == 0) width = 1;
            if (height == 0) height = 1;

            var bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 System.Drawing.Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat);

            System.Runtime.InteropServices.Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Gets the offset to the Texture Header
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTextureHeaderOffset(byte[] tpl)
        {
            var tmp = new[] { tpl[15], tpl[14], tpl[13], tpl[12] };
            return BitConverter.ToInt32(tmp, 0);
        }

        /// <summary>
        /// Gets the offset to the Texture Palette Header
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTexturePaletteHeaderOffset(byte[] tpl)
        {
            var tmp = new[] { tpl[19], tpl[18], tpl[17], tpl[16] };
            return BitConverter.ToInt32(tmp, 0);
        }

        /// <summary>
        /// Gets the offset to the Texture Palette
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTexturePaletteOffset(byte[] tpl)
        {
            var paletteheaderoffset = GetTexturePaletteHeaderOffset(tpl);

            var tmp = new[] { tpl[paletteheaderoffset + 11],
                tpl[paletteheaderoffset + 10], tpl[paletteheaderoffset + 9], tpl[paletteheaderoffset + 8] };
            return BitConverter.ToInt32(tmp, 0);
        }

        /// <summary>
        /// Gets the Texture Palette Format
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTexturePaletteFormat(byte[] tpl)
        {
            var paletteheaderoffset = GetTexturePaletteHeaderOffset(tpl);

            var tmp = new[] { tpl[paletteheaderoffset + 7],
                tpl[paletteheaderoffset + 6], tpl[paletteheaderoffset + 5], tpl[paletteheaderoffset + 4] };
            return BitConverter.ToInt32(tmp, 0);
        }

        /// <summary>
        /// Gets the item count of the Texture Palette
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTexturePaletteItemCount(byte[] tpl)
        {
            var paletteheaderoffset = GetTexturePaletteHeaderOffset(tpl);

            var tmp = new[] { tpl[paletteheaderoffset + 1], tpl[paletteheaderoffset] };
            return BitConverter.ToInt16(tmp, 0);
        }

        /// <summary>
        /// Gets the Texture Palette of the TPL
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static uint[] GetTexturePalette(byte[] tpl)
        {
            var paletteformat = GetTexturePaletteFormat(tpl);
            var itemcount = GetTexturePaletteItemCount(tpl);

            var output = new uint[itemcount];
            for (var i = 0; i < itemcount; i++)
            {
                if (i >= itemcount) continue;

                var pixel = BitConverter.ToUInt16(new[] { tpl[i * 2 + 1], tpl[i * 2] }, 0);

                int r;
                int g;
                int b;
                int a;
                switch (paletteformat)
                {
                    case 0:
                        r = (pixel >> 8);
                        b = r;
                        g = r;
                        a = ((pixel >> 0) & 0xff);
                        break;
                    case 1:
                        b = (((pixel >> 11) & 0x1F) << 3) & 0xff;
                        g = (((pixel >> 5) & 0x3F) << 2) & 0xff;
                        r = (((pixel >> 0) & 0x1F) << 3) & 0xff;
                        a = 255;
                        break;
                    default:
                        if ((pixel & (1 << 15)) != 0) //RGB555
                        {
                            a = 255;
                            b = (((pixel >> 10) & 0x1F) * 255) / 31;
                            g = (((pixel >> 5) & 0x1F) * 255) / 31;
                            r = (((pixel >> 0) & 0x1F) * 255) / 31;
                        }
                        else //RGB4A3
                        {
                            a = (((pixel >> 12) & 0x07) * 255) / 7;
                            b = (((pixel >> 8) & 0x0F) * 255) / 15;
                            g = (((pixel >> 4) & 0x0F) * 255) / 15;
                            r = (((pixel >> 0) & 0x0F) * 255) / 15;
                        }
                        break;
                }

                output[i] = (uint)((r << 0) | (g << 8) | (b << 16) | (a << 24));
            }

            return output;
        }

        /// <summary>
        /// Gets the Number of Textures in a Tpl
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTextureCount(byte[] tpl)
        {
            var tmp = new byte[4];
            tmp[3] = tpl[4];
            tmp[2] = tpl[5];
            tmp[1] = tpl[6];
            tmp[0] = tpl[7];
            return BitConverter.ToInt32(tmp, 0);
        }

        /// <summary>
        /// Gets the Format of the Texture in the Tpl
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTextureFormat(string tpl)
        {
            var temp = Tools.LoadFileToByteArray(tpl, 0, 512);
            return GetTextureFormat(temp);
        }

        /// <summary>
        /// Gets the Format of the Texture in the Tpl
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTextureFormat(byte[] tpl)
        {
            var offset = GetTextureHeaderOffset(tpl);

            var tmp = new byte[4];
            tmp[3] = tpl[offset + 4];
            tmp[2] = tpl[offset + 5];
            tmp[1] = tpl[offset + 6];
            tmp[0] = tpl[offset + 7];
            var format = BitConverter.ToUInt32(tmp, 0);

            if (format == 0 ||
                format == 1 ||
                format == 2 ||
                format == 3 ||
                format == 4 ||
                format == 5 ||
                format == 6 ||
                format == 8 ||
                format == 9 ||
                format == 10 ||
                format == 14) return (int)format;

            return -1; //Unsupported Format
        }

        /// <summary>
        /// Gets the Format Name of the Texture in the Tpl
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static string GetTextureFormatName(byte[] tpl)
        {
            switch (GetTextureFormat(tpl))
            {
                case 0:
                    return "I4";
                case 1:
                    return "I8";
                case 2:
                    return "IA4";
                case 3:
                    return "IA8";
                case 4:
                    return "RGB565";
                case 5:
                    return "RGB5A3";
                case 6:
                    return "RGBA8";
                case 8:
                    return "CI4";
                case 9:
                    return "CI8";
                case 10:
                    return "CI14X2";
                case 14:
                    return "CMP";
                default:
                    return "Unknown";
            }
        }

        public static int avg(int w0, int w1, int c0, int c1)
        {
            var a0 = c0 >> 11;
            var a1 = c1 >> 11;
            var a = (w0 * a0 + w1 * a1) / (w0 + w1);
            var c = (a << 11) & 0xffff;

            a0 = (c0 >> 5) & 63;
            a1 = (c1 >> 5) & 63;
            a = (w0 * a0 + w1 * a1) / (w0 + w1);
            c = c | ((a << 5) & 0xffff);

            a0 = c0 & 31;
            a1 = c1 & 31;
            a = (w0 * a0 + w1 * a1) / (w0 + w1);
            c = c | a;

            return c;
        }

        /// <summary>
        /// Gets the Width of the Texture in the Tpl
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTextureWidth(byte[] tpl)
        {
            var offset = GetTextureHeaderOffset(tpl);

            var tmp = new byte[2];
            tmp[1] = tpl[offset + 2];
            tmp[0] = tpl[offset + 3];
            return BitConverter.ToInt16(tmp, 0);
        }

        /// <summary>
        /// Gets the Height of the Texture in the Tpl
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTextureHeight(byte[] tpl)
        {
            var offset = GetTextureHeaderOffset(tpl);

            var tmp = new byte[2];
            tmp[1] = tpl[offset];
            tmp[0] = tpl[offset + 1];
            return BitConverter.ToInt16(tmp, 0);
        }

        /// <summary>
        /// Gets the offset to the Texturedata in the Tpl
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static int GetTextureOffset(byte[] tpl)
        {
            var offset = GetTextureHeaderOffset(tpl);

            var tmp = new byte[4];
            tmp[3] = tpl[offset + 8];
            tmp[2] = tpl[offset + 9];
            tmp[1] = tpl[offset + 10];
            tmp[0] = tpl[offset + 11];
            return BitConverter.ToInt32(tmp, 0);
        }

        /// <summary>
        /// Converts RGBA8 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromRGBA8(byte[] tpl)
        {
            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var inp = 0;
            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 4)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        for (var y1 = y; y1 < y + 4; y1++)
                        {
                            for (var x1 = x; x1 < x + 4; x1++)
                            {
                                var pixelbytes = new byte[2];
                                pixelbytes[1] = tpl[offset + inp * 2];
                                pixelbytes[0] = tpl[offset + inp * 2 + 1];
                                var pixel = BitConverter.ToUInt16(pixelbytes, 0);
                                inp++;

                                if ((x1 >= width) || (y1 >= height))
                                    continue;

                                if (k == 0)
                                {
                                    var a = (pixel >> 8) & 0xff;
                                    var r = (pixel >> 0) & 0xff;
                                    output[x1 + (y1 * width)] |= (UInt32)((r << 16) | (a << 24));
                                }
                                else
                                {
                                    var g = (pixel >> 8) & 0xff;
                                    var b = (pixel >> 0) & 0xff;
                                    output[x1 + (y1 * width)] |= (UInt32)((g << 8) | (b << 0));
                                }
                            }
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts RGB5A3 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromRGB5A3(byte[] tpl)
        {
            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var inp = 0;
            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 4)
                {
                    for (var y1 = y; y1 < y + 4; y1++)
                    {
                        for (var x1 = x; x1 < x + 4; x1++)
                        {
                            var pixelbytes = new byte[2];
                            pixelbytes[1] = tpl[offset + inp * 2];
                            pixelbytes[0] = tpl[offset + inp * 2 + 1];
                            var pixel = BitConverter.ToUInt16(pixelbytes, 0);
                            inp++;

                            if (y1 >= height || x1 >= width)
                                continue;

                            int r;
                            int b;
                            int a;
                            int g;
                            if ((pixel & (1 << 15)) != 0)
                            {
                                b = (((pixel >> 10) & 0x1F) * 255) / 31;
                                g = (((pixel >> 5) & 0x1F) * 255) / 31;
                                r = (((pixel >> 0) & 0x1F) * 255) / 31;
                                a = 255;
                            }
                            else
                            {
                                a = (((pixel >> 12) & 0x07) * 255) / 7;
                                b = (((pixel >> 8) & 0x0F) * 255) / 15;
                                g = (((pixel >> 4) & 0x0F) * 255) / 15;
                                r = (((pixel >> 0) & 0x0F) * 255) / 15;
                            }

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[(y1 * width) + x1] = (UInt32)rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts RGB565 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromRGB565(byte[] tpl)
        {
            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var inp = 0;
            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 4)
                {
                    for (var y1 = y; y1 < y + 4; y1++)
                    {
                        for (var x1 = x; x1 < x + 4; x1++)
                        {
                            var pixelbytes = new byte[2];
                            pixelbytes[1] = tpl[offset + inp * 2];
                            pixelbytes[0] = tpl[offset + inp * 2 + 1];
                            var pixel = BitConverter.ToUInt16(pixelbytes, 0);
                            inp++;

                            if (y1 >= height || x1 >= width)
                                continue;

                            var b = (((pixel >> 11) & 0x1F) << 3) & 0xff;
                            var g = (((pixel >> 5) & 0x3F) << 2) & 0xff;
                            var r = (((pixel >> 0) & 0x1F) << 3) & 0xff;
                            const int a = 255;

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[y1 * width + x1] = (UInt32)rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts I4 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromI4(byte[] tpl)
        {
            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var inp = 0;

            for (var y = 0; y < height; y += 8)
            {
                for (var x = 0; x < width; x += 8)
                {
                    for (var y1 = y; y1 < y + 8; y1++)
                    {
                        for (var x1 = x; x1 < x + 8; x1 += 2)
                        {
                            int pixel = tpl[offset + inp++];

                            if (y1 >= height || x1 >= width)
                                continue;

                            var r = (pixel >> 4) * 255 / 15;
                            var g = (pixel >> 4) * 255 / 15;
                            var b = (pixel >> 4) * 255 / 15;
                            var a = 255;

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[y1 * width + x1] = (UInt32)rgba;

                            r = (pixel & 0x0F) * 255 / 15;
                            g = (pixel & 0x0F) * 255 / 15;
                            b = (pixel & 0x0F) * 255 / 15;
                            a = 255;

                            rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[y1 * width + x1 + 1] = (UInt32)rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts IA4 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromIA4(byte[] tpl)
        {
            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var inp = 0;

            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 8)
                {
                    for (var y1 = y; y1 < y + 4; y1++)
                    {
                        for (var x1 = x; x1 < x + 8; x1++)
                        {
                            int pixel = tpl[offset + inp];
                            inp++;

                            if (y1 >= height || x1 >= width)
                                continue;

                            var r = ((pixel & 0x0F) * 255 / 15) & 0xff;
                            var g = ((pixel & 0x0F) * 255 / 15) & 0xff;
                            var b = ((pixel & 0x0F) * 255 / 15) & 0xff;
                            var a = (((pixel >> 4) * 255) / 15) & 0xff;

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[y1 * width + x1] = (UInt32)rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts I8 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromI8(byte[] tpl)
        {
            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var inp = 0;

            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 8)
                {
                    for (var y1 = y; y1 < y + 4; y1++)
                    {
                        for (var x1 = x; x1 < x + 8; x1++)
                        {
                            int pixel = tpl[offset + inp];
                            inp++;

                            if (y1 >= height || x1 >= width)
                                continue;

                            var r = pixel;
                            var g = pixel;
                            var b = pixel;
                            const int a = 255;

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[y1 * width + x1] = (UInt32)rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts IA8 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromIA8(byte[] tpl)
        {
            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var inp = 0;

            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 4)
                {
                    for (var y1 = y; y1 < y + 4; y1++)
                    {
                        for (var x1 = x; x1 < x + 4; x1++)
                        {
                            var pixelbytes = new byte[2];
                            pixelbytes[1] = tpl[offset + inp * 2];
                            pixelbytes[0] = tpl[offset + inp * 2 + 1];
                            var pixel = BitConverter.ToUInt16(pixelbytes, 0);
                            inp++;

                            if (y1 >= height || x1 >= width)
                                continue;

                            var r = (pixel >> 8);// &0xff;
                            var g = (pixel >> 8);// &0xff;
                            var b = (pixel >> 8);// &0xff;
                            var a = (pixel >> 0) & 0xff;

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[y1 * width + x1] = (UInt32)rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts CMP Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromCMP(byte[] tpl)
        {
            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var c = new UInt16[4];
            var pix = new int[4];
            var inp = 0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var ww = Tools.AddPadding(width, 8);

                    var x0 = x & 0x03;
                    var x1 = (x >> 2) & 0x01;
                    var x2 = x >> 3;

                    var y0 = y & 0x03;
                    var y1 = (y >> 2) & 0x01;
                    var y2 = y >> 3;

                    var off = (8 * x1) + (16 * y1) + (32 * x2) + (4 * ww * y2);

                    var tmp1 = new byte[2];
                    tmp1[1] = tpl[offset + off];
                    tmp1[0] = tpl[offset + off + 1];
                    c[0] = BitConverter.ToUInt16(tmp1, 0);
                    tmp1[1] = tpl[offset + off + 2];
                    tmp1[0] = tpl[offset + off + 3];
                    c[1] = BitConverter.ToUInt16(tmp1, 0);

                    if (c[0] > c[1])
                    {
                        c[2] = (UInt16)avg(2, 1, c[0], c[1]);
                        c[3] = (UInt16)avg(1, 2, c[0], c[1]);
                    }
                    else
                    {
                        c[2] = (UInt16)avg(1, 1, c[0], c[1]);
                        c[3] = 0;
                    }

                    var pixeldata = new byte[4];
                    pixeldata[3] = tpl[offset + off + 4];
                    pixeldata[2] = tpl[offset + off + 5];
                    pixeldata[1] = tpl[offset + off + 6];
                    pixeldata[0] = tpl[offset + off + 7];
                    var pixel = BitConverter.ToUInt32(pixeldata, 0);

                    var ix = x0 + (4 * y0);
                    int raw = c[(pixel >> (30 - (2 * ix))) & 0x03];

                    pix[0] = (raw >> 8) & 0xf8;
                    pix[1] = (raw >> 3) & 0xf8;
                    pix[2] = (raw << 3) & 0xf8;
                    pix[3] = 0xff;
                    if (((pixel >> (30 - (2 * ix))) & 0x03) == 3 && c[0] <= c[1]) pix[3] = 0x00;

                    var intout = (pix[0] << 16) | (pix[1] << 8) | (pix[2] << 0) | (pix[3] << 24);
                    output[inp] = (UInt32)intout;
                    inp++;
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts CI4 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromCI4(byte[] tpl)
        {
            var palette = GetTexturePalette(tpl);

            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height + 1];
            var i = 0;

            for (var y = 0; y < height; y += 8)
            {
                for (var x = 0; x < width; x += 8)
                {
                    for (var y1 = y; y1 < y + 8; y1++)
                    {
                        for (var x1 = x; x1 < x + 8; x1 += 2)
                        {
                            if (y1 >= height || x1 >= width)
                                continue;
                            
                            UInt16 pixel = tpl[offset + i++];

                            var r = ((palette[pixel >> 4] & 0xFF000000) >> 24);
                            var g = (palette[pixel >> 4] & 0x00FF0000) >> 16;
                            var b = (palette[pixel >> 4] & 0x0000FF00) >> 8;
                            var a = (palette[pixel >> 4] & 0x000000FF) >> 0;

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
						    output[y1 * width + x1] = rgba;

                            r = ((palette[pixel & 0x0F] & 0xFF000000) >> 24);
                            g = (palette[pixel & 0x0F] & 0x00FF0000) >> 16;
                            b = (palette[pixel & 0x0F] & 0x0000FF00) >> 8;
                            a = (palette[pixel & 0x0F] & 0x000000FF) >> 0;

                            rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);

                            output[y1 * width + x1 + 1] = rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts CI8 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromCI8(byte[] tpl)
        {
            var palette = GetTexturePalette(tpl);

            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var i = 0;

            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 8)
                {
                    for (var y1 = y; y1 < y + 4; y1++)
                    {
                        for (var x1 = x; x1 < x + 8; x1++)
                        {
                            if (y1 >= height || x1 >= width)
                                continue;

                            UInt16 pixel = tpl[offset + i++];

                            var r = ((palette[pixel] & 0xFF000000) >> 24);
                            var g = (palette[pixel] & 0x00FF0000) >> 16;
                            var b = (palette[pixel] & 0x0000FF00) >> 8;
                            var a = (palette[pixel] & 0x000000FF) >> 0;

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[y1 * width + x1] = rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Converts CI14X2 Tpl Array to RGBA Byte Array
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static byte[] FromCI14X2(byte[] tpl)
        {
            var palette = GetTexturePalette(tpl);

            var width = GetTextureWidth(tpl);
            var height = GetTextureHeight(tpl);
            var offset = GetTextureOffset(tpl);
            var output = new UInt32[width * height];
            var i = 0;

            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 4)
                {
                    for (var y1 = y; y1 < y + 4; y1++)
                    {
                        for (var x1 = x; x1 < x + 4; x1++)
                        {
                            if (y1 >= height || x1 >= width)
                                continue;

                            UInt16 pixel = tpl[offset + i++];

                            var r = ((palette[pixel & 0x3FFF] & 0xFF000000) >> 24);
                            var g = (palette[pixel & 0x3FFF] & 0x00FF0000) >> 16;
                            var b = (palette[pixel & 0x3FFF] & 0x0000FF00) >> 8;
                            var a = (palette[pixel & 0x3FFF] & 0x000000FF) >> 0;

                            var rgba = (r << 0) | (g << 8) | (b << 16) | (a << 24);
                            output[y1 * width + x1] = rgba;
                        }
                    }
                }
            }

            return Tools.UIntArrayToByteArray(output);
        }

        /// <summary>
        /// Gets the pixel data of a Bitmap as a Byte Array
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static uint[] BitmapToRGBA(Bitmap img)
        {
            var x = img.Width;
            var y = img.Height;
            var rgba = new UInt32[x * y];

            for (var i = 0; i < y; i += 4)
            {
                for (var j = 0; j < x; j += 4)
                {
                    for (var y1 = i; y1 < i + 4; y1++)
                    {
                        for (var x1 = j; x1 < j + 4; x1++)
                        {
                            if (y1 >= y || x1 >= x)
                                continue;

                            var color = img.GetPixel(x1, y1);
                            rgba[x1 + (y1 * x)] = (UInt32)color.ToArgb();
                        }
                    }
                }
            }

            return rgba;
        }

        /// <summary>
        /// Converts an Image to a Tpl.CPMR
        /// </summary>
        /// <param name="img"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static void ConvertToTPL(Bitmap img, string destination)
        {
            var tpl = ConvertToTPL(img);

            using (var fs = new FileStream(destination, FileMode.Create))
            {
                fs.Write(tpl, 0, tpl.Length);
                fs.Dispose();
            }
        }

        /// <summary>
        /// Converts an Image to a Tpl
        /// </summary>
        /// <param name="img"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static void ConvertToTPL(Image img, string destination)
        {
            var tpl = ConvertToTPL((Bitmap)img);

            using (var fs = new FileStream(destination, FileMode.Create))
            {
                fs.Write(tpl, 0, tpl.Length);
                fs.Dispose();
            }
        }

        /// <summary>
        /// Converts an Image to a Tpl
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ConvertToTPL(Image img)
        {
            return ConvertToTPL((Bitmap)img);
        }
        
        public static byte[] ToCMP(Bitmap img)
        {
            var pixeldata = BitmapToRGBA(img);
            var w = img.Width;
            var h = img.Height;
            var z = -1;
            var output = new byte[Tools.AddPadding(w, 4) * Tools.AddPadding(h, 4) * 2];

            for (var y1 = 0; y1 < h; y1 += 4)
            {
                for (var x1 = 0; x1 < w; x1 += 4)
                {
                    for (var y = y1; y < y1 + 4; y++)
                    {
                        for (var x = x1; x < x1 + 4; x++)
                        {
                            UInt16 newpixel;

                            if (y >= h || x >= w)
                            {
                                newpixel = 0;
                            }
                            else
                            {
                                var rgba = pixeldata[x + (y * w)];

                                var b = (rgba >> 16) & 0xff;
                                var g = (rgba >> 8) & 0xff;
                                var r = (rgba >> 0) & 0xff;

                                newpixel = (UInt16)(((b >> 3) << 11) | ((g >> 2) << 5) | ((r >> 3) << 0));
                            }

                            var temp = BitConverter.GetBytes(newpixel);
                            Array.Reverse(temp);

                            output[++z] = temp[0];
                            output[++z] = temp[1];
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Converts an Image to a Tpl
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ConvertToTPL(Bitmap img)
        {
            using (var ms = new MemoryStream())
            {
                const uint tplmagic = 0x20af30;
                const uint ntextures = 0x1;
                const uint headersize = 0xc;
                const uint texheaderoff = 0x14;
                const uint texpaletteoff = 0x0;

                var texheight = (UInt16)img.Height;
                var texwidth = (UInt16)img.Width;
                const uint texdataoffset = 0x40;
                var rest = new byte[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 };
                //This should do it for our needs.. rest includes padding

                const uint texformat = 0x0e;
                var rgbaData = ToCMP(img);

                var buffer = BitConverter.GetBytes(tplmagic); Array.Reverse(buffer);
                ms.Seek(0, SeekOrigin.Begin);
                ms.Write(buffer, 0, buffer.Length);

                buffer = BitConverter.GetBytes(ntextures); Array.Reverse(buffer);
                ms.Write(buffer, 0, buffer.Length);

                buffer = BitConverter.GetBytes(headersize); Array.Reverse(buffer);
                ms.Write(buffer, 0, buffer.Length);

                buffer = BitConverter.GetBytes(texheaderoff); Array.Reverse(buffer);
                ms.Write(buffer, 0, buffer.Length);

                buffer = BitConverter.GetBytes(texpaletteoff); Array.Reverse(buffer);
                ms.Write(buffer, 0, buffer.Length);

                buffer = BitConverter.GetBytes(texheight); Array.Reverse(buffer);
                ms.Write(buffer, 0, buffer.Length);

                buffer = BitConverter.GetBytes(texwidth); Array.Reverse(buffer);
                ms.Write(buffer, 0, buffer.Length);

                buffer = BitConverter.GetBytes(texformat); Array.Reverse(buffer);
                ms.Write(buffer, 0, buffer.Length);

                buffer = BitConverter.GetBytes(texdataoffset); Array.Reverse(buffer);
                ms.Write(buffer, 0, buffer.Length);

                ms.Write(rest, 0, rest.Length);

                ms.Write(rgbaData, 0, rgbaData.Length);

                ms.Dispose();
                return ms.ToArray();
            }
        }
    }

}