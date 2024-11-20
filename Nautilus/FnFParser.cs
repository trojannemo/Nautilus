using SharpMp4Parser.IsoParser.Boxes.ISO14496.Part12;
using SharpMp4Parser.IsoParser;
using SharpMp4Parser.Java;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Un4seen.Bass;
using System.Windows;

namespace Nautilus
{
    public class NemoFnFParser
    {
        private static byte[] opusSamples;
        private static List<byte[]> opusPackets;
        private static int channelCount;
        private static readonly IntPtr STREAMPROC_PUSH = new IntPtr(-1);

        [DllImport("bin\\bassopus.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int BASS_OPUS_StreamCreate(ref BASS_OPUS_HEAD head, uint flags, IntPtr proc, IntPtr user);

        [DllImport("bin\\bassopus.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int BASS_OPUS_StreamPutData(int handle, IntPtr buffer, int length);

        private delegate int STREAMPROC(int handle, IntPtr buffer, int length, IntPtr user);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct BASS_OPUS_HEAD
        {
            public byte version;
            public byte channels;
            public short preskip;
            public int inputrate;
            public short gain;
            public byte mapping;
            public byte streams;
            public byte coupled;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            public byte[] chanmap;
        }

        public int m4aToBassStream(string m4aFilePath, int channels)
        {
            var opusData = ExtractOpusDataFromMP4(m4aFilePath);
            if (opusData.Count() == 0) return 0;
            opusSamples = opusData.ToArray();
            channelCount = channels;
            return FortniteFestivalToBassStream();
        }

        private static List<byte> ExtractOpusDataFromMP4(string m4aFilePath)
        {
            List<byte> opusData = new List<byte>();
            opusPackets = new List<byte[]>();

            using (var fileStream = new FileStream(m4aFilePath, FileMode.Open, FileAccess.Read))
            {
                var isoFile = new IsoFile(new ByteStream(fileStream));
                var movieBox = isoFile.getMovieBox();
                if (movieBox == null)
                {
                    MessageBox.Show("No MovieBox found.");
                    return opusData;
                }

                // Get the TrackBox for the audio track
                var trackBox = movieBox.getBoxes<TrackBox>(typeof(TrackBox)).FirstOrDefault();
                if (trackBox == null)
                {
                    MessageBox.Show("No TrackBox found.");
                    return opusData;
                }

                var sampleTableBox = trackBox.getSampleTableBox();
                if (sampleTableBox == null)
                {
                    MessageBox.Show("No SampleTableBox found.");
                    return opusData;
                }

                var sampleSizeBox = sampleTableBox.getSampleSizeBox();
                var chunkOffsetBox = sampleTableBox.getChunkOffsetBox();

                var sampleSizes = sampleSizeBox.getSampleSizes();
                var chunkOffsets = chunkOffsetBox.getChunkOffsets();

                if (sampleSizes == null || chunkOffsets == null)
                {
                    MessageBox.Show("Sample sizes or chunk offsets are missing.");
                    return opusData;
                }

                long currentOffset = 0;
                while (currentOffset < fileStream.Length)
                {
                    fileStream.Seek(currentOffset, SeekOrigin.Begin);

                    // Read the next box header (8 bytes)
                    byte[] headerBytes = new byte[8];
                    fileStream.Read(headerBytes, 0, 8);

                    // Extract box size and type
                    long boxSize = BitConverter.ToUInt32(headerBytes.Take(4).Reverse().ToArray(), 0);
                    string boxType = Encoding.ASCII.GetString(headerBytes, 4, 4);

                    if (boxSize < 8)
                    {
                        currentOffset += boxSize;
                        continue;
                    }

                    if (boxType == "mdat" || boxType == "MDaT")
                    {
                        int sampleIndex = 0;
                        for (int chunkIndex = 0; chunkIndex < chunkOffsets.Length; chunkIndex++)
                        {
                            long chunkOffset = chunkOffsets[chunkIndex];

                            while (sampleIndex < sampleSizes.Length)
                            {
                                long sampleSize = sampleSizes[sampleIndex];

                                // Verify that sample offsets do not exceed file size
                                if (chunkOffset + sampleSize > fileStream.Length)
                                {
                                    MessageBox.Show("Sample offset exceeds file size.");
                                    return opusData;
                                }

                                // Read the sample data
                                byte[] sampleBytes = new byte[sampleSize];
                                fileStream.Seek(chunkOffset, SeekOrigin.Begin);
                                int bytesRead = fileStream.Read(sampleBytes, 0, (int)sampleSize);

                                if (bytesRead != sampleSize)
                                {
                                    MessageBox.Show("Failed to read the expected amount of sample data.");
                                    return opusData;
                                }

                                // Prepend the size identifier to the sample data
                                byte[] sizeBytes = BitConverter.GetBytes((int)sampleSize);
                                opusData.AddRange(sizeBytes); // Add size bytes

                                // Add to the Opus data list
                                opusData.AddRange(sampleBytes);
                                opusPackets.Add(sampleBytes);

                                // Move to the next sample
                                sampleIndex++;
                                chunkOffset += sampleSize;
                            }
                        }
                        break; // Stop after processing mdat
                    }
                    else
                    {
                        currentOffset += boxSize;
                    }
                }
            }

            return opusData;
        }

        private static int FortniteFestivalToBassStream()
        {
            try
            {
                BASS_OPUS_HEAD head = new BASS_OPUS_HEAD
                {
                    version = 1,
                    channels = (byte)channelCount,
                    preskip = 312,
                    inputrate = 48000,
                    gain = 0,
                    mapping = channelCount == 2 ? (byte)0 : (byte)255,
                    streams = (byte)(channelCount / 2),
                    coupled = (byte)(channelCount / 2),
                    chanmap = new byte[] { }
                };
                head.chanmap = new byte[255];
                for (int i = 0; i < head.channels; i++)
                {
                    head.chanmap[i] = (byte)i;
                }
                int streamHandle = BASS_OPUS_StreamCreate(ref head, (uint)(BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT), STREAMPROC_PUSH, IntPtr.Zero);

                if (streamHandle == 0)
                {
                    MessageBox.Show("Failed to create Opus stream. Error: " + Bass.BASS_ErrorGetCode());
                    return 0;
                }

                PushOpusPacketsToStream(streamHandle);
                return streamHandle;
            }
            catch (Exception e) 
            {
                MessageBox.Show("Error creating stream handle: " + e.Message + "\nBASS says: " + Bass.BASS_ErrorGetCode());
                return 0;
            }
        }

        private static void PushOpusPacketsToStream(int streamHandle)
        {
            GCHandle handle = GCHandle.Alloc(opusSamples, GCHandleType.Pinned);
            try
            {
                IntPtr pointer = handle.AddrOfPinnedObject();
                int offset = 0;
                int packetCount = 0;
                int totalBytesPushed = 0;

                while (offset < opusSamples.Length)
                {
                    // Check for size identifier
                    if (offset + 4 > opusSamples.Length)
                    {
                        MessageBox.Show("Not enough data for size identifier.");
                        break;
                    }

                    int packetSize = BitConverter.ToInt32(opusSamples, offset);
                    offset += 4;

                    if (packetSize <= 0 || offset + packetSize > opusSamples.Length)
                    {
                        MessageBox.Show("Invalid or incomplete packet data.");
                        break;
                    }

                    int result = BASS_OPUS_StreamPutData(streamHandle, IntPtr.Add(pointer, offset), packetSize);
                    if (result == -1)
                    {
                        MessageBox.Show("Failed to push data. Error: " + Bass.BASS_ErrorGetCode());
                        break;
                    }

                    offset += packetSize;
                    totalBytesPushed += packetSize;
                    packetCount++;

                    //MessageBox.Show($"Pushed packet {packetCount}: {packetSize} bytes");
                }

                //MessageBox.Show($"Total bytes pushed: {totalBytesPushed}");
                //MessageBox.Show($"Total packets processed: {packetCount}");

                // Signal end of stream
                BASS_OPUS_StreamPutData(streamHandle, IntPtr.Zero, (int)BASSStreamProc.BASS_STREAMPROC_END);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
