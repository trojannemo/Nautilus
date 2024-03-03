using System;
using System.Collections.Generic;
using System.IO;
using Nautilus.zlib;

namespace Nautilus
{
    public static class PikminMiloTools
    {
        private static string BlocksFolder;
        private static string FilesFolder;
        private static string TexturesFolder;

        /// <summary>
        /// Extracts images from Milo file and saves textures in "(miloDir)/ext_(milofilename)"
        /// as "texture(int)" starting from zero.
        /// </summary>
        /// <param name="inMilo">Milo file location</param>
        /// <param name="saveDDS">Save DDS?</param>
        /// <param name="saveTEX">Save TEX?</param>
        /// <param name="format">Format to output the images to</param>
        /// <param name="outTexCount">Number of textures in Milo file</param>
        public static void ExtractTextures(string inMilo, bool saveDDS, bool saveTEX, string format, out int outTexCount)
        {
            var texCount = 0;
            var Tools = new NemoTools();
            var ext = Path.GetExtension(inMilo);
            if (string.IsNullOrWhiteSpace(ext))
            {
                outTexCount = 0;
                return;
            }
            TexturesFolder = inMilo.Replace(ext, "_textures\\");
            BlocksFolder = TexturesFolder + "blocks\\";
            FilesFolder = TexturesFolder + "files\\";

            try
            {
                // Reads first four bytes to check its compression type.
                var br = new BinaryReader(File.OpenRead(inMilo));
                var firstFour = br.ReadBytes(4);
                br.Close();

                string compressionType;
                if (firstFour.IsMilo(out compressionType))
                {
                    var count = 0;
                    ProcessMilo(inMilo, compressionType, true, saveTEX, ref texCount, false, ref count, false, ref count);
                }
            }
            catch (Exception)
            { }
            outTexCount = texCount;

            //clean up afterwards
            if (outTexCount < 1)
            {
                Tools.DeleteFolder(TexturesFolder, true);
            }
            else
            {
                Tools.DeleteFolder(BlocksFolder, true);
                Tools.DeleteFolder(FilesFolder, true);

                var ddsfiles = Directory.GetFiles(TexturesFolder, "*.dds");
                foreach (var dds in ddsfiles)
                {
                    if (!string.IsNullOrWhiteSpace(format))
                    {
                        Tools.ConvertRBImage(dds, dds, format);
                    }
                    if (!saveDDS)
                    {
                        Tools.DeleteFile(dds);
                    }
                }
            }
            outTexCount = texCount;
        }

        /// <summary>
        /// Checks if first four bytes are consistant with a Milo.
        /// </summary>
        /// <param name="miloBytes">Milo file</param>
        /// <param name="compressionType">Compression Type</param>
        /// <returns>True: Is Milo | False: Is not Milo</returns>
        public static bool IsMilo(this byte[] miloBytes, out string compressionType)
        {
            string miloHeader = null;

            // Reads first four bytes to check its compression type.
            for (var i = 0x0; i < 0x4; i++)
            {
                miloHeader += miloBytes[i].ToString("X2");
            }

            // No compression. Used only for RBN tracks.
            switch (miloHeader)
            {
                case "AFDEBECA":
                    compressionType = "A";
                    return true;
                case "AFDEBECB":
                    compressionType = "B";
                    return true;
                case "AFDEBECD":
                    compressionType = "D";
                    return true;
                default:
                    compressionType = null;
                    return false;
            }
        }

        /// <summary>
        /// Processes Milo file for a few different things.
        /// </summary>
        /// <param name="inMilo">Milo file</param>
        /// <param name="compressionType">Compression type</param>
        /// <param name="saveDDS">Save DDS?</param>
        /// <param name="saveTEX">Save TEX?</param>
        /// <param name="texCount">Number of textures in Milo file</param>
        /// <param name="saveBlock">Save block?</param>
        /// <param name="blockCount"></param>
        /// <param name="saveFile">Save file?</param>
        /// <param name="fileCount"></param>
        private static void ProcessMilo(string inMilo, string compressionType, bool saveDDS, bool saveTEX, ref int texCount, bool saveBlock, ref int blockCount, bool saveFile, ref int fileCount)
        {
            if (!Directory.Exists(TexturesFolder))
            {
                Directory.CreateDirectory(TexturesFolder);
            }
            if (!Directory.Exists(BlocksFolder))
            {
                Directory.CreateDirectory(BlocksFolder);
            }
            if (!Directory.Exists(FilesFolder))
            {
                Directory.CreateDirectory(FilesFolder);
            }

            var br = new BinaryReader(File.OpenRead(inMilo));
            br.BaseStream.Position = 0x04;

            // Reads Offset and block count.

            var miloFirstOffset = br.ReadInt32();
            var miloBlocksCount = br.ReadInt32();
            br.ReadInt32();

            // Reads sizes of all blocks and places in array.
            // Bool array is for RB3 milo files. It's needed because some blocks in RB3 aren't compressed.

            var miloBlockSize = new int[miloBlocksCount];
            var compressed = new bool[miloBlocksCount];
            var miloBlockSizeOffset = 0;
            for (var i = 0x10; i < 0x10 + (4 * miloBlocksCount); i += 4)
            {
                br.BaseStream.Position = i;
                miloBlockSize[miloBlockSizeOffset] += br.ReadInt32();

                // For RB3/Blitz/DC milo files.
                // Easier to do this than read first three bytes and check fourth.
                if (miloBlockSize[miloBlockSizeOffset] > 16777216)
                {
                    compressed[miloBlockSizeOffset] = false;
                    miloBlockSize[miloBlockSizeOffset] -= 16777216;
                }
                else compressed[miloBlockSizeOffset] = true;

                miloBlockSizeOffset += 1;
            }

            // Reads each block and finds addeadde positions. Then it begins parsing every file for TEX header info.

            byte[] addeadde = { 0xAD, 0xDE, 0xAD, 0xDE };
            br.BaseStream.Position = miloFirstOffset;
            for (var i = 0; i < miloBlocksCount; i += 1)
            {
                var blockTemp = br.ReadBytes(miloBlockSize[i]);

                // This will decompress blocks for pre-RB3 games.
                if (compressionType == "B") DecompressBlock(blockTemp, out blockTemp, false);

                // This will decompress blocks for post-RB3 games. Some block lengths are zero.
                if (compressionType == "D" && blockTemp.Length != 0 && compressed[i])
                    DecompressBlock(blockTemp, out blockTemp, true);

                // This will save block to file.
                if (saveBlock && blockTemp.Length != 0)
                {
                    File.WriteAllBytes(BlocksFolder + "block" + blockCount + ".uncompressed", blockTemp);
                    blockCount += 1;
                }

                var b = true;
                var position = 0;
                var n = 0;
                var fileBegin = 0;

                while (b) // Loop will stop when addeadde bytes are no longer found.
                {
                    // This does the real magic by searching for the keyword seperator.
                    // Then it uses the offsets to create a byte array for isTex to parse.

                    var br2 = new BinaryReader(new MemoryStream(blockTemp));

                    if (blockTemp.Contains(ref addeadde, (position + n), out position))
                    {
                        br2.BaseStream.Position = fileBegin;
                        var fileTemp = br2.ReadBytes((position - 3) - fileBegin);
                        if (fileTemp.IsTex())
                        {
                            if (fileTemp[0] == 0x0A) // Reverses bytes for GH2 textures.
                            {
                                Array.Reverse(fileTemp, 17, 4); // Width
                                Array.Reverse(fileTemp, 21, 4); // Height
                                Array.Reverse(fileTemp, 25, 4); // Bpp
                                Array.Reverse(fileTemp, 29, 4); // Address Length
                            }

                            //get file name
                            var br3 = new BinaryReader(new MemoryStream(fileTemp));
                            br3.BaseStream.Position = 0x20;
                            var name_length = br3.ReadByte();
                            if (name_length == 0x00)
                            {
                                name_length = br3.ReadByte(); //Green Day
                                if (name_length == 0x00)
                                {
                                    br3.BaseStream.Position += 2;
                                    name_length = br3.ReadByte(); //Lego
                                }
                            }
                            string filename;
                            try
                            {
                                filename = System.Text.Encoding.UTF8.GetString(br3.ReadBytes(name_length));
                                filename = Path.GetFileNameWithoutExtension(filename);
                            }
                            catch (Exception)
                            {
                                filename = "";
                            }
                            br3.Dispose();

                            texCount += 1;
                            if (saveDDS || saveTEX)
                            {
                                var outputfile = TexturesFolder + (string.IsNullOrWhiteSpace(filename) ? "texture" + texCount : filename);
                                if (saveTEX)
                                {
                                    var texfile = outputfile + ".tex";
                                    if (File.Exists(texfile))
                                    {
                                        texfile = texfile.Replace(".tex", "_" + texCount + ".tex");
                                    }
                                    File.WriteAllBytes(texfile, fileTemp);
                                }
                                if (saveDDS)
                                {
                                    var ddsfile = outputfile + ".dds";
                                    if (File.Exists(ddsfile))
                                    {
                                        ddsfile = ddsfile.Replace(".dds", "_" + texCount + ".dds");
                                    }
                                    var swapped = Path.GetExtension(inMilo).ToLowerInvariant().Contains("xbox");
                                    var normal = filename.ToLowerInvariant().Contains("_norm");
                                    File.WriteAllBytes(ddsfile, fileTemp.ConvertTexToDDS(swapped, normal));
                                }
                            }
                        }

                        if (saveFile)
                        {
                            File.WriteAllBytes(FilesFolder + "file" + fileCount + ".raw", fileTemp);
                            fileCount += 1;
                        }

                        n = 1;
                        fileBegin = position + 1;
                    }

                    else
                    {
                        b = false;
                        br2.Close();
                    }
                }
            }
            br.Close();
        }

        /// <summary>
        /// Reads file from Milo block and determines if it's a texture.
        /// </summary>
        /// <param name="inFile">File from Milo</param>
        /// <returns>True: Is a texture | False: Is not a texture</returns>
        public static bool IsTex(this byte[] inFile)
        {
            var br = new BinaryReader(new MemoryStream(inFile));
            int firstByte = br.ReadByte();
            var isTex = false;

            br.BaseStream.Position = 0x03;
            int thirdByte = br.ReadByte();

            if (inFile.Length > 110 && (thirdByte == 10 | thirdByte == 11))
            {
                br.BaseStream.Position = 0x20;

                // LEGO has to be weird and begin character length four bytes later.
                // Some GDRB/TBRB Tex headers begin data one byte later.
                int gdrb = br.ReadByte(), lego = br.ReadByte();

                if (gdrb == 0 && lego != 0)
                {
                    gdrb = 1;
                    lego = 0;
                }
                else if (gdrb == 0 && lego == 0)
                {
                    gdrb = 0;
                    lego = 4;
                }
                else
                {
                    gdrb = 0;
                    lego = 0;
                }

                br.BaseStream.Position = (0x20 + gdrb + lego);

                int charCount = br.ReadByte();

                if (charCount < (inFile.Length - (32 + gdrb + lego)) // Prevents reader from going beyond file.
                    && charCount > 4 && inFile.Length > (43 + gdrb + charCount + 32 + 1 + lego)) // Sometimes they'll be blank files with only TEX & PNG_XBOX headers.
                {
                    br.BaseStream.Position += (charCount - 1);
                    int ext = br.ReadByte(); // Only for PS3. Looks for either "p" or "g".
                    int check = br.ReadByte(); // Looking for "0xC1".

                    // Some PS3 textures don't contain the "0xC1" after file address and will be "0x00".
                    // Wii textures will have "0xFF" after file address.

                    if (check == 193 | (check == 0 && ext == 103) | (check == 0 && ext == 112) | check == 255)
                    {
                        isTex = true;
                    }
                }
            }
            else if (inFile.Length > 110 && firstByte == 10) // For GH2 textures.
            {
                br.BaseStream.Position = 0x1D;
                var charCount = br.ReadInt32();

                if (charCount < (inFile.Length - 32) && charCount > 4) // Prevents reader from going beyond file.
                {
                    br.BaseStream.Position += charCount + 3;
                    int check = br.ReadByte();

                    if (check == 193)
                    {
                        isTex = true;
                    }
                }
            }

            br.Close();
            return isTex;
        }

        /// <summary>
        /// Converts TEX to DDS.
        /// </summary>
        /// <param name="texFile">TEX file</param>
        /// <param name="swapped">True: X360 texture | False: PS3 texture</param>
        /// <param name="norm">Is it a normal map?</param>
        /// <returns>DDS file</returns>
        public static byte[] ConvertTexToDDS(this byte[] texFile, bool swapped, bool norm = false)
        {
            var buffer = new byte[4];
            var swap = new byte[4];
            var rb3 = 0; // RB3/DC/Blitz have one extra byte in their TEX header.

            var br = new BinaryReader(new MemoryStream(texFile));
            br.BaseStream.Position = 0x20;

            // LEGO has to be weird and begin character length four bytes later.
            // Some GDRB/TBRB Tex headers begin data one byte later.
            int gdrb = br.ReadByte(), lego = br.ReadByte();

            if (gdrb == 0 && lego != 0)
            {
                gdrb = 1;
                lego = 0;
            }
            else if (gdrb == 0 && lego == 0)
            {
                gdrb = 0;
                lego = 4;
            }
            else
            {
                gdrb = 0;
                lego = 0;
            }

            br.BaseStream.Position = 17 + gdrb;
            var width = br.ReadBytes(4);
            var height = br.ReadBytes(4);
            var bpp = br.ReadBytes(4);

            br.BaseStream.Position += 3 + lego;
            int charCount = br.ReadByte();

            br.BaseStream.Position += charCount + 9;
            if (br.ReadByte() == 00) rb3 = 1;

            var offset = charCount + 74 + gdrb + rb3 + lego;
            br.Close();

            var format = 0;
            if (norm && swapped) //only for Xbox
            {
                format = 32;
            }
            else if (norm)
            {
                bpp[3] = 0x04; //PS3 normal map files are actually DXT1
            }

            //get filesize / 4 for number of times to loop
            //32 is the size of the HMX header to skip
            var loop = (texFile.Length - offset) / 4;

            //skip the HMX header
            var input = new MemoryStream(texFile, offset, texFile.Length - offset);

            //create dds file
            var output = new MemoryStream();
            var header = BuildDDSHeader(bpp, width, height, format);
            output.Write(header, 0, header.Length);

            //here we go
            for (var x = 0; x < loop; x++)
            {
                input.Read(buffer, 0, 4);

                if (!swapped)
                {
                    swap = buffer; // For PS3 textures.
                }
                else
                {
                    //XBOX images are byte swapped, so we gotta return it
                    swap[0] = buffer[1];
                    swap[1] = buffer[0];
                    swap[2] = buffer[3];
                    swap[3] = buffer[2];
                }
                output.Write(swap, 0, 4);
            }
            input.Dispose();
            output.Dispose();
            return output.ToArray();
        }

        /// <summary>
        /// Builds DDS header.
        /// </summary>
        /// <param name="format">Bpp info</param>
        /// <param name="width">Pixel width</param>
        /// <param name="height">Pixel height</param>
        /// <param name="normal">Byte to determine if file is normal map</param>
        /// <returns></returns>
        private static byte[] BuildDDSHeader(IList<byte> format, IList<byte> width, IList<byte> height, int normal)
        {
            var dds = new byte[] //512x512 DXT5
                {
                    0x44, 0x44, 0x53, 0x20, 0x7C, 0x00, 0x00, 0x00, 0x07, 0x10, 0x0A, 0x00, 0x00, 0x02, 0x00, 0x00,
                    0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x4E, 0x45, 0x4D, 0x4F, 0x00, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00,
                    0x04, 0x00, 0x00, 0x00, 0x44, 0x58, 0x54, 0x35, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                };

            dds[12] = height[3];
            dds[13] = height[2];
            dds[14] = height[1];
            dds[15] = height[0];

            dds[16] = width[3];
            dds[17] = width[2];
            dds[18] = width[1];
            dds[19] = width[0];

            if (normal == 32)
            {
                //Normal maps
                dds[84] = 0x41;
                dds[85] = 0x54;
                dds[86] = 0x49;
                dds[87] = 0x32;
            }
            else if (format[3] == 0x04)
            {
                //DXT1
                dds[87] = 0x31;
            }
            return dds;
        }

        /// <summary>
        /// Decompresses milo block.
        /// </summary>
        /// <param name="inBlock">Compressed block</param>
        /// <param name="outBlock">Uncompressed block</param>
        /// <param name="rb3">True: Post-RB3 Compression | False: Pre-RB3 Compression</param>
        public static void DecompressBlock(byte[] inBlock, out byte[] outBlock, bool rb3)
        {
            byte[] head = { 0x78, 0x9C }; // Default Compression.
            var compBlock = new MemoryStream();

            if (rb3) // The first four bytes of an RB3 block is the uncompressed size of block.
            {
                compBlock.Write(head, 0, head.Length);
                compBlock.Write(inBlock, 4, (inBlock.Length - 4));
            }
            else
            {
                compBlock.Write(head, 0, head.Length);
                compBlock.Write(inBlock, 0, inBlock.Length);
            }

            var outMemoryStream = new MemoryStream();
            var outZStream = new ZOutputStream(outMemoryStream);
            outZStream.Write(compBlock.ToArray(), 0, compBlock.ToArray().Length);

            outBlock = outMemoryStream.ToArray();
            outMemoryStream.Flush();
            outZStream.Flush();
            compBlock.Flush();
        }

        // Have to give credit to http://www.lyquidity.com/devblog/?p=79 for this. I only made one modification to it.
        // Which was adding an offset to start read.
        /// <summary>
        /// Finds a sequence of bytes in an array
        /// </summary>
        /// <param name="buffer">The haystack</param>
        /// <param name="sequence">The needle</param>
        /// <param name="offset">Offset to start search</param>
        /// <param name="position">The position of the needle in the haystack if the needle is found</param>
        /// <returns>True if the needle is found</returns>
        public static bool Contains(this byte[] buffer, ref byte[] sequence, int offset, out int position)
        {
            var currOffset = 0;

            for (position = offset; position < buffer.Length; position++)
            {
                var b = buffer[position];
                if (b == sequence[currOffset])
                {
                    if (currOffset == sequence.Length - 1) return true;
                    currOffset++;
                    continue;
                }

                if (currOffset == 0) continue;
                position -= currOffset;
                currOffset = 0;
            }
            return false;
        }
    }
}