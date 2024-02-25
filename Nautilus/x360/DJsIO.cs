using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Nautilus.x360;
using Microsoft.Win32.SafeHandles;

#region Extensions

namespace Nautilus.x360
{
    /// <summary>
    /// Class to hold IO extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns the count of blocks this file contains
        /// </summary>
        public static uint BlockCountSTFS(this DJsIO y)
        {
            if (y.IOType == DataType.Drive)
                return 0;
            if (y.Length == 0)
                return 0;
            return (uint)(((y.Length - 1) / 0x1000) + 1);
        }

        /// <summary>
        /// Returns the amount of data in the last block
        /// </summary>
        public static int BlockRemainderSTFS(this DJsIO y)
        {
            if (y.IOType == DataType.Drive)
                return 0;
            if (y.Length == 0)
                return 0;
            if (y.Length > 0xFFFFFFFF)
                return 0;
            //return (int)(y.Length % 0x1000);
            return (int)(((y.Length - 1) % 0x1000) + 1);
        }
    }
}

namespace X360.IO.SearchExtensions
{
    /// <summary>
    /// Class to hold IO extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Searches the stream for a set of bytes
        /// </summary>
        /// <param name="y"></param>
        /// <param name="xData"></param>
        /// <param name="xStopWhenFound"></param>
        /// <returns></returns>
        public static long[] SearchBinary(this DJsIO y, byte[] xData, bool xStopWhenFound)
        {
            var xReturn = new List<long>();
            for (var i = y.Position; i < y.Length; i++)
            {
                if (i + xData.Length > y.Length)
                    break;
                y.Position = i;
                var xbuff = y.ReadBytes(xData.Length);
                if (BitConverter.ToString(xData) != BitConverter.ToString(xbuff)) continue;
                xReturn.Add(i);
                if (xStopWhenFound)
                    break;
            }
            return xReturn.ToArray();
        }

        /// <summary>
        /// Searching the stream for an ASCII string
        /// </summary>
        /// <param name="y"></param>
        /// <param name="xData"></param>
        /// <param name="xStopWhenFound"></param>
        /// <returns></returns>
        public static long[] SearchASCII(this DJsIO y, string xData, bool xStopWhenFound)
        {
            var xbuff = Encoding.ASCII.GetBytes(xData);
            return SearchBinary(y, xbuff, xStopWhenFound);
        }

        /// <summary>
        /// Searches the file for a Unicode string, Endian based on stream's Endian
        /// </summary>
        /// <param name="y"></param>
        /// <param name="xData"></param>
        /// <param name="xStopWhenFound"></param>
        /// <returns></returns>
        public static long[] SearchUnicode(this DJsIO y, string xData, bool xStopWhenFound)
        {
            var xbuff = y.IsBigEndian ? Encoding.BigEndianUnicode.GetBytes(xData) : Encoding.Unicode.GetBytes(xData);
            return SearchBinary(y, xbuff, xStopWhenFound);
        }
    }
}

namespace X360.IO.ExtraExtensions
{
    /// <summary>
    /// Class to hold IO extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Attempts to return an Image from this stream
        /// </summary>
        /// <returns></returns>
        public static Image ImageFromStream(this DJsIO y)
        {
            return Image.FromStream(y.xStream);
        }

        /// <summary>
        /// Inserts bytes as needed
        /// </summary>
        /// <param name="xLocation"></param>
        /// <param name="xAddSize"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool InsertBlankData(this DJsIO y, long xLocation, int xAddSize)
        {
            if (y.IOType == DataType.Drive)
                return false;
            try
            {
                var xLOBefore = y.Length;
                var xShiftCt = xLOBefore - xLocation;
                for (var i = 0; i < xShiftCt; i++)
                {
                    y.Position = xLOBefore - i - 1;
                    var x = y.ReadByte();
                    y.Position = (xShiftCt - i) + xLocation + xAddSize;
                    y.Write(x);
                }
                y.Position = xLocation;
                for (var i = 0; i < xAddSize; i++)
                    y.Write((byte)0);
                return true;
            }
            catch { return false; }
        }
    }
}
#endregion

namespace Nautilus.x360
{
    #region Enums

    /// <summary>
    /// String type
    /// </summary>
    public enum StringForm : byte
    {
        /// <summary>
        /// ACSII String
        /// </summary>
        ASCII = 1,
        /// <summary>
        /// Unicode String
        /// </summary>
        Unicode = 2
    }

    /// <summary>
    /// File Type
    /// </summary>
    public enum DJFileMode : byte
    {
        /// <summary>
        /// Create a file
        /// </summary>
        Create,
        /// <summary>
        /// Open a file
        /// </summary>
        Open
    }

    /// <summary>
    /// Pad direction
    /// </summary>
    public enum PadLocale : byte
    {
        /// <summary>
        /// Pad to the left
        /// </summary>
        Left,
        /// <summary>
        /// Pad to the right
        /// </summary>
        Right
    }

    /// <summary>
    /// Generic pad types
    /// </summary>
    public static class PadType
    {
        /// <summary>
        /// Null pad
        /// </summary>
        public const char Null = (char)0;
        /// <summary>
        /// Space pad
        /// </summary>
        public const char Space = (char)0x20;
    }

    /// <summary>
    /// How to read  a string
    /// </summary>
    public enum StringRead : byte
    {
        /// <summary>
        /// Defined length
        /// </summary>
        Defined,
        /// <summary>
        /// Read to null
        /// </summary>
        ToNull,
        /// <summary>
        /// Reads a string with a byte size in front of it
        /// </summary>
        PrecedingLength
    }

    /// <summary>
    /// IO Type
    /// </summary>
    public enum DataType : byte
    {
        /// <summary>
        /// No specific type
        /// </summary>
        None,
        /// <summary>
        /// Memory IO
        /// </summary>
        Memory,
        /// <summary>
        /// File IO
        /// </summary>
        File,
        /// <summary>
        /// Device IO
        /// </summary>
        Drive,
        /// <summary>
        /// 
        /// </summary>
        Real,
        /// <summary>
        /// Contains multiple IO's
        /// </summary>
        MultiFile,
        /// <summary>
        /// Some other IO
        /// </summary>
        Other
    }

    enum RealType : byte { None, STFS, FATX, SVOD }
    #endregion

    /// <summary>
    /// IO Exceptions
    /// </summary>
    [DebuggerStepThrough]
    public static class IOExcepts
    {
        [CompilerGenerated]
        static readonly Exception xAccessError = new Exception("Underlining source not accessed");
        [CompilerGenerated]
        static readonly Exception xDirectoryErr = new Exception("Invalid Directory index");
        [CompilerGenerated]
        static readonly Exception xPosition = new Exception("Cannot move position past file size");
        [CompilerGenerated]
        static readonly Exception xIndex = new Exception("Out of bounds indexer");
        [CompilerGenerated]
        static readonly Exception xNoExist = new Exception("Path does not exist");
        [CompilerGenerated]
        static readonly Exception xCreateError = new Exception("Unable to create path or file");
        [CompilerGenerated]
        static readonly Exception xMultiAccessError = new Exception("Unable to access a file");

        /// <summary>
        /// Not accessed
        /// </summary>
        public static Exception AccessError { get { return xAccessError; } }
        /// <summary>
        /// Invalid directory index
        /// </summary>
        public static Exception DirectError { get { return xDirectoryErr; } }
        /// <summary>
        /// Position error
        /// </summary>
        public static Exception PositionError { get { return xPosition; } }
        /// <summary>
        /// Index out of bounds
        /// </summary>
        public static Exception Index { get { return xIndex; } }
        /// <summary>
        /// Path unexistant
        /// </summary>
        public static Exception DoesntExist { get { return xNoExist; } }
        /// <summary>
        /// Cannot create path or file
        /// </summary>
        public static Exception CreateError { get { return xCreateError; } }
        /// <summary>
        /// Returns this when one of the files in a multifile IO could not be accessed
        /// </summary>
        public static Exception MultiAccessError { get { return xMultiAccessError; } }
    }

    /// <summary>
    /// Object for generic IO
    /// </summary>
    [DebuggerStepThrough]
    public class DJsIO
    {
        #region Variables
        /// <summary>
        /// Bool if the Stream is properly accessed
        /// </summary>
        public bool Accessed { get { return GetAccessed(); } }
        /// <summary>
        /// The name of the actual file
        /// </summary>
        public string FileNameShort
        {
            get
            {
                if (txtidx == null || txtidx[0] == -1)
                    return null;
                return xFile.Substring(txtidx[0] + 1);
            }
        }
        /// <summary>
        /// The file path of this file
        /// </summary>
        public string FilePath
        {
            get
            {
                if (txtidx == null || txtidx[0] == -1)
                    return null;
                return xFile.Substring(0, (txtidx[0] + 1));
            }
        }
        /// <summary>
        /// Returns the full file location (applicable only to File Stream)
        /// </summary>
        public string FileNameLong { get { return xFile; } }
        /// <summary>
        /// The Extension of the File (File Stream only)
        /// </summary>
        public string FileExtension
        {
            get
            {
                if (txtidx == null || txtidx[0] == -1)
                    return null;
                return txtidx[1] < txtidx[0] ? "" : xFile.Substring(txtidx[1] + 1);
            }
        }
        /// <summary>
        /// Select what Endian to Read/Write in
        /// </summary>
        [CompilerGenerated]
        public bool IsBigEndian { get; set; }
        /// <summary>
        /// The type of this instance
        /// </summary>
        [CompilerGenerated]
        public DataType IOType { get { return xThisData; } }
        /// <summary>
        /// This type of IO
        /// </summary>
        [CompilerGenerated]
        protected DataType xThisData = DataType.None;
        /// <summary>
        /// The file name of which the stream is referenced to
        /// </summary>
        [CompilerGenerated]
        protected internal string xFile = "Null"; // Long file location (File Stream applicable only)
        /// <summary>
        /// The stream of the instance
        /// </summary>
        [CompilerGenerated]
        protected internal Stream xStream = null; // Stream
        [CompilerGenerated]
        int[] txtidx;
        #endregion

        #region StreamSetting
        /// <summary>
        /// Sets the strings of a file like string
        /// </summary>
        protected void XSetStrings()
        {
            if (xThisData != DataType.File)
                return;
            txtidx = new int[2];
            txtidx[0] = xFile.LastIndexOfAny(new[] { '/', '\\' });
            txtidx[1] = xFile.LastIndexOf('.');
        }

        // Sets the File Stream (if applicable)
        void XSetStream(DJFileMode xftype)
        {
            try
            {
                xStream = (xftype == DJFileMode.Create) ? File.Create(xFile) :
                    new FileStream(xFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                xThisData = DataType.File;
                XSetStrings();
            }
            catch (Exception) { xStream = null; throw; }
        }

        /// <summary>
        /// Create instance of IO directly to a file with file mode type
        /// </summary>
        /// <param name="xFileIn"></param>
        /// <param name="xType"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(string xFileIn, DJFileMode xType, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            xFile = xFileIn;
            XSetStream(xType);
        }

        /// <summary>
        /// Create instance of IO allowing the user to choose open (xSave = false)
        /// or save (xSave = true) location. xTitle and xFilter are the normal
        /// open/save Title
        /// </summary>
        /// <param name="BigEndian"></param>
        /// <param name="xTitle"></param>
        /// <param name="xFilter"></param>
        /// <param name="xType"></param>
        public DJsIO(DJFileMode xType, string xTitle, string xFilter, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            xTitle = xTitle.Replace("\0", "");
            xFilter = xFilter.Replace("\0", "");
            if (xType == DJFileMode.Open)
            {
                var xOFD = new OpenFileDialog { Title = xTitle, Filter = xFilter };
                xImp2(ref xOFD);
            }
            else
            {
                var xSFD = new SaveFileDialog { Title = xTitle, Filter = xFilter };
                xImp1(ref xSFD);
            }

        }

        void xImp1(ref SaveFileDialog xSFD)
        {
            if (xSFD.ShowDialog() != DialogResult.OK)
                return;
            xSFD.AddExtension = true;
            xFile = xSFD.FileName;
            XSetStream(DJFileMode.Create);
        }

        void xImp2(ref OpenFileDialog xOFD)
        {
            if (xOFD.ShowDialog() != DialogResult.OK)
                return;
            xOFD.AddExtension = true;
            xFile = xOFD.FileName;
            XSetStream(DJFileMode.Open);
        }

        /// <summary>
        /// Use your own Save File Dialog
        /// </summary>
        /// <param name="xSFDin"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(ref SaveFileDialog xSFDin, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            xImp1(ref xSFDin);
        }

        /// <summary>
        /// Use your own Open File Dialog
        /// </summary>
        /// <param name="xOFD"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(ref OpenFileDialog xOFD, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            xImp2(ref xOFD);
        }

        /// <summary>
        /// Imports a generic Stream
        /// </summary>
        /// <param name="ImportGeneric"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(Stream ImportGeneric, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            if (ImportGeneric.GetType() == typeof(FileStream))
            {
                xFile = ((FileStream)ImportGeneric).Name;
                XSetStrings();
                xThisData = DataType.File;
            }
            else xThisData = DataType.None;
            xStream = ImportGeneric;
        }

        /// <summary>
        /// Creates an instance of IO for byte array's.
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(byte[] xIn, bool BigEndian)
        {
            IsBigEndian = BigEndian;
            try
            {
                var xMS = new MemoryStream(xIn);
                xStream = xMS;
                xThisData = DataType.Memory;
            }
            catch
            {
                try
                {
                    xStream.Close();
                }
                catch (Exception)
                {
                }
                xStream = null;
            }
        }

        /// <summary>
        /// Creates a stream on a byte array to a specified size
        /// </summary>
        /// <param name="ArraySize"></param>
        /// <param name="BigEndian"></param>
        public DJsIO(long ArraySize, bool BigEndian)
        {
            try
            {
                IsBigEndian = BigEndian;
                var Buffer = new byte[ArraySize];
                var xMS = new MemoryStream(Buffer);
                xStream = xMS;
                xThisData = DataType.Memory;
            }
            catch
            {
                try { xStream.Close(); }
                catch (Exception)
                {
                }
                xStream = null;
            }
        }

        /// <summary>
        /// Makes a temporary file stream
        /// </summary>
        /// <param name="BigEndian"></param>
        public DJsIO(bool BigEndian)
        {
            IsBigEndian = BigEndian;
            xFile = string.Copy(VariousFunctions.GetTempFileLocale());
            XSetStream(DJFileMode.Create);
        }

        /// <summary>
        /// Default ref
        /// </summary>
        protected DJsIO() { }
        #endregion

        #region Reading
        /// <summary>
        /// Read a set amount of bytes
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public virtual byte[] ReadBytes(int xSize)
        {
            if (Position >= Length)
                throw IOExcepts.PositionError;
            if (xSize == 0)
                return new byte[] { };
            var xbuff = new byte[xSize];
            xStream.Read(xbuff, 0, xSize);
            return xbuff;
        }

        internal byte[] unbufferedread(int xSize)
        {
            var xbuff = new byte[xSize];
            xStream.Read(xbuff, 0, xSize);
            return xbuff;
        }

        /// <summary>
        /// Reads a signed 16-bit (2 bytes) integer
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            return ReadInt16(IsBigEndian);
        }

        /// <summary>
        /// Reads a signed 16-bit (2 bytes) integer in specified endian 
        /// </summary>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public short ReadInt16(bool BigEndian)
        {
            var buff = ReadBytes(2);
            return BitConv.ToInt16(buff, BigEndian);
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 24-bit integer (3 bytes)
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt24()
        {
            return ReadUInt24(IsBigEndian);
        }

        /// <summary>
        /// Reads an unsigned 24-bit (3 byte) integer in specified mode
        /// </summary>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public uint ReadUInt24(bool BigEndian)
        {
            var xData = ReadBytes(3);
            if (BigEndian)
                xData.EndianConvert();
            return ((uint)xData[2] << 16 | (uint)xData[1] << 8 |
                    xData[0]);
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 40-bit integer (5 bytes)
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt40()
        {
            return ReadUInt40(IsBigEndian);
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 40-bit integer (5 bytes) in a specified Endian
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt40(bool BigEndian)
        {
            var xData = ReadBytes(5);
            if (BigEndian)
                xData.EndianConvert();
            return ((ulong)xData[4] << 32 | (ulong)xData[3] << 24 |
                    (ulong)xData[2] << 16 | (ulong)xData[1] << 8 |
                    xData[0]);
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 48-bit integer (6 bytes)
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt48()
        {
            return ReadUInt48(IsBigEndian);
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 48-bit integer (6 bytes)
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt48(bool BigEndian)
        {
            var xData = ReadBytes(6);
            if (BigEndian)
                xData.EndianConvert();
            return ((ulong)xData[5] << 40 | (ulong)xData[4] << 32 |
                    (ulong)xData[3] << 24 | (ulong)xData[2] << 16 |
                    (ulong)xData[1] << 8 | xData[0]);
        }

        /* See note at bottom */
        /// <summary>
        /// Reads a 56-bit integer (7 bytes)
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt56()
        {
            return ReadUInt56(IsBigEndian);
        }

        /// <summary>
        /// Reads an unsinged 56 bit Int in specified mode
        /// </summary>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public ulong ReadUInt56(bool BigEndian)
        {
            var xData = ReadBytes(7);
            if (BigEndian)
                xData.EndianConvert();
            return ((ulong)xData[6] << 48 | (ulong)xData[5] << 40 |
                    (ulong)xData[4] << 32 | (ulong)xData[3] << 24 |
                    (ulong)xData[2] << 16 | (ulong)xData[1] << 8 |
                    xData[0]);
        }

        /// <summary>
        /// Reads a 32-bit (4 bytes) signed integer
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            return ReadInt32(IsBigEndian);
        }

        /// <summary>
        /// Read a 32-bit (4 bytes) signed integer to a specified endian
        /// </summary>
        /// <returns></returns>
        public int ReadInt32(bool BigEndian)
        {
            var buff = ReadBytes(4);
            return BitConv.ToInt32(buff, BigEndian);
        }

        /// <summary>
        /// Reads a 64-bit (8 bytes) integer
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            return ReadInt64(IsBigEndian);
        }

        /// <summary>
        /// Reads a 64-bit (8 bytes) integer to a specified endian
        /// </summary>
        /// <returns></returns>
        public long ReadInt64(bool BigEndian)
        {
            var buff = ReadBytes(8);
            return BitConv.ToInt64(buff, BigEndian);
        }

        /// <summary>
        /// Reads a 8-bit (1 byte) integer
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return ReadBytes(1)[0];
        }

        /// <summary>
        /// Reads a Signed 8 bit (1 byte) integer
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            return (sbyte)ReadBytes(1)[0];
        }

        /// <summary>
        /// Reads a Single
        /// </summary>
        /// <returns></returns>
        public float ReadSingle()
        {
            return ReadSingle(IsBigEndian);
        }

        /// <summary>
        /// Reads a Single in a specified endian
        /// </summary>
        /// <returns></returns>
        public float ReadSingle(bool BigEndian)
        {
            var buff = ReadBytes(4);
            return BitConv.ToSingle(buff, BigEndian);
        }

        /// <summary>
        /// Reads a Double
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            return ReadDouble(IsBigEndian);
        }

        /// <summary>
        /// Reads a Double in a specified endian
        /// </summary>
        /// <returns></returns>
        public double ReadDouble(bool BigEndian)
        {
            var buff = ReadBytes(8);
            return BitConv.ToDouble(buff, BigEndian);
        }

        /// <summary>
        /// Reads a 16-bit (2 bytes) integer
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            return ReadUInt16(IsBigEndian);
        }

        /// <summary>
        /// Reads an unsigned 16-bit (2 bytes) integer in a specified endian
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16(bool BigEndian)
        {
            var buff = ReadBytes(2);
            return BitConv.ToUInt16(buff, BigEndian);
        }

        /// <summary>
        /// Reads an unsigned 32-bit (4 bytes) integer
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            return ReadUInt32(IsBigEndian);
        }

        /// <summary>
        /// Reads an unsigned 32-bit (4 bytes) integer to a specified endian
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32(bool BigEndian)
        {
            var buff = ReadBytes(4);
            return BitConv.ToUInt32(buff, BigEndian);
        }

        /// <summary>
        /// Reads an unsigned 64-bit (8 bytes) integer
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            return ReadUInt64(IsBigEndian);
        }

        /// <summary>
        /// Reads an unsigned 64-bit (8 bytes) integer to a specified endian
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64(bool BigEndian)
        {
            var buff = ReadBytes(8);
            return BitConv.ToUInt64(buff, BigEndian);
        }

        /// <summary>
        /// Reads a bit bool (0 no, 1 yes)
        /// </summary>
        /// <returns></returns>
        public bool ReadBool()
        {
            return (ReadByte() & 1) == 1;
        }

        /// <summary>
        /// Reads an ASCII line
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            return ReadLine(StringForm.ASCII, 0xA, IsBigEndian);
        }

        /// <summary>
        /// Reads an ASCII line to a specified line break
        /// </summary>
        /// <param name="BreakType"></param>
        /// <returns></returns>
        public string ReadLine(byte BreakType)
        {
            return ReadLine(StringForm.ASCII, BreakType, IsBigEndian);
        }

        /// <summary>
        /// Reads a line from a specified string format
        /// </summary>
        /// <param name="xType"></param>
        /// <returns></returns>
        public string ReadLine(StringForm xType)
        {
            return ReadLine(xType, 0xA, IsBigEndian);
        }

        /// <summary>
        /// Reads a line of a specific string format and endian
        /// </summary>
        /// <param name="xType"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadLine(StringForm xType, bool BigEndian)
        {
            return ReadLine(xType, 0xA, BigEndian);
        }

        /// <summary>
        /// Reads a line from a specific string format, line break, and endian
        /// </summary>
        /// <param name="xType"></param>
        /// <param name="BreakIndicator"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadLine(StringForm xType, short BreakIndicator, bool BigEndian)
        {
            var buffer = new List<byte>();
            if (xType == StringForm.Unicode)
            {
                if (Position >= Length - 1)
                    return "";
                var buff = ReadBytes(2);
                while (buff[0] != BreakIndicator)
                {
                    try
                    {
                        buffer.AddRange(buff);
                        buff = ReadBytes(2);
                    }
                    catch { break; }
                }
                return BigEndian ? Encoding.BigEndianUnicode.GetString(buffer.ToArray()) : Encoding.Unicode.GetString(buffer.ToArray());
            }
            else
            {
                if (Position >= Length)
                    return "";
                var buff = ReadByte();
                while (buff != (byte)BreakIndicator)
                {
                    try
                    {
                        buffer.Add(buff);
                        buff = ReadByte();
                    }
                    catch { break; }
                }
                return Encoding.ASCII.GetString(buffer.ToArray()).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "");
            }
        }

        /// <summary>
        /// Reads a ToNull String
        /// </summary>
        /// <param name="xStringType"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType)
        {
            return ReadString(xStringType, 0, StringRead.ToNull, IsBigEndian);
        }

        /// <summary>
        /// Reads a string to null wif specified endian
        /// </summary>
        /// <param name="xStringType"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, bool BigEndian)
        {
            return ReadString(xStringType, 0, StringRead.ToNull, BigEndian);
        }

        /// <summary>
        /// Reads a string
        /// </summary>
        /// <param name="xStringSize"></param>
        /// <param name="xStringType"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, int xStringSize)
        {
            return ReadString(xStringType, xStringSize, StringRead.Defined, IsBigEndian);
        }

        /// <summary>
        /// Reads a string
        /// </summary>
        /// <param name="xStringSize"></param>
        /// <param name="xStringType"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, int xStringSize, bool BigEndian)
        {
            return ReadString(xStringType, xStringSize, StringRead.Defined, BigEndian);
        }

        /// <summary>
        /// Reads a string with special circumstance capability
        /// </summary>
        /// <param name="xStringSize"></param>
        /// <param name="xStringType"></param>
        /// <param name="xRead"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, int xStringSize, StringRead xRead)
        {
            return ReadString(xStringType, xStringSize, xRead, IsBigEndian);
        }

        /// <summary>
        /// Reads a string with special circumstance capability
        /// </summary>
        /// <param name="xStringSize"></param>
        /// <param name="xStringType"></param>
        /// <param name="xRead"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public string ReadString(StringForm xStringType, int xStringSize, StringRead xRead, bool BigEndian)
        {
            if (!Enum.IsDefined(typeof(StringForm), xStringType) || !Enum.IsDefined(typeof(StringRead), xRead))
                throw new Exception("Invalid parameters");
            if (xRead == StringRead.ToNull)
            {
                if (Position >= Length - 1)
                    return "";
                var buffer = new List<byte>();
                if (xStringType == StringForm.Unicode)
                {
                    var buff = ReadBytes(2);
                    const int i = 0;
                    while (xStringSize == 0 || i < xStringSize)
                    {
                        if (buff[0] == 0 && buff[1] == 0)
                            break;
                        try
                        {
                            buffer.AddRange(buff);
                            buff = ReadBytes(2);
                        }
                        catch { break; }
                    }
                    return BigEndian ? Encoding.BigEndianUnicode.GetString(buffer.ToArray()).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "") : Encoding.Unicode.GetString(buffer.ToArray()).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "");
                }
                else
                {
                    if (Position >= Length)
                        return "";
                    var buff = ReadByte();
                    var i = 0;
                    while (xStringSize == 0 || i < xStringSize)
                    {
                        if (buff == 0)
                            break;
                        try
                        {
                            buffer.Add(buff);
                            buff = ReadByte();
                        }
                        catch { break; }
                        i++;
                    }
                    return Encoding.ASCII.GetString(buffer.ToArray()).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "");
                }
            }
            if (xRead == StringRead.PrecedingLength)
            {
                var Buffer = new List<byte>();
                var len = ReadByte();
                for (var i = 0; i < len; i++)
                {
                    if (xStringType == StringForm.ASCII)
                        Buffer.AddRange(ReadBytes(2));
                    else Buffer.Add(ReadByte());
                }
                if (xStringType == StringForm.ASCII)
                    return Encoding.ASCII.GetString(Buffer.ToArray()).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "");
                return BigEndian ? Encoding.BigEndianUnicode.GetString(Buffer.ToArray()).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "") : Encoding.Unicode.GetString(Buffer.ToArray()).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "");
            }
            var buff2 = ReadBytes((byte)xStringType * xStringSize);
            if (xStringType == StringForm.ASCII)
                return Encoding.ASCII.GetString(buff2).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "");
            return BigEndian ? Encoding.BigEndianUnicode.GetString(buff2).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "") : Encoding.Unicode.GetString(buff2).Replace(PadType.Null.ToString(CultureInfo.InvariantCulture), "");
        }

        /// <summary>
        /// Reads bytes and returns it as a Hexadecimal String
        /// </summary>
        /// <param name="xLength"></param>
        /// <returns></returns>
        public string ReadHexString(int xLength)
        {
            return ReadBytes(xLength).HexString();
        }

        /// <summary>
        /// Reads a File Time Stamp (8 bytes for time)
        /// </summary>
        /// <returns></returns>
        public DateTime ReadFileTime()
        {
            return DateTime.FromFileTime(ReadInt64());
        }

        /// <summary>
        /// WARNING: Only use on small files, will throw Exception if not enough memory
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ReadStream()
        {
            var xReturn = new byte[Length];
            var posbefore = Position;
            Position = 0;
            for (long i = 0; i < xReturn.Length; i++)
                xReturn[i] = ReadByte();
            Position = posbefore;
            return xReturn;
        }
        #endregion

        #region Writing
        /// <summary>
        /// Writes a Byte Array
        /// </summary>
        /// <param name="xIn"></param>
        public virtual void Write(byte[] xIn)
        {
            if (xThisData == DataType.Real)
            {
                foreach (var x in xIn)
                    Write(x);
            }
            else if (xThisData != DataType.Drive)
                xStream.Write(xIn, 0, xIn.Length);
        }

        internal void unbufferedwrite(byte[] xIn)
        {
            xStream.Write(xIn, 0, xIn.Length);
        }

        /// <summary>
        /// Writes a signed 16-bit (2 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(short xIn)
        {
            Write(BitConv.GetBytes(xIn, IsBigEndian));
        }

        /// <summary>
        /// Writes a short in specified Endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(short xIn, bool BigEndian)
        {
            Write(BitConv.GetBytes(xIn, BigEndian));
        }

        /// <summary>
        /// Writes a signed 32-bit (4 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(int xIn)
        {
            Write(BitConv.GetBytes(xIn, IsBigEndian));
        }

        /// <summary>
        /// Writes a signed 32-bit (4 byte) integer to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(int xIn, bool BigEndian)
        {
            Write(BitConv.GetBytes(xIn, BigEndian));
        }

        /// <summary>
        /// Writes a signed 64-bit (8 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(long xIn)
        {
            Write(BitConv.GetBytes(xIn, IsBigEndian));
        }

        /// <summary>
        /// Writes a signed 64-bit (8 byte) integer to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(long xIn, bool BigEndian)
        {
            Write(BitConv.GetBytes(xIn, BigEndian));
        }

        /// <summary>
        /// Writes an unsigned 16-bit (2 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(ushort xIn)
        {
            Write(BitConv.GetBytes(xIn, IsBigEndian));
        }

        /// <summary>
        /// Write an unsigned 16-bit (2 bytes) integer in a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(ushort xIn, bool BigEndian)
        {
            Write(BitConv.GetBytes(xIn, BigEndian));
        }

        /// <summary>
        /// Write an unsigned 32-bit (4 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(uint xIn)
        {
            Write(BitConv.GetBytes(xIn, IsBigEndian));
        }

        /// <summary>
        /// Write an unsigned 32-bit (4 bytes) integer in a specifed endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(uint xIn, bool BigEndian)
        {
            Write(BitConv.GetBytes(xIn, BigEndian));
        }

        /// <summary>
        /// Writes an unsigned 64-bit (8 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(ulong xIn)
        {
            Write(BitConv.GetBytes(xIn, IsBigEndian));
        }

        /// <summary>
        /// Writes an unsigned 64-bit (8 bytes) integer to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(ulong xIn, bool BigEndian)
        {
            Write(BitConv.GetBytes(xIn, BigEndian));
        }

        /// <summary>
        /// Writes a single/float
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(float xIn)
        {
            Write(BitConv.GetBytes(xIn, IsBigEndian));
        }

        /// <summary>
        /// Writes a single/float to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(float xIn, bool BigEndian)
        {
            Write(BitConv.GetBytes(xIn, BigEndian));
        }

        /// <summary>
        /// Writes a double
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(double xIn)
        {
            Write(BitConv.GetBytes(xIn, IsBigEndian));
        }

        /// <summary>
        /// Writes a double to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void Write(double xIn, bool BigEndian)
        {
            Write(BitConv.GetBytes(xIn, BigEndian));
        }

        /// <summary>
        /// Writes a Signed Byte
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(sbyte xIn) { Write((byte)xIn); }

        /// <summary>
        /// Writes a bool byte (1 yes, 0 no)
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(bool xIn)
        {
            Write((byte)(xIn ? 1 : 0));
        }

        /// <summary>
        /// Writes 24-bit (3 byte) integers
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteUInt24(uint xIn)
        {
            WriteUInt24(xIn, IsBigEndian);
        }

        /// <summary>
        /// Writes 24-bit (3 byte) integers to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void WriteUInt24(uint xIn, bool BigEndian)
        {
            var xList = BitConv.GetBytes(xIn, false).ToList();
            xList.RemoveAt(3);
            if (BigEndian)
                xList.Reverse();
            Write(xList.ToArray());
        }

        /// <summary>
        /// Writes 40-bit (5 byte) integers
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteUInt40(ulong xIn)
        {
            WriteUInt40(xIn, IsBigEndian);
        }

        /// <summary>
        /// Writes 40-bit (5 bytes) integers to specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void WriteUInt40(ulong xIn, bool BigEndian)
        {
            var xList = BitConv.GetBytes(xIn, false).ToList();
            xList.RemoveAt(5);
            xList.RemoveAt(5);
            xList.RemoveAt(5);
            if (BigEndian)
                xList.Reverse();
            Write(xList.ToArray());
        }

        /// <summary>
        /// Writes a 48-bit (6 bytes) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteUInt48(ulong xIn)
        {
            WriteUInt48(xIn, IsBigEndian);
        }

        /// <summary>
        /// Writes 48-bit (6 bytes) integer to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void WriteUInt48(ulong xIn, bool BigEndian)
        {
            var xList = BitConv.GetBytes(xIn, false).ToList();
            xList.RemoveAt(6);
            xList.RemoveAt(6);
            if (BigEndian)
                xList.Reverse();
            Write(xList.ToArray());
        }

        /// <summary>
        /// Writes a 56-bit (7 byte) integer
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteUInt56(ulong xIn)
        {
            WriteUInt56(xIn, IsBigEndian);
        }

        /// <summary>
        /// Writes 56-bit (7 byte) integers to a specified endian
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public void WriteUInt56(ulong xIn, bool BigEndian)
        {
            var xList = BitConv.GetBytes(xIn, BigEndian).ToList();
            xList.RemoveAt(7);
            if (BigEndian)
                xList.Reverse();
            Write(xList.ToArray());
        }

        /// <summary>
        /// Writes an ASCII string
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(string xIn)
        {
            Write(Encoding.ASCII.GetBytes(xIn.ToCharArray()));
        }

        /// <summary>
        /// Writes a specified type of string
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="xType"></param>
        public void Write(string xIn, StringForm xType)
        {
            if (xType == StringForm.ASCII)
                Write(xIn);
            else
            {
                Write(IsBigEndian
                          ? Encoding.BigEndianUnicode.GetBytes(xIn.ToCharArray())
                          : Encoding.Unicode.GetBytes(xIn.ToCharArray()));
            }
        }

        /// <summary>
        /// Pads a string and writes it
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="xType"></param>
        /// <param name="xDesiredSize"></param>
        /// <param name="xPadLocale"></param>
        /// <param name="PadChar"></param>
        public void Write(string xIn, StringForm xType, int xDesiredSize, PadLocale xPadLocale, char PadChar)
        {
            if (!Enum.IsDefined(typeof(StringForm), xType) || !Enum.IsDefined(typeof(PadLocale), xPadLocale))
                throw new Exception("Invalid Parameters");
            xIn = xPadLocale == PadLocale.Right ? xIn.PadRight(xDesiredSize, PadChar) : xIn.PadLeft(xDesiredSize, PadChar);
            switch (xType)
            {
                case StringForm.ASCII:
                    Write(Encoding.ASCII.GetBytes(xIn.ToCharArray()));
                    break;
                case StringForm.Unicode:
                    Write(IsBigEndian
                              ? Encoding.BigEndianUnicode.GetBytes(xIn.ToCharArray())
                              : Encoding.Unicode.GetBytes(xIn.ToCharArray()));
                    break;
            }
        }

        /// <summary>
        /// Attempts to write a hexadecimal string
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteHexString(string xIn)
        {
            Write(xIn.HexToBytes());
        }

        /// <summary>
        /// Writes a byte (8-bits)
        /// </summary>
        /// <param name="xIn"></param>
        public void Write(byte xIn) { Write(new[] { xIn }); }

        /// <summary>
        /// Writes a File Time TimeStamp
        /// </summary>
        /// <param name="xIn"></param>
        public void WriteFileTime(DateTime xIn)
        {
            Write(xIn.ToFileTime());
        }

        /// <summary>
        /// Flushes the stream and writes the pending data (if any)
        /// </summary>
        public virtual void Flush()
        {
            if (xThisData == DataType.File)
                xStream.Flush();
        }

        #endregion

        #region Misc
        /// <summary>
        /// Returns the stream length
        /// </summary>
        public virtual long Length
        {
            get
            {
                return xStream.Length;
            }
        }

        /// <summary>
        /// Gets a text based size for users
        /// </summary>
        public string LengthFriendly { get { return VariousFunctions.GetFriendlySize(Length); } }

        /// <summary>
        /// Returns the stream position
        /// </summary>
        public virtual long Position
        {
            get
            {
                return xStream.Position;
            }
            set
            {
                if (value != xStream.Position)
                    xStream.Seek(value, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        public virtual bool Close()
        {
            if (!Accessed) return true;
            try { xStream.Close(); }
            catch { return false; }
            return true;
        }

        /// <summary>
        /// Disposes of the stream
        /// </summary>
        /// <returns></returns>
        public virtual bool Dispose()
        {
            return Dispose(false);
        }

        internal bool Dispose(bool DeleteFile)
        {
            if (!Close())
                return false;
            if (xThisData != DataType.Real)
            {
                try { xStream.Dispose(); }
                catch { return false; }
                if (xThisData == DataType.File && DeleteFile)
                    VariousFunctions.DeleteFile(FileNameLong);
            }
            xFile = null;
            txtidx = null;
            return true;
        }

        /// <summary>
        /// Reopens the stream
        /// </summary>
        /// <returns></returns>
        public virtual bool OpenAgain()
        {
            if (Accessed) return Accessed;
            if (xThisData == DataType.Real)
                return false;
            switch (xThisData)
            {
                case DataType.File:
                    XSetStream(DJFileMode.Open);
                    break;
                case DataType.Drive:
                    break;
            }
            return Accessed;
        }

        /// <summary>
        /// Returns the stream
        /// </summary>
        /// <returns></returns>
        public virtual Stream GrabStream()
        {
            return xThisData == DataType.Real ? null : xStream;
        }

        /// <summary>
        /// Sets the length of the stream (not always applicable)
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public virtual bool SetLength(long xLen)
        {
            if (xThisData != DataType.File)
                return false;
            try
            {
                xStream.SetLength(xLen);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Gets a bool if instance is valid
        /// </summary>
        /// <returns></returns>
        protected virtual bool GetAccessed() { return xStream.CanRead && xStream.CanWrite; }
        #endregion
    }

    /// <summary>
    /// IO to handle Drives
    /// </summary>
    [DebuggerStepThrough]
    public sealed class DriveIO : DJsIO
    {
        [CompilerGenerated]
        internal Drive xDrive;

        void driveset(ref Drive xIn, bool BigEndian)
        {
            try
            {
                IsBigEndian = BigEndian;
                xIn.MakeHandle();
                xStream = new FileStream(xIn.Handle, FileAccess.ReadWrite);
                xThisData = DataType.Drive;
                xDrive = xIn;
            }
            catch (Exception) { xStream = null; throw; }
        }

        /// <summary>
        /// Accesses a stream from a Drive
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public DriveIO(Drive xIn, bool BigEndian) { driveset(ref xIn, BigEndian); }

        /// <summary>
        /// Accesses a stream from a Drive
        /// </summary>
        /// <param name="xIn"></param>
        /// <param name="BigEndian"></param>
        public DriveIO(ref Drive xIn, bool BigEndian) { driveset(ref xIn, BigEndian); }

        /// <summary>
        /// Writes bytes to the stream
        /// </summary>
        /// <param name="xIn"></param>
        public override void Write(byte[] xIn)
        {
            // Have to align to sector, my buffer system
            // Get position
            var pos = xStream.Position;
            // Gets bytes away from beginning of sector
            var leftover = (int)(pos % xDrive.Geometry.BytesPerSector);
            // Set Position to sector beginning
            if (leftover != 0)
                Position = pos - leftover;
            // Get size of our buffer relative to starting sector
            var size = xIn.Length + leftover;
            var count = (int)((((size - 1) / xDrive.Geometry.BytesPerSector) + 1));
            // For some reason, my IO doesn't want to write just one sector
            if (count == 1)
                count++;
            // Read the sectors needed to write
            size = (int)(count * xDrive.Geometry.BytesPerSector);
            var buffer = new byte[size];
            xStream.Read(buffer, 0, size);
            Array.Copy(xIn, 0, buffer, leftover, xIn.Length);
            // Go back to original position (automatically aligns to sector start even when it says it isn't v.v)
            Position = pos - leftover;
            xStream.Write(buffer, 0, buffer.Length);
            xStream.Position = pos + xIn.Length;
        }

        /// <summary>
        /// Reads bytes to the stream
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public override byte[] ReadBytes(int xSize)
        {
            var pos = xStream.Position;
            var leftover = (int)(pos % xDrive.Geometry.BytesPerSector);
            if (leftover != 0)
                Position = pos - leftover;
            var size = (int)(((((xSize + leftover) - 1) / xDrive.Geometry.BytesPerSector) + 1) * xDrive.Geometry.BytesPerSector);
            var xbuff = new byte[size];
            xStream.Read(xbuff, 0, size);
            xStream.Position = pos + xSize;
            return xbuff.BytePiece(leftover, xSize);
        }

        /// <summary>
        /// Grabs the length of the stream
        /// </summary>
        public override long Length { get { return xDrive.Geometry.DiskSize; } }

        /// <summary>
        /// DOES NOT WORK IN THIS CLASS
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// Tries to open stream again
        /// </summary>
        /// <returns></returns>
        public override bool OpenAgain()
        {
            xDrive.MakeHandle();
            driveset(ref xDrive, IsBigEndian);
            return xDrive.Accessed;
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            xDrive.Handle.Close();
            return xDrive.Handle.IsClosed;
        }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadStream() { return null; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public override bool SetLength(long xLen) { return false; }
    }

    /// <summary>
    /// STFS Stream
    /// </summary>
    [DebuggerStepThrough]
    public sealed class STFSStreamIO : DJsIO
    {
        [CompilerGenerated]
        internal FileEntry xFileEnt;
        [CompilerGenerated]
        internal STFSPackage xRef { get { return xFileEnt.xPackage; } }
        [CompilerGenerated]
        int pos;
        [CompilerGenerated]
        int idx;
        [CompilerGenerated]
        bool accessed;

        DJsIO xIO { get { return xRef.xIO; } }

        /// <summary>
        /// Creates a Real Time STFS File Stream
        /// </summary>
        /// <param name="Instance"></param>
        /// <param name="BigEndian"></param>
        internal STFSStreamIO(FileEntry Instance, bool BigEndian)
        {
            xFileEnt = Instance;
            if (xRef.xActive)
                throw IOExcepts.AccessError;
            xFile = Instance.GetPath();
            XSetStrings();
            xThisData = DataType.Real;
            IsBigEndian = BigEndian;
            accessed = true;
            Position = 0;
        }

        /// <summary>
        /// Reads bytes from a stream
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public override byte[] ReadBytes(int xSize)
        {
            var buff = new byte[xSize];
            for (long i = 0; i < xSize; i++)
            {
                buff[i] = xIO.ReadByte();
                Position = pos + 1;
            }
            return buff;
        }

        /// <summary>
        /// Writes a byte array to the stream
        /// </summary>
        /// <param name="xIn"></param>
        public override void Write(byte[] xIn)
        {
            if (pos + xIn.Length >= xFileEnt.Size)
                throw IOExcepts.PositionError;
            foreach (var x in xIn)
                xIO.Write(x);
        }

        /// <summary>
        /// Closes the STFS stream
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            xRef.xActive = false;
            accessed = false;
            return true;
        }

        /// <summary>
        /// Sets the position of the stream
        /// </summary>
        public override long Position
        {
            get { return pos; }
            set
            {
                if (xRef == null)
                    return;
                if (value == pos)
                    return;
                var indx = (int)(value / 0x1000);
                if (xFileEnt.xBlocks.Length < idx)
                    throw IOExcepts.PositionError;
                var left = (int)(value % 0x1000);
                if (((indx * 0x1000) + left) > Length)
                    throw IOExcepts.PositionError;
                if (idx != indx)
                {
                    idx = indx;
                    xRef.xIO.Position = xRef.GenerateDataOffset(xFileEnt.xBlocks[idx].ThisBlock);
                }
                xRef.xIO.Position = ((xRef.xIO.Position & 0x7FFFFFFFFFFFF000) + left);
                pos = (int)value;
            }
        }

        /// <summary>
        /// Grabs the length of the stream
        /// </summary>
        public override long Length { get { return xFileEnt.Size; } }

        /// <summary>
        /// Disposes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Dispose()
        {
            Close();
            xFileEnt = null;
            return true;
        }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override Stream GrabStream() { return null; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public override bool SetLength(long xLen) { return false; }

        /// <summary>
        /// Checks if it is opened
        /// </summary>
        /// <returns></returns>
        protected override bool GetAccessed() { return accessed; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override bool OpenAgain() { return false; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class MultiFileIO : DJsIO
    {
        [CompilerGenerated]
        List<DJsIO> xIOz;
        [CompilerGenerated]
        readonly long size;
        [CompilerGenerated]
        int idx;
        [CompilerGenerated]
        DJsIO xIO { get { return xIOz[idx]; } }
        [CompilerGenerated]
        long fileloc;

        /// <summary>
        /// Creates an instances of multiple files interpretted as one
        /// </summary>
        /// <param name="Files">Files in order of piece number</param>
        /// <param name="BigEndian">Byte Endian</param>
        public MultiFileIO(ICollection<string> Files, bool BigEndian)
        {
            if (Files == null || Files.Count == 0)
                throw IOExcepts.MultiAccessError;
            var xios = new List<DJsIO>();
            try
            {
                foreach (var x in Files)
                {
                    xios.Add(new DJsIO(x, DJFileMode.Open, BigEndian));
                    if (!xios[xios.Count - 1].Accessed)
                        throw new Exception();
                    size += xios[xios.Count - 1].Length;
                }
                xIOz = xios;
            }
            catch
            {
                foreach (var x in xios)
                    x.Dispose();
                throw IOExcepts.MultiAccessError;
            }
            xThisData = DataType.MultiFile;
        }

        /// <summary>
        /// Reads bytes from the stream
        /// </summary>
        /// <param name="xSize"></param>
        /// <returns></returns>
        public override byte[] ReadBytes(int xSize)
        {
            var buff = new byte[xSize];
            for (var i = 0; i < xSize; i++)
            {
                buff[i] = xIO.ReadByte();
                Position = fileloc + xIO.Position;
            }
            return buff;
        }

        /// <summary>
        /// Writes bytes to the stream
        /// </summary>
        /// <param name="xIn"></param>
        public override void Write(byte[] xIn)
        {
            foreach (var x in xIn)
                Write(x);
            //Write(xIn);

        }

        /// <summary>
        /// Gets or sets the position of the stream
        /// </summary>
        public override long Position
        {
            get { return fileloc + xIO.Position; }
            set
            {
                if (Position == value && fileloc + xIO.Length != value)
                    return;
                if (fileloc + xIO.Length > value)
                {
                    var leftover = (value - fileloc);
                    xIO.Position = leftover;
                }
                else
                {
                    var indx = idx;
                    if (value < fileloc)
                        indx = 0;
                    for (var i = indx; i < xIOz.Count; i++)
                    {
                        idx = i;
                        if (fileloc + xIO.Length <= value)
                        {
                            fileloc += xIO.Length;
                            continue;
                        }
                        var leftover = (value - fileloc);
                        xIO.Position = leftover;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get's the length of the stream
        /// </summary>
        public override long Length { get { return size; } }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override byte[] ReadStream() { return null; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <param name="xLen"></param>
        /// <returns></returns>
        public override bool SetLength(long xLen) { return false; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override Stream GrabStream() { return null; }

        /// <summary>
        /// NOT FOR THIS STREAM
        /// </summary>
        /// <returns></returns>
        public override bool OpenAgain() { return Accessed; }

        /// <summary>
        /// Checks if instance is valid
        /// </summary>
        /// <returns></returns>
        protected override bool GetAccessed()
        {
            var xReturn = xIOz != null;
            if (xReturn)
            {
                xReturn = xIOz.Aggregate(true, (current, x) => current & x.Accessed);
            }
            return xReturn;
        }

        /// <summary>
        /// Flushes the stream
        /// </summary>
        public override void Flush()
        {
            foreach (var x in xIOz)
                x.Flush();
        }

        /// <summary>
        /// Closes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            if (xIOz == null)
                return true;
            foreach (var x in xIOz)
                x.Dispose();
            return true;
        }

        /// <summary>
        /// Disposes the stream
        /// </summary>
        /// <returns></returns>
        public override bool Dispose()
        {
            Close();
            xIOz = null;
            return true;
        }
    }

    #region Drive Essentials
    /// <summary>
    /// Object to hold Disk Geometry details
    /// </summary>
    [StructLayout(LayoutKind.Sequential), DebuggerStepThrough]
    public struct DiskGeometry
    {
        readonly long cylinders;
        readonly uint mediaType;
        readonly uint tracksPerCylinder;
        readonly uint sectorsPerTrack;
        readonly uint bytesPerSector;

        /// <summary>
        /// Bytes Per Sector
        /// </summary>
        public uint BytesPerSector { get { return bytesPerSector; } }
        /// <summary>
        /// Disk Size
        /// </summary>
        public long DiskSize { get { return cylinders * tracksPerCylinder * sectorsPerTrack * bytesPerSector; } }
    }

    /// <summary>
    /// Type of device
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// Physical type
        /// </summary>
        PhysicalDrive,
        /// <summary>
        /// Logical type
        /// </summary>
        LogicalDrive
    }

    /// <summary>
    /// Class for Accessing Drives
    /// </summary>
    [DebuggerStepThrough]
    public sealed class Drive
    {
        #region Imports
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(
        string lpFileName,
        FileAccess dwDesiredAccess,
        FileShare dwShareMode,
        IntPtr lpSecurityAttributes,
        FileMode dwCreationDisposition,
        FlagsAndAttributes dwFlagsAndAttributes,
        IntPtr hTemplateFile);

        [DllImport("kernel32.dll")]
        private static extern bool DeviceIoControl(SafeHandle hDevice, uint dwIoControlCode,
        IntPtr lpInBuffer, uint nInBufferSize, ref DiskGeometry lpOutBuffer,
        uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);
        #endregion

        [CompilerGenerated]
        SafeFileHandle xSFH;
        [CompilerGenerated]
        readonly byte xIndex;
        [CompilerGenerated]
        DiskGeometry xGeom;
        [CompilerGenerated]
        readonly DeviceType xType;

        internal SafeFileHandle Handle { get { return xSFH; } }
        /// <summary>
        /// Is accessed
        /// </summary>
        public bool Accessed { get { return (xSFH != null && !xSFH.IsInvalid); } }
        /// <summary>
        /// This geometry
        /// </summary>
        public DiskGeometry Geometry { get { return xGeom; } }
        /// <summary>
        /// This type
        /// </summary>
        public DeviceType Type { get { return xType; } }
        /// <summary>
        /// Device name
        /// </summary>
        public string DeviceName
        {
            get
            {
                if (xType == DeviceType.PhysicalDrive)
                    return (xType + xIndex.ToString(CultureInfo.InvariantCulture));
                return ((char)xIndex) + ":";
            }
        }

        /// <summary>
        /// Grabs a physical drive from an index
        /// </summary>
        /// <param name="index"></param>
        public Drive(byte index) { xIndex = index; xType = DeviceType.PhysicalDrive; MakeHandle(); GetGeom(); }

        /// <summary>
        /// Grabs a logical drive from a letter
        /// </summary>
        /// <param name="Letter"></param>
        public Drive(char Letter) { xIndex = (byte)Letter; xType = DeviceType.LogicalDrive; MakeHandle(); GetGeom(); }

        /// <summary>
        /// Grabs drive
        /// </summary>
        /// <param name="index"></param>
        /// <param name="xtype"></param>
        public Drive(byte index, DeviceType xtype) { xIndex = index; xType = xtype; MakeHandle(); GetGeom(); }

        /// <summary>
        /// IO Attributes
        /// </summary>
        [Flags]
        public enum FlagsAndAttributes : uint
        {
            /// <summary>
            /// Read Only
            /// </summary>
            ReadOnly = 1,
            /// <summary>
            /// Hidden
            /// </summary>
            Hidden = 2,
            /// <summary>
            /// System
            /// </summary>
            System = 4,
            /// <summary>
            /// Directory
            /// </summary>
            Directory = 0x10,
            /// <summary>
            /// Archive
            /// </summary>
            Archive = 0x20,
            /// <summary>
            /// Device
            /// </summary>
            Device = 0x40,
            /// <summary>
            /// Normal
            /// </summary>
            Normal = 0x80,
            /// <summary>
            /// Temporary
            /// </summary>
            Temporary = 0x100,
            /// <summary>
            /// Sparse File
            /// </summary>
            SparseFile = 0x200,
            /// <summary>
            /// Reparse Point
            /// </summary>
            ReparsePoint = 0x400,
            /// <summary>
            /// Compressed
            /// </summary>
            Compressed = 0x800,
            /// <summary>
            /// Offline
            /// </summary>
            Offline = 0x1000,
            /// <summary>
            /// Not Content Indexed
            /// </summary>
            NotContentIndexed = 0x2000,
            /// <summary>
            /// Encrypted
            /// </summary>
            Encrypted = 0x4000,
            /// <summary>
            /// Write through
            /// </summary>
            Write_Through = 0x80000000,
            /// <summary>
            /// Overlapped
            /// </summary>
            Overlapped = 0x40000000,
            /// <summary>
            /// No Buffering
            /// </summary>
            NoBuffering = 0x20000000,
            /// <summary>
            /// Random Access
            /// </summary>
            RandomAccess = 0x10000000,
            /// <summary>
            /// Sequential Scan
            /// </summary>
            SequentialScan = 0x8000000,
            /// <summary>
            /// Delete on close
            /// </summary>
            DeleteOnClose = 0x4000000,
            /// <summary>
            /// Backup Semantics
            /// </summary>
            BackupSemantics = 0x2000000,
            /// <summary>
            /// Posix Semantics
            /// </summary>
            PosixSemantics = 0x1000000,
            /// <summary>
            /// Open Reparse Point
            /// </summary>
            OpenReparsePoint = 0x200000,
            /// <summary>
            /// Open No Recall
            /// </summary>
            OpenNoRecall = 0x100000,
            /// <summary>
            /// First Pipe Instance
            /// </summary>
            FirstPipeInstance = 0x80000
        }

        internal void GetGeom()
        {
            xGeom = new DiskGeometry();
            uint blah;
            DeviceIoControl(xSFH, 0x70000, IntPtr.Zero, 0, ref xGeom,
                (uint)Marshal.SizeOf(typeof(DiskGeometry)), out blah, IntPtr.Zero);
        }

        internal void MakeHandle()
        {
            try { xSFH.Close(); }
            catch (Exception)
            {
            }
            xSFH = CreateFile(@"\\.\" + DeviceName.ToUpper(),
                 FileAccess.ReadWrite,
                 FileShare.ReadWrite,
                 IntPtr.Zero,
                 FileMode.Open,
                 FlagsAndAttributes.Device | FlagsAndAttributes.NoBuffering |
                 FlagsAndAttributes.Write_Through,
                 IntPtr.Zero);
        }
    }
    #endregion
}
