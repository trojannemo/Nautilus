using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Nautilus.Extensions;
using Nautilus.Util;

namespace Nautilus.Texture
{
    internal class TextureConverter
    {
        public string TextureFormat = "DXT1";

        static int RGB565ToARGB(ushort input)
        {
            return Color.FromArgb(0xFF,
              ((input >> 11) & 0x1F) << 3 | ((input >> 11) & 0x1F) >> 2,
              ((input >> 5) & 0x3F) << 2 | ((input >> 5) & 0x3F) >> 4,
              (input & 0x1F) << 3 | (input & 0x1F) >> 2).ToArgb();
        }
        static ushort ARGBToRGB565(Color input)
        {
            return (ushort)
              ((((input.R * 0x1F / 0xFF) & 0x1F) << 11) |
               (((input.G * 0x3F / 0xFF) & 0x3F) << 5) |
               (((input.B * 0x1F / 0xFF) & 0x1F)));
        }
        // TODO: Decode DXT5 alpha channel
        public Bitmap ToBitmap(Texture t, int mipmap)
        {
            var m = t.Mipmaps[mipmap];
            var output = new Bitmap(m.Width, m.Height, PixelFormat.Format32bppArgb);
            int[] imageData = new int[m.Width * m.Height];
            if (m.Data.Length == (imageData.Length * 4))
            {
                Buffer.BlockCopy(m.Data, 0, imageData, 0, m.Data.Length);
            }
            else if (m.Data.Length == imageData.Length)
            {
                TextureFormat = "DXT5";
                DecodeDXT(m, imageData, true);
            }
            else if (m.Data.Length == (imageData.Length / 2))
            {
                DecodeDXT(m, imageData, false);
            }
            else
            {
                throw new Exception($"Don't know what to do with this texture (version={t.Version})");
            }
            // Copy data to bitmap
            {
                var data = output.LockBits(new Rectangle(0, 0, m.Width, m.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                System.Runtime.InteropServices.Marshal.Copy(imageData, 0, data.Scan0, imageData.Length);
                output.UnlockBits(data);
            }
            return output;
        }

        private static void DecodeDXT(Texture.Mipmap m, int[] imageData, bool DXT5)
        {
            int[] colors = new int[4];
            using (var s = new MemoryStream(m.Data))
            {
                ushort[] c = new ushort[4];
                byte[] iData = new byte[4];
                for (var y = 0; y < m.Height; y += 4)
                    for (var x = 0; x < m.Width; x += 4)
                    {
                        if (DXT5) s.Seek(8, SeekOrigin.Current);
                        ushort c0 = s.ReadUInt16LE();
                        ushort c1 = s.ReadUInt16LE();
                        colors[0] = RGB565ToARGB(c0);
                        colors[1] = RGB565ToARGB(c1);
                        var color0 = Color.FromArgb(colors[0]);
                        var color1 = Color.FromArgb(colors[1]);
                        s.Read(iData, 0, 4);
                        if (c0 > c1)
                        {
                            colors[2] = Color.FromArgb(0xFF,
                              (color0.R * 2 + color1.R) / 3,
                              (color0.G * 2 + color1.G) / 3,
                              (color0.B * 2 + color1.B) / 3).ToArgb();
                            colors[3] = Color.FromArgb(0xFF,
                              (color0.R + (color1.R * 2)) / 3,
                              (color0.G + (color1.G * 2)) / 3,
                              (color0.B + (color1.B * 2)) / 3).ToArgb();
                        }
                        else
                        {
                            colors[2] = Color.FromArgb(0xFF,
                              (color0.R + color1.R) / 2,
                              (color0.G + color1.G) / 2,
                              (color0.B + color1.B) / 2).ToArgb();
                            colors[3] = Color.Black.ToArgb();
                        }
                        var offset = y * m.Width + x;
                        for (var i = 0; i < 4; i++)
                        {
                            for (var j = 0; j < 4; j++)
                            {
                                var idx = (iData[i] >> (2 * j)) & 0x3;
                                imageData[offset + i * m.Width + j] = colors[idx];
                            }
                        }
                    }
            }
        }

        private static double ColorDist(Color c1, Color c2)
        {
            // According to Wikipedia, these coefficients should produce an OK delta
            return Math.Sqrt(
                2 * Math.Pow(c1.R - c2.R, 2)
              + 4 * Math.Pow(c1.G - c2.G, 2)
              + 3 * Math.Pow(c1.B - c2.B, 2));
        }

        private static IEnumerable<Color> EnumerateBlockColors(Bitmap img, int x, int y)
        {
            for (var y0 = 0; y0 < 4; y0++)
                for (var x0 = 0; x0 < 4; x0++)
                    yield return img.GetPixel(x + x0, y + y0);
        }

        private static byte[] EncodeDxt(Image image, int mapLevel, int nominalSize = 256)
        {
            var img = new Bitmap(image, new Size(nominalSize >> mapLevel, nominalSize >> mapLevel));
            var data = new byte[img.Width * img.Height / 2];
            var idx = 0;
            for (var y = 0; y < img.Height; y += 4)
                for (var x = 0; x < img.Width; x += 4)
                {
                    // Pick the farthest-apart colors in this block as the endpoints
                    int i0 = 0, j0 = 1;
                    var blockColors = EnumerateBlockColors(img, x, y).ToArray();
                    double highest = 0;
                    for (var i = 0; i < 16; i++)
                    {
                        for (var j = i + 1; j < 16; j++)
                        {
                            var d = ColorDist(blockColors[i], blockColors[j]);
                            if (d >= highest)
                            {
                                i0 = i;
                                j0 = j;
                                highest = d;
                            }
                        }
                    }
                    var c1 = blockColors[i0];
                    var c2 = blockColors[j0];
                    var colors = new[]
                    {
            c1, c2,
            Color.FromArgb(0xFF,
              (c1.R * 2 + c2.R) / 3,
              (c1.G * 2 + c2.G) / 3,
              (c1.B * 2 + c2.B) / 3),
            Color.FromArgb(0xFF,
              (c1.R + (c2.R * 2)) / 3,
              (c1.G + (c2.G * 2)) / 3,
              (c1.B + (c2.B * 2)) / 3)
          };
                    var color0 = ARGBToRGB565(colors[0]);
                    var color1 = ARGBToRGB565(colors[1]);
                    Color tmp;

                    if (color0 < color1)
                    {
                        // swap colors
                        color0 ^= color1;
                        color1 ^= color0;
                        color0 ^= color1;
                        tmp = colors[0];
                        colors[0] = colors[1];
                        colors[1] = tmp;
                    }
                    if (color0 == color1)
                    {
                        // The square is uniform, so just tell the later code not to use color3
                        colors[3] = Color.Black;
                    }
                    data[idx++] = (byte)(color0 & 0xFF);
                    data[idx++] = (byte)(color0 >> 8);
                    data[idx++] = (byte)(color1 & 0xFF);
                    data[idx++] = (byte)(color1 >> 8);

                    for (var j = 0; j < 4; j++, idx++)
                    {
                        for (var i = 0; i < 4; i++)
                        {
                            var pixel = blockColors[i + 4 * j];
                            double lowest = double.MaxValue;
                            int bestColor = 0;
                            for (var k = 0; k < colors.Length; k++)
                            {
                                var diff = ColorDist(colors[k], pixel);
                                if (diff < lowest)
                                {
                                    lowest = diff;
                                    bestColor = k;
                                }
                            }
                            data[idx] |= (byte)(bestColor << (i * 2));
                        }
                    }
                }
            return data;
        }

        static byte[] HeaderData256x256 = new byte[]
        {
      0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00,
      0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x04, 0x00, 0x00, 0x00,
      0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        static byte[] FooterData256x256 = new byte[]
        {
      0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x0F, 0xC2, 0x73, 0x3D, 0x1C, 0x99, 0x4B, 0x3D,
      0x05, 0xC1, 0x0D, 0x3E, 0x00, 0x00, 0x80, 0x3F, 0x08, 0x00, 0x00, 0x00
        };

        public static Texture ToTexture(Image image)
        {
            Texture.Mipmap[] maps = new Texture.Mipmap[7];
            for (var i = 0; i < maps.Length; i++)
            {
                maps[i] = new Texture.Mipmap
                {
                    Width = 256 / (1 << i),
                    Height = 256 / (1 << i),
                    Data = EncodeDxt(image, i)
                };
            }
            return new Texture
            {
                HeaderData = HeaderData256x256,
                FooterData = FooterData256x256,
                Version = 6,
                Mipmaps = maps
            };
        }
    }    
}

namespace Nautilus.Extensions
{
    internal static class StreamExtensions
    {
        /// <summary>
        /// Read a signed 8-bit integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static sbyte ReadInt8(this Stream s) => unchecked((sbyte)s.ReadUInt8());

        /// <summary>
        /// Read an unsigned 8-bit integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte ReadUInt8(this Stream s)
        {
            byte ret;
            byte[] tmp = new byte[1];
            s.Read(tmp, 0, 1);
            ret = tmp[0];
            return ret;
        }

        /// <summary>
        /// Read an unsigned 16-bit little-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ushort ReadUInt16LE(this Stream s) => unchecked((ushort)s.ReadInt16LE());

        /// <summary>
        /// Read a signed 16-bit little-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static short ReadInt16LE(this Stream s)
        {
            int ret;
            byte[] tmp = new byte[2];
            s.Read(tmp, 0, 2);
            ret = tmp[0] & 0x00FF;
            ret |= (tmp[1] << 8) & 0xFF00;
            return (short)ret;
        }

        public static void WriteInt16LE(this Stream s, short i)
        {
            s.WriteUInt16LE(unchecked((ushort)i));
        }

        public static void WriteUInt16LE(this Stream s, ushort i)
        {
            byte[] tmp = new byte[2];
            tmp[0] = (byte)(i & 0xFF);
            tmp[1] = (byte)((i >> 8) & 0xFF);
            s.Write(tmp, 0, 2);
        }

        /// <summary>
        /// Read an unsigned 16-bit Big-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ushort ReadUInt16BE(this Stream s) => unchecked((ushort)s.ReadInt16BE());

        /// <summary>
        /// Read a signed 16-bit Big-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static short ReadInt16BE(this Stream s)
        {
            int ret;
            byte[] tmp = new byte[2];
            s.Read(tmp, 0, 2);
            ret = (tmp[0] << 8) & 0xFF00;
            ret |= tmp[1] & 0x00FF;
            return (short)ret;
        }

        public static void WriteUInt24LE(this Stream s, uint i)
        {
            byte[] tmp = new byte[3];
            tmp[0] = (byte)(i & 0xFF);
            tmp[1] = (byte)((i >> 8) & 0xFF);
            tmp[2] = (byte)((i >> 16) & 0xFF);
            s.Write(tmp, 0, 3);
        }
        public static void WriteUInt24BE(this Stream s, uint i)
        {
            byte[] tmp = new byte[3];
            tmp[2] = (byte)(i & 0xFF);
            tmp[1] = (byte)((i >> 8) & 0xFF);
            tmp[0] = (byte)((i >> 16) & 0xFF);
            s.Write(tmp, 0, 3);
        }

        /// <summary>
        /// Read an unsigned 24-bit little-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static uint ReadUInt24LE(this Stream s)
        {
            int ret;
            byte[] tmp = new byte[3];
            s.Read(tmp, 0, 3);
            ret = tmp[0] & 0x0000FF;
            ret |= (tmp[1] << 8) & 0x00FF00;
            ret |= (tmp[2] << 16) & 0xFF0000;
            return unchecked((uint)ret);
        }

        /// <summary>
        /// Read a signed 24-bit little-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ReadInt24LE(this Stream s)
        {
            int ret;
            byte[] tmp = new byte[3];
            s.Read(tmp, 0, 3);
            ret = tmp[0] & 0x0000FF;
            ret |= (tmp[1] << 8) & 0x00FF00;
            ret |= (tmp[2] << 16) & 0xFF0000;
            if ((tmp[2] & 0x80) == 0x80)
            {
                ret |= 0xFF << 24;
            }
            return ret;
        }

        /// <summary>
        /// Read an unsigned 24-bit Big-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static uint ReadUInt24BE(this Stream s)
        {
            int ret;
            byte[] tmp = new byte[3];
            s.Read(tmp, 0, 3);
            ret = tmp[2] & 0x0000FF;
            ret |= (tmp[1] << 8) & 0x00FF00;
            ret |= (tmp[0] << 16) & 0xFF0000;
            return (uint)ret;
        }

        /// <summary>
        /// Read a signed 24-bit Big-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ReadInt24BE(this Stream s)
        {
            int ret;
            byte[] tmp = new byte[3];
            s.Read(tmp, 0, 3);
            ret = tmp[2] & 0x0000FF;
            ret |= (tmp[1] << 8) & 0x00FF00;
            ret |= (tmp[0] << 16) & 0xFF0000;
            if ((tmp[0] & 0x80) == 0x80)
            {
                ret |= 0xFF << 24; // sign-extend
            }
            return ret;
        }

        /// <summary>
        /// Read an unsigned 32-bit little-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static uint ReadUInt32LE(this Stream s) => unchecked((uint)s.ReadInt32LE());

        /// <summary>
        /// Read a signed 32-bit little-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ReadInt32LE(this Stream s)
        {
            int ret;
            byte[] tmp = new byte[4];
            s.Read(tmp, 0, 4);
            ret = tmp[0] & 0x000000FF;
            ret |= (tmp[1] << 8) & 0x0000FF00;
            ret |= (tmp[2] << 16) & 0x00FF0000;
            ret |= (tmp[3] << 24);
            return ret;
        }

        public static void WriteInt32LE(this Stream s, int i)
        {
            s.WriteUInt32LE(unchecked((uint)i));
        }

        public static void WriteUInt32LE(this Stream s, uint i)
        {
            byte[] tmp = new byte[4];
            tmp[0] = (byte)(i & 0xFF);
            tmp[1] = (byte)((i >> 8) & 0xFF);
            tmp[2] = (byte)((i >> 16) & 0xFF);
            tmp[3] = (byte)((i >> 24) & 0xFF);
            s.Write(tmp, 0, 4);
        }

        /// <summary>
        /// Read an unsigned 32-bit Big-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static uint ReadUInt32BE(this Stream s) => unchecked((uint)s.ReadInt32BE());

        /// <summary>
        /// Read a signed 32-bit Big-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ReadInt32BE(this Stream s)
        {
            int ret;
            byte[] tmp = new byte[4];
            s.Read(tmp, 0, 4);
            ret = (tmp[0] << 24);
            ret |= (tmp[1] << 16) & 0x00FF0000;
            ret |= (tmp[2] << 8) & 0x0000FF00;
            ret |= tmp[3] & 0x000000FF;
            return ret;
        }

        /// <summary>
        /// Read an unsigned 64-bit little-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ulong ReadUInt64LE(this Stream s) => unchecked((ulong)s.ReadInt64LE());

        /// <summary>
        /// Read a signed 64-bit little-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ReadInt64LE(this Stream s)
        {
            long ret;
            byte[] tmp = new byte[8];
            s.Read(tmp, 0, 8);
            ret = tmp[4] & 0x000000FFL;
            ret |= (tmp[5] << 8) & 0x0000FF00L;
            ret |= (tmp[6] << 16) & 0x00FF0000L;
            ret |= (tmp[7] << 24) & 0xFF000000L;
            ret <<= 32;
            ret |= tmp[0] & 0x000000FFL;
            ret |= (tmp[1] << 8) & 0x0000FF00L;
            ret |= (tmp[2] << 16) & 0x00FF0000L;
            ret |= (tmp[3] << 24) & 0xFF000000L;
            return ret;
        }

        public static void WriteInt64LE(this Stream s, long i)
        {
            s.WriteUInt64LE(unchecked((ulong)i));
        }

        public static void WriteUInt64LE(this Stream s, ulong i)
        {
            byte[] tmp = new byte[8];
            tmp[0] = (byte)(i & 0xFF);
            tmp[1] = (byte)((i >> 8) & 0xFF);
            tmp[2] = (byte)((i >> 16) & 0xFF);
            tmp[3] = (byte)((i >> 24) & 0xFF);
            i >>= 32;
            tmp[4] = (byte)(i & 0xFF);
            tmp[5] = (byte)((i >> 8) & 0xFF);
            tmp[6] = (byte)((i >> 16) & 0xFF);
            tmp[7] = (byte)((i >> 24) & 0xFF);
            s.Write(tmp, 0, 8);
        }

        /// <summary>
        /// Read an unsigned 64-bit big-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ulong ReadUInt64BE(this Stream s) => unchecked((ulong)s.ReadInt64BE());

        /// <summary>
        /// Read a signed 64-bit big-endian integer from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ReadInt64BE(this Stream s)
        {
            long ret;
            byte[] tmp = new byte[8];
            s.Read(tmp, 0, 8);
            ret = tmp[3] & 0x000000FFL;
            ret |= (tmp[2] << 8) & 0x0000FF00L;
            ret |= (tmp[1] << 16) & 0x00FF0000L;
            ret |= (tmp[0] << 24) & 0xFF000000L;
            ret <<= 32;
            ret |= tmp[7] & 0x000000FFL;
            ret |= (tmp[6] << 8) & 0x0000FF00L;
            ret |= (tmp[5] << 16) & 0x00FF0000L;
            ret |= (tmp[4] << 24) & 0xFF000000L;
            return ret;
        }

        /// <summary>
        /// Reads a multibyte value of the specified length from the stream.
        /// </summary>
        /// <param name="s">The stream</param>
        /// <param name="bytes">Must be less than or equal to 8</param>
        /// <returns></returns>
        public static long ReadMultibyteBE(this Stream s, byte bytes)
        {
            if (bytes > 8) return 0;
            long ret = 0;
            var b = s.ReadBytes(bytes);
            for (uint i = 0; i < b.Length; i++)
            {
                ret <<= 8;
                ret |= b[i];
            }
            return ret;
        }

        /// <summary>
        /// Read a single-precision (4-byte) floating-point value from the stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float ReadFloat(this Stream s)
        {
            byte[] tmp = new byte[4];
            s.Read(tmp, 0, 4);
            return BitConverter.ToSingle(tmp, 0);
        }

        /// <summary>
        /// Read a half-precision (2-byte) floating point value from the stream.
        /// Return value is aliased to a single precision float because C# does not support half floats.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float ReadHalfFloat(this Stream s) => ParseHalfFloat(s.ReadUInt16LE());

        unsafe public static float ParseHalfFloat(int half)
        {
            int sign = half >> 15;
            int exponent = ((half >> 10) & 0x1F);
            int mantissa = half & 0x03FF;
            int single;
            if (exponent == 0)
            {
                // Subnormal
                if (mantissa == 0) return 0;
                int exp = -15;
                int mask = 0x3FF;
                // Find the first leading 1.
                while (mantissa == (mantissa & mask))
                {
                    mask >>= 1;
                    exp--;
                }
                // AND the mantissa with the mask because the SP float is *not* subnormal and has an implied "1."
                single = (sign << 31) | (((128 + exp) & 0xFF) << 23) | ((mantissa & mask) << (30 + exp));
            }
            else
            {
                single = exponent == 31 ?
                  // Infinity
                  (mantissa == 0 ? (sign << 31) | (0xFF << 23)
                  // NaN
                  : (sign << 31) | (0xFF << 23) | 1)
                  // Normal
                  : (sign << 31) | (((exponent + 112) & 0xFF) << 23) | (mantissa << 13);
            }
            // TODO: Any other option besides unsafe code or allocating an unnecessary byte array?
            return *(float*)(&single);
            /* Eek, unsafe, but BitConverter.ToSingle is also unsafe according to reference source,
               and it requires allocating another byte array and multiple method calls... */
            // return BitConverter.ToSingle(BitConverter.GetBytes(single), 0);
        }


        /// <summary>
        /// Read a null-terminated ASCII string from the given stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReadASCIINullTerminated(this Stream s, int limit = -1)
        {
            StringBuilder sb = new StringBuilder(255);
            char cur;
            while ((limit == -1 || sb.Length < limit) && (cur = (char)s.ReadByte()) != 0)
            {
                sb.Append(cur);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Read a null-terminated UTF8 string from the given stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReadFixedLengthNullTerminatedString(this Stream s, int length)
        {
            var stringBytes = s.ReadBytes(length);
            var endIdx = 0;
            for (int i = 0; i < stringBytes.Length; i++)
            {
                endIdx = i;
                if (stringBytes[i] == 0)
                    break;
            }
            return Encoding.UTF8.GetString(stringBytes, 0, endIdx);
        }

        public static string ReadFixedLengthString(this Stream s, int length)
        {
            var stringBytes = s.ReadBytes(length);
            return Encoding.UTF8.GetString(stringBytes, 0, stringBytes.Length);
        }

        /// <summary>
        /// Read a length-prefixed string of the specified encoding type from the file.
        /// The length is a 32-bit little endian integer.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e">The encoding to use to decode the string.</param>
        /// <returns></returns>
        public static string ReadLengthPrefixedString(this Stream s, Encoding e, bool bigEdn = false)
        {
            int length = bigEdn ? s.ReadInt32BE() : s.ReadInt32LE();
            byte[] chars = new byte[length];
            s.Read(chars, 0, length);
            return e.GetString(chars);
        }

        /// <summary>
        /// Read a length-prefixed UTF-8 string from the given stream.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReadLengthUTF8(this Stream s, bool bigEdn = false)
        {
            return s.ReadLengthPrefixedString(Encoding.UTF8, bigEdn);
        }

        /// <summary>
        /// Read a given number of bytes from a stream into a new byte array.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count">Number of bytes to read (maximum)</param>
        /// <returns>New byte array of size &lt;=count.</returns>
        public static byte[] ReadBytes(this Stream s, int count)
        {
            // Size of returned array at most count, at least difference between position and length.
            int realCount = (int)((s.Position + count > s.Length) ? (s.Length - s.Position) : count);
            byte[] ret = new byte[realCount];
            s.Read(ret, 0, realCount);
            return ret;
        }

        /// <summary>
        /// Read a variable-length integral value as found in MIDI messages.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static uint ReadMidiMultiByte(this Stream s)
        {
            int ret = 0;
            byte b = (byte)(s.ReadByte());
            ret += b & 0x7f;
            if (0x80 == (b & 0x80))
            {
                ret <<= 7;
                b = (byte)(s.ReadByte());
                ret += b & 0x7f;
                if (0x80 == (b & 0x80))
                {
                    ret <<= 7;
                    b = (byte)(s.ReadByte());
                    ret += b & 0x7f;
                    if (0x80 == (b & 0x80))
                    {
                        ret <<= 7;
                        b = (byte)(s.ReadByte());
                        ret += b & 0x7f;
                        if (0x80 == (b & 0x80))
                            throw new InvalidDataException("Variable-length MIDI number > 4 bytes");
                    }
                }
            }
            return (uint)ret;
        }

        public static void WriteMidiMultiByte(this Stream s, uint i)
        {
            if (i > 0x7FU)
            {
                int max = 7;
                while ((i >> max) > 0x7FU) max += 7;
                while (max > 0)
                {
                    s.WriteByte((byte)(((i >> max) & 0x7FU) | 0x80));
                    max -= 7;
                }
            }
            s.WriteByte((byte)(i & 0x7FU));
        }

        public static void WriteLE(this Stream s, ushort i) => s.WriteUInt16LE(i);
        public static void WriteLE(this Stream s, uint i) => s.WriteUInt32LE(i);
        public static void WriteLE(this Stream s, ulong i) => s.WriteUInt64LE(i);
        public static void WriteLE(this Stream s, short i) => s.WriteInt16LE(i);
        public static void WriteLE(this Stream s, int i) => s.WriteInt32LE(i);
        public static void WriteLE(this Stream s, long i) => s.WriteInt64LE(i);
        public static ushort FlipEndian(this ushort i) => (ushort)((i & 0xFFU) << 8 | (i & 0xFF00U) >> 8);
        public static uint FlipEndian(this uint i) => (i & 0x000000FFU) << 24 | (i & 0x0000FF00U) << 8 |
                                                      (i & 0x00FF0000U) >> 8 | (i & 0xFF000000U) >> 24;
        public static ulong FlipEndian(this ulong i) => (i & 0x00000000000000FFUL) << 56 | (i & 0x000000000000FF00UL) << 40 |
                                                         (i & 0x0000000000FF0000UL) << 24 | (i & 0x00000000FF000000UL) << 8 |
                                                         (i & 0x000000FF00000000UL) >> 8 | (i & 0x0000FF0000000000UL) >> 24 |
                                                         (i & 0x00FF000000000000UL) >> 40 | (i & 0xFF00000000000000UL) >> 56;
        public static short FlipEndian(this short i) => unchecked((short)((ushort)i).FlipEndian());
        public static int FlipEndian(this int i) => unchecked((int)((uint)i).FlipEndian());
        public static long FlipEndian(this long i) => unchecked((long)((ulong)i).FlipEndian());
        public static void WriteBE(this Stream s, ushort i) => s.WriteUInt16LE(i.FlipEndian());
        public static void WriteBE(this Stream s, uint i) => s.WriteUInt32LE(i.FlipEndian());
        public static void WriteBE(this Stream s, ulong i) => s.WriteUInt64LE(i.FlipEndian());
        public static void WriteBE(this Stream s, short i) => s.WriteInt16LE(i.FlipEndian());
        public static void WriteBE(this Stream s, int i) => s.WriteInt32LE(i.FlipEndian());
        public static void WriteBE(this Stream s, long i) => s.WriteInt64LE(i.FlipEndian());

    }
}

namespace Nautilus.Texture
{
    public class Texture
    {
        public class Mipmap
        {
            public int Width;
            public int Height;
            public int Flags;
            public byte[] Data;
        }
        public int Version;
        public Mipmap[] Mipmaps;
        public byte[] HeaderData;
        public byte[] FooterData;
    }
}

namespace Nautilus.Texture
{
    public class TextureReader : ReaderBase<Texture>
    {
        public static Texture ReadStream(Stream s)
        {
            return new TextureReader(s).Read();
        }
        public TextureReader(Stream s) : base(s) { }

        public override Texture Read()
        {
            var magic = Int();
            if (magic != 6 && magic != 4 && magic != 3)
            {
                throw new Exception($"Unknown texture magic {magic}");
            }
            var version = Int();
            var hdrData = magic == 6 ? FixedArr(Byte, version == 0xC ? 0x7Cu : 0xACu)
              : magic == 3 ? FixedArr(Byte, 0x8Cu)
              : FixedArr(Byte, 0xA4u);
            var MipmapLevels = UInt();
            var Mipmaps = FixedArr(() => new Texture.Mipmap
            {
                Width = Int(),
                Height = Int(),
                Flags = version == 0xC ? Int().Then(Skip(8)) : Int()
            }, MipmapLevels);
            UInt();
            for (var i = 0; i < Mipmaps.Length; i++)
            {
                if (magic == 3)
                {
                    Mipmaps[i].Data = FixedArr(Byte, (uint)(Mipmaps[i].Width * Mipmaps[i].Height) / 2);
                }
                else
                {
                    Mipmaps[i].Data = Arr(Byte);
                }
            }
            var footerData = FixedArr(Byte, 0x1C);
            return new Texture
            {
                Version = magic,
                Mipmaps = Mipmaps,
                HeaderData = hdrData,
                FooterData = footerData
            };
        }
    }
}

namespace Nautilus.Texture
{
    public class TextureWriter : WriterBase<Texture>
    {
        public static void WriteStream(Texture r, Stream s)
        {
            new TextureWriter(s).WriteStream(r);
        }
        private TextureWriter(Stream s) : base(s) { }
        public override void WriteStream(Texture r)
        {
            Write(r.Version);
            s.Write(r.HeaderData, 0, r.HeaderData.Length);
            Write(r.Mipmaps, level =>
            {
                Write(level.Width);
                Write(level.Height);
                Write(level.Flags);
            });
            Write(6);
            foreach (var map in r.Mipmaps)
            {
                Write(map.Data.Length);
                s.Write(map.Data, 0, map.Data.Length);
            }
            s.Write(r.FooterData, 0, r.FooterData.Length);
        }
    }
}

namespace Nautilus.Util
{
    static class ReaderExtensions
    {
        // For doing side effects else fluently in an expression
        public static T Then<T>(this T v, Action a)
        {
            a();
            return v;
        }
        public static T Then<T>(this T v, Action<T> a)
        {
            a(v);
            return v;
        }
    }
    public abstract class ReaderBase<D> : BinReader
    {
        public ReaderBase(System.IO.Stream s) : base(s) { }
        public virtual D Read() => (D)Read(typeof(D));
    }
}

namespace Nautilus.Util
{
    public class BinReader
    {
        protected Stream s;
        public BinReader(Stream stream)
        {
            s = stream;
        }
        public T Object<T>()
        {
            T ret = default;
            foreach (var field in typeof(T).GetFields())
            {
                var type = field.FieldType;
                field.SetValue(ret, Read(field.FieldType));
            }
            return default;
        }
        public object Read(Type t)
        {
            var readers =
            new Dictionary<Type, Func<object>> {
        { typeof(int), () => Int() },
        { typeof(uint), () => UInt() },
        { typeof(long), () => Long() },
        { typeof(ulong), () => ULong() },
        { typeof(float), () => Float() },
        { typeof(short), () => Short() },
        { typeof(ushort), () => UShort() },
        { typeof(byte), () => Byte() },
        { typeof(string), () => String() },
        { typeof(bool), () => Bool() },
            };
            if (readers.ContainsKey(t))
                return readers[t]();
            var method = GetType().GetMethod(nameof(Object)).MakeGenericMethod(t);
            return method.Invoke(this, new object[] { });
        }
        public static Func<T> Seq<T>(Action a, Func<T> v)
        {
            return () =>
            {
                a();
                return v();
            };
        }
        public T Check<T>(T v, T expected, string where = null)
        {
            if (v.Equals(expected))
                return v;
            throw new Exception($"Invalid data encountered at {s.Position:X} {where}: expected {expected}, got {v}");
        }

        public byte CheckRange(byte v, byte minimum, byte maximum)
        {
            if (minimum <= v && maximum >= v)
                return v;
            throw new Exception($"Range of {minimum} -> {maximum} exceeded at {s.Position:X}: got {v} ");
        }
        public int CheckRange(int v, int minimum, int maximum)
        {
            if (minimum <= v && maximum >= v)
                return v;
            throw new Exception($"Range of {minimum} -> {maximum} exceeded at {s.Position:X}: got {v} ");
        }
        // For reading a fixed size array of something
        public T[] FixedArr<T>(Func<T> constructor, uint size)
        {
            if (size > s.Length - s.Position)
            {
                throw new Exception($"Invalid array size {size:X} encountered at {s.Position:X}. File is corrupt or not understood.");
            }
            var arr = new T[size];
            for (var i = 0; i < size; i++)
                arr[i] = constructor();

            return arr;
        }
        // For reading a length-prefixed array of something
        public T[] Arr<T>(Func<T> constructor, uint maxSize = 0)
        {
            var size = UInt();
            if (maxSize != 0 && size > maxSize)
                throw new Exception($"Array was too big ({size} > {maxSize}) at {s.Position:X}");
            return FixedArr(constructor, size);
        }
        public T[] CheckedArr<T>(Func<T> constructor, uint size)
        {
            var fileSize = UInt();
            if (fileSize != size)
                throw new Exception($"Invalid array size ({fileSize} != {size}) at {s.Position:X}");
            return FixedArr(constructor, size);
        }
        // For skipping unknown data
        public Action Skip(int count) => () => s.Position += count;
        // For reading simple types
        public int Int() => s.ReadInt32LE();
        public uint UInt() => s.ReadUInt32LE();
        public long Long() => s.ReadInt64LE();
        public ulong ULong() => s.ReadUInt64LE();
        public float Half() => s.ReadHalfFloat();
        public float Float() => s.ReadFloat();
        public short Short() => s.ReadInt16LE();
        public ushort UShort() => s.ReadUInt16LE();
        public byte Byte() => (byte)s.ReadByte();
        public string String() => s.ReadLengthPrefixedString(Encoding.UTF8);
        public string String(int length) => s.ReadFixedLengthNullTerminatedString(length);
        public string FixedString(int length) => s.ReadFixedLengthString(length);
        public string UE4String() => String(Int());
        public uint UInt24() => s.ReadUInt24LE();
        /// <summary>
        /// Reads a byte as a boolean, throwing if it's not 1 or 0
        /// </summary>
        public bool Bool() => CheckRange(Byte(), (byte)0, (byte)1) != 0;
    }
}

namespace Nautilus.Util
{
    class EncryptedReadStream : Stream
    {
        private long position;
        private int key;
        private int curKey;
        private long keypos;
        private Stream file;
        public byte xor;

        internal EncryptedReadStream(Stream file, byte xor = 0)
        {
            file.Position = 0;
            // The initial key is found in the first 4 bytes.
            this.key = cryptRound(file.ReadInt32LE());
            this.position = 0;
            this.keypos = 0;
            this.curKey = this.key;
            this.file = file;
            this.Length = file.Length - 4;
            this.xor = xor;
        }

        public override bool CanRead => position < Length && position >= 0;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length { get; }

        public override long Position
        {
            get
            {
                return position;
            }

            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        private void updateKey()
        {
            if (keypos == position)
                return;
            if (keypos > position) // reset key
            {
                keypos = 0;
                curKey = key;
            }
            while (keypos < position) // don't think there's a faster way to do this
            {
                curKey = cryptRound(curKey);
                keypos++;
            }
        }

        private int cryptRound(int key)
        {
            int ret = (key - ((key / 0x1F31D) * 0x1F31D)) * 0x41A7 - (key / 0x1F31D) * 0xB14;
            if (ret <= 0)
                ret += 0x7FFFFFFF;
            return ret;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            // ensure file is at correct offset
            file.Seek(this.position + 4, SeekOrigin.Begin);
            if (offset + count > buffer.Length)
            {
                throw new IndexOutOfRangeException("Attempt to fill buffer past its end");
            }
            if (this.Position == this.Length || this.Position + count > this.Length)
            {
                count = (int)(this.Length - this.Position);
                //throw new System.IO.EndOfStreamException("Cannot read past end of file.");
            }

            int bytesRead = file.Read(buffer, offset, count);

            for (uint i = 0; i < bytesRead; i++)
            {
                buffer[offset + i] ^= (byte)(this.curKey ^ xor);
                this.position++;
                updateKey();
            }
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            int adjust = origin == SeekOrigin.Current ? 0 : 4;
            this.position = file.Seek(offset + adjust, origin) - 4;
            updateKey();
            return position;
        }

        #region Not Used

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

namespace Nautilus.Util
{
    public abstract class WriterBase<D> : BinWriter
    {
        public WriterBase(System.IO.Stream s) : base(s) { }
        public abstract void WriteStream(D v);
    }
}

namespace Nautilus.Util
{
    public class BinWriter
    {
        protected Stream s;
        public BinWriter(Stream s)
        {
            this.s = s;
        }
        public void Write(byte v) => s.WriteByte(v);
        public void Write(short v) => s.WriteInt16LE(v);
        public void Write(ushort v) => s.WriteUInt16LE(v);
        public void Write(int v) => s.WriteInt32LE(v);
        public void Write(uint v) => s.WriteUInt32LE(v);
        public void Write(long v) => s.WriteInt64LE(v);
        public void Write(ulong v) => s.WriteUInt64LE(v);
        public void Write(float v) => s.Write(BitConverter.GetBytes(v), 0, 4);
        public void Write(bool v) => s.WriteByte((byte)(v ? 1 : 0));
        public void Write(string v)
        {
            var bytes = Encoding.UTF8.GetBytes(v);
            s.WriteInt32LE(bytes.Length);
            s.Write(bytes, 0, bytes.Length);
        }
        public void Write(string v, int length)
        {
            var bytes = Encoding.UTF8.GetBytes(v);
            s.Write(bytes, 0, bytes.Length);
            s.WriteByte(0);
            s.Position += length - bytes.Length - 1;
        }
        public void Write<T>(T[] arr, Action<T> writer)
        {
            // Treat uninitialized arrays as empty ones
            if (arr == null)
            {
                Write(0);
                return;
            }
            Write(arr.Length);
            foreach (var x in arr)
            {
                writer(x);
            }
        }
    }
}