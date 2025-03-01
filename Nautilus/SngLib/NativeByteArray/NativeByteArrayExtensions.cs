using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Nautilus.Cysharp.Collections
{
    public static class NativeMemoryArrayExtensions
    {
        public static async Task ReadFromAsync(this NativeByteArray buffer, Stream stream, IProgress<long> progress = null, CancellationToken cancellationToken = default)
        {
            long readTotal = 0;
            int read;

            byte[] bufferChunk = new byte[8192]; // ✅ Fixed 8KB chunk (no huge buffer)

            while ((read = await stream.ReadAsync(bufferChunk, 0, bufferChunk.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                progress?.Report(readTotal);

                // ✅ Correct pointer arithmetic for unmanaged memory
                unsafe
                {
                    Marshal.Copy(bufferChunk, 0, IntPtr.Add((IntPtr)buffer.buffer, (int)(readTotal)), read);
                }

                readTotal += read; // ✅ Update read total after copying
            }

            // ✅ Ensure correct final buffer size
            buffer.Resize(readTotal);
        }

        public static async Task WriteToFileAsync(this NativeByteArray buffer, string path, FileMode mode = FileMode.Create, IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            using (var fs = new FileStream(path, mode, FileAccess.Write, FileShare.ReadWrite, 1, useAsync: true))
            {
                await buffer.WriteToAsync(fs, progress: progress, cancellationToken: cancellationToken);
            }
        }

        public static async Task WriteToAsync(this NativeByteArray buffer, Stream stream, int chunkSize = int.MaxValue, IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            foreach (var item in buffer.AsReadOnlyMemoryList(chunkSize))
            {
                byte[] data = item.ToArray(); // Convert ReadOnlyMemory<byte> to byte[]
                await stream.WriteAsync(data, 0, data.Length, cancellationToken); // Use correct overload

                progress?.Report(data.Length);
            }
        }
    }
}