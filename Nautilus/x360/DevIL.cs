using System;
using System.Runtime.InteropServices;

namespace Nautilus.x360
{
    public static class Il
    {
        private const CallingConvention CALLING_CONVENTION = CallingConvention.Winapi;
        private const string DEVIL_NATIVE_LIBRARY = "DevIL.dll";
        public const int IL_FALSE = 0;
        public const int IL_TRUE = 1;
        public const int IL_COLOUR_INDEX = 6400;
        public const int IL_COLOR_INDEX = 6400;
        public const int IL_RGB = 6407;
        public const int IL_RGBA = 6408;
        public const int IL_BGR = 32992;
        public const int IL_BGRA = 32993;
        public const int IL_LUMINANCE = 6409;
        public const int IL_LUMINANCE_ALPHA = 6410;
        public const int IL_BYTE = 5120;
        public const int IL_UNSIGNED_BYTE = 5121;
        public const int IL_SHORT = 5122;
        public const int IL_UNSIGNED_SHORT = 5123;
        public const int IL_INT = 5124;
        public const int IL_UNSIGNED_INT = 5125;
        public const int IL_FLOAT = 5126;
        public const int IL_DOUBLE = 5130;
        public const int IL_VENDOR = 7936;
        public const int IL_LOAD_EXT = 7937;
        public const int IL_SAVE_EXT = 7938;
        public const int IL_VERSION_1_6_8 = 1;
        public const int IL_VERSION = 168;
        public const int IL_ORIGIN_BIT = 1;
        public const int IL_FILE_BIT = 2;
        public const int IL_PAL_BIT = 4;
        public const int IL_FORMAT_BIT = 8;
        public const int IL_TYPE_BIT = 16;
        public const int IL_COMPRESS_BIT = 32;
        public const int IL_LOADFAIL_BIT = 64;
        public const int IL_FORMAT_SPECIFIC_BIT = 128;
        public const int IL_ALL_ATTRIB_BITS = 1048575;
        public const int IL_PAL_NONE = 1024;
        public const int IL_PAL_RGB24 = 1025;
        public const int IL_PAL_RGB32 = 1026;
        public const int IL_PAL_RGBA32 = 1027;
        public const int IL_PAL_BGR24 = 1028;
        public const int IL_PAL_BGR32 = 1029;
        public const int IL_PAL_BGRA32 = 1030;
        public const int IL_TYPE_UNKNOWN = 0;
        public const int IL_BMP = 1056;
        public const int IL_CUT = 1057;
        public const int IL_DOOM = 1058;
        public const int IL_DOOM_FLAT = 1059;
        public const int IL_ICO = 1060;
        public const int IL_JPG = 1061;
        public const int IL_JFIF = 1061;
        public const int IL_LBM = 1062;
        public const int IL_PCD = 1063;
        public const int IL_PCX = 1064;
        public const int IL_PIC = 1065;
        public const int IL_PNG = 1066;
        public const int IL_PNM = 1067;
        public const int IL_SGI = 1068;
        public const int IL_TGA = 1069;
        public const int IL_TIF = 1070;
        public const int IL_CHEAD = 1071;
        public const int IL_RAW = 1072;
        public const int IL_MDL = 1073;
        public const int IL_WAL = 1074;
        public const int IL_LIF = 1076;
        public const int IL_MNG = 1077;
        public const int IL_JNG = 1077;
        public const int IL_GIF = 1078;
        public const int IL_DDS = 1079;
        public const int IL_DCX = 1080;
        public const int IL_PSD = 1081;
        public const int IL_EXIF = 1082;
        public const int IL_PSP = 1083;
        public const int IL_PIX = 1084;
        public const int IL_PXR = 1085;
        public const int IL_XPM = 1086;
        public const int IL_HDR = 1087;
        public const int IL_JASC_PAL = 1141;
        public const int IL_NO_ERROR = 0;
        public const int IL_INVALID_ENUM = 1281;
        public const int IL_OUT_OF_MEMORY = 1282;
        public const int IL_FORMAT_NOT_SUPPORTED = 1283;
        public const int IL_INTERNAL_ERROR = 1284;
        public const int IL_INVALID_VALUE = 1285;
        public const int IL_ILLEGAL_OPERATION = 1286;
        public const int IL_ILLEGAL_FILE_VALUE = 1287;
        public const int IL_INVALID_FILE_HEADER = 1288;
        public const int IL_INVALID_PARAM = 1289;
        public const int IL_COULD_NOT_OPEN_FILE = 1290;
        public const int IL_INVALID_EXTENSION = 1291;
        public const int IL_FILE_ALREADY_EXISTS = 1292;
        public const int IL_OUT_FORMAT_SAME = 1293;
        public const int IL_STACK_OVERFLOW = 1294;
        public const int IL_STACK_UNDERFLOW = 1295;
        public const int IL_INVALID_CONVERSION = 1296;
        public const int IL_BAD_DIMENSIONS = 1297;
        public const int IL_FILE_READ_ERROR = 1298;
        public const int IL_FILE_WRITE_ERROR = 1298;
        public const int IL_LIB_GIF_ERROR = 1505;
        public const int IL_LIB_JPEG_ERROR = 1506;
        public const int IL_LIB_PNG_ERROR = 1507;
        public const int IL_LIB_TIFF_ERROR = 1508;
        public const int IL_LIB_MNG_ERROR = 1509;
        public const int IL_UNKNOWN_ERROR = 1535;
        public const int IL_ORIGIN_SET = 1536;
        public const int IL_ORIGIN_LOWER_LEFT = 1537;
        public const int IL_ORIGIN_UPPER_LEFT = 1538;
        public const int IL_ORIGIN_MODE = 1539;
        public const int IL_FORMAT_SET = 1552;
        public const int IL_FORMAT_MODE = 1553;
        public const int IL_TYPE_SET = 1554;
        public const int IL_TYPE_MODE = 1555;
        public const int IL_FILE_OVERWRITE = 1568;
        public const int IL_FILE_MODE = 1569;
        public const int IL_CONV_PAL = 1584;
        public const int IL_DEFAULT_ON_FAIL = 1586;
        public const int IL_USE_KEY_COLOUR = 1589;
        public const int IL_USE_KEY_COLOR = 1589;
        public const int IL_SAVE_INTERLACED = 1593;
        public const int IL_INTERLACE_MODE = 1594;
        public const int IL_QUANTIZATION_MODE = 1600;
        public const int IL_WU_QUANT = 1601;
        public const int IL_NEU_QUANT = 1602;
        public const int IL_NEU_QUANT_SAMPLE = 1603;
        public const int IL_MAX_QUANT_INDEXS = 1604;
        public const int IL_FASTEST = 1632;
        public const int IL_LESS_MEM = 1633;
        public const int IL_DONT_CARE = 1634;
        public const int IL_MEM_SPEED_HINT = 1637;
        public const int IL_USE_COMPRESSION = 1638;
        public const int IL_NO_COMPRESSION = 1639;
        public const int IL_COMPRESSION_HINT = 1640;
        public const int IL_SUB_NEXT = 1664;
        public const int IL_SUB_MIPMAP = 1665;
        public const int IL_SUB_LAYER = 1666;
        public const int IL_COMPRESS_MODE = 1792;
        public const int IL_COMPRESS_NONE = 1793;
        public const int IL_COMPRESS_RLE = 1794;
        public const int IL_COMPRESS_LZO = 1795;
        public const int IL_COMPRESS_ZLIB = 1796;
        public const int IL_TGA_CREATE_STAMP = 1808;
        public const int IL_JPG_QUALITY = 1809;
        public const int IL_PNG_INTERLACE = 1810;
        public const int IL_TGA_RLE = 1811;
        public const int IL_BMP_RLE = 1812;
        public const int IL_SGI_RLE = 1813;
        public const int IL_TGA_ID_STRING = 1815;
        public const int IL_TGA_AUTHNAME_STRING = 1816;
        public const int IL_TGA_AUTHCOMMENT_STRING = 1817;
        public const int IL_PNG_AUTHNAME_STRING = 1818;
        public const int IL_PNG_TITLE_STRING = 1819;
        public const int IL_PNG_DESCRIPTION_STRING = 1820;
        public const int IL_TIF_DESCRIPTION_STRING = 1821;
        public const int IL_TIF_HOSTCOMPUTER_STRING = 1822;
        public const int IL_TIF_DOCUMENTNAME_STRING = 1823;
        public const int IL_TIF_AUTHNAME_STRING = 1824;
        public const int IL_JPG_SAVE_FORMAT = 1825;
        public const int IL_CHEAD_HEADER_STRING = 1826;
        public const int IL_PCD_PICNUM = 1827;
        public const int IL_PNG_ALPHA_INDEX = 1828;
        public const int IL_DXTC_FORMAT = 1797;
        public const int IL_DXT1 = 1798;
        public const int IL_DXT2 = 1799;
        public const int IL_DXT3 = 1800;
        public const int IL_DXT4 = 1801;
        public const int IL_DXT5 = 1802;
        public const int IL_DXT_NO_COMP = 1803;
        public const int IL_KEEP_DXTC_DATA = 1804;
        public const int IL_DXTC_DATA_FORMAT = 1805;
        public const int IL_3DC = 1806;
        public const int IL_RXGB = 1807;
        public const int IL_ATI1N = 1808;
        public const int IL_CUBEMAP_POSITIVEX = 1024;
        public const int IL_CUBEMAP_NEGATIVEX = 2048;
        public const int IL_CUBEMAP_POSITIVEY = 4096;
        public const int IL_CUBEMAP_NEGATIVEY = 8192;
        public const int IL_CUBEMAP_POSITIVEZ = 16384;
        public const int IL_CUBEMAP_NEGATIVEZ = 32768;
        public const int IL_VERSION_NUM = 3554;
        public const int IL_IMAGE_WIDTH = 3556;
        public const int IL_IMAGE_HEIGHT = 3557;
        public const int IL_IMAGE_DEPTH = 3558;
        public const int IL_IMAGE_SIZE_OF_DATA = 3559;
        public const int IL_IMAGE_BPP = 3560;
        public const int IL_IMAGE_BYTES_PER_PIXEL = 3560;
        public const int IL_IMAGE_BITS_PER_PIXEL = 3561;
        public const int IL_IMAGE_FORMAT = 3562;
        public const int IL_IMAGE_TYPE = 3563;
        public const int IL_PALETTE_TYPE = 3564;
        public const int IL_PALETTE_SIZE = 3565;
        public const int IL_PALETTE_BPP = 3566;
        public const int IL_PALETTE_NUM_COLS = 3567;
        public const int IL_PALETTE_BASE_TYPE = 3568;
        public const int IL_NUM_IMAGES = 3569;
        public const int IL_NUM_MIPMAPS = 3570;
        public const int IL_NUM_LAYERS = 3571;
        public const int IL_ACTIVE_IMAGE = 3572;
        public const int IL_ACTIVE_MIPMAP = 3573;
        public const int IL_ACTIVE_LAYER = 3574;
        public const int IL_CUR_IMAGE = 3575;
        public const int IL_IMAGE_DURATION = 3576;
        public const int IL_IMAGE_PLANESIZE = 3577;
        public const int IL_IMAGE_BPC = 3578;
        public const int IL_IMAGE_OFFX = 3579;
        public const int IL_IMAGE_OFFY = 3580;
        public const int IL_IMAGE_CUBEFLAGS = 3581;
        public const int IL_IMAGE_ORIGIN = 3582;
        public const int IL_IMAGE_CHANNELS = 3583;
        public const int IL_SEEK_SET = 0;
        public const int IL_SEEK_CUR = 1;
        public const int IL_SEEK_END = 2;
        public const int IL_EOF = -1;

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilActiveImage(int Number);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilActiveLayer(int Number);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilActiveMipmap(int Number);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilApplyPal(string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilApplyProfile(string InProfile, string OutProfile);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilBindImage(int Image);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilBlit(int Source, int DestX, int DestY, int DestZ, int SrcX, int SrcY, int SrcZ, int Width, int Height, int Depth);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilClearColour(float Red, float Green, float Blue, float Alpha);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilClearImage();

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilCloneCurImage();

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilCompressFunc(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilConvertImage(int DestFormat, int DestType);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilConvertPal(int DestFormat);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilCopyImage(int Src);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilCopyPixels(int XOff, int YOff, int ZOff, int Width, int Height, int Depth, int Format, int Type, IntPtr Data);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilCreateSubImage(int Type, int Num);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilDefaultImage();

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilDeleteImage(int Num);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilDeleteImages(int Num, ref int Image);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilDeleteImages(int Num, int[] Images);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilDisable(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilEnable(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilFormatFunc(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilGenImages(int Num, out int Images);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilGenImages(int Num, [Out] int[] Images);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilGenImage();

        [DllImport("bin\\DevIL.dll")]
        public static extern IntPtr ilGetAlpha(int Type);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilGetBoolean(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilGetBooleanv(int Mode, out bool Param);

        [DllImport("bin\\DevIL.dll")]
        public static extern IntPtr ilGetData();

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilGetDXTCData(IntPtr Buffer, int BufferSize, int DXTCFormat);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilGetError();

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilGetInteger(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilGetIntegerv(int Mode, out int Param);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilGetIntegerv(int Mode, [Out] int[] Param);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilGetLumpPos();

        [DllImport("bin\\DevIL.dll")]
        public static extern IntPtr ilGetPalette();

        [DllImport("bin\\DevIL.dll")]
        public static extern string ilGetString(int StringName);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilHint(int Target, int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilInit();

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilIsDisabled(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilIsEnabled(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilIsImage(int Image);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilIsValid(int Type, string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilIsValidF(int Type, IntPtr File);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilIsValidL(int Type, IntPtr Lump, int Size);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilIsValidL(int Type, byte[] Lump, int Size);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilKeyColour(float Red, float Green, float Blue, float Alpha);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoad(int Type, string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadF(int Type, IntPtr File);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadImage(string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadL(int Type, IntPtr Lump, int Size);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadL(int Type, byte[] Lump, int Size);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadPal(string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilModAlpha(double AlphaValue);

        [DllImport("bin\\DevIL.dll")]
        public static extern byte ilOriginFunc(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilOverlayImage(int Source, int XCoord, int YCoord, int ZCoord);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilPopAttrib();

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilPushAttrib(int Bits);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilRegisterFormat(int Format);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilRegisterLoad(string Ext, IL_LOADPROC Load);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilRegisterMipNum(int Num);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilRegisterNumImages(int Num);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilRegisterOrigin(int Origin);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilRegisterPal(IntPtr Pal, int Size, int Type);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilRegisterSave(string Ext, IL_SAVEPROC Save);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilRegisterType(int Type);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilRemoveLoad(string Ext);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilRemoveSave(string Ext);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilResetMemory();

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilResetRead();

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilResetWrite();

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilSave(int Type, string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilSaveF(int Type, IntPtr File);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilSaveImage(string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilSaveL(int Type, IntPtr Lump, int Size);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilSaveL(int Type, byte[] Lump, int Size);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilSavePal(string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilSetAlpha(double AlphaValue);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilSetData(IntPtr Data);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilSetDuration(int Duration);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilSetInteger(int Mode, int Param);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilSetMemory(mAlloc AllocFunc, mFree FreeFunc);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilSetPixels(int XOff, int YOff, int ZOff, int Width, int Height, int Depth, int Format, int Type, IntPtr Data);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilSetRead(fOpenRProc Open, fCloseRProc Close, fEofProc Eof, fGetcProc Getc, fReadProc Read, fSeekRProc Seek, fTellRProc Tell);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilSetString(int Mode, string str);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilSetWrite(fOpenWProc Open, fCloseWProc Close, fPutcProc Putc, fSeekWProc Seek, fTellWProc Tell, fWriteProc Write);

        [DllImport("bin\\DevIL.dll")]
        public static extern void ilShutDown();

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilTexImage(int Width, int Height, int Depth, byte numChannels, int Format, int Type, IntPtr Data);

        [DllImport("bin\\DevIL.dll")]
        public static extern int ilTypeFromExt(string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilTypeFunc(int Mode);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadData(string FileName, int Width, int Height, int Depth, byte Bpp);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadDataF(IntPtr File, int Width, int Height, int Depth, byte Bpp);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadDataL(IntPtr Lump, int Size, int Width, int Height, int Depth, byte Bpp);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadDataL(byte[] Lump, int Size, int Width, int Height, int Depth, byte Bpp);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilSaveData(string FileName);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilLoadFromJpegStruct(IntPtr JpegDecompressorPtr);

        [DllImport("bin\\DevIL.dll")]
        public static extern bool ilSaveFromJpegStruct(IntPtr JpegCompressorPtr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void fCloseRProc(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool fEofProc(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fGetcProc(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr fOpenRProc(string str);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fReadProc(IntPtr ptr, int a, int b, IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fSeekRProc(IntPtr handle, int a, int b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fTellRProc(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void fCloseWProc(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr fOpenWProc(string str);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fPutcProc(byte byt, IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fSeekWProc(IntPtr handle, int a, int b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fTellWProc(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fWriteProc(IntPtr ptr, int a, int b, IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void mAlloc(int a);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void mFree(IntPtr ptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int IL_LOADPROC(string str);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int IL_SAVEPROC(string str);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ILinfo
    {
        public int Id;
        public byte[] Data;
        public int Width;
        public int Height;
        public int Depth;
        public byte Bpp;
        public int SizeOfData;
        public int Format;
        public int Type;
        public int Origin;
        public byte[] Palette;
        public int PalType;
        public int PalSize;
        public int CubeFlags;
        public int NumNext;
        public int NumMips;
        public int NumLayers;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ILpointf
    {
        public float x;
        public float y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ILpointi
    {
        public int x;
        public int y;
    }

    public static class Ilu
    {
        private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
        private const string ILU_NATIVE_LIBRARY = "ILU.dll";
        public const int ILU_VERSION_1_6_8 = 1;
        public const int ILU_VERSION = 168;
        public const int ILU_FILTER = 9728;
        public const int ILU_NEAREST = 9729;
        public const int ILU_LINEAR = 9730;
        public const int ILU_BILINEAR = 9731;
        public const int ILU_SCALE_BOX = 9732;
        public const int ILU_SCALE_TRIANGLE = 9733;
        public const int ILU_SCALE_BELL = 9734;
        public const int ILU_SCALE_BSPLINE = 9735;
        public const int ILU_SCALE_LANCZOS3 = 9736;
        public const int ILU_SCALE_MITCHELL = 9737;
        public const int ILU_INVALID_ENUM = 1281;
        public const int ILU_OUT_OF_MEMORY = 1282;
        public const int ILU_INTERNAL_ERROR = 1284;
        public const int ILU_INVALID_VALUE = 1285;
        public const int ILU_ILLEGAL_OPERATION = 1286;
        public const int ILU_INVALID_PARAM = 1289;
        public const int ILU_PLACEMENT = 1792;
        public const int ILU_LOWER_LEFT = 1793;
        public const int ILU_LOWER_RIGHT = 1794;
        public const int ILU_UPPER_LEFT = 1795;
        public const int ILU_UPPER_RIGHT = 1796;
        public const int ILU_CENTER = 1797;
        public const int ILU_CONVOLUTION_MATRIX = 1808;
        public const int ILU_VERSION_NUM = 3554;
        public const int ILU_VENDOR = 7936;

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluAlienify();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluBlurAvg(int Iter);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluBlurGaussian(int Iter);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluBuildMipmaps();

        [DllImport("bin\\ILU.dll")]
        public static extern int iluColoursUsed();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluCompareImage(int Comp);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluContrast(float Contrast);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluCrop(int XOff, int YOff, int ZOff, int Width, int Height, int Depth);

        [DllImport("bin\\ILU.dll")]
        public static extern void iluDeleteImage(int Id);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluEdgeDetectE();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluEdgeDetectP();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluEdgeDetectS();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluEmboss();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluEnlargeCanvas(int Width, int Height, int Depth);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluEnlargeImage(float XDim, float YDim, float ZDim);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluEqualize();

        [DllImport("bin\\ILU.dll")]
        public static extern string iluErrorString(int Error);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluConvolution(int[] matrix, int scale, int bias);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluFlipImage();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluGammaCorrect(float Gamma);

        [DllImport("bin\\ILU.dll")]
        public static extern int iluGenImage();

        [DllImport("bin\\ILU.dll")]
        public static extern void iluGetImageInfo(out ILinfo Info);

        [DllImport("bin\\ILU.dll")]
        public static extern int iluGetInteger(int Mode);

        [DllImport("bin\\ILU.dll")]
        public static extern void iluGetIntegerv(int Mode, out int Param);

        [DllImport("bin\\ILU.dll")]
        public static extern string iluGetString(int StringName);

        [DllImport("bin\\ILU.dll")]
        public static extern void iluImageParameter(int PName, int Param);

        [DllImport("bin\\ILU.dll")]
        public static extern void iluInit();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluInvertAlpha();

        [DllImport("bin\\ILU.dll")]
        public static extern int iluLoadImage(string FileName);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluMirror();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluNegative();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluNoisify(float Tolerance);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluPixelize(int PixSize);

        [DllImport("bin\\ILU.dll")]
        public static extern void iluRegionfv(ILpointf[] Points, int n);

        [DllImport("bin\\ILU.dll")]
        public static extern void iluRegioniv(ILpointi[] Points, int n);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluReplaceColour(byte Red, byte Green, byte Blue, float Tolerance);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluRotate(float Angle);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluRotate3D(float x, float y, float z, float Angle);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluSaturate1f(float Saturation);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluSaturate4f(float r, float g, float b, float Saturation);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluScale(int Width, int Height, int Depth);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluScaleColours(float r, float g, float b);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluSharpen(float Factor, int Iter);

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluSwapColours();

        [DllImport("bin\\ILU.dll")]
        public static extern bool iluWave(float Angle);
    }
}