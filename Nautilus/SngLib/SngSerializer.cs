using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using BinaryEx;
using Nautilus.Cysharp.Collections;

namespace Nautilus.Sng
{
    public static class SngSerializer
    {
        private const string FileIdentifier = "SNGPKG";
        private static readonly byte[] FileIdentifierBytes = Encoding.ASCII.GetBytes(FileIdentifier);

        [ThreadStatic]
        private static Vector<byte>[] dataIndexVectors;

        // values loop every 256 characters since 16 and 256 are aligned
        [ThreadStatic]
        private static byte[] loopLookup = new byte[256];

        private static void InitializeLookup(byte[] seed)
        {
            loopLookup = new byte[256];
            for (int i = 0; i < loopLookup.Length; i++)
            {
                loopLookup[i] = (byte)(i ^ seed[i & 0xF]);
            }
        }

        /// <summary>
        /// Precompute a lookup table for index values for masking algorithm
        /// as they will loop and always be the same for the same values of
        /// Vector<byte>.Count and seed
        /// </summary>
        private static void InitializeDataIndexVectors(byte[] seed)
        {
            InitializeLookup(seed);
            int vecSize = Vector<byte>.Count;
            int arraySize = loopLookup.Length / vecSize;
            dataIndexVectors = new Vector<byte>[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                dataIndexVectors[i] = new Vector<byte>(loopLookup, i * vecSize);
            }
        }

        private static void DoMaskData(Span<byte> data, long filePos)
        {
            int vecElements = Vector<byte>.Count;
            long totalVectorFilePos = (long)Math.Ceiling(filePos / (double)vecElements) * vecElements;
            int nonAlignedBytes = (int)(totalVectorFilePos - filePos);

            for (int i = 0; i < nonAlignedBytes; ++i, ++filePos)
            {
                data[i] = (byte)(data[i] ^ loopLookup[filePos & 0xFF]);
            }

            var remainingBytes = data.Length - nonAlignedBytes;
            int startPos = nonAlignedBytes;

            int vecSegments = remainingBytes / vecElements;
            int totalVecElements = vecSegments * vecElements;

            int lastByteIndex = totalVecElements + startPos;
            var lookupSizeMask = (loopLookup.Length / vecElements) - 1;

            long lookupIndex = filePos / vecElements;

            for (int vectorIndex = 0; vectorIndex < vecSegments; ++vectorIndex, ++lookupIndex)
            {
                int byteIndex = (vectorIndex * vecElements) + startPos;

                // ✅ Fix: Convert Span<byte> to byte[]
                Vector<byte> dataVector = new Vector<byte>(data.Slice(byteIndex, vecElements).ToArray());

                Vector<byte> maskedData = dataVector ^ dataIndexVectors[lookupIndex & lookupSizeMask];

                // ✅ Fix: Convert Span<byte> to byte[] before copying back
                byte[] tempArray = new byte[vecElements];
                maskedData.CopyTo(tempArray);
                tempArray.CopyTo(data.Slice(byteIndex, vecElements));
            }

            long endOfVecFilePos = filePos + totalVecElements;

            for (int i = lastByteIndex; i < data.Length; ++i, ++endOfVecFilePos)
            {
                data[i] = (byte)(data[i] ^ loopLookup[endOfVecFilePos & 0xFF]);
            }
        }

        /// <summary>
        /// An optimized Masking routine using Vector<byte> objects along with some pre-computation
        /// This is about 5-10x faster than the standard implementation on coreclr, in unity/mono it's slower
        /// </summary>
        private static void MaskData(NativeByteArray data, byte[] seed)
        {
            if (dataIndexVectors == null)
            {
                // Precompute the lookup table for xormask
                InitializeDataIndexVectors(seed);
            }
            long filePos = 0;
            foreach (var segment in data.AsMemoryList())
            {
                var sp = segment.Span;
                DoMaskData(sp, filePos);
                filePos += sp.Length;
            }
        }

        private static void EnsureValidLength(Stream data, ulong length)
        {
            if ((ulong)data.Length < length)
            {
                throw new FormatException("File data size does not match expected file size. This is likely an ancient sng file that is no longer supported. Please report this SNG file wherever it came from.");
            }
        }

        public static SngFile LoadSngFile(Stream stream)
        {
            using (var fs = stream)
            {// ✅ Use the provided stream (either FileStream or MemoryStream)
                return LoadSngFileInternal(fs);
            }
        }

        private static SngFile LoadSngFileInternal(Stream fs)
        {
            // Keep the original file-loading logic
            byte[] identifier = new byte[6];
            fs.Read(identifier, 0, identifier.Length);

            if (!FileIdentifierBytes.SequenceEqual(identifier))
                throw new FormatException("Invalid SNG file identifier");

            var sngFile = new SngFile
            {
                Version = fs.ReadUInt32LE(),
                XorMask = new byte[16]
            };
            fs.Read(sngFile.XorMask, 0, sngFile.XorMask.Length);

            if (sngFile.Version != SngFile.CurrentVersion)
                throw new NotSupportedException("Unsupported SNG file version");

            // Read metadata
            ulong metadataLen = fs.ReadUInt64LE();
            EnsureValidLength(fs, metadataLen);
            ulong metadataCount = fs.ReadUInt64LE();
            for (ulong i = 0; i < metadataCount; i++)
            {
                int keyLen = fs.ReadInt32LE();
                if (keyLen < 0)
                    throw new FormatException("Metadata Key length value cannot be negative");

                EnsureValidLength(fs, (ulong)keyLen);
                var keyBytes = new byte[keyLen];
                fs.Read(keyBytes, 0, keyBytes.Length); // ✅ FIXED
                string key = Encoding.UTF8.GetString(keyBytes);

                int valueLen = fs.ReadInt32LE();
                EnsureValidLength(fs, (ulong)valueLen);
                if (valueLen < 0)
                    throw new FormatException("Metadata Value length value cannot be negative");

                var valueBytes = new byte[valueLen];
                fs.Read(valueBytes, 0, valueBytes.Length); // ✅ FIXED
                string value = Encoding.UTF8.GetString(valueBytes);

                sngFile.SetString(key, value);
            }

            // Read file metadata
            ulong fileIndexLen = fs.ReadUInt64LE();
            EnsureValidLength(fs, fileIndexLen);
            ulong fileCount = fs.ReadUInt64LE();
            EnsureValidLength(fs, fileCount);

            var fileInfo = new (ulong index, ulong size, string fileName)[fileCount];
            for (ulong i = 0; i < fileCount; i++)
            {
                var fileNameLength = fs.ReadByte();
                if (fileNameLength < 0)
                    throw new FormatException("File name length value cannot be negative");

                var fileNameBytes = new byte[fileNameLength];
                fs.Read(fileNameBytes, 0, fileNameBytes.Length); // ✅ FIXED
                string fileName = Encoding.UTF8.GetString(fileNameBytes);

                ulong contentsLen = fs.ReadUInt64LE();
                EnsureValidLength(fs, contentsLen);
                ulong contentsIndex = fs.ReadUInt64LE();
                EnsureValidLength(fs, contentsIndex);
                fileInfo[i] = (contentsIndex, contentsLen, fileName);
            }

            var filesBytes = fs.ReadUInt64LE(); // File section length
            EnsureValidLength(fs, (ulong)fs.Position + filesBytes);

            foreach ((ulong pos, ulong size, string name) in fileInfo)
            {
                if (size > long.MaxValue)
                {
                    Console.WriteLine("Warning: This tool doesn't support loading files longer than 7 Exabytes, skipping file");
                    continue;
                }

                // Read file contents
                if (fs.Position != (long)pos)
                    fs.Seek((long)pos, SeekOrigin.Begin);

                var contents = new NativeByteArray((long)size, skipZeroClear: true);
                fs.ReadToNativeArray(contents, (long)size); // ✅ FIXED (Assumes `ReadToNativeArray` is correct)

                // Unmask data
                MaskData(contents, sngFile.XorMask);

                sngFile.AddFile(name, contents);
            }

            dataIndexVectors = null;

            return sngFile;
        }

        public static SngFile LoadSngFile(byte[] sngBytes)
        {
            using (var fs = new MemoryStream(sngBytes))
            {
                // Read header
                byte[] identifier = new byte[6];
                fs.Read(identifier, 0, identifier.Length); // ✅ FIXED

                if (!FileIdentifierBytes.SequenceEqual(identifier))
                    throw new FormatException("Invalid SNG file identifier");

                var sngFile = new SngFile
                {
                    Version = fs.ReadUInt32LE(),
                    XorMask = new byte[16]
                };
                fs.Read(sngFile.XorMask, 0, sngFile.XorMask.Length); // ✅ FIXED

                if (sngFile.Version != SngFile.CurrentVersion)
                    throw new NotSupportedException("Unsupported SNG file version");

                // Read metadata
                ulong metadataLen = fs.ReadUInt64LE();
                EnsureValidLength(fs, metadataLen);
                ulong metadataCount = fs.ReadUInt64LE();
                for (ulong i = 0; i < metadataCount; i++)
                {
                    int keyLen = fs.ReadInt32LE();
                    if (keyLen < 0)
                        throw new FormatException("Metadata Key length value cannot be negative");

                    EnsureValidLength(fs, (ulong)keyLen);
                    var keyBytes = new byte[keyLen];
                    fs.Read(keyBytes, 0, keyBytes.Length); // ✅ FIXED
                    string key = Encoding.UTF8.GetString(keyBytes);

                    int valueLen = fs.ReadInt32LE();
                    EnsureValidLength(fs, (ulong)valueLen);
                    if (valueLen < 0)
                        throw new FormatException("Metadata Value length value cannot be negative");

                    var valueBytes = new byte[valueLen];
                    fs.Read(valueBytes, 0, valueBytes.Length); // ✅ FIXED
                    string value = Encoding.UTF8.GetString(valueBytes);

                    sngFile.SetString(key, value);
                }

                // Read file metadata
                ulong fileIndexLen = fs.ReadUInt64LE();
                EnsureValidLength(fs, fileIndexLen);
                ulong fileCount = fs.ReadUInt64LE();
                EnsureValidLength(fs, fileCount);

                var fileInfo = new (ulong index, ulong size, string fileName)[fileCount];
                for (ulong i = 0; i < fileCount; i++)
                {
                    var fileNameLength = fs.ReadByte();
                    if (fileNameLength < 0)
                        throw new FormatException("File name length value cannot be negative");

                    var fileNameBytes = new byte[fileNameLength];
                    fs.Read(fileNameBytes, 0, fileNameBytes.Length); // ✅ FIXED
                    string fileName = Encoding.UTF8.GetString(fileNameBytes);

                    ulong contentsLen = fs.ReadUInt64LE();
                    EnsureValidLength(fs, contentsLen);
                    ulong contentsIndex = fs.ReadUInt64LE();
                    EnsureValidLength(fs, contentsIndex);
                    fileInfo[i] = (contentsIndex, contentsLen, fileName);
                }

                var filesBytes = fs.ReadUInt64LE(); // File section length
                EnsureValidLength(fs, (ulong)fs.Position + filesBytes);

                foreach ((ulong pos, ulong size, string name) in fileInfo)
                {
                    if (size > long.MaxValue)
                    {
                        Console.WriteLine("Warning: This tool doesn't support loading files longer than 7 Exabytes, skipping file");
                        continue;
                    }

                    // Read file contents
                    if (fs.Position != (long)pos)
                        fs.Seek((long)pos, SeekOrigin.Begin);

                    var contents = new NativeByteArray((long)size, skipZeroClear: true);
                    fs.ReadToNativeArray(contents, (long)size); // ✅ FIXED (Assumes `ReadToNativeArray` is correct)

                    // Unmask data
                    MaskData(contents, sngFile.XorMask);

                    sngFile.AddFile(name, contents);
                }

                dataIndexVectors = null;

                return sngFile;
            }
        }

        private static ulong CountNonNullFiles(IEnumerable<NativeByteArray> data)
        {
            ulong count = 0;
            foreach (var item in data)
            {
                if (item != null)
                {
                    count++;
                }
            }
            return count;
        }

        private static ulong CountNonNullMetadata(IDictionary<string, string> metadata)
        {
            ulong count = 0;
            foreach (KeyValuePair<string, string> pair in metadata) // ✅ Fix: Use KeyValuePair
            {
                string key = pair.Key;   // ✅ Fix: Explicitly access key
                string value = pair.Value; // ✅ Fix: Explicitly access value

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                {
                    continue;
                }
                count++;
            }
            return count;
        }

        private static long GetHeaderLength(this SngFile sngFile)
        {
            // version + SngIdentify + XorMask
            return sizeof(uint) + FileIdentifierBytes.Length + sngFile.XorMask.Length;
        }

        private static long GetMetadataLength(this SngFile sngFile)
        {
            long metadataLength = 0;
            metadataLength += sizeof(ulong); // metadata count

            foreach (KeyValuePair<string, string> pair in sngFile.Metadata) // ✅ Fix: Explicitly use KeyValuePair
            {
                string key = pair.Key;   // ✅ Fix: Explicitly extract key
                string value = pair.Value; // ✅ Fix: Explicitly extract value

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                {
                    continue;
                }
                metadataLength += sizeof(int) * 2; // key/value length
                metadataLength += Encoding.UTF8.GetByteCount(key);
                metadataLength += Encoding.UTF8.GetByteCount(value);
            }
            return metadataLength;
        }

        private static (long fileIndexLength, long fileSectionLength) GetFileLengths(this SngFile sngFile)
        {
            long fileIndexLength = 0;
            long fileSectionLength = 0;
            fileIndexLength += sizeof(ulong); // File count

            foreach (KeyValuePair<string, NativeByteArray> pair in sngFile.Files) // ✅ Fix: Use KeyValuePair explicitly
            {
                string key = pair.Key;   // ✅ Fix: Extract key explicitly
                NativeByteArray value = pair.Value; // ✅ Fix: Extract value explicitly

                if (value == null)
                    continue;

                fileIndexLength += sizeof(byte); // fileName length
                fileIndexLength += sizeof(ulong) * 2; // length/index
                fileIndexLength += Encoding.UTF8.GetByteCount(key); // file name
                fileSectionLength += value.Length;
            }
            return (fileIndexLength, fileSectionLength);
        }

        private static void WriteHeader(this SngFile sngFile, byte[] bytesOut, ref int pos)
        {
            bytesOut.WriteBytes(ref pos, FileIdentifierBytes, FileIdentifierBytes.Length);
            bytesOut.WriteUInt32LE(ref pos, sngFile.Version);
            bytesOut.WriteBytes(ref pos, sngFile.XorMask, 16);
        }

        private static void WriteMetadata(this SngFile sngFile, long metadataSize, byte[] bytesOut, ref int pos)
        {
            bytesOut.WriteUInt64LE(ref pos, (ulong)metadataSize);
            bytesOut.WriteUInt64LE(ref pos, CountNonNullMetadata(sngFile.Metadata));
            foreach (var metadata in sngFile.Metadata)
            {
                if (string.IsNullOrEmpty(metadata.Key) || string.IsNullOrEmpty(metadata.Value))
                {
                    continue;
                }
                byte[] bytesKey = Encoding.UTF8.GetBytes(metadata.Key);

                bytesOut.WriteInt32LE(ref pos, bytesKey.Length);
                bytesOut.WriteBytes(ref pos, bytesKey, bytesKey.Length);

                var bytesValue = Encoding.UTF8.GetBytes(metadata.Value);
                bytesOut.WriteInt32LE(ref pos, bytesValue.Length);
                bytesOut.WriteBytes(ref pos, bytesValue, bytesValue.Length);
            }
        }

        private static void WriteFileIndex(this SngFile sngFile, long fileIndexSize, ulong startOfFileIndex, byte[] bytesOut, ref int pos)
        {
            bytesOut.WriteUInt64LE(ref pos, (ulong)fileIndexSize);
            bytesOut.WriteUInt64LE(ref pos, (ulong)CountNonNullFiles(sngFile.Files.Values));
            ulong fileOffset = startOfFileIndex;

            foreach (KeyValuePair<string, NativeByteArray> pair in sngFile.Files) // ✅ Fix: Use KeyValuePair explicitly
            {
                string key = pair.Key;   // ✅ Fix: Extract key explicitly
                NativeByteArray value = pair.Value; // ✅ Fix: Extract value explicitly

                if (value == null)
                    continue;

                byte[] fileNameBytes = Encoding.UTF8.GetBytes(key);
                byte filenameLength = (byte)fileNameBytes.Length;
                bytesOut.WriteByte(ref pos, filenameLength);
                bytesOut.WriteBytes(ref pos, fileNameBytes, filenameLength);
                bytesOut.WriteUInt64LE(ref pos, (ulong)value.Length);
                bytesOut.WriteUInt64LE(ref pos, fileOffset);
                fileOffset += (ulong)value.Length;
            }
        }

        public static void SaveSngFile(SngFile sngFile, string path)
        {
            if (sngFile.Version != SngFile.CurrentVersion)
                throw new NotSupportedException("Unsupported SNG file version");

            // Calculate full file size for allocating a MemoryMappedFile
            // We need to keep the individual lengths around for writing out the header as well

            // header section
            long headerSize = sngFile.GetHeaderLength();

            // metadata section
            long metaDataPreLength = sizeof(ulong);
            long metaDataLength = sngFile.GetMetadataLength();

            // fileIndex and File sections
            long fileIndexPreLength = sizeof(ulong);
            long filePreLength = sizeof(ulong);
            (long fileIndexLength, long fileSectionLength) = sngFile.GetFileLengths();

            // long totalLength = headerSize + metaDataPreLength + metaDataLength + fileIndexPreLength + fileIndexLength + filePreLength + fileSectionLength;
            long lengthMinusData = headerSize + metaDataPreLength + metaDataLength + fileIndexPreLength + fileIndexLength + filePreLength;

            int pos = 0;
            var headerData = new byte[lengthMinusData];
            sngFile.WriteHeader(headerData, ref pos);
            sngFile.WriteMetadata(metaDataLength, headerData, ref pos);
            sngFile.WriteFileIndex(fileIndexLength, (ulong)headerData.Length, headerData, ref pos);
            headerData.WriteUInt64LE(ref pos, (ulong)fileSectionLength);

            using (var fs = File.OpenWrite(path))
            {
                fs.Write(headerData, 0, headerData.Length);
                // Write file contents
                foreach (var fileEntry in sngFile.Files)
                {
                    if (fileEntry.Value == null)
                        continue;

                    var contents = fileEntry.Value;
                    MaskData(contents, sngFile.XorMask);

                    fs.WriteFromNativeArray(contents);
                }
            }

            dataIndexVectors = null;
        }
    }
}