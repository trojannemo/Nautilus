using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace C3Tools
{
    public class MoggFile
    {
        private CryptVersion _version;
        private int OggMapVersion { get; set; }
        private int OggBuffer { get; set; }
        public byte[] OggData { get; set; }
        private List<MoggChunk> OggChunks { get; set; }
        private byte[] PUBLIC_KEY { get; set; }
        private readonly NemoTools Tools;
        private readonly byte[] OggS = { 0x4F, 0x67, 0x67, 0x53 };
        private readonly byte[] HMXA = { 0x48, 0x4D, 0x58, 0x41 };
        private readonly byte[] HMX_PRIVATE_KEY_0B = { 0x37, 0xB2, 0xE2, 0xB9, 0x1C, 0x74, 0xFA, 0x9E, 0x38, 0x81, 0x08, 0xEA, 0x36, 0x23, 0xDB, 0xE4 };
        private static byte[] HMX_PRIVATE_KEY_0C = { 0x01, 0x22, 0x00, 0x38, 0xD2, 0x01, 0x78, 0x8B, 0xDD, 0xCD, 0xD0, 0xF0, 0xFE, 0x3E, 0x24, 0x7F };
        private static byte[] HMX_PRIVATE_KEY_0E = { 0x51, 0x73, 0xAD, 0xE5, 0xB3, 0x99, 0xB8, 0x61, 0x58, 0x1A, 0xF9, 0xB8, 0x1E, 0xA7, 0xBE, 0xBF };
        private static byte[] HMX_PRIVATE_KEY_0F = { 0xC6, 0x22, 0x94, 0x30, 0xD8, 0x3C, 0x84, 0x14, 0x08, 0x73, 0x7C, 0xF2, 0x23, 0xF6, 0xEB, 0x5A };
        private static byte[] HMX_PRIVATE_KEY_10 = { 0x02, 0x1A, 0x83, 0xF3, 0x97, 0xE9, 0xD4, 0xB8, 0x06, 0x74, 0x14, 0x6B, 0x30, 0x4C, 0x00, 0x91 };
        //private readonly byte[] C3_PRIVATE_KEY_C = { 0x2D, 0x68, 0x64, 0x6C, 0x94, 0x73, 0x17, 0x47, 0x97, 0x3D, 0x64, 0xDF, 0x89, 0x17, 0x63, 0x8B };
        private readonly byte[] C3_PRIVATE_KEY_D = { 0xC0, 0x87, 0x69, 0x00, 0xE2, 0x7C, 0x73, 0xEB, 0xCC, 0xD4, 0x21, 0x3D, 0x70, 0x2A, 0x4F, 0xED };
        //private readonly byte[] C3_PRIVATE_KEY_E = { 0x13, 0x9B, 0xC8, 0xE8, 0x23, 0x71, 0xAB, 0x89, 0x42, 0xA0, 0x82, 0x3F, 0xCF, 0xF4, 0x67, 0xCD };
        //private readonly byte[] C3_PRIVATE_KEY_F = { 0x95, 0x5F, 0x04, 0x59, 0x8D, 0xE7, 0xDA, 0x4B, 0x51, 0x05, 0x1C, 0x17, 0xA6, 0x74, 0xED, 0x47 };
        //private readonly byte[] C3_PRIVATE_KEY_10 = { 0x12, 0x1B, 0x44, 0x57, 0x5E, 0xC2, 0x18, 0x64, 0x70, 0xCC, 0x6C, 0xCA, 0x74, 0x76, 0xFD, 0x1A };
        private readonly byte[] C3_PRIVATE_KEY_PS3 = { 0x31, 0x98, 0xE0, 0x6D, 0x16, 0xB4, 0x80, 0x0D, 0xB1, 0xCA, 0xF9, 0x2C, 0xD8, 0xD5, 0x16, 0x82 };

        public MoggFile()
        {
            Clear();
            Tools = new NemoTools();
        }

        public void Clear()
        {
            OggData = new byte[] { };
            PUBLIC_KEY = new byte[] { };
            _version = CryptVersion.x0A;
            OggMapVersion = (int)CryptVersion.x0A;
            OggChunks = new List<MoggChunk>();
        }

        private bool LoadFromFile(byte[] mData)
        {
            using (var ms = new MemoryStream(mData))
            {
                using (var br = new BinaryReader(ms))
                {
                    _version = (CryptVersion)br.ReadInt32();
                    br.ReadInt32(); //OggOffset
                    OggMapVersion = br.ReadInt32();
                    OggBuffer = br.ReadInt32();
                    var entryCount = br.ReadInt32();
                    OggChunks = new List<MoggChunk>();
                    for (var i = 0; i < entryCount; i++)
                    {
                        var offset = br.ReadInt32();
                        var value = br.ReadInt32();
                        OggChunks.Add(new MoggChunk(offset, value));
                    }
                    if (_version != CryptVersion.x0A)
                    {
                        var keySize = _version == CryptVersion.x0B ? 0x10 : 0x48;
                        PUBLIC_KEY = br.ReadBytes(keySize);
                    }
                    var oggSize = (int)(br.BaseStream.Length - br.BaseStream.Position);
                    OggData = br.ReadBytes(oggSize);
                }
            }
            return true;
        }

        private void SaveToFile(string mOut, bool includeHeader, bool includeKey, bool encrypt = false)
        {
            Tools.DeleteFile(mOut);
            using (var fs = File.OpenWrite(mOut))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    if (includeHeader || encrypt)
                    {
                        bw.Write(encrypt ? (int)CryptVersion.x0D : (int)CryptVersion.x0A);
                        bw.Write(includeKey || encrypt ? HeaderSize : HeaderSize - PUBLIC_KEY.Length);
                        bw.Write(OggMapVersion);
                        bw.Write(OggBuffer);
                        bw.Write(OggChunks.Count);
                        foreach (var entry in OggChunks)
                        {
                            bw.Write(entry.Offset);
                            bw.Write(entry.Value);
                        }
                        if (includeKey || encrypt)
                        {
                            bw.Write(PUBLIC_KEY);
                        }
                    }
                    bw.Write(OggData);
                }
            }
        }

        /// <summary>
        /// Decrypts ogg data inside mogg file
        /// </summary>
        /// <param name="output">Path for decrypted file</param>
        /// <param>Ignored for RB1 moggs (null) | Game-generated otherwise</param>
        /// <param name="mData">Byte[] contents of mogg file</param>
        /// <param name="isC3">Use C3 private keys</param>
        /// <param name="includeHeader">Whether to write out the mogg header</param>
        /// <param name="includeKey">Whether to write out the decryption key</param>
        /// <param name="mode">Whether to write to file or to OggData byte array</param>
        public bool Decrypt(byte[] mData, bool isC3, bool includeHeader = true, bool includeKey = true, DecryptMode mode = DecryptMode.ToFile, string output = "")
        {
            CryptVersion version;
            byte[] PRIVATE_KEY;
            using (var br = new BinaryReader(new MemoryStream(mData)))
            {
                version = (CryptVersion)br.ReadInt32();
            }
            if (!EncryptionIsSupported(version)) return false;
            if (!LoadFromFile(mData)) return false;
            if (PUBLIC_KEY.Length != 0x10 && PUBLIC_KEY.Length != 0x48) return false;
            if (!isC3 && !Tools.HasMasterPassword()) return false;
            switch (_version)
            {
                case CryptVersion.x0B:
                    PRIVATE_KEY = HMX_PRIVATE_KEY_0B;
                    break;
                case CryptVersion.x0C:
                case CryptVersion.x0D:
                    PRIVATE_KEY = MoggCrypt.GenPrivateKey(ref HMX_PRIVATE_KEY_0C, ref mData, false);
                    break;
                case CryptVersion.x0E:
                    PRIVATE_KEY = MoggCrypt.GenPrivateKey(ref HMX_PRIVATE_KEY_0E, ref mData, true);
                    break;
                case CryptVersion.x0F:
                    PRIVATE_KEY = MoggCrypt.GenPrivateKey(ref HMX_PRIVATE_KEY_0F, ref mData, true);
                    break;
                case CryptVersion.x10:
                    PRIVATE_KEY = MoggCrypt.GenPrivateKey(ref HMX_PRIVATE_KEY_10, ref mData, true);
                    break;
                default:
                    return false;
            }
            var decrypted = MoggCrypt.CryptStream(new MemoryStream(OggData), PRIVATE_KEY, PUBLIC_KEY);
            if (!decrypted) return false;
            try
            {
                var header = new BinaryReader(new MemoryStream(OggData)).ReadBytes(4);
                if (header.SequenceEqual(HMXA))
                {
                    FixHMXAHeader(); //decryption succeeded but need to convert HMXA to OggS
                }
                else if (!header.SequenceEqual(OggS))
                {
                    return false; //decryption failed
                }
                if (mode == DecryptMode.ToMemory) return true;
                SaveToFile(output, includeHeader, includeKey);
            }
            catch (Exception)
            {
                return false;
            }
            return File.Exists(output);
        }

        private void FixHMXAHeader(bool encrypt = false)
        {
            using (var ms = new MemoryStream(OggData))
            {
                var HMXAHeader = new byte[0x3A];
                ms.Read(HMXAHeader, 0, HMXAHeader.Length);

                //transformation code by stackoverflow
                var Serial = BitConverter.ToUInt32(new[] { HMXAHeader[0x0F], HMXAHeader[0x0E], HMXAHeader[0x0D], HMXAHeader[0x0C] }, 0);
                var Checksum = BitConverter.ToUInt32(new[] { HMXAHeader[0x17], HMXAHeader[0x16], HMXAHeader[0x15], HMXAHeader[0x14] }, 0);

                var Mogg0x18 = BitConverter.ToUInt32(new[] { PUBLIC_KEY[0x18], PUBLIC_KEY[0x19], PUBLIC_KEY[0x1A], PUBLIC_KEY[0x1B] }, 0);
                var ChecksumCrypt = (Mogg0x18 ^ 0x36363636) * (0x00190000 + 0x0000660D) + 0x3C6F0000 - 0xCA1;

                var Mogg0x10 = BitConverter.ToUInt32(new[] { PUBLIC_KEY[0x10], PUBLIC_KEY[0x11], PUBLIC_KEY[0x12], PUBLIC_KEY[0x13] }, 0);
                var SerialCrypt = (Mogg0x10 ^ 0x5c5c5c5c) * (0x00190000 + 0x0000660d) + 0x3c6f0000 - 0xca1;
                SerialCrypt = (SerialCrypt * (0x00190000 + 0x660d)) + 0x3c6f0000 - 0xca1;

                SerialCrypt = SerialCrypt ^ Serial;
                ChecksumCrypt = ChecksumCrypt ^ Checksum;

                //write the values to the OggS data portion of the file
                ms.Seek(0, SeekOrigin.Begin);
                ms.Write(encrypt ? HMXA : OggS, 0, OggS.Length);
                var SerialBytes = ReverseBytes(BitConverter.GetBytes(SerialCrypt));
                ms.Seek(0x0C, SeekOrigin.Begin);
                ms.Write(SerialBytes, 0, SerialBytes.Length);
                var CheckSumBytes = ReverseBytes(BitConverter.GetBytes(ChecksumCrypt));
                ms.Seek(0x14, SeekOrigin.Begin);
                ms.Write(CheckSumBytes, 0, CheckSumBytes.Length);
            }
        }

        public bool Encrypt(byte[] mData, string mOut, bool doPS3 = false)
        {
            CryptVersion version;
            using (var br = new BinaryReader(new MemoryStream(mData)))
            {
                version = (CryptVersion)br.ReadInt32();
            }
            if (version != CryptVersion.x0A) return false;
            if (!LoadFromFile(mData)) return false;
            PUBLIC_KEY = doPS3 ? MoggCrypt.C3_PUBLIC_KEY_PS3 : MoggCrypt.C3_PUBLIC_KEY_NEW;
            FixHMXAHeader(true);
            var encrypted = MoggCrypt.CryptStream(new MemoryStream(OggData), doPS3 ? C3_PRIVATE_KEY_PS3 : C3_PRIVATE_KEY_D,
                            doPS3 ? MoggCrypt.C3_PUBLIC_KEY_PS3 : MoggCrypt.C3_PUBLIC_KEY_NEW);
            if (!encrypted) return false;
            SaveToFile(mOut, true, true, true);
            return File.Exists(mOut);
        }

        private static byte[] ReverseBytes(IList<byte> input)
        {
            var output = new byte[input.Count];
            for (var i = 0; i < input.Count; i++)
            {
                output[i] = input[input.Count - 1 - i];
            }
            return output;
        }

        private static bool EncryptionIsSupported(CryptVersion version)
        {
            switch (version)
            {
                //case CryptVersion.x0A: - already decrypted, not valid for decrypting
                case CryptVersion.x0B:
                case CryptVersion.x0C:
                case CryptVersion.x0D:
                case CryptVersion.x0E:
                case CryptVersion.x0F:
                case CryptVersion.x10:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Mogg Header Size
        /// </summary>
        private int HeaderSize
        {
            get
            {
                // Ogg data always starts after header
                return 20 + (OggChunks.Count * 8) + PUBLIC_KEY.Length;
            }
        }
    }

    public class MoggCrypt
    {
        private readonly ICryptoTransform crypto;
        private readonly byte[] cipherIn;
        private byte[] cipherOut;
        private int blockOffset;
        public static readonly byte[] C3_PUBLIC_KEY_B =
        {
            0x00, 0x00, 0x00, 0x00, 0x63, 0x33, 0x2D, 0x63, 0x75, 0x73, 0x74, 0x6F, 0x6D, 0x73, 0x31, 0x34
        };
        public static readonly byte[] C3_PUBLIC_KEY_C =
        {
            0x00, 0x00, 0x00, 0x00, 0x63, 0x75, 0x73, 0x74, 0x6F, 0x6D, 0x73, 0x2D, 0x62, 0x79, 0x2D, 0x63, 
            0x75, 0x73, 0x74, 0x6F, 0x6D, 0x73, 0x63, 0x72, 0x65, 0x61, 0x74, 0x6F, 0x72, 0x73, 0x63, 0x6F, 
            0x6C, 0x6C, 0x65, 0x63, 0x74, 0x69, 0x76, 0x65, 0x2D, 0x74, 0x6F, 0x6F, 0x6C, 0x73, 0x2D, 0x62, 
            0x79, 0x2D, 0x74, 0x72, 0x6F, 0x6A, 0x61, 0x6E, 0x6E, 0x65, 0x6D, 0x6F, 0x2D, 0x32, 0x30, 0x31, 
            0x35, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        public static readonly byte[] C3_PUBLIC_KEY_NEW =
        {
            0x00, 0x00, 0x00, 0x00, 0x63, 0x75, 0x73, 0x74, 0x6F, 0x6D, 0x73, 0x2D, 0x63, 0x72, 0x65, 0x61, 
            0x74, 0x6F, 0x72, 0x73, 0x00, 0x00, 0x00, 0x00, 0xC3, 0xC3, 0xC3, 0xC3, 0x00, 0x00, 0x00, 0x00, 
            0xC3, 0xC3, 0xC3, 0xC3, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B,
            0x0C, 0x0D, 0x0E, 0x0F, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xC3, 0xC3, 0xC3, 0xC3, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        public static readonly byte[] C3_PUBLIC_KEY_PS3 =
        {
            //key from PS3 RB2 Spoonman mogg
            0x00, 0x00, 0x00, 0x00, 0x34, 0x96, 0xC1, 0x1F, 0x05, 0x09, 0x12, 0x66, 0x9E, 0x20, 0x09, 0xEF, 
            0x30, 0x8B, 0xA2, 0x21, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x12, 0x6A, 0x47, 0x00, 0x00, 0x00, 0x00, 
            0xAC, 0x05, 0x7D, 0xF0, 0x8B, 0x29, 0x1D, 0x90, 0x2C, 0x58, 0x7A, 0x0C, 0x10, 0xC3, 0x53, 0xEB, 
            0x4E, 0x64, 0xEA, 0xDA, 0x41, 0xC8, 0xA5, 0xEC, 0x42, 0x26, 0x7F, 0x00, 0x04, 0xF0, 0x55, 0x27, 
            0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        private static readonly byte[] hiddenKeys = {
          0x7F, 0x95, 0x5B, 0x9D, 0x94, 0xBA, 0x12, 0xF1, 0xD7, 0x5A, 0x67, 0xD9, 0x16, 0x45, 0x28, 0xDD, 
          0x61, 0x55, 0x55, 0xAF, 0x23, 0x91, 0xD6, 0x0A, 0x3A, 0x42, 0x81, 0x18, 0xB4, 0xF7, 0xF3, 0x04, 
          0x78, 0x96, 0x5D, 0x92, 0x92, 0xB0, 0x47, 0xAC, 0x8F, 0x5B, 0x6D, 0xDC, 0x1C, 0x41, 0x7E, 0xDA, 
          0x6A, 0x55, 0x53, 0xAF, 0x20, 0xC8, 0xDC, 0x0A, 0x66, 0x43, 0xDD, 0x1C, 0xB2, 0xA5, 0xA4, 0x0C, 
          0x7E, 0x92, 0x5C, 0x93, 0x90, 0xED, 0x4A, 0xAD, 0x8B, 0x07, 0x36, 0xD3, 0x10, 0x41, 0x78, 0x8F, 
          0x60, 0x08, 0x55, 0xA8, 0x26, 0xCF, 0xD0, 0x0F, 0x65, 0x11, 0x84, 0x45, 0xB1, 0xA0, 0xFA, 0x57, 
          0x79, 0x97, 0x0B, 0x90, 0x92, 0xB0, 0x44, 0xAD, 0x8A, 0x0E, 0x60, 0xD9, 0x14, 0x11, 0x7E, 0x8D, 
          0x35, 0x5D, 0x5C, 0xFB, 0x21, 0x9C, 0xD3, 0x0E, 0x32, 0x40, 0xD1, 0x48, 0xB8, 0xA7, 0xA1, 0x0D, 
          0x28, 0xC3, 0x5D, 0x97, 0xC1, 0xEC, 0x42, 0xF1, 0xDC, 0x5D, 0x37, 0xDA, 0x14, 0x47, 0x79, 0x8A, 
          0x32, 0x5C, 0x54, 0xF2, 0x72, 0x9D, 0xD3, 0x0D, 0x67, 0x4C, 0xD6, 0x49, 0xB4, 0xA2, 0xF3, 0x50, 
          0x28, 0x96, 0x5E, 0x95, 0xC5, 0xE9, 0x45, 0xAD, 0x8A, 0x5D, 0x64, 0x8E, 0x17, 0x40, 0x2E, 0x87, 
          0x36, 0x58, 0x06, 0xFD, 0x75, 0x90, 0xD0, 0x5F, 0x3A, 0x40, 0xD4, 0x4C, 0xB0, 0xF7, 0xA7, 0x04, 
          0x2C, 0x96, 0x01, 0x96, 0x9B, 0xBC, 0x15, 0xA6, 0xDE, 0x0E, 0x65, 0x8D, 0x17, 0x47, 0x2F, 0xDD, 
          0x63, 0x54, 0x55, 0xAF, 0x76, 0xCA, 0x84, 0x5F, 0x62, 0x44, 0x80, 0x4A, 0xB3, 0xF4, 0xF4, 0x0C, 
          0x7E, 0xC4, 0x0E, 0xC6, 0x9A, 0xEB, 0x43, 0xA0, 0xDB, 0x0A, 0x64, 0xDF, 0x1C, 0x42, 0x24, 0x89, 
          0x63, 0x5C, 0x55, 0xF3, 0x71, 0x90, 0xDC, 0x5D, 0x60, 0x40, 0xD1, 0x4D, 0xB2, 0xA3, 0xA7, 0x0D, 
          0x2C, 0x9A, 0x0B, 0x90, 0x9A, 0xBE, 0x47, 0xA7, 0x88, 0x5A, 0x6D, 0xDF, 0x13, 0x1D, 0x2E, 0x8B, 
          0x60, 0x5E, 0x55, 0xF2, 0x74, 0x9C, 0xD7, 0x0E, 0x60, 0x40, 0x80, 0x1C, 0xB7, 0xA1, 0xF4, 0x02, 
          0x28, 0x96, 0x5B, 0x95, 0xC1, 0xE9, 0x40, 0xA3, 0x8F, 0x0C, 0x32, 0xDF, 0x43, 0x1D, 0x24, 0x8D, 
          0x61, 0x09, 0x54, 0xAB, 0x27, 0x9A, 0xD3, 0x58, 0x60, 0x16, 0x84, 0x4F, 0xB3, 0xA4, 0xF3, 0x0D, 
          0x25, 0x93, 0x08, 0xC0, 0x9A, 0xBD, 0x10, 0xA2, 0xD6, 0x09, 0x60, 0x8F, 0x11, 0x1D, 0x7A, 0x8F, 
          0x63, 0x0B, 0x5D, 0xF2, 0x21, 0xEC, 0xD7, 0x08, 0x62, 0x40, 0x84, 0x49, 0xB0, 0xAD, 0xF2, 0x07, 
          0x29, 0xC3, 0x0C, 0x96, 0x96, 0xEB, 0x10, 0xA0, 0xDA, 0x59, 0x32, 0xD3, 0x17, 0x41, 0x25, 0xDC, 
          0x63, 0x08, 0x04, 0xAE, 0x77, 0xCB, 0x84, 0x5A, 0x60, 0x4D, 0xDD, 0x45, 0xB5, 0xF4, 0xA0, 0x05 
        };
        private static readonly byte[] HvKey_transform = {
          0x39, 0xA2, 0xBF, 0x53, 0x7D, 0x88, 0x1D, 0x03, 0x35, 0x38, 0xA3, 0x80, 0x45,
          0x24, 0xEE, 0xCA, 0x25, 0x6D, 0xA5, 0xC2, 0x65, 0xA9, 0x94, 0x73, 0xE5, 0x74,
          0xEB, 0x54, 0xE5, 0x95, 0x3F, 0x1C
        };

        /// <summary>
        /// Used to decrypt ogg data in moggs. Both keys must be 16 bytes.
        /// </summary>
        /// <param name="PRIVATE_KEY">Key generated from game</param>
        /// <param name="PUBLIC_KEY">Key found in mogg</param>
        public MoggCrypt(byte[] PRIVATE_KEY, byte[] PUBLIC_KEY)
        {
            blockOffset = 0;

            // Copies the first 16 bytes. Can be taken directly from mogg file.
            cipherIn = new byte[16];
            Array.Copy(PUBLIC_KEY, cipherIn, 16);

            // Creates Rijndael class
            var aes = Rijndael.Create();
            aes.Key = PRIVATE_KEY; // Use RB1_KEY for 0x0B encryption
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;

            // Creates encryptor we use to generate cipher out blocks
            crypto = aes.CreateEncryptor();

            // Gets first cipher out value
            GenerateCipherOut();
        }

        /// <summary>
        /// Generates cipher out data from cipher in data using crypto.
        /// </summary>
        private void GenerateCipherOut()
        {
            cipherOut = crypto.TransformFinalBlock(cipherIn, 0, cipherIn.Length);
        }

        /// <summary>
        /// Crypts byte array
        /// </summary>
        /// <param name="input">Encrypted bytes</param>
        /// <returns>Decrypted bytes</returns>
        private byte[] CryptBytes(byte[] input)
        {
            if (blockOffset == cipherIn.Length)
            {
                // Checks each number in cipher
                for (var j = 0; j < cipherIn.Length; j++)
                {
                    // Adds one to current value
                    // If it adds to zero, goes to next value in cipherIn
                    cipherIn[j]++;
                    if (cipherIn[j] != 0) break;
                }
                // Generates cipherOut from new cipherIn data
                GenerateCipherOut();
                blockOffset = 0;
            }
            var cipher = new byte[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                cipher[i] = cipherOut[blockOffset + i];
            }
            blockOffset += input.Length;
            if (input.Length != 8) return BitConverter.GetBytes(input[0] ^ cipher[0]);
            var val = BitConverter.ToInt64(input, 0) ^ BitConverter.ToInt64(cipher, 0);
            return BitConverter.GetBytes(val);
        }

        /// <summary>
        /// Decrypts/encrypts ogg data in current stream.
        /// </summary>
        /// <param name="MOGG_STREAM">mogg file stream</param>
        /// <param name="PRIVATE_KEY">Key generated from game</param>
        /// <param name="PUBLIC_KEY">Key found in mogg</param>
        public static bool CryptStream(Stream MOGG_STREAM, byte[] PRIVATE_KEY, byte[] PUBLIC_KEY)
        {
            var PUBLIC_KEY_SHORT = new byte[0x10];
            using (var ms = new MemoryStream(PUBLIC_KEY))
            {
                ms.Read(PUBLIC_KEY_SHORT, 0, PUBLIC_KEY_SHORT.Length);
            }
            var mc = new MoggCrypt(PRIVATE_KEY, PUBLIC_KEY_SHORT);
            long offset = 0;
            const int size = 8;
            var bytes = new byte[size];
            //decrypt the stream, 8 bytes at a time
            for (var i = 0; i < MOGG_STREAM.Length / size; i++)
            {
                MOGG_STREAM.Seek(offset, SeekOrigin.Begin);
                MOGG_STREAM.Read(bytes, 0, bytes.Length);
                bytes = mc.CryptBytes(bytes);
                MOGG_STREAM.Seek(offset, SeekOrigin.Begin);
                MOGG_STREAM.Write(bytes, 0, bytes.Length);
                offset += bytes.Length;
            }
            var remainder = MOGG_STREAM.Length % size;
            if (remainder == 0) return true; //no remainder, stop here
            //decrypt the (up to 7) remaining bytes one at a time
            bytes = new byte[1];
            for (var i = 0; i < remainder; i++)
            {
                MOGG_STREAM.Seek(offset, SeekOrigin.Begin);
                MOGG_STREAM.Read(bytes, 0, bytes.Length);
                bytes[0] = mc.CryptBytes(bytes)[0];
                MOGG_STREAM.Seek(offset, SeekOrigin.Begin);
                MOGG_STREAM.Write(bytes, 0, bytes.Length);
                offset += bytes.Length;
            }
            return true;
        }

        #region code by maxton to generate private key
        /// <summary>
        /// Generates the key for decrypting the mogg.
        /// </summary>
        /// <param name="HvKey">The hardcoded key from the game binary.</param>
        /// <param name="moggData">Bytes of the mogg file.</param>
        /// <param name="useNewCrypt">New or old crypt method?</param>
        /// <returns>The generated key.</returns>
        public static byte[] GenPrivateKey(ref byte[] HvKey, ref byte[] moggData, bool useNewCrypt)
        {
            var fileKey = new byte[16];
            var hiddenKey = new byte[32];

            var numEntries = BitConverter.ToInt32(moggData, 16);
            //fileKey2 from 56-byte structure
            Array.Copy(moggData, 20 + numEntries * 8 + 16 + 32, fileKey, 0, 16);

            //decrypt fileKey2 with Rijndael
            using (var aesAlg = new AesManaged()) // idiomatic C#
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 128;
                aesAlg.Padding = PaddingMode.None;
                aesAlg.Key = HvKey;
                // Create a decrytor to perform the stream transform.
                var decryptor = aesAlg.CreateDecryptor();
                decryptor.TransformBlock(fileKey, 0, fileKey.Length, fileKey, 0);
            }

            //Get table offset from 56-byte structure
            var tableOffset = BitConverter.ToInt32(moggData, 20 + numEntries * 8 + 16 + 48);
            var a4 = BitConverter.ToInt32(moggData, 20 + numEntries * 8 + 16);
            var a5 = BitConverter.ToInt32(moggData, 20 + numEntries * 8 + 16 + 8);
            tableOffset = (tableOffset % 6) + 6;

            Array.Copy(hiddenKeys, 32 * tableOffset, hiddenKey, 0, 32);
            hiddenKey = revealKey(HvKey_transform, hiddenKey);
            var ground_bytes = grind_array(a4, a5, hex_string_to_bytes(hiddenKey), useNewCrypt);

            var retKey = new byte[16];
            for (var i = 0; i < 16; i++)
            {
                retKey[i] = (byte)(ground_bytes[i] ^ fileKey[i]);
            }
            return retKey;
        }

        /// <summary>
        /// Grinds up an array to get closer to the ctr_key. From the game method
        /// ByteGrinder::GrindArray.
        /// </summary>
        /// <param name="a1">first 32-bit integer from the 56-byte struct in new moggs</param>
        /// <param name="a2">third 32-bit integer from the 56-byte struct in new moggs</param>
        /// <param name="bar">Get this from KeyChain</param>
        /// <param name="useNewCrypt">Set to true if mogg version > 13</param>
        /// <returns></returns>
        private static byte[] grind_array(int a1, int a2, byte[] bar, bool useNewCrypt)
        {
            var hashmap_5bits = new int[256];
            var hashmap_6bits = new int[256];
            int[] hashmap;
            var used_up_slots = new byte[64];
            var O69_funcs = new byte[64];
            int i, op;
            int mogg_unk0 = a1, mogg_unk2 = a2;

            //hashTo5Bits
            for (i = 0; i < 256; i++)
            {
                hashmap_5bits[i] = (byte)((byte)a1 >> 3);
                a1 = 0x19660D * a1 + 0x3C6EF35F;
            }

            //hashTo6Bits
            a1 = mogg_unk2;
            for (i = 0; i < 256; i++)
            {
                hashmap_6bits[i] = (byte)(((byte)a1 >> 2) & 0x3f);
                a1 = 0x19660D * a1 + 0x3C6EF35F;
            }

            if (a2 == 0)
                a2 = 0x303f; //seed the "random sequence" generator

            for (i = 0; i < 32; i++)
            {//getRandomSequence32B
                do
                {
                    a2 = 0x19660D * a2 + 0x3C6EF35F;
                    op = (a2 >> 2) & 0x1F;
                } while (used_up_slots[op] != 0);
                O69_funcs[i] = (byte)op;
                used_up_slots[op] = 1;
            }
            if (useNewCrypt) //second 32 funcs
            {
                for (i = 32; i < 64; i++)
                {//getRandomSequence32B
                    do
                    {
                        mogg_unk0 = 0x19660D * mogg_unk0 + 0x3C6EF35F;
                        op = ((mogg_unk0 >> 2) & 0x1F) + 32;
                    } while (used_up_slots[op] != 0);
                    O69_funcs[i] = (byte)op;
                    used_up_slots[op] = 1;
                }
                hashmap = hashmap_6bits;
            }
            else
            {
                hashmap = hashmap_5bits;
            }
            int j;
            for (j = 0; j < 16; j++)
            {
                var ix = 0;
                var foo = bar[j];
                for (i = 0; i < 8; i++)
                {
                    op = O69_funcs[hashmap[bar[ix]]];
                    foo = O_funcs(foo, bar[ix + 1], op);
                    ix += 2;
                }
                bar[j] = foo;
            }
            return bar;
        }

        /// <summary>
        /// Encapsulates all the opX functions, which map in Data langauage to the OX functions.
        /// The opX functions to which the O functions map are mentioned in comments.
        /// </summary>
        /// <param name="a1">First argument / int from DataArray</param>
        /// <param name="a2">Second argument / int from DataArray</param>
        /// <param name="op">Which O-function to use</param>
        /// <returns>Something which the game calls $foo</returns>
        private static byte O_funcs(byte a1, byte a2, int op)
        {
            byte tmp;
            int result;
            switch (op)
            {
                case 0: //op13
                    tmp = rotr(a1, (a2 == 0) ? 1 : 0);
                    result = a2 + tmp;
                    break;
                case 1: //op16
                    tmp = rotr(a1, 3);
                    result = a2 + tmp;
                    break;
                case 2: //op20
                    tmp = rotl(a1, 1);
                    result = a2 + tmp;
                    break;
                case 3: //op11
                    result = a2 ^ ((a1 >> (a2 & 7)) | (byte)(a1 << (-a2 & 7)));
                    break;
                case 4: //op24
                    tmp = rotl(a1, 4);
                    result = a2 ^ tmp;
                    break;
                case 5: //op30
                    tmp = rotr(a1, 3);
                    result = a2 + (a2 ^ tmp);
                    break;
                case 6: //op19
                    tmp = rotl(a1, 2);
                    result = a2 + tmp;
                    break;
                case 7: //op7
                    result = a2 + ((a1 == 0) ? 1 : 0);
                    break;
                case 8: //op10
                    tmp = rotr(a1, (a2 == 0) ? 1 : 0);
                    result = a2 ^ tmp;
                    break;
                case 9: //op28
                    tmp = rotl(a1, 3);
                    result = a2 ^ (a2 + tmp);
                    break;
                case 10: //op18
                    tmp = rotl(a1, 3);
                    result = a2 + tmp;
                    break;
                case 11: //op17
                    tmp = rotl(a1, 4);
                    result = a2 + tmp;
                    break;
                case 12: //op0
                    result = a1 ^ a2;
                    break;
                case 13: //op6
                    result = a2 ^ ((a1 == 0) ? 1 : 0);
                    break;
                case 14: //op29
                    tmp = rotr(a1, 3);
                    result = a2 ^ (a2 + tmp);
                    break;
                case 15: //op25
                    tmp = rotl(a1, 3);
                    result = a2 ^ tmp;
                    break;
                case 16: //op26
                    tmp = rotl(a1, 2);
                    result = a2 ^ tmp;
                    break;
                case 17: //op31
                    tmp = rotl(a1, 3);
                    result = a2 + (a2 ^ tmp);
                    break;
                case 18: //op9
                    result = a2 + (a1 ^ a2);
                    break;
                case 19: //op1
                    result = a1 + a2;
                    break;
                case 20: //op23
                    tmp = rotr(a1, 3);
                    result = a2 ^ tmp;
                    break;
                case 21: //op8
                    result = a2 ^ (a1 + a2);
                    break;
                case 22: //op3
                    result = rotr(a1, ((a2 == 0) ? 1 : 0));
                    break;
                case 23: //op14
                    tmp = rotr(a1, 1);
                    result = a2 + tmp;
                    break;
                case 24: //op2
                    result = (a1 >> (a2 & 7)) | (a1 << (-a2 & 7));
                    break;
                case 25: //op4
                    tmp = (byte)(a1 == 0 ? (a2 == 0 ? 0x80 : 0x01) : 0x00);
                    result = tmp;
                    break;
                case 26: //op15
                    tmp = rotr(a1, 2);
                    result = a2 + tmp;
                    break;
                case 27: //op21
                    tmp = rotr(a1, 1);
                    result = a2 ^ tmp;
                    break;
                case 28: //op5
                    a1 = (byte)~a1;
                    goto case 24;
                case 29: //op22
                    tmp = rotr(a1, 2);
                    result = a2 ^ tmp;
                    break;
                case 30: //op12
                    result = a2 + ((a1 >> (a2 & 7)) | (byte)(a1 << (-a2 & 7)));
                    break;
                case 31: //op27
                    tmp = rotl(a1, 1);
                    result = a2 ^ tmp;
                    break;

                /* begin new funcs */

                case 32: //op60
                    result = (byte)(((a1 << 8) | 0xAA | (byte)(~a1)) >> 4) ^ a2;
                    break;
                case 33: //op32
                    result = (byte)((((byte)(~a1) | (uint)(a1 << 8)) >> 3) ^ a2);
                    break;
                case 34: //op36
                    result = (byte)(((((a1 << 8) ^ 0xFF00) | a1) >> 2) ^ a2);
                    break;
                case 35: //op43
                    result = (byte)((((a1 ^ 0x5C) | (a1 << 8)) >> 5) ^ a2);
                    break;
                case 36: //op59
                    result = (byte)((((a1 << 8) | 0x65 | (a1 ^ 0x3C)) >> 2) ^ a2);
                    break;
                case 37: //op44
                    result = (byte)((((a1 ^ 0x36) | (a1 << 8)) >> 2) ^ a2);
                    break;
                case 38: //op46
                    result = (byte)((((a1 ^ 0x36) | (a1 << 8)) >> 4) ^ a2);
                    break;
                case 39: //op52
                    result = (byte)(((a1 ^ 0x5C | (a1 << 8) | 0x36) >> 1) ^ a2);
                    break;
                case 40: //op33
                    result = (byte)((((byte)(~a1) | (a1 << 8)) >> 5) ^ a2);
                    break;
                case 41: //op38
                    result = (byte)((((~a1 << 8) | a1) >> 6) ^ a2);
                    break;
                case 42: //op42
                    result = (byte)((((a1 ^ 0x5C) | (a1 << 8)) >> 3) ^ a2);
                    break;
                case 43: //op57
                    result = (byte)((((a1 ^ 0x3C) | 0x65 | (a1 << 8)) >> 5) ^ a2);
                    break;
                case 44: //op47
                    result = (byte)((((a1 ^ 0x36) | (a1 << 8)) >> 1) ^ a2);
                    break;
                case 45: //op58
                    result = (byte)((((a1 ^ 0x65) | (a1 << 8) | 0x3C) >> 6) ^ a2);
                    break;
                case 46: //op41
                    result = (byte)((((a1 ^ 0x5C) | (a1 << 8)) >> 2) ^ a2);
                    break;
                case 47: //op61
                    result = (byte)((((a2 ^ 0xAA) | (a2 << 8) | 0xFF) >> 3) ^ a1); //are any others reversed like this?
                    break;
                case 48: //op51
                    result = (byte)((((a1 ^ 0x63) | (a1 << 8) | 0x5C) >> 6) ^ a2);
                    break;
                case 49: //op53
                    result = (byte)((((a1 ^ 0x5C) | (a1 << 8) | 0x36) >> 7) ^ a2);
                    break;
                case 50: //op40
                    result = (byte)((((a1 ^ 0x5C) | (a1 << 8)) >> 6) ^ a2);
                    break;
                case 51: //op39
                    result = (byte)(((((a1 << 8) ^ 0xFF00) | a1) >> 3) ^ a2);
                    break;
                case 52: //op35
                    result = (byte)((((byte)(~a1) | (a1 << 8)) >> 6) ^ a2);
                    break;
                case 53: //op37
                    result = (byte)(((((a1 << 8) ^ 0xFF00) | a1) >> 5) ^ a2);
                    break;
                case 54: //op56
                    result = (byte)((((a1 ^ 0x3C) | 0x65 | (a1 << 8)) >> 4) ^ a2);
                    break;
                case 55: //op49
                    result = (byte)((((a1 ^ 0x63) | (a1 << 8) | 0x5C) >> 3) ^ a2);
                    break;
                case 56: //op50
                    result = (byte)((((a1 ^ 0x63) | (a1 << 8) | 0x5C) >> 5) ^ a2);
                    break;
                case 57: //op62
                    result = (byte)((((a1 ^ 0xAF) | (a1 << 8) | 0xFA) >> 5) ^ a2);
                    break;
                case 58: //op55
                    result = (byte)((((a1 ^ 0x5C) | (a1 << 8) | 0x36) >> 5) ^ a2);
                    break;
                case 59: //op54
                    result = (byte)((((a1 ^ 0x5C) | (a1 << 8) | 0x36) >> 3) ^ a2);
                    break;
                case 60: //op45
                    result = (byte)((((a1 ^ 0x36) | (a1 << 8)) >> 3) ^ a2);
                    break;
                case 61: //op48
                    result = (byte)((((a1 ^ 0x63) | (a1 << 8) | 0x5C) >> 4) ^ a2);
                    break;
                case 62: //op63
                    result = (byte)((((a1 ^ 0xFF) | (a1 << 8) | 0xAF) >> 6) ^ a2);
                    break;
                case 63: //op34
                    result = (byte)((((byte)(~a1) | (a1 << 8)) >> 2) ^ a2);
                    break;
                default:
                    return 0x00;
            }
            return (byte)(result & 0xFF);
        }
        /// <summary>
        /// Reveals the "key" for ByteGrinder::GrindArray. From the KeyChain class.
        /// To simplify things, functions/methods were inlined here. Some of their
        /// names remain in comments.
        /// 
        /// The key is not really a key, but more a sequence of choices for the 
        /// Data script that is generated and executed by ByteGrinder::GrindArray
        /// </summary>
        /// <param name="transform">Data with which the key is XORed</param>
        /// <param name="buf32">The 32-byte value from the hiddenKeys table</param>
        /// <returns>The revealed key, encoded in ASCII as hex chars</returns>
        private static byte[] revealKey(IList<byte> transform, byte[] buf32)
        {
            for (var i = 0; i < 14; i++)
            {//superShuffle
                #region swaps
                swap(ref buf32, 19, 2);
                swap(ref buf32, 22, 1);
                swap(ref buf32, 23, 6);
                swap(ref buf32, 26, 5);
                swap(ref buf32, 27, 10);
                swap(ref buf32, 30, 9);
                swap(ref buf32, 31, 14);
                swap(ref buf32, 2, 13);
                swap(ref buf32, 3, 18);
                swap(ref buf32, 6, 17);
                swap(ref buf32, 7, 22);
                swap(ref buf32, 10, 21);
                swap(ref buf32, 11, 26);
                swap(ref buf32, 14, 25);
                swap(ref buf32, 15, 30);
                swap(ref buf32, 18, 29);
                swap(ref buf32, 29, 2);
                swap(ref buf32, 28, 3);
                swap(ref buf32, 25, 6);
                swap(ref buf32, 24, 7);
                swap(ref buf32, 21, 10);
                swap(ref buf32, 20, 11);
                swap(ref buf32, 17, 14);
                swap(ref buf32, 16, 15);
                swap(ref buf32, 13, 18);
                swap(ref buf32, 12, 19);
                swap(ref buf32, 9, 22);
                swap(ref buf32, 8, 23);
                swap(ref buf32, 5, 26);
                swap(ref buf32, 4, 27);
                swap(ref buf32, 1, 30);
                swap(ref buf32, 0, 31);
                swap(ref buf32, 16, 2);
                swap(ref buf32, 28, 3);
                swap(ref buf32, 12, 6);
                swap(ref buf32, 24, 7);
                swap(ref buf32, 8, 10);
                swap(ref buf32, 20, 11);
                swap(ref buf32, 4, 14);
                swap(ref buf32, 16, 15);
                swap(ref buf32, 0, 18);
                swap(ref buf32, 12, 19);
                swap(ref buf32, 28, 22);
                swap(ref buf32, 8, 23);
                swap(ref buf32, 24, 26);
                swap(ref buf32, 4, 27);
                swap(ref buf32, 20, 30);
                swap(ref buf32, 0, 31);
                swap(ref buf32, 29, 2);
                swap(ref buf32, 15, 3);
                swap(ref buf32, 25, 6);
                swap(ref buf32, 11, 7);
                swap(ref buf32, 21, 10);
                swap(ref buf32, 7, 11);
                swap(ref buf32, 17, 14);
                swap(ref buf32, 3, 15);
                swap(ref buf32, 13, 18);
                swap(ref buf32, 31, 19);
                swap(ref buf32, 9, 22);
                swap(ref buf32, 27, 23);
                swap(ref buf32, 5, 26);
                swap(ref buf32, 23, 27);
                swap(ref buf32, 1, 30);
                swap(ref buf32, 19, 31);
                swap(ref buf32, 29, 21);
                swap(ref buf32, 28, 3);
                swap(ref buf32, 25, 25);
                swap(ref buf32, 24, 7);
                swap(ref buf32, 21, 29);
                swap(ref buf32, 20, 11);
                swap(ref buf32, 17, 1);
                swap(ref buf32, 16, 15);
                swap(ref buf32, 13, 5);
                swap(ref buf32, 12, 19);
                swap(ref buf32, 9, 9);
                swap(ref buf32, 8, 23);
                swap(ref buf32, 5, 13);
                swap(ref buf32, 4, 27);
                swap(ref buf32, 1, 17);
                swap(ref buf32, 0, 31);
                swap(ref buf32, 29, 2);
                swap(ref buf32, 28, 22);
                swap(ref buf32, 25, 6);
                swap(ref buf32, 24, 26);
                swap(ref buf32, 21, 10);
                swap(ref buf32, 20, 30);
                swap(ref buf32, 17, 14);
                swap(ref buf32, 16, 2);
                swap(ref buf32, 13, 18);
                swap(ref buf32, 12, 6);
                swap(ref buf32, 9, 22);
                swap(ref buf32, 8, 10);
                swap(ref buf32, 5, 26);
                swap(ref buf32, 4, 14);
                swap(ref buf32, 1, 30);
                swap(ref buf32, 0, 18);
                #endregion
            }
            for (var i = 0; i < 32; i++)
            {//mash
                buf32[i] ^= transform[i];
            }
            return buf32;
        }

        /// <summary>
        /// Rotate a byte left by dist bits
        /// </summary>
        /// <param name="b">Byte to rotate left</param>
        /// <param name="dist">Bits by which to rotate</param>
        /// <returns>The rotated byte</returns>
        private static byte rotl(byte b, int dist)
        {
            return (byte)(((b << dist) | (b >> (8 - dist))) & 0xFF);
        }

        /// <summary>
        /// Rotate a byte right by distance dist
        /// </summary>
        /// <param name="b">Byte to rotate right</param>
        /// <param name="dist">Bits by which to rotate</param>
        /// <returns>The rotated byte</returns>
        private static byte rotr(byte b, int dist)
        {
            return (byte)(((b >> dist) | (b << (8 - dist))) & 0xFF);
        }

        private static byte hex_char_to_nibble(byte h)
        {
            if (h > 96)
                return (byte)(h - 87);
            if (h > 64)
                return (byte)(h - 55);
            if (h > 47)
                return (byte)(h - 48);
            return 0;
        }

        /// <summary>
        /// Converts a 32-char ASCII hex string to its 16-byte binary equivalent
        /// </summary>
        /// <param name="h">The string</param>
        /// <returns>The binary form</returns>
        private static byte[] hex_string_to_bytes(IList<byte> h)
        {
            var ret = new byte[16];
            for (var i = 0; i < 16; i++)
            {
                ret[i] = (byte)(hex_char_to_nibble(h[i * 2]) << 4 |
                                hex_char_to_nibble(h[i * 2 + 1]));
            }
            return ret;
        }

        private static void swap(ref byte[] bytes, int i1, int i2)
        {
            var tmp = bytes[i1];
            bytes[i1] = bytes[i2];
            bytes[i2] = tmp;
        }
        #endregion
    }

    public class MoggChunk
    {
        public MoggChunk(int offset, int value)
        {
            Offset = offset;
            Value = value;
        }
        public int Offset { get; set; }
        public int Value { get; set; }
    }

    public enum DecryptMode
    {
        ToFile,
        ToMemory
    }

    public enum CryptVersion
    {
        x0A = 0x0A, //No encryption
        x0B = 0x0B, //RB1, RB1 DLC
        x0C = 0x0C, //RB2, AC/DC Live, some RB2 DLC
        x0D = 0x0D, //C3 use only, never observed used by HMX
        x0E = 0x0E, //Lego, Green Day, most RB2 DLC
        x0F = 0x0F, //RBN
        x10 = 0x10, //RB3, RB3 DLC
    }
}
