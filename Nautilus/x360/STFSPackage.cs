using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Globalization;

namespace Nautilus.x360
{
    /// <summary>
    /// Base class for STFS items
    /// </summary>
    public class ItemEntry
    {
        #region Variables
        [CompilerGenerated]
        internal STFSPackage xPackage;
        [CompilerGenerated]
        internal int xCreated;
        [CompilerGenerated]
        internal int xAccessed;
        [CompilerGenerated]
        internal int xSize;
        [CompilerGenerated]
        internal uint xBlockCount;
        [CompilerGenerated]
        internal uint xStartBlock;
        [CompilerGenerated]
        string xName;
        [CompilerGenerated]
        internal ushort xEntryID = 0;
        [CompilerGenerated]
        internal ushort xFolderPointer = 0xFFFF;
        [CompilerGenerated]
        byte xFlag;
        [CompilerGenerated]
        long xDirectoryOffset;
        #endregion

        /// <summary>
        /// Time Created
        /// </summary>
        public DateTime Created { get { return TimeStamps.FatTimeDT(xCreated); } }
        /// <summary>
        /// Entry size
        /// </summary>
        public int Size { get { return xSize; } }
        /// <summary>
        /// Time last accessed
        /// </summary>
        public DateTime Accessed { get { return TimeStamps.FatTimeDT(xAccessed); } }
        /// <summary>
        /// Unknown flag
        /// </summary>
        public bool UnknownFlag
        {
            get { return ((xFlag >> 6) & 1) == 1; }
            set
            {
                if (value)
                    xFlag = (byte)(((FolderFlag ? 1 : 0) << 7) | (1 << 6) | xNameLength);
                else xFlag = (byte)(((FolderFlag ? 1 : 0) << 7) | (0 << 6) | xNameLength);
            }
        }
        byte xNameLength
        {
            get { return (byte)(xFlag & 0x3F); }
            set
            {
                if (value > 0x28)
                    value = 0x28;
                xFlag = (byte)(((FolderFlag ? 1 : 0) << 7) | ((UnknownFlag ? 1 : 0) << 6) | value);
            }
        }
        /// <summary>
        /// Parent directory pointer
        /// </summary>
        public ushort FolderPointer { get { return xFolderPointer; } }
        /// <summary>
        /// Deleted flag
        /// </summary>
        public bool IsDeleted { get { return xNameLength == 0; } }
        /// <summary>
        /// Offset in the package
        /// </summary>
        public long DirectoryOffset { get { return xDirectoryOffset; } }
        /// <summary>
        /// Is a folder
        /// </summary>
        public bool FolderFlag { get { return ((xFlag >> 7) & 1) == 1; } }
        /// <summary>
        /// Entry name
        /// </summary>
        public string Name
        {
            get { return xName; }
            set
            {
                value.IsValidXboxName();
                if (value.Length >= 0x28)
                    value = value.Substring(0, 0x28);
                xName = value;
                xNameLength = (byte)value.Length;
            }
        }
        /// <summary>
        /// Entry ID
        /// </summary>
        public ushort EntryID { get { return xEntryID; } }
        /// <summary>
        /// Start Blocm
        /// </summary>
        public uint StartBlock { get { return xStartBlock; } }
        /// <summary>
        /// Block Count
        /// </summary>
        public uint BlockCount { get { return xBlockCount; } }

        internal ItemEntry(byte[] xDataIn, long DirectOffset, ushort xID, STFSPackage xPackageIn)
        {
            try
            {
                xPackage = xPackageIn;
                var xFileIO = new DJsIO(xDataIn, true) { Position = 0 };
                xEntryID = xID;
                xFileIO.Position = 0x28;
                xFlag = xFileIO.ReadByte();
                if (xNameLength > 0x28)
                    xNameLength = 0x28;
                xFileIO.Position = 0;
                if (xNameLength == 0)
                    return;
                xName = xFileIO.ReadString(StringForm.ASCII, xNameLength);
                xName.IsValidXboxName();
                xFileIO.Position = 0x2F;
                xStartBlock = xFileIO.ReadUInt24(false);
                xFolderPointer = xFileIO.ReadUInt16();
                xSize = xFileIO.ReadInt32();
                xBlockCount = (uint)(((xSize - 1) / 0x1000) + 1);
                xCreated = xFileIO.ReadInt32();
                xAccessed = xFileIO.ReadInt32();
                xDirectoryOffset = DirectOffset;
            }
            catch { xNameLength = 0; }
        }

        internal ItemEntry(string NameIn, int SizeIn, bool xIsFolder, ushort xID, ushort xFolder, STFSPackage xPackageIn)
        {
            xPackage = xPackageIn;
            xEntryID = xID;
            xFolderPointer = xFolder;
            xName = NameIn.Length >= 0x28 ? NameIn.Substring(0, 0x28) : NameIn;
            xFlag = (byte)(((xIsFolder ? 1 : 0) << 7) | xName.Length);
            var x = DateTime.Now;
            xCreated = TimeStamps.FatTimeInt(x);
            xAccessed = xCreated;
            if (xIsFolder)
            {
                xSize = 0;
                xStartBlock = 0;
                xBlockCount = 0;
            }
            else
            {
                xSize = SizeIn;
                if (xSize != 0)
                    xBlockCount = (uint)(((xSize - 1) / 0x1000) + 1);
            }
        }

        internal ItemEntry(ItemEntry x)
        {
            xName = x.xName;
            xAccessed = x.xAccessed;
            xCreated = x.xCreated;
            xBlockCount = x.xBlockCount;
            xDirectoryOffset = x.xDirectoryOffset;
            xFlag = x.xFlag;
            xEntryID = x.xEntryID;
            xFolderPointer = x.xFolderPointer;
            xSize = x.xSize;
            xStartBlock = x.xStartBlock;
            xFlag = (byte)((x.FolderFlag ? 1 : 0) << 7 | (x.UnknownFlag ? 1 : 0) << 6 | xName.Length);
            xPackage = x.xPackage;
        }

        internal void xFixOffset() { xDirectoryOffset = xPackage.STFSStruct.GenerateDataOffset(xPackage.xFileBlocks[xEntryID / 0x40].ThisBlock) + ((0x40 * xEntryID) % 0x40); }

        int DelFold(ushort foldID)
        {
            for (var i = 0; i < xPackage.xFileDirectory.Count; i++)
            {
                if (xPackage.xFileDirectory[i].FolderPointer != foldID) continue;
                xPackage.xFileDirectory[i].xNameLength = 0;
                xPackage.xFileDirectory.RemoveAt(i--);
            }
            for (var i = 0; i < xPackage.xFolderDirectory.Count; i++)
            {
                var x = xPackage.xFolderDirectory[i];
                if (x.FolderPointer == foldID)
                    i = DelFold(x.EntryID);
            }
            return xPackage.xDeleteEntry(this);
        }

        /// <summary>
        /// Deletes entry
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            if (!xPackage.ActiveCheck())
                return false;
            try
            {
                if (FolderFlag)
                    DelFold(EntryID);
                else
                {
                    xNameLength = 0;
                    xPackage.xDeleteEntry(this);
                }
                return true;
            }
            catch { return (xPackage.xActive = false); }
        }

        /// <summary>
        /// Grabs the binary data representation
        /// </summary>
        /// <returns></returns>
        public byte[] GetEntryData()
        {
            try
            {
                var xReturn = new List<byte>();
                xReturn.AddRange(Encoding.ASCII.GetBytes(xName.ToCharArray()));
                xReturn.AddRange(new byte[0x28 - xName.Length]);
                xReturn.Add(xFlag);
                var xbuff = new List<byte>();
                xbuff.AddRange(BitConv.GetBytes(xBlockCount, false));
                xbuff.RemoveAt(3);
                xReturn.AddRange(xbuff);
                xReturn.AddRange(xbuff);
                xbuff.Clear();
                xbuff.AddRange(BitConv.GetBytes(xStartBlock, false));
                xbuff.RemoveAt(3);
                xReturn.AddRange(xbuff);
                xbuff.Clear();
                xReturn.AddRange(BitConv.GetBytes(xFolderPointer, true));
                xReturn.AddRange(BitConv.GetBytes(xSize, true));
                xReturn.AddRange(BitConv.GetBytes(xCreated, false));
                xReturn.AddRange(BitConv.GetBytes(xAccessed, false));
                return xReturn.ToArray();
            }
            catch { return new byte[0]; }
        }

        /// <summary>
        /// Writes the binary data
        /// </summary>
        /// <returns></returns>
        public bool WriteEntry()
        {
            if (!xPackage.ActiveCheck())
                return false;
            try
            {
                xPackage.xIO.Position = xDirectoryOffset;
                xPackage.xIO.Write(GetEntryData());
                xPackage.xIO.Flush();
                return true;
            }
            catch { return (xPackage.xActive = false); }
        }

        /// <summary>
        /// Grabs the path
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            var xReturn = Name;
            var currfold = xFolderPointer;
            while (currfold != 0xFFFF)
            {
                ItemEntry xAbove = xPackage.xGetFolder(currfold);
                if (xAbove == null)
                    return null;
                xReturn = xAbove.Name + "/" + xReturn;
                currfold = xAbove.xFolderPointer;
            }
            return xReturn;
        }
    }

    /// <summary>
    /// Object for STFS File Entry
    /// </summary>
    public sealed class FileEntry : ItemEntry
    {
        internal FileEntry(ItemEntry xEntry) : base(xEntry) { }

        internal FileEntry(string NameIn, int SizeIn, bool xIsFolder, ushort xID, ushort xFolder, STFSPackage xPackageIn)
            : base(NameIn, SizeIn, xIsFolder, xID, xFolder, xPackageIn) { }

        [CompilerGenerated]
        internal BlockRecord[] xBlocks;
        [CompilerGenerated]
        internal DJsIO RealStream = null;

        internal bool Opened { get { return xBlocks != null && xBlocks.Length > 0; } }

        internal bool ReadBlocks()
        {
            try
            {
                if (RealStream != null)
                    return false;
                xPackage.GetBlocks(xBlockCount, xStartBlock, out xBlocks);
                if (xBlocks.Length < xBlockCount)
                    ClearBlocks();
                return Opened;
            }
            catch { return false; }
        }

        internal bool ClearBlocks()
        {
            try
            {
                if (RealStream != null)
                    return false;
                xBlocks = null;
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Fixes the hashes of the file
        /// </summary>
        /// <param name="SignTypeOrNull"></param>
        /// <returns></returns>
        public bool FixHashes(RSAParams SignTypeOrNull)
        {
            if (SignTypeOrNull != null && !SignTypeOrNull.Valid)
                throw CryptoExcepts.ParamError;
            if (!xPackage.ActiveCheck())
                return false;
            if (!Opened && !ReadBlocks())
                return (xPackage.xActive = false);
            try
            {
                // Fixes each hash associated with each block for each Level
                var x1 = new List<uint>();
                foreach (var x in xBlocks)
                {
                    xPackage.XTakeHash(xPackage.GenerateDataOffset(x.ThisBlock), xPackage.GenerateHashOffset(x.ThisBlock, TreeLevel.L0), 0x1000);
                    if (!x1.Contains(x.ThisBlock / Constants.BlockLevel[0]))
                        x1.Add(x.ThisBlock / Constants.BlockLevel[0]);
                }
                if (xPackage.xSTFSStruct.BlockCount >= Constants.BlockLevel[0])
                {
                    var x2 = new List<uint>();
                    foreach (var x in x1)
                    {
                        xPackage.XTakeHash(xPackage.GenerateBaseOffset((x * Constants.BlockLevel[0]), TreeLevel.L0),
                            xPackage.GenerateHashOffset((x * Constants.BlockLevel[0]), TreeLevel.L1), 0x1000);
                        if (!x2.Contains(x / Constants.BlockLevel[0]))
                            x2.Add(x / Constants.BlockLevel[0]);
                    }
                    if (xPackage.xSTFSStruct.BlockCount > 0x70E4)
                    {
                        foreach (var x in x2)
                            xPackage.XTakeHash(xPackage.GenerateBaseOffset((x * Constants.BlockLevel[1]), TreeLevel.L1),
                                xPackage.GenerateHashOffset((x * Constants.BlockLevel[1]), TreeLevel.L2), 0x1000);
                    }
                }
                if (SignTypeOrNull != null)
                    xPackage.xWriteHeader(SignTypeOrNull);
                ClearBlocks();
                return true;
            }
            catch
            {
                ClearBlocks();
                return (xPackage.xActive = false);
            }
        }

        /// <summary>
        /// Replace the file via IO
        /// </summary>
        /// <param name="xIOIn"></param>
        /// <returns></returns>
        public bool Replace(DJsIO xIOIn)
        {
            if (!xPackage.ActiveCheck())
                return false;
            return (xReplace(xIOIn) & true);
        }

        /// <summary>
        /// Replace the file via File Location
        /// </summary>
        /// <param name="FileIn"></param>
        /// <returns></returns>
        public bool Replace(string FileIn)
        {
            DJsIO xIO = null;
            try
            {
                xIO = new DJsIO(FileIn, DJFileMode.Open, true);
                var Success = xReplace(xIO);
                xIO.Dispose();
                return Success;
            }
            catch
            {
                if (xIO != null)
                    xIO.Dispose();
                return xPackage.xActive = false;
            }
        }

        internal bool xReplace(DJsIO xIOin)
        {
            if (!Opened)
                ReadBlocks();
            if (!xIOin.Accessed || xIOin.Length > 0xFFFFFFFF)
                return false;
            try
            {
                // Allocates new blocks for new data
                var xEntAlloc = xPackage.xAllocateBlocks(xPackage.xCurEntBlckCnt, 0);
                var xFileAlloc = xPackage.xAllocateBlocks(xIOin.BlockCountSTFS(), xEntAlloc[xEntAlloc.Length - 1].ThisBlock + 1);
                // Updates entry
                xStartBlock = xFileAlloc[0].ThisBlock;
                xSize = (int)xIOin.Length;
                xBlockCount = xIOin.BlockCountSTFS();
                if (!xPackage.xDoAdd(ref xIOin, ref xEntAlloc, ref xFileAlloc)) return false;
                xPackage.xDeleteChain(xBlocks);
                ClearBlocks();
                return true;
            }
            catch { ClearBlocks(); return false; }
        }

        internal bool xExtract(DJsIO xIOOut)
        {
            if (!Opened && !ReadBlocks())
                return false;
            try
            {
                // Gets data and writes it
                xIOOut.Position = 0;
                for (uint i = 0; i < xBlockCount; i++)
                {
                    xPackage.xIO.Position = xPackage.GenerateDataOffset(xBlocks[i].ThisBlock);
                    xIOOut.Write(i < (xBlockCount - 1)
                                     ? xPackage.xIO.ReadBytes(0x1000)
                                     : xPackage.xIO.ReadBytes((((Size - 1) % 0x1000) + 1)));
                }
                xIOOut.Flush();
                ClearBlocks();
                return true;
            }
            catch
            {
                ClearBlocks();
                return false;
            }
        }

        /// <summary>
        /// Extracts the entry via user selection
        /// </summary>
        /// <param name="DialogTitle"></param>
        /// <param name="DialogFilter"></param>
        /// <returns></returns>
        public bool Extract(string DialogTitle, string DialogFilter)
        {
            var FileOut = VariousFunctions.GetUserFileLocale(DialogTitle, DialogFilter, false);
            return FileOut != null && ExtractToFile(FileOut);
        }

        /// <summary>
        /// Extracts entry to a location
        /// </summary>
        /// <param name="FileOut"></param>
        /// <returns></returns>
        public bool ExtractToFile(string FileOut)
        {
            if (!xPackage.ActiveCheck())
                return false;
            bool xReturn;
            var xIO = new DJsIO(true);
            try
            {
                xReturn = xExtract(xIO);
                xIO.Close();
                if (xReturn)
                {
                    if (!VariousFunctions.MoveFile(xIO.FileNameLong, FileOut))
                        throw new Exception();
                }
            }
            catch
            {
                xReturn = false;
                xIO.Close();
            }
            VariousFunctions.DeleteFile(xIO.FileNameLong);
            xPackage.xActive = false;
            return xReturn;
        }

        /// <summary>
        /// Extracts the entire file to memory, in a byte array. Caution: may use too much RAM.
        /// </summary>
        /// <returns></returns>
        public byte[] Extract()
        {
            if (!Opened && !ReadBlocks())
                throw new Exception("File not opened or couldn't read blocks.");
            try
            {
                using (var ms = new MemoryStream())
                {
                    ms.Position = 0;
                    for (uint i = 0; i < xBlockCount; i++)
                    {
                        xPackage.xIO.Position = xPackage.GenerateDataOffset(xBlocks[i].ThisBlock);
                        byte[] buffer;
                        if (i < (xBlockCount - 1))
                        {
                            buffer = xPackage.xIO.ReadBytes(0x1000);
                            ms.Write(buffer, 0, 0x1000);
                        }
                        else
                        {
                            buffer = xPackage.xIO.ReadBytes((((Size - 1) % 0x1000) + 1));
                            ms.Write(buffer, 0, buffer.Length);
                        }
                    }
                    ClearBlocks();
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                ClearBlocks();
                return null;
            }
        }

        /// <summary>
        /// Extracts certain number of bytes of a file into a byte array.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] Extract(uint bytes)
        {
            if (!Opened && !ReadBlocks())
                throw new Exception("File not opened or couldn't read blocks.");
            try
            {
                using (var ms = new MemoryStream())
                {
                    ms.Capacity = (int)bytes;
                    ms.Position = 0;
                    for (uint i = 0; i < xBlockCount; i++)
                    {
                        xPackage.xIO.Position = xPackage.GenerateDataOffset(xBlocks[i].ThisBlock);
                        byte[] buffer;
                        if (i < (xBlockCount - 1))
                        {
                            buffer = xPackage.xIO.ReadBytes(0x1000);
                            ms.Write(buffer, 0, (int)(ms.Position - 0x1000 < bytes ? 0x1000 : 0x1000 - (bytes - ms.Position)));
                        }
                        else
                        {
                            buffer = xPackage.xIO.ReadBytes((((Size - 1) % 0x1000) + 1));
                            ms.Write(buffer, 0, (int)(ms.Position - buffer.Length < bytes ? buffer.Length : buffer.Length - (bytes - ms.Position)));
                        }
                        if (ms.Position == bytes)
                        {
                            break;
                        }
                    }
                    ClearBlocks();
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                ClearBlocks();
                return null;
            }
        }

        internal bool xInject(DJsIO xIOin)
        {
            if (!Opened && !ReadBlocks())
                return false;
            try
            {
                if (xIOin.Length > 0xFFFFFFFF)
                    return false;
                var y = xIOin.BlockCountSTFS();
                var x = xBlocks.ToList();
                List<BlockRecord> xdel = null;
                if (y > xBlocks.Length)// Allocates data for blocks needed
                    x.AddRange(xPackage.xAllocateBlocks((uint)(y - xBlocks.Length), 0));
                else
                {
                    xdel = new List<BlockRecord>();
                    for (var i = (int)y; i < x.Count; i++)
                    {
                        xdel.Add(x[i]);
                        x.RemoveAt(i--);
                    }
                }
                var success = xPackage.xWriteTo(ref xIOin, x.ToArray());
                if (success)
                {
                    xBlocks = x.ToArray();
                    if (xdel != null && xdel.Count != 0)
                        xPackage.xDeleteChain(xdel.ToArray());
                }
                return (success & ClearBlocks());
            }
            catch { ClearBlocks(); return false; }
        }

        /// <summary>
        /// Overwrites the file data from an IO
        /// </summary>
        /// <param name="xIOin"></param>
        /// <returns></returns>
        public bool Inject(DJsIO xIOin)
        {
            if (!xPackage.ActiveCheck())
                return false;
            return (xInject(xIOin) & true);
        }

        /// <summary>
        /// Overwrites the data from a file location
        /// </summary>
        /// <param name="FileIn"></param>
        /// <returns></returns>
        public bool Inject(string FileIn)
        {
            DJsIO xIO = null;
            try
            {
                xIO = new DJsIO(FileIn, DJFileMode.Open, true);
                var Success = Inject(xIO);
                xIO.Dispose();
                return Success;
            }
            catch
            {
                if (xIO != null)
                    xIO.Dispose();
                return xPackage.xActive = false;
            }
        }

        /// <summary>
        /// Returns a real time STFS file stream
        /// </summary>
        /// <param name="MakeCopy"></param>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public DJsIO GrabSTFSStream(bool MakeCopy, bool BigEndian)
        {
            try
            {
                if (RealStream != null)
                    return RealStream;
                if (!xPackage.ActiveCheck())
                    return null;
                if (MakeCopy)
                {
                    var xtemp = new DJsIO(true);
                    if (!xExtract(xtemp))
                    {
                        xtemp.Close();
                        VariousFunctions.DeleteFile(xtemp.FileNameLong);
                        return null;
                    }
                    var success = xReplace(xtemp);
                    xtemp.Close();
                    VariousFunctions.DeleteFile(xtemp.FileNameLong);
                    if (!success)
                        return null;
                }
                if (!Opened && !ReadBlocks())
                    return null;
                return (RealStream = new STFSStreamIO(this, BigEndian));
            }
            catch { RealStream = null; xPackage.xActive = false; return null; }
        }

        internal DJsIO xGetTempIO(bool BigEndian)
        {
            if (!Opened && !ReadBlocks())
                return null;
            var xIO = new DJsIO(BigEndian);
            if (!xExtract(xIO))
            {
                xIO.Close();
                VariousFunctions.DeleteFile(xIO.FileNameLong);
                xIO = null;
            }
            ClearBlocks();
            return xIO;
        }

        /// <summary>
        /// Extracts the file to a temporary location
        /// </summary>
        /// <param name="BigEndian"></param>
        /// <returns></returns>
        public DJsIO GetTempIO(bool BigEndian)
        {
            if (!xPackage.ActiveCheck())
                return null;
            var xReturn = xGetTempIO(BigEndian);
            xPackage.xActive = false;
            return xReturn;
        }

        internal FileEntry Copy()
        {
            var x = new FileEntry(this) { xBlocks = xBlocks };
            return x;
        }
    }

    /// <summary>
    /// Class for STFS Folder items
    /// </summary>
    public sealed class FolderEntry : ItemEntry
    {
        internal FolderEntry(ItemEntry xEntry) : base(xEntry) { }

        internal FolderEntry(string NameIn, int SizeIn, ushort xID, ushort xFolder, STFSPackage xPackageIn)
            : base(NameIn, SizeIn, true, xID, xFolder, xPackageIn) { }

        internal bool folderextract(bool xInclude, string xOut)
        {
            try
            {
                if (!VariousFunctions.xCheckDirectory(xOut))
                    return false;
                foreach (var x in xPackage.xFileDirectory)
                {
                    if (x.FolderPointer != xEntryID)
                        continue;
                    var xIO = new DJsIO(VariousFunctions.xGetUnusedFile(xOut + "/" + x.Name), DJFileMode.Create, true);
                    if (!xIO.Accessed) continue;
                    x.xExtract(xIO);
                    xIO.Dispose();
                }
                foreach (var z in xPackage.xFolderDirectory.Where(z => z.FolderPointer == EntryID))
                {
                    z.folderextract(xInclude, xOut + "/" + z.Name);
                }
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Extract the files
        /// </summary>
        /// <param name="xOutLocale"></param>
        /// <param name="xIncludeSubItems"></param>
        /// <returns></returns>
        public bool Extract(string xOutLocale, bool xIncludeSubItems)
        {
            if (!xPackage.ActiveCheck())
                return false;
            if (xOutLocale[xOutLocale.Length - 1] == '/' ||
                xOutLocale[xOutLocale.Length - 1] == '\\')
                xOutLocale = xOutLocale.Substring(0, xOutLocale.Length - 1);
            if (!VariousFunctions.xCheckDirectory(xOutLocale))
            {
                return false;
            }
            folderextract(xIncludeSubItems, xOutLocale);
            return true;
        }

        /// <summary>
        /// Extract the files
        /// </summary>
        /// <param name="xIncludeSubItems"></param>
        /// <param name="xDescription"></param>
        /// <returns></returns>
        public bool Extract(bool xIncludeSubItems, string xDescription)
        {
            if (!xPackage.ActiveCheck())
                return false;
            var y = VariousFunctions.GetUserFolderLocale("Select a save location");
            return y != null && folderextract(xIncludeSubItems, y);
        }

        /// <summary>
        /// Grabs the subfolders
        /// </summary>
        /// <returns></returns>
        public FolderEntry[] GetSubFolders()
        {
            if (!xPackage.ActiveCheck())
                return null;
            var xReturn = xGetFolders();
            xPackage.xActive = false;
            return xReturn;
        }

        internal FolderEntry[] xGetFolders()
        {
            return xPackage.xFolderDirectory.Where(x => x.FolderPointer == EntryID).ToArray();
        }

        /// <summary>
        /// Grabs the files
        /// </summary>
        /// <returns></returns>
        public FileEntry[] GetSubFiles()
        {
            if (!xPackage.ActiveCheck())
                return null;
            var xReturn = xGetFiles();
            xPackage.xActive = false;
            return xReturn;
        }

        internal FileEntry[] xGetFiles()
        {
            return xPackage.xFileDirectory.Where(x => x.FolderPointer == EntryID).ToArray();
        }
    }

    /// <summary>
    /// Class for STFS Licenses
    /// </summary>
    public sealed class STFSLicense
    {
        [CompilerGenerated]
        internal long xID;
        [CompilerGenerated]
        internal int xInt1;
        [CompilerGenerated]
        internal int xInt2;
        [CompilerGenerated]
        readonly bool xfirst;

        /// <summary>
        /// ID
        /// </summary>
        public long ID { get { return xID; } }
        /// <summary>
        /// Bits
        /// </summary>
        public int Var1 { get { return xInt1; } }
        /// <summary>
        /// Flags
        /// </summary>
        public int Flags { get { return xInt2; } }

        internal STFSLicense(long xid, int x1, int x2, bool first)
        {
            xInt1 = x1;
            xInt2 = x2;
            xID = xid;
            xfirst = first;
        }

        /// <summary>
        /// Clear all licenses
        /// </summary>
        /// <returns></returns>
        public bool Clear()
        {
            try
            {
                if (xfirst)
                    xID = -1;
                else xID = 0;
                xInt1 = 0;
                xInt2 = 0;
                return true;
            }
            catch { return false; }
        }
    }

    /// <summary>
    /// Class for Header info
    /// </summary>
    public sealed class HeaderData
    {
        #region Non-Property Variables
        [CompilerGenerated]
        internal PackageMagic xMagic = PackageMagic.Unknown;
        [CompilerGenerated]
        List<STFSLicense> xLisc = new List<STFSLicense>();
        [CompilerGenerated]
        internal PackageType xThisType = PackageType.SavedGame;
        [CompilerGenerated]
        byte[] xPackageImage;
        [CompilerGenerated]
        byte[] xContentImage;
        /// <summary>
        /// Meta Data Version
        /// </summary>
        [CompilerGenerated]
        public uint MetaDataVersion = 2;
        [CompilerGenerated]
        long xContentSize;
        /// <summary>
        /// Media ID
        /// </summary>
        [CompilerGenerated]
        public uint MediaID;
        /// <summary>
        /// Version
        /// </summary>
        [CompilerGenerated]
        public uint Version_;
        /// <summary>
        /// Base Version
        /// </summary>
        [CompilerGenerated]
        public uint Version_Base;
        /// <summary>
        /// Title ID
        /// </summary>
        [CompilerGenerated]
        public uint TitleID = 0xFFFE07D1;
        /// <summary>
        /// Platform
        /// </summary>
        [CompilerGenerated]
        public byte Platform;
        /// <summary>
        /// Executable Type
        /// </summary>
        [CompilerGenerated]
        public byte ExecutableType;
        /// <summary>
        /// Disc Number
        /// </summary>
        [CompilerGenerated]
        public byte DiscNumber;
        /// <summary>
        /// Disc In Set
        /// </summary>
        [CompilerGenerated]
        public byte DiscInSet;
        /// <summary>
        /// Save Game ID
        /// </summary>
        [CompilerGenerated]
        public uint SaveGameID;
        /// <summary>
        /// Data File Count
        /// </summary>
        [CompilerGenerated]
        public uint DataFileCount;
        /// <summary>
        /// Data File Size
        /// </summary>
        [CompilerGenerated]
        public long DataFileSize;
        /// <summary>
        /// Reserved
        /// </summary>
        [CompilerGenerated]
        public long Reserved;
        [CompilerGenerated]
        byte[] xSeriesID = new byte[0x10];
        [CompilerGenerated]
        byte[] xSeasonID = new byte[0x10];
        /// <summary>
        /// Season Number
        /// </summary>
        [CompilerGenerated]
        public ushort SeasonNumber;
        /// <summary>
        /// Episode Number
        /// </summary>
        [CompilerGenerated]
        public ushort EpidsodeNumber;
        [CompilerGenerated]
        long xSaveConsoleID;
        /// <summary>
        /// Profile ID
        /// </summary>
        [CompilerGenerated]
        public long ProfileID;
        [CompilerGenerated]
        byte[] xDeviceID = new byte[20];
        [CompilerGenerated]
        readonly string[] xTitles = new string[9];
        [CompilerGenerated]
        readonly string[] xDescriptions = new string[9];
        [CompilerGenerated]
        string xPublisher = "";
        [CompilerGenerated]
        string xTitle = "";
        [CompilerGenerated]
        byte IDTransferByte;
        [CompilerGenerated]
        bool xLoaded;
        [CompilerGenerated]
        Languages xCurrent = Languages.English;
        #endregion

        #region Property Variables
        /// <summary>
        /// Signature type
        /// </summary>
        public PackageMagic Magic { get { return xMagic; } }
        /// <summary>
        /// Transfer flags
        /// </summary>
        public TransferLock IDTransfer { get { return (TransferLock)(IDTransferByte >> 6); } set { IDTransferByte = (byte)((((byte)value) & 3) << 6); } }
        /// <summary>
        /// Package type
        /// </summary>
        public PackageType ThisType { get { return xThisType; } set { xThisType = value; } }
        /// <summary>
        /// STFS Licenses
        /// </summary>
        public STFSLicense[] Liscenses { get { return xLisc.ToArray(); } }
        /// <summary>
        /// STFS Licenses
        /// </summary>
        public long RecordedContentSize { get { return xContentSize; } }
        /// <summary>
        /// Series ID
        /// </summary>
        public byte[] SeriesID { get { return xSeriesID; } set { VariousFunctions.xSetByteArray(ref xSeriesID, value); } }
        /// <summary>
        /// Season ID
        /// </summary>
        public byte[] SeasonID { get { return xSeasonID; } set { VariousFunctions.xSetByteArray(ref xSeasonID, value); } }
        /// <summary>
        /// Console ID (creator)
        /// </summary>
        public long SaveConsoleID
        {
            get { return xSaveConsoleID; }
            set
            {
                if (value > 0xFFFFFFFFFF)
                    value = 0xFFFFFFFFFF;
                xSaveConsoleID = value;
            }
        }
        /// <summary>
        /// Device ID
        /// </summary>
        public byte[] DeviceID { get { return xDeviceID; } set { VariousFunctions.xSetByteArray(ref xDeviceID, value); } }
        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get { return xDescriptions[(byte)xCurrent]; }
            set
            {
                if (value.Length > 0x80)
                    value = value.Substring(0, 0x80);
                xDescriptions[(byte)xCurrent] = value;
            }
        }
        /// <summary>
        /// Display Title
        /// </summary>
        public string Title_Display
        {
            get { return xTitles[(byte)xCurrent]; }
            set
            {
                if (value.Length > 0x80)
                    value = value.Substring(0, 0x80);
                xTitles[(byte)xCurrent] = value;
            }
        }
        /// <summary>
        /// Package Title
        /// </summary>
        public string Title_Package
        {
            get { return xTitle; }
            set
            {
                if (value.Length > 0x40)
                    value = value.Substring(0, 0x40);
                xTitle = value;
            }
        }
        /// <summary>
        /// Publisher
        /// </summary>
        public string Publisher
        {
            get { return xPublisher; }
            set
            {
                if (value.Length > 0x40)
                    value = value.Substring(0, 0x40);
                xPublisher = value;
            }
        }
        /// <summary>
        /// Content image
        /// </summary>
        public Image ContentImage
        {
            get { return ContentImageBinary.BytesToImage(); }
            set
            {
                var x = value.ImageToBytes(System.Drawing.Imaging.ImageFormat.Png);
                if (x.Length > 0x4000)
                    return;
                xContentImage = x;
            }
        }
        /// <summary>
        /// Package image
        /// </summary>
        public Image PackageImage
        {
            get { return PackageImageBinary.BytesToImage(); }
            set
            {
                var x = value.ImageToBytes(System.Drawing.Imaging.ImageFormat.Png);
                if (x.Length > 0x4000)
                    return;
                xPackageImage = x;
            }
        }
        /// <summary>
        /// Bytes of the Package image
        /// </summary>
        public byte[] PackageImageBinary
        {
            get { return xPackageImage; }
            set
            {
                if (value.Length > 0x4000)
                    return;
                xPackageImage = value;
            }
        }
        /// <summary>
        /// Bytes of the Content image
        /// </summary>
        public byte[] ContentImageBinary
        {
            get { return xContentImage; }
            set
            {
                if (value.Length > 0x4000)
                    return;
                xContentImage = value;
            }
        }
        #endregion

        void read(DJsIO xIO, PackageMagic MagicType)
        {
            xMagic = MagicType;
            xIO.Position = 0x22C;
            xLisc = new List<STFSLicense>();
            for (var i = 0; i < 0x10; i++)
                xLisc.Add(new STFSLicense(xIO.ReadInt64(), xIO.ReadInt32(), xIO.ReadInt32(), i == 0));
            xIO.Position = 0x344;
            xThisType = (PackageType)xIO.ReadUInt32();
            MetaDataVersion = xIO.ReadUInt32();
            xContentSize = xIO.ReadInt64();
            MediaID = xIO.ReadUInt32();
            Version_ = xIO.ReadUInt32();
            Version_Base = xIO.ReadUInt32();
            TitleID = xIO.ReadUInt32();
            Platform = xIO.ReadByte();
            ExecutableType = xIO.ReadByte();
            DiscNumber = xIO.ReadByte();
            DiscInSet = xIO.ReadByte();
            SaveGameID = xIO.ReadUInt32();
            SaveConsoleID = (long)xIO.ReadUInt40();
            ProfileID = xIO.ReadInt64();
            xIO.Position = 0x39D;
            DataFileCount = xIO.ReadUInt32();
            DataFileSize = xIO.ReadInt64();
            Reserved = xIO.ReadInt64();
            xSeriesID = xIO.ReadBytes(0x10);
            xSeasonID = xIO.ReadBytes(0x10);
            SeasonNumber = xIO.ReadUInt16();
            EpidsodeNumber = xIO.ReadUInt16();
            xIO.Position += 0x28;
            xDeviceID = xIO.ReadBytes(0x14);
            for (var i = 0; i < 9; i++)
                xTitles[i] = xIO.ReadString(StringForm.Unicode, 0x80).Replace("\0", "");
            for (var i = 0; i < 9; i++)
                xDescriptions[i] = xIO.ReadString(StringForm.Unicode, 0x80).Replace("\0", "");
            xPublisher = xIO.ReadString(StringForm.Unicode, 0x40).Replace("\0", "");
            xTitle = xIO.ReadString(StringForm.Unicode, 0x40).Replace("\0", "");
            IDTransferByte = xIO.ReadByte();
            // Package Image
            var xSize = xIO.ReadInt32();
            xIO.Position = 0x171A;
            xPackageImage = xIO.ReadBytes(xSize < 0x4000 ? xSize : 0x4000);
            // Content Image
            xIO.Position = 0x1716;
            xSize = xIO.ReadInt32();
            xIO.Position = 0x571A;
            xContentImage = xIO.ReadBytes(xSize < 0x4000 ? xSize : 0x4000);
            xLoaded = true;
        }

        internal HeaderData(STFSPackage xPackage, PackageMagic MagicType) { read(xPackage.xIO, MagicType); }

        /// <summary>
        /// Initializes a default object
        /// </summary>
        public HeaderData()
        {
            xLoaded = true;
            for (var i = 0; i < 9; i++)
            {
                xTitles[i] = "";
                xDescriptions[i] = "";
            }
            xLisc.Add(new STFSLicense(-1, 0, 0, true));
            for (var i = 0; i < 0xF; i++)
                xLisc.Add(new STFSLicense(0, 0, 0, false));
            IDTransfer = TransferLock.AllowTransfer;
            xPackageImage = Nautilus.Properties.Resources.RB3.ImageToBytes(System.Drawing.Imaging.ImageFormat.Png);
            xContentImage = xPackageImage;
        }

        internal HeaderData(DJsIO xIOIn, PackageMagic MagicType) { read(xIOIn, MagicType); }

        internal bool Write(ref DJsIO x)
        {
            if (!xLoaded)
                return false;
            try
            {
                if (x == null || !x.Accessed)
                    return false;
                x.Position = 0x22C;
                foreach (var b in xLisc)
                {
                    x.Write(b.ID);
                    x.Write(b.Var1);
                    x.Write(b.Flags);
                }
                x.Position = 0x344;
                x.Write((uint)xThisType);
                x.Write(MetaDataVersion);
                x.Write(xContentSize);
                x.Write(MediaID);
                x.Write(Version_);
                x.Write(Version_Base);
                x.Write(TitleID);
                x.Write(Platform);
                x.Write(ExecutableType);
                x.Write(DiscNumber);
                x.Write(DiscInSet);
                x.Write(SaveGameID);
                x.WriteUInt40((ulong)SaveConsoleID);
                x.Write(ProfileID);
                x.Position = 0x39D;
                x.Write(DataFileCount);
                x.Write(DataFileSize);
                x.Write(Reserved);
                x.Write(SeriesID);
                x.Write(SeasonID);
                x.Write(SeasonNumber);
                x.Write(EpidsodeNumber);
                x.Position += 0x28;
                x.Write(xDeviceID);
                for (var i = 0; i < 9; i++)
                    x.Write(xTitles[i], StringForm.Unicode, 0x80, PadLocale.Right, PadType.Null);
                for (var i = 0; i < 9; i++)
                    x.Write(xDescriptions[i], StringForm.Unicode, 0x80, PadLocale.Right, PadType.Null);
                x.Write(xPublisher, StringForm.Unicode, 0x40, PadLocale.Right, PadType.Null);
                x.Write(xTitle, StringForm.Unicode, 0x40, PadLocale.Right, PadType.Null);
                x.Write(IDTransferByte);
                x.Write(xPackageImage.Length);
                x.Write(xContentImage.Length);
                x.Write(xPackageImage);
                x.Write(new byte[0x4000 - xPackageImage.Length]);
                x.Write(xContentImage);
                x.Write(new byte[(0x4000 - xContentImage.Length)]);
                return true;
            }
            catch { return false; }
        }

        internal bool WriteText(ref DJsIO x)
        {
            if (!xLoaded || x == null || !x.Accessed)
                return false;
            try
            {
                x.Position = 0;
                x.Write("Modified Header Details by Nemo" + Environment.NewLine);
                x.Write("Display Title: " + xTitles[0] + Environment.NewLine);
                x.Write("Display Description: " + xDescriptions[0] + Environment.NewLine);
                x.Write("Title ID: " + TitleID.ToString("X2") + Environment.NewLine);
                x.Write("Package Title: " + xTitle + Environment.NewLine);
                x.Write("Signature Type - " + xMagic + Environment.NewLine);
                x.Write("Package Type: " + xThisType + Environment.NewLine);
                x.Write("Publisher Name: " + xPublisher + Environment.NewLine);
                x.Flush();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Set the language to grab the description/title
        /// </summary>
        /// <param name="xDesired"></param>
        public void SetLanguage(Languages xDesired)
        {
            xCurrent = xDesired;
        }

        /// <summary>
        /// Clear all the licenses
        /// </summary>
        /// <returns></returns>
        public bool ClearLicenses()
        {
            if (!xLoaded)
                return false;
            foreach (var x in xLisc)
                x.Clear();
            return true;
        }

        /// <summary>
        /// Clears licenses, ID's, and makes the ID's worthless
        /// </summary>
        /// <returns></returns>
        public bool MakeAnonymous()
        {
            if (!xLoaded)
                return false;
            try
            {
                xDeviceID = new byte[20];
                xSaveConsoleID = 0;
                ProfileID = 0;
                IDTransfer = TransferLock.AllowTransfer;
                ClearLicenses();
                return true;
            }
            catch { return false; }
        }

        internal void SetSize(long x)
        {
            xContentSize = x;
        }

        /// <summary>
        /// Attempts to add a license
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Bits"></param>
        /// <param name="Flags"></param>
        /// <returns></returns>
        public bool AddLicense(long ID, int Bits, int Flags)
        {
            for (var i = 0; i < 0x10; i++)
            {
                if (xLisc[i].xInt1 != 0 || xLisc[1].xInt2 != 0) continue;
                xLisc[i].xID = ID;
                xLisc[i].xInt1 = Bits;
                xLisc[i].xInt2 = Flags;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Allows a DLC to be unlocked
        /// </summary>
        /// <returns></returns>
        public bool UnlockDLC()
        {
            if (!xLoaded)
                return false;
            return (ClearLicenses() && AddLicense(-1, 0, 1));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class STFSPackage
    {
        #region Variables
        [CompilerGenerated]
        HeaderData xHeader;
        [CompilerGenerated]
        internal List<FileEntry> xFileDirectory = new List<FileEntry>();
        [CompilerGenerated]
        internal List<FolderEntry> xFolderDirectory = new List<FolderEntry>();
        [CompilerGenerated]
        internal DJsIO xIO;
        [CompilerGenerated]
        internal STFSDescriptor xSTFSStruct;
        [CompilerGenerated]
        internal BlockRecord[] xFileBlocks;
        [CompilerGenerated]
        internal bool xActive = false; // To protect against errors from multithreading
        [CompilerGenerated]
        FolderEntry xroot;
        
        /// <summary>
        /// Package read correctly
        /// </summary>
        public bool ParseSuccess { get { return xIO != null; } }
        /// <summary>
        /// The STFS struct of the package
        /// </summary>
        public STFSDescriptor STFSStruct { get { return xSTFSStruct; } }
        /// <summary>
        /// Header metadata
        /// </summary>
        public HeaderData Header { get { return xHeader; } }
        /// <summary>
        /// Root Directory of package
        /// </summary>
        public FolderEntry RootDirectory { get { return xroot; } }

        uint xNewEntBlckCnt(uint xCount)
        {
            var x = (uint)(xFileDirectory.Count + xFolderDirectory.Count + xCount);
            if (x != 0)
                return ((x - 1) / 0x40) + 1;
            return 0;
        }

        internal uint xCurEntBlckCnt
        {
            get
            {
                var x = (xFileDirectory.Count + xFolderDirectory.Count);
                if (x != 0)
                    return (uint)(((x - 1) / 0x40) + 1);
                return 0;
            }
        }
        #endregion

        #region Local Methods
        /// <summary>
        /// Extracts via out locale
        /// </summary>
        /// <param name="xOutLocale"></param>
        /// <param name="xIncludeSubItems"></param>
        /// <param name="xIncludeHeader"></param>
        /// <returns></returns>
        bool xExtractPayload(string xOutLocale, bool xIncludeSubItems, bool xIncludeHeader)
        {
            try
            {
                xOutLocale = xOutLocale.Replace("\\", "/");
                if (xOutLocale[xOutLocale.Length - 1] == '/')
                    xOutLocale = xOutLocale.Substring(0, xOutLocale.Length - 1);
                if (!VariousFunctions.xCheckDirectory(xOutLocale))
                {
                    return false;
                }
                if (xIncludeHeader)
                {
                    // Records the meta data
                    var xhead = new DJsIO(VariousFunctions.xGetUnusedFile(xOutLocale + "/Header Details.txt")
                        , DJFileMode.Create, true);
                    xHeader.WriteText(ref xhead);
                    xhead.Dispose();
                    xhead = new DJsIO(VariousFunctions.xGetUnusedFile(xOutLocale + "/Content Image.png")
                        , DJFileMode.Create, true) {Position = 0};
                    xhead.Write(xHeader.ContentImageBinary);
                    xhead.Dispose();
                    xhead = new DJsIO(VariousFunctions.xGetUnusedFile(xOutLocale + "/Package Image.png")
                        , DJFileMode.Create, true) {Position = 0};
                    xhead.Write(xHeader.PackageImageBinary);
                    xhead.Dispose();
                }
                xOutLocale += "/Root";
                if (!VariousFunctions.xCheckDirectory(xOutLocale))
                    return (xActive = false);
                // Runs a regular folder extract
                RootDirectory.folderextract(xIncludeSubItems, xOutLocale);
                return true;
            }
            catch { return (xActive = false); }
        }
        
        /// <summary>
        /// Checks to see if the package was parsed
        /// </summary>
        /// <returns></returns>
        protected internal bool ParseCheck()
        {
            if (xIO == null || !xIO.Accessed || !ParseSuccess)
                throw STFSExcepts.Unsuccessful;
            return true;
        }

        /// <summary>
        /// Checks if the package is fine
        /// </summary>
        /// <returns></returns>
        internal bool ActiveCheck()
        {
            if (!ParseCheck())
                return false;
            //if (xActive)
                //return false;
            return (xActive = true);
        }

        /// <summary>
        /// Returns the blocks of a file
        /// </summary>
        /// <param name="xCount"></param>
        /// <param name="xStartBlock"></param>
        /// <param name="xOutBlocks"></param>
        /// <returns></returns>
        internal bool GetBlocks(uint xCount, uint xStartBlock, out BlockRecord[] xOutBlocks)
        {
            // Follows the blocks for the specified max count
            var xBlockList = new List<BlockRecord>();
            var xBlock = GetRecord(xStartBlock, TreeLevel.L0);
            if (xBlock.ThisBlock >= xSTFSStruct.xBlockCount)
                throw STFSExcepts.InvalBlock;
            for (uint i = 0; i < xCount; i++)
            {
                if (!xBlockList.ContainsBlock(xBlock))
                    xBlockList.Add(xBlock);
                else break; // If it contains, it's just going to loop
                if (xBlock.NextBlock == Constants.STFSEnd)
                    break; // Stop means stop
                if (xBlock.NextBlock >= xSTFSStruct.xBlockCount)
                    throw STFSExcepts.InvalBlock;
                // Gets the next block record
                xBlock = GetRecord(xBlock.NextBlock, TreeLevel.L0);
            }
            xOutBlocks = xBlockList.ToArray();
            // Success if 1 - end block is reached and 2 - count is the count of the blocks found
            return (xBlockList.Count == xCount);
        }

        /// <summary>
        /// Writes a SHA1 hash from base IO
        /// </summary>
        /// <param name="xRead"></param>
        /// <param name="xWrite"></param>
        /// <param name="xSize"></param>
        /// <returns></returns>
        internal bool XTakeHash(long xRead, long xWrite, int xSize)
        {
            try { return XTakeHash(ref xIO, xRead, xWrite, xSize, ref xIO); }
            catch { return false; }
        }

        /// <summary>
        /// Writes a SHA1 hash reading from base IO to another IO
        /// </summary>
        /// <param name="xRead"></param>
        /// <param name="xWrite"></param>
        /// <param name="xSize"></param>
        /// <param name="io"></param>
        /// <returns></returns>
        static void XTakeHash(long xRead, long xWrite, int xSize, ref DJsIO io)
        {
            try
            {
                XTakeHash(ref io, xRead, xWrite, xSize, ref io);
            }
            catch (Exception)
            {}
        }

        /// <summary>
        /// Reads from one IO, hashes, stores it in another IO
        /// </summary>
        /// <param name="ioin"></param>
        /// <param name="xRead"></param>
        /// <param name="xWrite"></param>
        /// <param name="xSize"></param>
        /// <param name="ioout"></param>
        /// <returns></returns>
        static bool XTakeHash(ref DJsIO ioin, long xRead, long xWrite, int xSize, ref DJsIO ioout)
        {
            try
            {
                ioin.Position = xRead;
                var xData = ioin.ReadBytes(xSize);
                ioout.Position = xWrite;
                ioout.Write(SHA1Quick.ComputeHash(xData));
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Returns a bool if the corresponding offset/hash is the same
        /// </summary>
        /// <param name="xRead"></param>
        /// <param name="xSize"></param>
        /// <param name="xHash"></param>
        /// <returns></returns>
        bool XVerifyHash(long xRead, int xSize, ref byte[] xHash)
        {
            try
            {
                // Compares strings of the hashes
                xIO.Position = xRead;
                var xData = xIO.ReadBytes(xSize);
                return (BitConverter.ToString(xHash) == BitConverter.ToString(SHA1Quick.ComputeHash(xData)));
            }
            catch { return false; }
        }

        /// <summary>
        /// Produces a file via entries
        /// </summary>
        /// <param name="xFile"></param>
        /// <returns></returns>
        bool xEntriesToFile(out DJsIO xFile)
        {
            xFile = null;
            try
            {
                // Not much explaination is needed, just writes entries into a file
                xFile = new DJsIO(true);
                ushort xCurEnt = 0;
                foreach (var v in xFolderDirectory.Where(v => !v.IsDeleted))
                {
                    xFile.Position = 0x40 * xCurEnt;
                    // Reorders the folders to current entry
                    // Note: Don't have to do this, but I think it's sexy to handle folders at top of directory
                    var v1 = v;
                    foreach (var y in xFolderDirectory.Where(y => y.xFolderPointer == v1.EntryID))
                    {
                        y.xFolderPointer = xCurEnt;
                    }
                    var v2 = v;
                    foreach (var y in xFileDirectory.Where(y => y.xFolderPointer == v2.EntryID))
                    {
                        y.xFolderPointer = xCurEnt;
                    }
                    // Sets current entry
                    v.xEntryID = xCurEnt;
                    // Writes
                    xFile.Write(v.GetEntryData());
                    xCurEnt++;
                }
                for (var i = 0; i < xFolderDirectory.Count; i++)
                {
                    // Write new folder pointer
                    xFile.Position = (0x40 * i) + 0x32;
                    xFile.Write(xFolderDirectory[i].xFolderPointer);
                }
                foreach (var y in xFileDirectory.Where(y => !y.IsDeleted))
                {
                    // Sets
                    y.xEntryID = xCurEnt;
                    xFile.Position = 0x40 * xCurEnt;
                    // Writes
                    xFile.Write(y.GetEntryData());
                    xCurEnt++;
                }
                xFile.Flush();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Writes a file via blocks
        /// </summary>
        /// <param name="xIOIn"></param>
        /// <param name="xBlocks"></param>
        /// <returns></returns>
        internal bool xWriteTo(ref DJsIO xIOIn, BlockRecord[] xBlocks)
        {
            if (!xIOIn.Accessed || (xIOIn.BlockCountSTFS() != xBlocks.Length))
                return false;
            try
            {
                xIOIn.Position = 0;
                for (var i = 0; i < xBlocks.Length - 1; i++)
                {
                    // Finds spot and writes block of data
                    xIO.Position = GenerateDataOffset(xBlocks[i].ThisBlock);
                    xIO.Write(xIOIn.ReadBytes(0x1000));
                }
                xIO.Position = GenerateDataOffset(xBlocks[xBlocks.Length - 1].ThisBlock);
                xIO.Write(xIOIn.ReadBytes(xIOIn.BlockRemainderSTFS()));
                xIO.Flush();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Add a file to the package
        /// </summary>
        /// <param name="xIOIn"></param>
        /// <param name="xEntAlloc"></param>
        /// <param name="xFileAlloc"></param>
        /// <returns></returns>
        internal bool xDoAdd(ref DJsIO xIOIn, ref BlockRecord[] xEntAlloc, ref BlockRecord[] xFileAlloc)
        {
            // Gets Entry Table file
            DJsIO xEntFile;
            if (!xEntriesToFile(out xEntFile))
                return (xActive = false);
            // Writes it
            if (!xWriteTo(ref xEntFile, xEntAlloc))
            {
                xEntFile.Close();
                VariousFunctions.DeleteFile(xEntFile.FileNameLong);
                return (xActive = false);
            }
            xEntFile.Close();
            VariousFunctions.DeleteFile(xEntFile.FileNameLong);
            // Writes the new file
            if (!xWriteTo(ref xIOIn, xFileAlloc))
                return (xActive = false);
            if (!xWriteChain(xEntAlloc))
                return (xActive = false);
            if (!xWriteChain(xFileAlloc))
                return (xActive = false);
            // Fixes internal variables and then writes hashes
            xDeleteChain(xFileBlocks);
            xFileBlocks = xEntAlloc;
            xSTFSStruct.xDirectoryBlock = xEntAlloc[0].ThisBlock;
            foreach (var x in xFileDirectory)
                x.xFixOffset();
            foreach (var x in xFolderDirectory)
                x.xFixOffset();
            xWriteDescriptor(ref xIO);
            return true;
        }

        /// <summary>
        /// Writes the STFS Descriptor to make perm. changes
        /// </summary>
        /// <returns></returns>
        void xWriteDescriptor(ref DJsIO io)
        {
            io.Position = 0x379;
            xSTFSStruct.xDirectoryBlockCount = (ushort)xCurEntBlckCnt;
            io.Write(xSTFSStruct.GetData());
            io.Flush();
        }

        /// <summary>
        /// Generates the data location of the block
        /// </summary>
        /// <param name="xBlock"></param>
        /// <returns></returns>
        internal long GenerateDataOffset(uint xBlock)
        {
            return xSTFSStruct.GenerateDataOffset(xBlock);
        }

        /// <summary>
        /// Generates a hash offset via block
        /// </summary>
        /// <param name="xBlock"></param>
        /// <param name="xTree"></param>
        /// <returns></returns>
        internal long GenerateHashOffset(uint xBlock, TreeLevel xTree)
        {
            var xReturn = xSTFSStruct.GenerateHashOffset(xBlock, xTree);
            if (xSTFSStruct.ThisType == STFSType.Type1) // Grabs the one up level block record for shifting
                xReturn += (GetRecord(xBlock, (TreeLevel)((byte)xTree + 1)).Index << 0xC);
            return xReturn;
        }

        /// <summary>
        /// Generates the Hash Base
        /// </summary>
        /// <param name="xBlock"></param>
        /// <param name="xTree"></param>
        /// <returns></returns>
        internal long GenerateBaseOffset(uint xBlock, TreeLevel xTree)
        {
            var xReturn = xSTFSStruct.GenerateBaseOffset(xBlock, xTree);
            if (xSTFSStruct.ThisType == STFSType.Type1) // Grabs the one up level block record for shifting
                xReturn += (GetRecord(xBlock, (TreeLevel)((byte)xTree + 1)).Index << 0xC);
            return xReturn;
        }

        /// <summary>
        /// Verifies the Header signature
        /// </summary>
        /// <param name="xDev"></param>
        /// <returns></returns>
        Verified VerifySignature(bool xDev)
        {
            try
            {
                var xRSAKeyz = new RSAParameters();
                short xSigSpot = 0;
                switch (xHeader.Magic)
                {
                    case PackageMagic.CON: // signature is the same way for both Dev and Stock
                        {
                            xSigSpot = 0x1AC;
                            xIO.Position = 0x28;
                            xRSAKeyz.Exponent = xIO.ReadBytes(4);
                            xRSAKeyz.Modulus = ScrambleMethods.StockScramble(xIO.ReadBytes(0x80), false);
                        }
                        break;
                    case PackageMagic.LIVE:
                        {
                            xSigSpot = 4;
                            if (!xDev)
                            {
                                xRSAKeyz.Exponent = new byte[] { 0, 1, 0, 1 };
                                xRSAKeyz.Modulus = Nautilus.Properties.Resources.XK1;
                            }
                            else
                            {
                                xRSAKeyz.Exponent = new byte[] { 0, 0, 0, 3 };
                                var xLK = new DJsIO(Nautilus.Properties.Resources.XK4, true);
                                xRSAKeyz.Modulus = xLK.ReadBytes(0x100);
                                xLK.Close();
                            }
                        }
                        break;
                }
                xIO.Position = xSigSpot;
                var xSiggy = ScrambleMethods.StockScramble(xIO.ReadBytes(xRSAKeyz.Modulus.Length), true);
                xIO.Position = 0x22C;
                var xHeadr = xIO.ReadBytes(0x118);
                return new Verified(ItemType.Signature, RSAQuick.SignatureVerify(xRSAKeyz, SHA1Quick.ComputeHash(xHeadr), xSiggy), 0x22C, xSigSpot);
            }
            catch { throw CryptoExcepts.CryptoVeri; }
        }

        /// <summary>
        /// Sets a package comming in to this package
        /// </summary>
        /// <param name="xIn"></param>
        void SetSamePackage(ref STFSPackage xIn)
        {
            xIO = xIn.xIO;
            xSTFSStruct = xIn.STFSStruct;
            xFolderDirectory = xIn.xFolderDirectory;
            xFileDirectory = xIn.xFileDirectory;
            xHeader = xIn.xHeader;
            xFileBlocks = xIn.xFileBlocks;
            xActive = xIn.xActive;
            xroot = xIn.xroot;
            xIn = null;
            foreach (var x in xFileDirectory)
                x.xPackage = this;
            foreach (var x in xFolderDirectory)
                x.xPackage = this;
        }

        /// <summary>
        /// Writes hash tables
        /// </summary>
        /// <returns></returns>
        bool xWriteTables()
        {
            for (uint i = 0; i < STFSStruct.BlockCount; i++)
            {
                XTakeHash(GenerateDataOffset(i),
                    GenerateHashOffset(i, TreeLevel.L0),
                    0x1000, ref xIO);
            }
            if (STFSStruct.BlockCount > Constants.BlockLevel[0])
            {
                // Get level 1 count
                var ct = (((xSTFSStruct.BlockCount - 1) / Constants.BlockLevel[0]) + 1);
                for (uint i = 0; i < ct; i++)
                {
                    XTakeHash(GenerateBaseOffset(i * Constants.BlockLevel[0], TreeLevel.L0),
                    GenerateHashOffset(i * Constants.BlockLevel[0], TreeLevel.L1),
                        0x1000, ref xIO);
                }
                if (STFSStruct.BlockCount > Constants.BlockLevel[1])
                {
                    ct = (((xSTFSStruct.BlockCount - 1) / Constants.BlockLevel[1]) + 1);
                    for (uint i = 0; i < ct; i++)
                    {
                        XTakeHash(GenerateHashOffset((i * Constants.BlockLevel[1]), TreeLevel.L1),
                            GenerateHashOffset((i * Constants.BlockLevel[1]), TreeLevel.L2),
                            0x1000, ref xIO);
                    }
                }
            }
            xIO.Flush();
            return true;
        }

        /// <summary>
        /// Writes header to the file
        /// </summary>
        /// <param name="xParams"></param>
        /// <returns></returns>
        internal bool xWriteHeader(RSAParams xParams)
        {
            if (!xParams.Valid)
                throw CryptoExcepts.ParamError;
            // Writes, hashes, and signs data to a temp file
            var x = new DJsIO(true);
            if (!x.Accessed)
                return false;
            if (!xHeader.Write(ref x))
            {
                x.Close();
                return false;
            }
            xHeader.SetSize(xIO.Length - xSTFSStruct.BaseBlock);
            x.Position = 0x340;
            x.Write(xSTFSStruct.ThisType == STFSType.Type0 ? 0xAD0E : 0x971A);
            // Fills to bottom of header
            x.Position = x.Length;
            x.Write(new byte[(0x8E6 + (xSTFSStruct.BaseBlock - 0xA000))]);
            x.Position = 0x379;
            xWriteDescriptor(ref x);
            long xLocale;
            if (xSTFSStruct.xBlockCount <= Constants.BlockLevel[0])
                xLocale = GenerateBaseOffset(0, TreeLevel.L0);
            else if (xSTFSStruct.xBlockCount <= Constants.BlockLevel[1])
                xLocale = GenerateBaseOffset(0, TreeLevel.L1);
            else xLocale = GenerateBaseOffset(0, TreeLevel.L2);
            XTakeHash(ref xIO, xLocale, 0x381, 0x1000, ref x);
            var xSize = xSTFSStruct.BaseBlock == 0xA000 ? 0x9CBC : 0xACBC;
            XTakeHash(0x344, 0x32C, xSize, ref x);
            x.Position = 0x22C;
            var xHash = SHA1Quick.ComputeHash(x.ReadBytes(0x118));
            x.Position = 4;
            if (xParams.Type == PackageMagic.CON)
            {
                x.Write(xParams.Certificate);
                x.Write(ScrambleMethods.StockScramble(RSAQuick.SignatureGenerate(xParams.RSAKeys, xHash), true));
            }
            else
            {
                x.Write(ScrambleMethods.DevScramble(RSAQuick.SignatureGenerate(xParams.RSAKeys, xHash)));
                x.Write(new byte[0x128]);
            }
            x.IsBigEndian = true;
            x.Position = 0;
            x.Write(((uint)xParams.Type));
            x.Flush();
            xHeader.xMagic = xParams.Type;
            // Writes header to Package just incase of a emergency close, the Package still attains original strucure
            xIO.Position = 0;
            xIO.Write(x.ReadStream());
            xIO.Flush();
            x.Close();
            VariousFunctions.DeleteFile(x.FileNameLong);
            return true;
        }

        /// <summary>
        /// Gets a folder name via it's Entry ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        internal string GetFolderNameByID(ushort ID)
        {
            return (from x in xFolderDirectory where x.EntryID == ID select x.Name).FirstOrDefault();
        }

        /// <summary>
        /// Gets a folder entry by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        internal FolderEntry xGetFolder(ushort ID)
        {
            return xFolderDirectory.FirstOrDefault(x => x.EntryID == ID);
        }

        /// <summary>
        /// Gets the last folder before the target ItemEntry
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        internal FolderEntry xGetParentFolder(string Path)
        {
            Path = Path.xExtractLegitPath();
            var folds = Path.Split(new[] { '/' }).ToList();
            foreach (var x in folds)
                x.IsValidXboxName();
            folds.RemoveAt(folds.Count - 1); // Last entry
            var xcurrent = xroot;
            foreach (var x in folds)
            {
                var found = false;
                // Grab folders pointing to current instance
                var folderz = xcurrent.xGetFolders();
                var x1 = x;
                foreach (var y in folderz.Where(y => y.Name.ToLowerInvariant() == x1.ToLowerInvariant()))
                {
                    // Set new found variables
                    found = true;
                    xcurrent = y;
                    break;
                }
                if (!found)
                {
                    return null;
                }
            }
            return xcurrent; // Must've been found, return it
        }

        /// <summary>
        /// Searches the files for the name and folder pointer
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="FolderPointer"></param>
        /// <returns></returns>
        internal FileEntry xGetFile(string Name, ushort FolderPointer)
        {
            return xFileDirectory.FirstOrDefault(x => x.FolderPointer == FolderPointer && x.Name.ToLowerInvariant() == Name.ToLowerInvariant());
        }

        /// <summary>
        /// Adds a file to the package
        /// </summary>
        /// <param name="xIOIn"></param>
        /// <param name="xFileName"></param>
        /// <param name="Folder"></param>
        /// <returns></returns>
        bool xAddFile(DJsIO xIOIn, string xFileName, ushort Folder)
        {
            try
            {
                if (xIOIn == null || !xIOIn.Accessed || xIOIn.Length > ((xSTFSStruct.SpaceBetween[2] - 1) << 0xC))
                    return (xActive = false);
                if (xFileDirectory.Any(m => m.FolderPointer == Folder && m.Name == xFileName))
                {
                    return (xActive = false);
                }
                // Allocates blocks
                var xEntAlloc = xAllocateBlocks(xNewEntBlckCnt(1), 0);
                var xFileAlloc = xAllocateBlocks(xIOIn.BlockCountSTFS(), xEntAlloc[xEntAlloc.Length - 1].ThisBlock + 1);
                // Adds new file info
                var x = new ItemEntry(xFileName, (int)xIOIn.Length, false, (ushort)(xFileDirectory.Count + xFolderDirectory.Count), Folder, this);
                var y = new FileEntry(x) {xStartBlock = xFileAlloc[0].ThisBlock};
                xFileDirectory.Add(y);
                return xDoAdd(ref xIOIn, ref xEntAlloc, ref xFileAlloc);
            }
            catch (Exception) { return (xActive = false);
            }
        }

        /// <summary>
        /// Deletes an item via its entry
        /// </summary>
        /// <param name="x"></param>
        internal int xDeleteEntry(ItemEntry x)
        {
            if (x.FolderFlag)
            {
                for (var i = 0; i < xFolderDirectory.Count; i++)
                {
                    if (xFolderDirectory[i].EntryID != x.EntryID) continue;
                    xFolderDirectory.RemoveAt(i);
                    return i;
                }
            }
            else
            {
                for (var i = 0; i < xFileDirectory.Count; i++)
                {
                    if (xFileDirectory[i].EntryID != x.EntryID) continue;
                    if (xFileDirectory[i].ReadBlocks())
                        xDeleteChain(xFileDirectory[i].xBlocks);
                    xFileDirectory.RemoveAt(i);
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// For sorting folders by path
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        static int sortpathct(CFolderEntry x1, CFolderEntry x2)
        {
            return x1.xthispath.xPathCount().CompareTo(x2.xthispath.xPathCount());
        }

        /// <summary>
        /// Gets a name via hash string and Title ID
        /// </summary>
        /// <returns></returns>
        string dlcname()
        {
            try
            {
                xIO.Position = 0x32C;
                return xIO.ReadBytes(0x14).HexString() + ((byte)(xHeader.TitleID >> 16)).ToString("X2");
            }
            catch { return "00000000000000000000000000000000000000000000"; }
        }

        enum SwitchType { None, Allocate, Delete }

        void SwitchNWrite(BlockRecord RecIn, SwitchType Change)
        {
            var canswitch = (xSTFSStruct.ThisType == STFSType.Type1);
            var current = xSTFSStruct.TopRecord;
            var pos = new long[] { 0, 0, 0 };
            // Grab base starting points
            if (RecIn.ThisBlock >= Constants.BlockLevel[0] ||
                xSTFSStruct.xBlockCount > Constants.BlockLevel[0])
            {
                if (RecIn.ThisBlock >= Constants.BlockLevel[1] ||
                    xSTFSStruct.xBlockCount > Constants.BlockLevel[1])
                    pos[0] = xSTFSStruct.GenerateHashOffset(RecIn.ThisBlock, TreeLevel.L2) + 0x14;
                pos[1] = xSTFSStruct.GenerateHashOffset(RecIn.ThisBlock, TreeLevel.L1) + 0x14;
            }
            pos[2] = xSTFSStruct.GenerateHashOffset(RecIn.ThisBlock, TreeLevel.L0) + 0x14;
            var len = GenerateDataOffset(RecIn.ThisBlock) + 0x1000;
            if (xIO.Length < len)
                xIO.SetLength(len);
            switch (Change)
            {
                case SwitchType.Allocate:
                    xSTFSStruct.TopRecord.BlocksFree--;
                    break;
                case SwitchType.Delete:
                    xSTFSStruct.TopRecord.BlocksFree++;
                    break;
            }
            if (pos[0] != 0)
            {
                //KILL ON RECONSTRUCTION
                if (canswitch)
                    pos[0] += (current.Index << 0xC);
                // ---------------------

                xIO.Position = pos[0];
                current = new BlockRecord(xIO.ReadUInt32());
               if (Change != SwitchType.None)
                {
                    if (Change == SwitchType.Allocate)
                        current.BlocksFree--; // Takes away a free block
                    else current.BlocksFree++; // Adds a free block
                    xIO.Position = pos[0];
                    xIO.Write(current.Flags);
                    xIO.Flush();
                }
            }
            // Follows same pattern
            if (pos[1] != 0)
            {
                //KILL ON RECONSTRUCTION
                if (canswitch)
                    pos[1] += (current.Index << 0xC);
                // ---------------------

                xIO.Position = pos[1];
                current = new BlockRecord(xIO.ReadUInt32());
                if (Change != SwitchType.None)
                {
                    if (Change == SwitchType.Allocate)
                        current.BlocksFree--; // Takes away a free block
                    else current.BlocksFree++; // Adds a free block
                    xIO.Position = pos[1];
                    xIO.Write(current.Flags);
                    xIO.Flush();
                }
            }
            
            //KILL ON RECONSTRUCTION
            if (canswitch)
                pos[2] += (current.Index << 0xC);
            // ---------------------

            switch (Change)
            {
                case SwitchType.Allocate:
                    RecIn.Status = RecIn.Status == HashStatus.Old ? HashStatus.Reused : HashStatus.New;
                    break;
                case SwitchType.Delete:
                    RecIn.MarkOld();
                    break;
            }
            xIO.Position = pos[2];
            xIO.Write(RecIn.Flags);
            xIO.Flush();
            if (RecIn.ThisBlock >= xSTFSStruct.xBlockCount)
                xSTFSStruct.xBlockCount = RecIn.ThisBlock + 1;
        }

        BlockRecord GetRecord(uint xBlock, TreeLevel xLevel)
        {
            if (xLevel == TreeLevel.LT)
                return xSTFSStruct.TopRecord;
            var current = xSTFSStruct.TopRecord;
            var canswitch = (xSTFSStruct.ThisType == STFSType.Type1);
            if (xSTFSStruct.xBlockCount > Constants.BlockLevel[1])
            {
                // Grab base position
                xIO.Position = (xSTFSStruct.GenerateHashOffset(xBlock, TreeLevel.L2) + 0x14);
                if (canswitch)
                    xIO.Position += (current.Index << 0xC);
                current = new BlockRecord(xIO.ReadUInt32()); // Read new flag
                if (xLevel == TreeLevel.L2)
                {
                    // return if needed
                    current.ThisBlock = xBlock;
                    current.ThisLevel = TreeLevel.L2;
                    return current;
                }
            }
            else if (xLevel == TreeLevel.L2)
                return xSTFSStruct.TopRecord;
            // Follows same procedure
            if (xSTFSStruct.xBlockCount > Constants.BlockLevel[0])
            {
                xIO.Position = (xSTFSStruct.GenerateHashOffset(xBlock, TreeLevel.L1)) + 0x14;
                if (canswitch)
                    xIO.Position += (current.Index << 0xC);
                current = new BlockRecord(xIO.ReadUInt32());
                if (xLevel == TreeLevel.L1)
                {
                    current.ThisBlock = xBlock;
                    current.ThisLevel = TreeLevel.L1;
                    return current;
                }
            }
            else if (xLevel == TreeLevel.L1)
                return xSTFSStruct.TopRecord;
            xIO.Position = (xSTFSStruct.GenerateHashOffset(xBlock, TreeLevel.L0)) + 0x14;
            if (canswitch)
                xIO.Position += (current.Index << 0xC);
            current = new BlockRecord(xIO.ReadUInt32()) {ThisBlock = xBlock, ThisLevel = TreeLevel.L0};
            return current;
        }

        internal bool xWriteChain(BlockRecord[] xRecs)
        {
            for (var i = 0; i < xRecs.Length; i++)
            {
                xRecs[i].NextBlock = (i + 1) < xRecs.Length ? xRecs[i + 1].ThisBlock : Constants.STFSEnd;
                SwitchNWrite(xRecs[i], SwitchType.Allocate);
            }
            return true;
        }

        internal bool xDeleteChain(BlockRecord[] xBlocks)
        {
            if (xBlocks == null)
                return true;
            foreach (var x in xBlocks)
            {
                SwitchNWrite(x, SwitchType.Delete);
            }
            return true;
        }

        internal BlockRecord[] xAllocateBlocks(uint count, uint xStart)
        {
            if ((xSTFSStruct.BlockCount + count) > xSTFSStruct.SpaceBetween[2])
                return new BlockRecord[0];
            var xReturn = new List<BlockRecord>();
            for (uint i = 0; i < count; i++)
            {
                BlockRecord x = null;
                while (x == null)
                {
                    if (xStart > xSTFSStruct.SpaceBetween[2])
                        break;
                    // Grab record or make new one
                    if (xStart < xSTFSStruct.xBlockCount)
                    {
                        var y = GetRecord(xStart, TreeLevel.L0);
                        if (y.Status == HashStatus.Old || y.Status == HashStatus.Unused)
                            x = y;
                    }
                    else
                    {
                        if (xStart == Constants.BlockLevel[0])
                        {
                            xIO.Position = GenerateHashOffset(0, TreeLevel.L1) + (xSTFSStruct.TopRecord.Index << 0xC) + 0x14;
                            xIO.Write(xSTFSStruct.TopRecord.Flags);
                            xIO.Flush();
                        }
                        else if (xStart == Constants.BlockLevel[1])
                        {
                            xIO.Position = GenerateHashOffset(0, TreeLevel.L2) + (xSTFSStruct.TopRecord.Index << 0xC) + 0x14;
                            xIO.Write(xSTFSStruct.TopRecord.Flags);
                            xIO.Flush();
                        }
                        x = new BlockRecord(HashStatus.New, Constants.STFSEnd)
                            {
                                ThisBlock = xStart,
                                ThisLevel = TreeLevel.L0
                            };
                    }
                    xStart++;
                }
                xReturn.Add(x);
            }
            return xReturn.ToArray();
        }
        #endregion

        #region Package initialization
        /// <summary>
        /// Lets user auto select package
        /// </summary>
        public STFSPackage()
            : this(new DJsIO(DJFileMode.Open, "Open an Xbox Package", "", true))
        {
            if (!ParseSuccess && xIO != null)
                xIO.Dispose();
        }

        /// <summary>
        /// Initializes a package parse from an already accessed file
        /// </summary>
        /// <param name="xIOIn"></param>
        public STFSPackage(DJsIO xIOIn)
        {
            if (!xIOIn.Accessed)
                return;
            xIO = xIOIn;
            xActive = true;
            try
            {
                xIO.Position = 0;
                xIO.IsBigEndian = true;
                var xBuff = xIOIn.ReadUInt32();
                PackageMagic xMagic;
                if (Enum.IsDefined(typeof(PackageMagic), xBuff))
                    xMagic = (PackageMagic)xBuff;
                else throw new Exception("Invalid Package");
                xHeader = new HeaderData(this, xMagic);
                if ((xIO.Length % 0x1000) != 0)
                {
                    xIO.Position = xIO.Length;
                    xIO.Write(new byte[(int)(0x1000 - (xIO.Length % 0x1000))]);
                    xIO.Flush();
                }
                if (xHeader.ThisType == PackageType.HDDInstalledGame ||
                    xHeader.ThisType == PackageType.OriginalXboxGame ||
                    xHeader.ThisType == PackageType.GamesOnDemand ||
                    xHeader.ThisType == PackageType.SocialTitle)
                    throw STFSExcepts.Game;
                xSTFSStruct = new STFSDescriptor(this);
                xFileBlocks = new BlockRecord[0];
                GetBlocks(xSTFSStruct.DirectoryBlockCount, xSTFSStruct.DirectoryBlock, out xFileBlocks);
                ushort xEntryID = 0;
                foreach (var xCurrentOffset in xFileBlocks.Select(x => GenerateDataOffset(x.ThisBlock)))
                {
                    for (var i = 0; i < 0x40; i++)
                    {
                        xIO.Position = (xCurrentOffset + (0x40 * i));
                        if (xIO.ReadByte() == 0)
                            continue;
                        xIO.Position--;
                        var xItem = new ItemEntry(xIO.ReadBytes(0x40), (xIO.Position - 0x40), xEntryID, this);
                        if (xItem.IsDeleted)
                            continue;
                        if (!xItem.FolderFlag)
                            xFileDirectory.Add(new FileEntry(xItem));
                        else xFolderDirectory.Add(new FolderEntry(xItem));
                        xEntryID++;
                    }
                }
                xroot = new FolderEntry("", 0, 0xFFFF, 0xFFFF, this);
                xActive = false;
            }
            catch (Exception) { xIO = null; throw; }

        }

        /// <summary>
        /// Attempts to parse a file from a specific location
        /// </summary>
        /// <param name="xLocation"></param>
        public STFSPackage(string xLocation)
            : this(new DJsIO(xLocation, DJFileMode.Open, true)) { }

        /// <summary>
        /// Create an STFS Package
        /// </summary>
        /// <param name="xSession"></param>
        /// <param name="xSigning"></param>
        /// <param name="xOutPath"></param>
        public STFSPackage(CreateSTFS xSession, RSAParams xSigning, string xOutPath)
        {
            xActive = true;
            if (!xSigning.Valid)
                throw CryptoExcepts.ParamError;
            if (xSession.xFileDirectory.Count == 0)
                throw new Exception();
            try
            {
                //AddToLog("Setting Package variables");
                //new Thread(DLLIdentify.PrivilegeCheck).Start(Thread.CurrentThread);
                xroot = new FolderEntry("", 0, 0xFFFF, 0xFFFF, this);
                switch (xSession.HeaderData.ThisType)
                {
                    case PackageType.ThematicSkin:
                        {
                            var x1 = new DJsIO(true);
                            var x2 = new DJsIO(true);
                            x1.Write((int)xSession.ThemeSettings.StyleType);
                            x1.Flush();
                            x1.Close();
                            if (!xSession.AddFile(x1.FileNameLong, "DashStyle"))
                                throw STFSExcepts.ThemeError;
                            x2.Write("SphereColor=" + ((byte)xSession.ThemeSettings.Sphere).ToString(CultureInfo.InvariantCulture).PadRight(2, '\0'));
                            x2.Write(new byte[] { 0xD, 0xA });
                            x2.Write("AvatarLightingDirectional=" +
                                     xSession.ThemeSettings.AvatarLightingDirectional0.ToString("#0.0") + "," +
                                     xSession.ThemeSettings.AvatarLightingDirectional1.ToString("#0.0000") + "," +
                                     xSession.ThemeSettings.AvatarLightingDirectional2.ToString("#0.0") + ",0x" +
                                     xSession.ThemeSettings.AvatarLightingDirectional3.ToString("X"));
                            x2.Write(new byte[] { 0xD, 0xA });
                            x2.Write("AvatarLightingAmbient=0x" + xSession.ThemeSettings.AvatarLightingAmbient.ToString("X"));
                            x2.Write(new byte[] { 0xD, 0xA });
                            x2.Flush();
                            x2.Close();
                            if (!xSession.AddFile(x2.FileNameLong, "parameters.ini"))
                                throw STFSExcepts.ThemeError;
                        }
                        break;
                    case PackageType.SocialTitle:
                    case PackageType.OriginalXboxGame:
                    case PackageType.HDDInstalledGame:
                    case PackageType.GamesOnDemand:
                        throw STFSExcepts.Game;
                }
                //xLog = LogIn;
                xHeader = xSession.HeaderData;
                xSTFSStruct = new STFSDescriptor(xSession.STFSType, 0);
                xIO = new DJsIO(true);
                var DirectoryBlockz = new List<BlockRecord>();
                // switched2 = true;
                uint xcurblock = 0;
                for (ushort i = 0; i < xSession.GetDirectoryCount; i++)
                {
                    DirectoryBlockz.Add(new BlockRecord());
                    DirectoryBlockz[DirectoryBlockz.Count - 1].ThisBlock = xcurblock++;
                }
                xFileBlocks = DirectoryBlockz.ToArray();
                xWriteChain(xFileBlocks);
                xSTFSStruct.xDirectoryBlockCount = (ushort)xFileBlocks.Length;
                ushort xCurID = 0;
                xSession.xFolderDirectory.Sort(sortpathct);
                foreach (var x in xSession.xFolderDirectory)
                {
                    ushort pointer = 0xFFFF;
                    if (x.xthispath.xPathCount() > 1)
                        pointer = xGetParentFolder(x.Path).EntryID;
                    xFolderDirectory.Add(new FolderEntry(x.Name, 0, xCurID++, pointer, this));
                    xFolderDirectory[xFolderDirectory.Count - 1].xFixOffset();
                }
                //var debug = System.Windows.Forms.Application.StartupPath + "\\debug.txt";
                foreach (var x in xSession.xFileDirectory)
                {
                    ushort pointer = 0xFFFF;
                    if (x.xthispath.xPathCount() > 1)
                        try
                        {
                            pointer = xGetParentFolder(x.Path).EntryID;
                        }
                        catch (Exception)
                        {
                            /*var sw = new StreamWriter(debug, true);
                            sw.WriteLine(x.xthispath);
                            sw.Close();
                            continue;*/
                        }                        
                    xFileDirectory.Add(new FileEntry(x.Name, x.GetLength(), false, xCurID++, pointer, this));
                    var xAlloc = new List<BlockRecord>();
                    for (uint i = 0; i < x.BlockCount(); i++)
                    {
                        xAlloc.Add(new BlockRecord());
                        xAlloc[xAlloc.Count - 1].ThisBlock = xcurblock++;
                    }
                    xFileDirectory[xFileDirectory.Count - 1].xBlockCount = (uint)xAlloc.Count;
                    xFileDirectory[xFileDirectory.Count - 1].xStartBlock = xAlloc[0].ThisBlock;
                    xFileDirectory[xFileDirectory.Count - 1].xPackage = this;
                    xFileDirectory[xFileDirectory.Count - 1].xFixOffset();
                    xWriteChain(xAlloc.ToArray());
                }
                //AddToLog("Writing Entry Table");
                DJsIO xent;
                if (!xEntriesToFile(out xent))
                    throw new Exception();
                xWriteTo(ref xent, xFileBlocks);
                xent.Close();
                VariousFunctions.DeleteFile(xent.FileNameLong);
                //AddToLog("Writing Files");
                uint curblck = xSession.GetDirectoryCount;
                foreach (var z in xSession.xFileDirectory)
                {
                    var w = new List<BlockRecord>();
                    var ct = z.BlockCount();
                    for (uint y = 0; y < ct; y++)
                    {
                        w.Add(new BlockRecord());
                        w[w.Count - 1].ThisBlock = curblck++;
                    }
                    DJsIO x = null;
                    try
                    {
                        x = new DJsIO(z.FileLocale, DJFileMode.Open, true);
                        xWriteTo(ref x, w.ToArray());
                    }
                    catch { }
                    if (x != null)
                        x.Dispose();
                }
                xWriteTables();
                xWriteHeader(xSigning);
                xIO.Close();
                VariousFunctions.MoveFile(xIO.FileNameLong, xOutPath);
                xIO = new DJsIO(xOutPath, DJFileMode.Open, true);
                xActive = false;
            }
            catch (Exception) { xFileDirectory = null; xFolderDirectory = null; xIO.Dispose(); throw; }
        }

        /// <summary>
        /// Function for partial classes, importing packages
        /// </summary>
        /// <param name="xIn"></param>
        protected STFSPackage(ref STFSPackage xIn) { xActive = true; SetSamePackage(ref xIn); xActive = false; }
        #endregion

        #region Public Methods
        /* Structure Fixing */
        /// <summary>
        /// Updates the information in the header
        /// </summary>
        /// <returns></returns>
        public bool UpdateHeader(RSAParams xParams)
        {
            if (!ActiveCheck())
                return false;
            try
            {
                var xSuccess = xWriteHeader(xParams);
                xActive = false;
                return xSuccess;
            }
            catch (Exception) { xActive = false; throw; }
        }

        /// <summary>
        /// Writes Tables and updates Header
        /// </summary>
        /// <param name="xParams"></param>
        /// <returns></returns>
        public bool FlushPackage(RSAParams xParams)
        {
            if (!ActiveCheck())
                return false;
            try
            {
                var xsucceeded = (xWriteTables() && xWriteHeader(xParams));
                xActive = false;
                return xsucceeded;
            }
            catch (Exception) { xActive = false; throw; }
        }

        /* Structure Verification */
        /// <summary>
        /// Returns a List of details containing the package
        /// </summary>
        /// <returns></returns>
        public Verified[] VerifyHashTables()
        {
            if (!ActiveCheck())
                return null;
            var xReturn = new List<Verified>();
            try
            {
                // Verifies each level needed
                for (uint i = 0; i < xSTFSStruct.BlockCount; i++)
                {
                    var lvl = GetRecord(i, TreeLevel.L1);
                    if (lvl.BlocksFree >= Constants.BlockLevel[0]) continue;
                    var xDataBlock = GenerateDataOffset(i);
                    if (xDataBlock >= xIO.Length) continue;
                    var xHashLocale = xSTFSStruct.GenerateHashOffset(i, 0) + (lvl.Index << 0xC);
                    xIO.Position = xHashLocale;
                    var xHash = xIO.ReadBytes(20);
                    xReturn.Add(new Verified(ItemType.Data, XVerifyHash(xDataBlock, 0x1000, ref xHash), xDataBlock, xHashLocale));
                }
                if (STFSStruct.BlockCount > Constants.BlockLevel[0])
                {
                    var ct = (((xSTFSStruct.xBlockCount - 1) / Constants.BlockLevel[0]) + 1);
                    for (uint i = 0; i < ct; i++)
                    {
                        var lvl = GetRecord(i * Constants.BlockLevel[0], TreeLevel.L2);
                        var current = GetRecord(i * Constants.BlockLevel[0], TreeLevel.L1);
                        if (lvl.BlocksFree >= Constants.BlockLevel[1] && current.BlocksFree >= Constants.BlockLevel[0])
                            continue;
                        var xInputLocale = xSTFSStruct.GenerateBaseOffset(i * Constants.BlockLevel[0], TreeLevel.L0) + (current.Index << 0xC);
                        if (xInputLocale >= xIO.Length) continue;
                        var xHashLocale = xSTFSStruct.GenerateHashOffset((i * Constants.BlockLevel[0]), TreeLevel.L1) + (lvl.Index << 0xC);
                        xIO.Position = xHashLocale;
                        var xHash = xIO.ReadBytes(20);
                        xReturn.Add(new Verified(ItemType.TableTree0, XVerifyHash(xInputLocale, 0x1000, ref xHash), xInputLocale, xHashLocale));
                    }
                    if (STFSStruct.BlockCount > Constants.BlockLevel[1])
                    {
                        ct = (((xSTFSStruct.xBlockCount - 1) / Constants.BlockLevel[1]) + 1);
                        for (uint i = 0; i < ct; i++)
                        {
                            var current = GetRecord(i * Constants.BlockLevel[1], TreeLevel.L2);
                            if (current.BlocksFree >= Constants.BlockLevel[1]) continue;
                            var xInputLocale = xSTFSStruct.GenerateBaseOffset((i * Constants.BlockLevel[1]), TreeLevel.L1) + (current.Index << 0xC);
                            var xHashLocale = GenerateHashOffset((i * Constants.BlockLevel[1]), TreeLevel.L2);
                            xIO.Position = xHashLocale;
                            var xHash = xIO.ReadBytes(20);
                            xReturn.Add(new Verified(ItemType.TableTree1, XVerifyHash(xInputLocale, 0x1000, ref xHash), xInputLocale, xHashLocale));
                        }
                    }
                }
                xActive = false;
                return xReturn.ToArray();
            }
            catch (Exception) { xActive = false; throw; }
        }

        /// <summary>
        /// Verify the header
        /// </summary>
        /// <returns></returns>
        public Verified[] VerifyHeader()
        {
            if (!ActiveCheck())
                return null;
            try
            {
                var xReturn = new List<Verified>();
                // Verifies master hash with currently written header
                xIO.Position = 0x395;
                xIO.IsBigEndian = true;
                var xBlockCount = xIO.ReadInt32();
                long xLocale;
                if (xBlockCount <= Constants.BlockLevel[0])
                    xLocale = GenerateBaseOffset(0, 0);
                else if (xBlockCount <= Constants.BlockLevel[1])
                    xLocale = GenerateBaseOffset(0, TreeLevel.L1);
                else xLocale = GenerateBaseOffset(0, TreeLevel.L2);
                xIO.Position = 0x381;
                var xHash = xIO.ReadBytes(20);
                xReturn.Add(new Verified(ItemType.Master, XVerifyHash(xLocale, 0x1000, ref xHash), xLocale, (0x381)));
                // Verifies currently written header
                var xSize = xSTFSStruct.BaseBlock == 0xA000 ? 0x9CBC : 0xACBC;
                xIO.Position = 0x32C;
                xHash = xIO.ReadBytes(20);
                xReturn.Add(new Verified(ItemType.Header, XVerifyHash(0x344, xSize, ref xHash), 0x344, 0x32C));
                switch (xHeader.Magic)
                {
                    case PackageMagic.CON:
                        {
                            // Verifies Certificate
                            var xRSAKeyz = new RSAParameters
                                {
                                    Exponent = new byte[] {0, 0, 0, 3},
                                    Modulus = Nautilus.Properties.Resources.XK6
                                };
                            xIO.Position = 4;
                            var xCert = xIO.ReadBytes(0xA8);
                            var xSig = xIO.ReadBytes(0x100);
                            xReturn.Add(new Verified(ItemType.Certificate, RSAQuick.SignatureVerify(xRSAKeyz, SHA1Quick.ComputeHash(xCert), ScrambleMethods.StockScramble(xSig, true)), 4, 0xAC));
                            xReturn.Add(VerifySignature(false)); // Doesn't matter, same thing for CON
                            xActive = false;
                            return xReturn.ToArray();
                        }
                    default:
                        {
                            xReturn.Add(VerifySignature(false));
                            xReturn.Add(VerifySignature(true));
                            xActive = false;
                            return xReturn.ToArray();
                        }
                }
            }
            catch { xActive = false; throw STFSExcepts.General; }
        }

        /* Entry Functions */
        /// <summary>
        /// Adds a folder to the package via the root
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        public bool AddFolder(string FolderPath)
        {
            if (!ActiveCheck())
                return false;
            try
            {
                if (xFileDirectory.Count + xFolderDirectory.Count + 1 >= 0x65535)
                    return (xActive = false);
                FolderPath = FolderPath.Replace("\\", "/");
                if (FolderPath[0] == '/')
                    FolderPath = FolderPath.Substring(1, FolderPath.Length - 1);
                if (FolderPath[FolderPath.Length - 1] == '/')
                    FolderPath = FolderPath.Substring(0, FolderPath.Length - 1);
                var folds = FolderPath.Split(new[] { '/' }).ToList();
                foreach (var x in folds)
                    x.IsValidXboxName();
                folds.RemoveAt(folds.Count - 1);
                ushort[] curid = {0xFFFF};
                foreach (var x in folds)
                {
                    var found = false;
                    var x1 = x;
                    foreach (var y in xFolderDirectory.Where(y => y.FolderPointer == curid[0] && y.Name.ToLowerInvariant() == x1.ToLowerInvariant()))
                    {
                        found = true;
                        curid[0] = y.FolderPointer;
                        if (y.Name == x)
                            break;
                    }
                    if (!found)
                        return (xActive = false);
                }
                if (xFileDirectory.Count + xFolderDirectory.Count + 1 >= 0x65535)
                    return (xActive = false);
                return true;
            }
            catch { return (xActive = false); }
        }

        /// <summary>
        /// Extracts the package via out location
        /// </summary>
        /// <param name="xOutLocale"></param>
        /// <param name="xIncludeSubItems"></param>
        /// <param name="xIncludeHeader"></param>
        /// <returns></returns>
        public bool ExtractPayload(string xOutLocale, bool xIncludeSubItems, bool xIncludeHeader)
        {
            return ActiveCheck() && xExtractPayload(xOutLocale, xIncludeSubItems, xIncludeHeader);
        }

        /// <summary>
        /// GUI based extraction
        /// </summary>
        /// <param name="xIncludeSubItems"></param>
        /// <param name="xDescription"></param>
        /// <param name="xIncludeHeader"></param>
        /// <returns></returns>
        public bool ExtractPayload(bool xIncludeSubItems, string xDescription, bool xIncludeHeader)
        {
            if (!ActiveCheck())
                return false;
            var y = VariousFunctions.GetUserFolderLocale(xDescription);
            if (y != null)
                return xExtractPayload(y, xIncludeSubItems, xIncludeHeader);
            return (xActive = false);
        }

        /// <summary>
        /// Gets a file by path
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public FileEntry GetFile(string Path)
        {
            if (!ActiveCheck())
                return null;
            try
            {
                if (string.IsNullOrWhiteSpace(Path))
                    throw new Exception();
                Path = Path.Replace("\\", "/");
                if (Path[0] == '/')
                    Path = Path.Substring(1, Path.Length - 1);
                if (Path[Path.Length - 1] == '/')
                    Path = Path.Substring(0, Path.Length - 1);
                var parent = xGetParentFolder(Path);
                if (parent == null)
                    throw new Exception();
                var file = Path.Split(new[] { '/' }).LastValue();
                if (string.IsNullOrWhiteSpace(file))
                    throw new Exception();
                var z = xGetFile(file, parent.EntryID);
                xActive = false;
                return z;
            }
            catch { xActive = false; return null; }
        }

        /// <summary>
        /// Gets a file by the name and pointer
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="FolderPointer"></param>
        /// <returns></returns>
        public FileEntry GetFile(string Name, ushort FolderPointer)
        {
            if (!ActiveCheck())
                return null;
            try
            {
                var xReturn = xGetFile(Name, FolderPointer);
                xActive = false;
                return xReturn;
            }
            catch { xActive = false; return null; }
        }

        /// <summary>
        /// Gets a folder by it's ID
        /// </summary>
        /// <param name="FolderID"></param>
        /// <returns></returns>
        public FolderEntry GetFolder(ushort FolderID)
        {
            if (!ActiveCheck())
                return null;
            try
            {
                var xReturn = xGetFolder(FolderID);
                xActive = false;
                return xReturn;
            }
            catch { xActive = false; return null; }
        }

        /// <summary>
        /// Gets a folder by it's path
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public FolderEntry GetFolder(string Path)
        {
            if (!ActiveCheck())
                return null;
            try
            {
                Path = Path.Replace("\\", "/");
                if (Path[0] == '/')
                    Path = Path.Substring(1, Path.Length - 1);
                if (Path[Path.Length - 1] == '/')
                    Path = Path.Substring(0, Path.Length - 1);
                var parent = xGetParentFolder(Path);
                if (parent == null)
                    throw new Exception();
                var folder = Path.Split(new[] { '/' }).LastValue();
                if (string.IsNullOrWhiteSpace(folder))
                    throw new Exception();
                var foldz = parent.xGetFolders();
                foreach (var x in foldz.Where(x => x.Name.ToLowerInvariant() == folder.ToLowerInvariant()))
                {
                    xActive = false;
                    return x;
                }
                throw new Exception();
            }
            catch { xActive = false; return null; }
        }

        /// <summary>
        /// Grabs the files via the pointer
        /// </summary>
        /// <param name="FolderPointer"></param>
        /// <returns></returns>
        public FileEntry[] GetFiles(ushort FolderPointer)
        {
            if (!ActiveCheck())
                return null;
            try
            {
                xActive = false;
                return xFileDirectory.Where(x => x.FolderPointer == FolderPointer).ToArray();
            }
            catch { xActive = false; return null; }
        }

        /// <summary>
        /// Grabs the files via the path
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        public FileEntry[] GetFiles(string FolderPath)
        {
            if (!ActiveCheck())
                return null;
            try
            {
                if (string.IsNullOrWhiteSpace(FolderPath))
                    throw new Exception();
                FolderPath = FolderPath.Replace("\\", "/");
                if (FolderPath[0] == '/')
                    FolderPath = FolderPath.Substring(1, FolderPath.Length - 1);
                if (FolderPath[FolderPath.Length - 1] == '/')
                    FolderPath = FolderPath.Substring(0, FolderPath.Length - 1);
                FolderPath += "/a"; // Fake a random name so i can just use the parent folder function
                var parent = xGetParentFolder(FolderPath);
                if (parent == null)
                    throw new Exception();
                xActive = false;
                return xFileDirectory.Where(x => x.FolderPointer == parent.FolderPointer).ToArray();
            }
            catch { xActive = false; return null; }
        }

        /// <summary>
        /// Adds a file under a Folder ID
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="xIOIn"></param>
        /// <param name="FolderID"></param>
        /// <param name="xType"></param>
        /// <returns></returns>
        public bool MakeFile(string Name, DJsIO xIOIn, ushort FolderID, AddType xType)
        {
            if (!ActiveCheck())
                return false;
            foreach (var x in xFileDirectory.Where(x => x.FolderPointer == FolderID && x.Name.ToLowerInvariant() == Name.ToLowerInvariant()))
            {
                if (xType == AddType.NoOverWrite)
                    return (xActive = false);
                return x.Replace(xIOIn);
            }
            try { return xAddFile(xIOIn, Name, FolderID); }
            catch { return (xActive = false); }
        }

        /// <summary>
        /// Adds a file
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="xIOIn"></param>
        /// <param name="xType"></param>
        /// <returns></returns>
        public bool MakeFile(string Path, DJsIO xIOIn, AddType xType)
        {
            if (!ActiveCheck())
                return false;
            if (string.IsNullOrWhiteSpace(Path))
                return (xActive = false);
            Path = Path.Replace("\\", "/");
            if (Path[0] == '/')
                Path = Path.Substring(1, Path.Length - 1);
            if (Path[Path.Length - 1] == '/')
                Path = Path.Substring(0, Path.Length - 1);
            var parent = xGetParentFolder(Path);
            if (parent == null)
                return (xActive = false);
            var file = Path.Split(new[] { '/' }).LastValue();
            if (string.IsNullOrWhiteSpace(file))
                return (xActive = false);
            var z = xGetFile(file, parent.FolderPointer);
            if (z != null && xType == AddType.NoOverWrite)
                return (xActive = false);
            if (z != null) return xType == AddType.Inject ? z.xInject(xIOIn) : z.xReplace(xIOIn);
            if (xFileDirectory.Count + xFolderDirectory.Count + 1 >= 65535)
                return (xActive = false);
            return xAddFile(xIOIn, file, parent.FolderPointer);
        }

        /* Misc Functions */
        /// <summary>
        /// Backs up this package to a specific location
        /// </summary>
        /// <param name="xOutLocation"></param>
        /// <returns>ya boi</returns>
        public bool MakeBackup(string xOutLocation)
        {
            if (!ActiveCheck())
                return false;
            try
            {
                File.Copy(xIO.FileNameLong, xOutLocation);
                return true;
            }
            catch { return (xActive = false); }
        }

        /// <summary>
        /// Rebuilds the package using package creation
        /// </summary>
        /// <param name="xParams"></param>
        /// <returns></returns>
        public bool RebuildPackage(RSAParams xParams)
        {
            if (!ActiveCheck())
                return false;
            if (!xParams.Valid)
                return (xActive = false);
            var x = new CreateSTFS {HeaderData = xHeader, STFSType = xSTFSStruct.ThisType};
            // Populate
            foreach (var y in xFolderDirectory)
                x.AddFolder(y.GetPath());
            foreach (var y in xFileDirectory)
            {
                var io = y.xGetTempIO(true);
                if (io == null || !io.Accessed) continue;
                io.Close();
                x.AddFile(io.FileNameLong, y.GetPath());
            }
            var xreturn = new STFSPackage(x, xParams, VariousFunctions.GetTempFileLocale());
            if (xreturn.ParseSuccess)
            {
                xIO.Close();
                xreturn.xIO.Close();
                if (!VariousFunctions.MoveFile(xreturn.xIO.FileNameLong, xIO.FileNameLong))
                    return (xActive = false);
                xreturn.xIO = xIO;
                SetSamePackage(ref xreturn);
                xIO.OpenAgain();
                return true;
            }
            xreturn.xIO.Close();
            VariousFunctions.DeleteFile(xreturn.xIO.FileNameLong);
            return (xActive = false);
        }

        /// <summary>
        /// Returns the name used for DLC names
        /// </summary>
        /// <returns></returns>
        public string GetCurrentDLCFileName()
        {
            if (!ActiveCheck())
                return null;
            var xReturn = dlcname();
            xActive = false;
            return xReturn;
        }
        #endregion

        #region Package IO stuff
        /// <summary>
        /// File location
        /// </summary>
        public string FileNameLong { get { return xIO.FileNameLong; } }
        /// <summary>
        /// File Name
        /// </summary>
        public string FileNameShort { get { return xIO.FileNameShort; } }
        /// <summary>
        /// File Path
        /// </summary>
        public string FilePath { get { return xIO.FilePath; } }
        /// <summary>
        /// File Extension
        /// </summary>
        public string FileExtension { get { return xIO.FileExtension; } }

        /// <summary>
        /// Close the IO
        /// </summary>
        /// <returns></returns>
        public bool CloseIO()
        {
            //if (xActive)
            //    return false;
            xActive = true;
            if (xIO != null)
            {
                xIO.Close();
            }
            return true;
        }
        #endregion
    }

    static class extenz
    {
        public static uint[] Reverse(this uint[] xIn)
        {
            var xreturn = new List<uint>(xIn);
            xreturn.Reverse();
            xIn = xreturn.ToArray();
            return xIn;
        }

        public static string LastValue(this string[] xIn)
        {
            return xIn.Length == 0 ? null : xIn[xIn.Length - 1];
        }

        public static bool ContainsBlock(this List<BlockRecord> x, BlockRecord y)
        {
            return x.Any(z => z.ThisBlock == y.ThisBlock);
        }
    }
}