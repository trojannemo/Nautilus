using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Nautilus.x360
{
    /// <summary>
    /// Class to hold creation entries
    /// </summary>
    public class CItemEntry
    {
        [CompilerGenerated]
        internal CreateSTFS create;
        [CompilerGenerated]
        internal string xthispath;

        /// <summary>
        /// NOT CASE SENSITIVE
        /// </summary>
        public string Path { get { return xthispath; } }

        internal string getparentpath()
        {
            var idx = xthispath.LastIndexOf('/');
            return idx == -1 ? "" : xthispath.Substring(0, idx).ToLowerInvariant();
        }
        /// <summary>
        /// Item name
        /// </summary>
        public string Name
        {
            get { return xthispath.xExtractName(); }
            set
            {
                value.IsValidXboxName();
                if (value.Length > 0x28)
                    value = value.Substring(0, 0x28);
                var idx = xthispath.LastIndexOf('/');
                if (idx == -1)
                    xthispath = value;
                else xthispath = xthispath.Substring(0, idx) + "/" + value;
            }
        }

        internal CItemEntry(string path, ref CreateSTFS xCreate)
        {
            xthispath = path;
            create = xCreate;
        }
    }

    static class CreateTools
    {
        public static uint BlockCount(string file)
        {
            if (!File.Exists(file))
                return Constants.STFSEnd;
            var len = new FileInfo(file).Length;
            return (uint)(((len - 1) / 0x1000) + 1);
        }
    }

    /// <summary>
    /// Class to hold file entries
    /// </summary>
    public sealed class CFileEntry : CItemEntry
    {
        [CompilerGenerated]
        readonly string filelocale = "";

        /// <summary>
        /// Item location
        /// </summary>
        public string FileLocale { get { return filelocale; } }
        /// <summary>
        /// Block count of file
        /// </summary>
        /// <returns></returns>
        public uint BlockCount() { return CreateTools.BlockCount(filelocale); }

        /// <summary>
        /// Grabs the length of the item
        /// </summary>
        /// <returns></returns>
        public int GetLength() { return (int)new FileInfo(filelocale).Length; }

        internal CFileEntry(string xFile, string path, CreateSTFS xCreate)
            : base(path, ref xCreate) { filelocale = xFile; }

        internal CFileEntry(string xFile, string path, ref CreateSTFS xCreate)
            : base(path, ref xCreate) { filelocale = xFile; }
    }

    /// <summary>
    /// Object to hold directory entries
    /// </summary>
    public sealed class CFolderEntry : CItemEntry
    {
        internal CFolderEntry(string path, CreateSTFS xCreate)
            : base(path, ref xCreate) { }

        internal CFolderEntry(string path, ref CreateSTFS xCreate)
            : base(path, ref xCreate) { }

        /// <summary>
        /// Grabs the files under the directory
        /// </summary>
        /// <returns></returns>
        public CFileEntry[] GetFiles()
        {
            return create.xFileDirectory.Where(x => x.getparentpath() == xthispath.ToLowerInvariant()).ToArray();
        }
    }

    /// <summary>
    /// Sphere colors
    /// </summary>
    public enum SphereColor : byte
    {
        /// <summary>
        /// Default
        /// </summary>
        Default,
        /// <summary>
        /// Gray
        /// </summary>
        Gray,
        /// <summary>
        /// Black
        /// </summary>
        Black,
        /// <summary>
        /// Red-Pink
        /// </summary>
        RedPink,
        /// <summary>
        /// Yellow
        /// </summary>
        Yellow,
        /// <summary>
        /// Blue-Green
        /// </summary>
        BlueGreen,
        /// <summary>
        /// Baby Blue
        /// </summary>
        BabyBlue,
        /// <summary>
        /// Gray-Blue
        /// </summary>
        GrayBlue,
        /// <summary>
        /// Highlight Pink
        /// </summary>
        HighlightPink,
        /// <summary>
        /// Tan
        /// </summary>
        Tan,
        /// <summary>
        /// Brown
        /// </summary>
        Brown,
        /// <summary>
        /// Gold
        /// </summary>
        Gold,
        /// <summary>
        /// Green
        /// </summary>
        Green,
        /// <summary>
        /// Magenta
        /// </summary>
        Magenta,
        /// <summary>
        /// Blue
        /// </summary>
        Blue,
        /// <summary>
        /// Violet
        /// </summary>
        Violet,
        /// <summary>
        /// Light Gray
        /// </summary>
        LightGray
    }

    /// <summary>
    /// Dash Style
    /// </summary>
    public enum DashStyle : byte
    {
        /// <summary>
        /// Default
        /// </summary>
        Default,
        /// <summary>
        /// Dark
        /// </summary>
        Dark,
        /// <summary>
        /// Light
        /// </summary>
        Light
    }

    /// <summary>
    /// Theme Params
    /// </summary>
    public sealed class ThemeParams
    {
        /// <summary>
        /// Sphere color of instance
        /// </summary>
        public SphereColor Sphere { get; set; }
        /// <summary>
        /// Avatar Lighting Direction 0
        /// </summary>
        public decimal AvatarLightingDirectional0 { get; set; }
        /// <summary>
        /// Avatar Lighting Direction 1
        /// </summary>
        public decimal AvatarLightingDirectional1 { get; set; }
        /// <summary>
        /// Avatar Lighting Direction 2
        /// </summary>
        public decimal AvatarLightingDirectional2 { get; set; }
        /// <summary>
        /// Avatar Lighting Direction 3
        /// </summary>
        public uint AvatarLightingDirectional3 { get; set; }
        /// <summary>
        /// Avatar Ambient
        /// </summary>
        public uint AvatarLightingAmbient { get; set; }
        /// <summary>
        /// Style Type
        /// </summary>
        public DashStyle StyleType { get; set; }
        /// <summary>
        /// Creates an instance of this object
        /// </summary>
        public ThemeParams()
        {
            Sphere = SphereColor.Default;
            StyleType = DashStyle.Default;
            AvatarLightingAmbient = 0x383838FF;
            AvatarLightingDirectional0 = (decimal)-0.5;
            AvatarLightingDirectional1 = (decimal)-0.6123;
            AvatarLightingDirectional2 = -1;
            AvatarLightingDirectional3 = 0xB49664FF;
        }
    }

    /// <summary>
    /// Object to create a package
    /// </summary>
    public sealed class CreateSTFS
    {
        [CompilerGenerated]
        readonly uint[] xBckStp = { 0xAA, 0x70E4, 0 };
        [CompilerGenerated]
        internal List<CFileEntry> xFileDirectory = new List<CFileEntry>();
        [CompilerGenerated]
        internal List<CFolderEntry> xFolderDirectory = new List<CFolderEntry>();
        [CompilerGenerated]
        STFSType xStruct = STFSType.Type0;
        /// <summary>
        /// Header meta info
        /// </summary>
        [CompilerGenerated]
        public HeaderData HeaderData = new HeaderData();
        [CompilerGenerated]
        readonly ThemeParams xtheme = new ThemeParams();
        [CompilerGenerated]
        readonly CFolderEntry root;

        /// <summary>
        /// Root Path
        /// </summary>
        public CFolderEntry RootPath { get { return root; } }
        /// <summary>
        /// STFSType
        /// </summary>
        public STFSType STFSType
        {
            get { return xStruct; }
            set
            {
                if (value == STFSType.Type0 || value == STFSType.Type1)
                    xStruct = value;
                else xStruct = STFSType.Type0;
            }
        }
        internal uint[] BlockStep
        {
            get
            {
                xBckStp[2] = (uint)((xStruct == STFSType.Type0) ? 0xFE7DA : 0xFD00B);
                return xBckStp;
            }
        }
        /// <summary>
        /// Theme Settings
        /// </summary>
        public ThemeParams ThemeSettings { get { return xtheme; } }

        // Subtract 1 for prevention of Modular error
        internal byte GetDirectoryCount { get { return (byte)(((xFileDirectory.Count + xFolderDirectory.Count - 1) / 0x40) + 1); } }

        short UppedDirectCount { get { return (byte)(((xFileDirectory.Count + xFolderDirectory.Count - 1) / 0x40) + 1); } }

        internal uint TotalBlocks
        {
            get
            {
                return xFileDirectory.Aggregate<CFileEntry, uint>(GetDirectoryCount, (current, x) => current + x.BlockCount());
            }
        }

        uint UppedTotalBlocks(uint xFileAdd) { return (uint)(UppedDirectCount + xFileAdd); }
        /// <summary>
        /// Initializes an instance of this project
        /// </summary>
        public CreateSTFS() { root = new CFolderEntry("", this); }
        /// <summary>
        /// Adds a file via location and its path
        /// </summary>
        /// <param name="FileLocation"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public bool AddFile(string FileLocation, string FilePath)
        {
            if (UppedDirectCount >= 0x3FF ||
                UppedTotalBlocks(CreateTools.BlockCount(FileLocation)) > BlockStep[2] ||
                string.IsNullOrWhiteSpace(FilePath))
                return false;
            FilePath = FilePath.xExtractLegitPath();
            if (containsfile(FilePath))
                return false;
            xFileDirectory.Add(new CFileEntry(FileLocation, FilePath, this));
            return true;
        }
        /// <summary>
        /// Adds a folder
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        public bool AddFolder(string FolderPath)
        {
            if (FolderPath == null)
                return false;
            FolderPath = FolderPath.xExtractLegitPath();
            if (string.IsNullOrWhiteSpace(FolderPath)) return false;
            var idx = FolderPath.LastIndexOf('/');
            string name;
            if (idx == -1)
                name = FolderPath;
            else
            {
                name = FolderPath.Substring(idx + 1, FolderPath.Length - 1 - idx);
                var parentpath = FolderPath.Substring(0, idx);
                if (!containspath(parentpath))
                    return false;
            }
            if (containspath(FolderPath))
                return false;
            name.IsValidXboxName();
            xFolderDirectory.Add(new CFolderEntry(FolderPath, this));
            return true;
        }

        bool containspath(string path)
        {
            return xFolderDirectory.Any(x => x.Path.ToLowerInvariant() == path.ToLowerInvariant());
        }

        bool containsfile(string path)
        {
            return xFolderDirectory.Any(x => x.xthispath.ToLowerInvariant() == path.ToLowerInvariant());
        }

        /// <summary>
        /// Deletes a folder
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        public bool DeleteFolder(string FolderPath)
        {
            FolderPath = FolderPath.xExtractLegitPath();
            if (!containspath(FolderPath))
            {
                return false;
            }

            var index1 = 0;
            var index2 = 0;
            var index3 = 0;
            var index4 = 0;
            var index5 = 0;

            //delete inner folders up to 5 subfolders
            foreach (var a in xFolderDirectory.Where(a => a.getparentpath().ToLowerInvariant() == FolderPath.ToLowerInvariant()))
            {
                var a1 = a;
                foreach (var b in xFolderDirectory.Where(b => b.getparentpath().ToLowerInvariant() == a1.Path.ToLowerInvariant()))
                {
                    var b1 = b;
                    foreach (var c in xFolderDirectory.Where(c => c.getparentpath().ToLowerInvariant() == b1.Path.ToLowerInvariant()))
                    {
                        var c1 = c;
                        foreach (var d in xFolderDirectory.Where(d => d.getparentpath().ToLowerInvariant() == c1.Path.ToLowerInvariant()))
                        {
                            var d1 = d;
                            foreach (var e in xFolderDirectory.Where(e => e.getparentpath().ToLowerInvariant() == d1.Path.ToLowerInvariant()))
                            {
                                for (var i = 0; i < xFolderDirectory.Count; i++)
                                {
                                    if (xFolderDirectory[i].Path.ToLowerInvariant() != e.Path.ToLowerInvariant()) continue;
                                    index5 = i;
                                    for (var ind = 0; ind < xFileDirectory.Count; ind++)
                                    {
                                        if (xFileDirectory[ind].getparentpath().ToLowerInvariant() == e.Path.ToLowerInvariant())
                                            xFileDirectory.RemoveAt(ind);
                                    }
                                }
                            }
                            for (var i = 0; i < xFolderDirectory.Count; i++)
                            {
                                if (xFolderDirectory[i].Path.ToLowerInvariant() != d.Path.ToLowerInvariant()) continue;
                                index4 = i;
                                for (var ind = 0; ind < xFileDirectory.Count; ind++)
                                {
                                    if (xFileDirectory[ind].getparentpath().ToLowerInvariant() == d.Path.ToLowerInvariant())
                                        xFileDirectory.RemoveAt(ind);
                                }
                            }
                        }
                        for (var i = 0; i < xFolderDirectory.Count; i++)
                        {
                            if (xFolderDirectory[i].Path.ToLowerInvariant() != c.Path.ToLowerInvariant()) continue;
                            index3 = i;
                            for (var ind = 0; ind < xFileDirectory.Count; ind++)
                            {
                                if (xFileDirectory[ind].getparentpath().ToLowerInvariant() == c.Path.ToLowerInvariant())
                                    xFileDirectory.RemoveAt(ind);
                            }
                        }
                    }
                    for (var i = 0; i < xFolderDirectory.Count; i++)
                    {
                        if (xFolderDirectory[i].Path.ToLowerInvariant() != b.Path.ToLowerInvariant()) continue;
                        index2 = i;
                        for (var ind = 0; ind < xFileDirectory.Count; ind++)
                        {
                            if (xFileDirectory[ind].getparentpath().ToLowerInvariant() == b.Path.ToLowerInvariant())
                                xFileDirectory.RemoveAt(ind);
                        }
                    }
                }

                for (var i = 0; i < xFolderDirectory.Count; i++)
                {
                    if (xFolderDirectory[i].Path.ToLowerInvariant() != a.Path.ToLowerInvariant()) continue;
                    index1 = i;
                    for (var ind = 0; ind < xFileDirectory.Count; ind++)
                    {
                        if (xFileDirectory[ind].getparentpath().ToLowerInvariant() == a.Path.ToLowerInvariant())
                            xFileDirectory.RemoveAt(ind);
                    }
                }
            }

            //remove subfolders, from innermost out, down to 5 subfolders 
            //from the main folder being deleted
            if (index5 > 0)
            {
                xFolderDirectory.RemoveAt(index5);
            }
            if (index4 > 0)
            {
                xFolderDirectory.RemoveAt(index4);
            }
            if (index3 > 0)
            {
                xFolderDirectory.RemoveAt(index3);
            }
            if (index2 > 0)
            {
                xFolderDirectory.RemoveAt(index2);
            }
            if (index1 > 0)
            {
                xFolderDirectory.RemoveAt(index1);
            }

            //delete actual (outer) folder specified
            for (var i = 0; i < xFolderDirectory.Count; i++)
            {
                if (xFolderDirectory[i].Path.ToLowerInvariant() != FolderPath.ToLowerInvariant()) continue;
                for (var ind = 0; ind < xFileDirectory.Count; ind++)
                {
                    if (xFileDirectory[ind].getparentpath().ToLowerInvariant() == FolderPath.ToLowerInvariant())
                        xFileDirectory.RemoveAt(ind--);
                }
                xFolderDirectory.RemoveAt(i);
            }
            return true;
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public bool DeleteFile(string FilePath)
        {
            FilePath = FilePath.xExtractLegitPath();
            for (var i = 0; i < xFileDirectory.Count; i++)
            {
                if (xFileDirectory[i].xthispath == FilePath.ToLowerInvariant())
                    xFileDirectory.RemoveAt(i);
            }
            return true;
        }
        /// <summary>
        /// Grabs a file via path
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public CFileEntry GetFile(string FilePath)
        {
            FilePath = FilePath.xExtractLegitPath();
            return xFileDirectory.FirstOrDefault(x => x.Path.ToLowerInvariant() == FilePath.ToLowerInvariant());
        }

        /// <summary>
        /// Grabs a folder via path
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        public CFolderEntry GetFolder(string FolderPath)
        {
            FolderPath = FolderPath.xExtractLegitPath();
            return string.IsNullOrWhiteSpace(FolderPath) ? root : xFolderDirectory.FirstOrDefault(x => x.Path.ToLowerInvariant() == FolderPath.ToLowerInvariant());
        }
    }
}