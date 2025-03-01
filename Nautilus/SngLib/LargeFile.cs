using System.Buffers;
using Nautilus.Cysharp.Collections;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Nautilus.Sng
{
    public static class LargeFile
    {
        public static async Task ReadAllBytesAsync(string path, NativeByteArray arr)
        {
            using (FileStream f = File.OpenRead(path))
            {
                await arr.ReadFromAsync(f); // ✅ Stream directly into NativeByteArray
            }
        }

        public static void ReadToNativeArray(this Stream stream, NativeByteArray arr, long readCount)
        {
            var writer = arr.CreateBufferWriter();
            long readTotal = 0;
            int read;

            byte[] buffer = new byte[8192]; // Fixed 8KB buffer to avoid large allocations

            while (readTotal < readCount && (read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                readTotal += read;
                writer.Write(buffer.AsSpan(0, read)); // Only write the bytes that were read
            }

            // Ensure `arr` size is set correctly
            arr.Resize(readTotal);
        }

        public static void WriteFromNativeArray(this Stream stream, NativeByteArray arr, int chunkSize = int.MaxValue)
        {
            foreach (var item in arr.AsReadOnlyMemoryList(chunkSize))
            {
                stream.Write(item.ToArray(), 0, item.Length);
            }
        }
    }
}