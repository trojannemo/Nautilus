using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nautilus.Cysharp.Collections
{
    [DebuggerTypeProxy(typeof(NativeMemoryArrayDebugView))]
    public sealed unsafe class NativeByteArray : IDisposable
    {
        public static readonly NativeByteArray Empty;
        internal byte* buffer;
        long allocatedLength;
        long length;
        bool isDisposed;
        readonly bool addMemoryPressure;
        readonly bool internalAllocated;

        public long Length => length;

        static NativeByteArray()
        {
            Empty = new NativeByteArray(0);
            Empty.Dispose();
        }

        public NativeByteArray(byte* unsafePtr, long length)
        {
            this.length = length;
            this.allocatedLength = length;
            internalAllocated = false;
            buffer = unsafePtr;
        }

        // default to 16mb
        public NativeByteArray(bool skipZeroClear = false, bool addMemoryPressure = false) : this(0x1000000, skipZeroClear, addMemoryPressure)
        { }

        public NativeByteArray(long length, bool skipZeroClear = false, bool addMemoryPressure = false)
        {
            this.length = length;
            this.allocatedLength = length;
            this.addMemoryPressure = addMemoryPressure;

            internalAllocated = true;

            if (length == 0)
            {
                buffer = null; // Replaces Unsafe.NullRef<byte>()
            }
            else
            {
                if (skipZeroClear)
                {
                    buffer = (byte*)Marshal.AllocHGlobal((IntPtr)length);
                }
                else
                {
                    buffer = (byte*)Marshal.AllocHGlobal((IntPtr)length);
                    Unsafe.InitBlock(buffer, 0, (uint)length); // Zero out manually
                }

                if (addMemoryPressure)
                {
                    GC.AddMemoryPressure(length);
                }
            }
        }


        public ref byte this[long index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((ulong)index >= (ulong)length) ThrowHelper.ThrowIndexOutOfRangeException();
                return ref Unsafe.AsRef<byte>(buffer + index);
            }
        }

        private static long NextPO2(long v)
        {
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v |= v >> 32;  // added line to handle 64 bits
            v++;
            return v;
        }

        public bool Resize(long newLength)
        {
            if (newLength <= allocatedLength)
            {
                length = newLength;
                return true;
            }
            if (!internalAllocated)
            {
                return false;
            }

            // Compute the next power of two (PO2) for efficient memory growth
            newLength = NextPO2(newLength);

            // Reallocate memory using Marshal.ReAllocHGlobal()
            IntPtr newBuffer = Marshal.ReAllocHGlobal((IntPtr)buffer, (IntPtr)newLength);
            buffer = (byte*)newBuffer;

            allocatedLength = newLength;
            length = newLength;
            return true;
        }

        public Span<byte> AsSpan()
        {
            return AsSpan(0);
        }

        public Span<byte> AsSpan(long start)
        {
            if ((ulong)start > (ulong)length) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(start));
            return AsSpan(start, checked((int)(length - start)));
        }

        public Span<byte> AsSpan(long start, int length)
        {
            if ((ulong)(start + length) > (ulong)this.length) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(length));
            return new Span<byte>(buffer + start, length);
        }

        public Memory<byte> AsMemory()
        {
            return AsMemory(0);
        }

        public Memory<byte> AsMemory(long start)
        {
            if ((ulong)start > (ulong)length) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(start));
            return AsMemory(start, checked((int)(length - start)));
        }

        public Memory<byte> AsMemory(long start, int length)
        {
            if ((ulong)(start + length) > (ulong)this.length) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(length));
            return new PointerMemoryManager(buffer + start, length).Memory;
        }

        public Stream AsStream()
        {
            return new UnmanagedMemoryStream(buffer, length);
        }

        public Stream AsStream(long offset)
        {
            if ((ulong)offset > (ulong)length) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(offset));
            return new UnmanagedMemoryStream(buffer + offset, length);
        }

        public Stream AsStream(FileAccess fileAccess)
        {
            return new UnmanagedMemoryStream(buffer, length, length, fileAccess);
        }

        public Stream AsStream(long offset, FileAccess fileAccess)
        {
            if ((ulong)offset > (ulong)length) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(offset));
            return new UnmanagedMemoryStream(buffer + offset, length, length, fileAccess);
        }

        private static byte dummyByte = 0; // Static dummy variable

        public ref byte GetPinnableReference()
        {
            if (length == 0)
            {
                return ref dummyByte; // Return a safe reference
            }
            return ref this[0];
        }

        public bool TryGetFullSpan(out Span<byte> span)
        {
            if (length < int.MaxValue)
            {
                span = new Span<byte>(buffer, (int)length);
                return true;
            }
            else
            {
                span = default;
                return false;
            }
        }

        public IBufferWriter<byte> CreateBufferWriter()
        {
            return new NativeMemoryArrayBufferWriter(this);
        }

        public SpanSequence AsSpanSequence(int chunkSize = int.MaxValue)
        {
            return new SpanSequence(this, chunkSize);
        }

        public MemorySequence AsMemorySequence(int chunkSize = int.MaxValue)
        {
            return new MemorySequence(this, chunkSize);
        }

        public IReadOnlyList<Memory<byte>> AsMemoryList(int chunkSize = int.MaxValue)
        {
            if (length == 0) return Array.Empty<Memory<byte>>();

            var array = new Memory<byte>[length <= chunkSize ? 1 : length / chunkSize + 1];
            {
                var i = 0;
                foreach (var item in AsMemorySequence(chunkSize))
                {
                    array[i++] = item;
                }
            }

            return array;
        }

        public IReadOnlyList<ReadOnlyMemory<byte>> AsReadOnlyMemoryList(int chunkSize = int.MaxValue)
        {
            if (length == 0) return Array.Empty<ReadOnlyMemory<byte>>();

            var array = new ReadOnlyMemory<byte>[length <= chunkSize ? 1 : (length / chunkSize + 1)];
            {
                var i = 0;
                foreach (var item in AsMemorySequence(chunkSize))
                {
                    array[i++] = item;
                }
            }

            return array;
        }

        public ReadOnlySequence<byte> AsReadOnlySequence(int chunkSize = int.MaxValue)
        {
            if (length == 0) return ReadOnlySequence<byte>.Empty;

            var array = new Segment[length <= chunkSize ? 1 : length / chunkSize + 1];
            {
                var i = 0;
                foreach (var item in AsMemorySequence(chunkSize))
                {
                    array[i++] = new Segment(item);
                }
            }

            long running = 0;
            for (int i = 0; i < array.Length; i++)
            {
                var next = i < array.Length - 1 ? array[i + 1] : null;
                array[i].SetRunningIndexAndNext(running, next);
                running += array[i].Memory.Length;
            }

            var firstSegment = array[0];
            var lastSegment = array[array.Length - 1];
            return new ReadOnlySequence<byte>(firstSegment, 0, lastSegment, lastSegment.Memory.Length);
        }

        public SpanSequence GetEnumerator()
        {
            return AsSpanSequence(int.MaxValue);
        }

        public override string ToString()
        {
            return typeof(byte).Name + "[" + length + "]";
        }

        public void Dispose()
        {
            DisposeCore();
            GC.SuppressFinalize(this);
        }

        void DisposeCore()
        {
            if (!isDisposed && internalAllocated)
            {
                isDisposed = true;

                // Check if buffer is null instead of using Unsafe.IsNullRef()
                if (buffer == null) return;

                // Free memory using Marshal.FreeHGlobal()
                Marshal.FreeHGlobal((IntPtr)buffer);

                if (addMemoryPressure)
                {
                    GC.RemoveMemoryPressure(length);
                }

                buffer = null; // Avoid dangling pointers
            }
        }


        ~NativeByteArray()
        {
            DisposeCore();
        }

        public struct SpanSequence
        {
            readonly NativeByteArray nativeArray;
            readonly int chunkSize;
            long index;
            long sliceStart;

            internal SpanSequence(NativeByteArray nativeArray, int chunkSize)
            {
                this.nativeArray = nativeArray;
                index = 0;
                sliceStart = 0;
                this.chunkSize = chunkSize;
            }

            public SpanSequence GetEnumerator() => this;

            public Span<byte> Current
            {
                get
                {
                    return nativeArray.AsSpan(sliceStart, (int)Math.Min(chunkSize, nativeArray.length - sliceStart));
                }
            }

            public bool MoveNext()
            {
                if (index < nativeArray.length)
                {
                    sliceStart = index;
                    index += chunkSize;
                    return true;
                }
                return false;
            }
        }

        public struct MemorySequence
        {
            readonly NativeByteArray nativeArray;
            readonly int chunkSize;
            long index;
            long sliceStart;

            internal MemorySequence(NativeByteArray nativeArray, int chunkSize)
            {
                this.nativeArray = nativeArray;
                index = 0;
                sliceStart = 0;
                this.chunkSize = chunkSize;
            }

            public MemorySequence GetEnumerator() => this;

            public Memory<byte> Current
            {
                get
                {
                    return nativeArray.AsMemory(sliceStart, (int)Math.Min(chunkSize, nativeArray.length - sliceStart));
                }
            }

            public bool MoveNext()
            {
                if (index < nativeArray.length)
                {
                    sliceStart = index;
                    index += chunkSize;
                    return true;
                }
                return false;
            }
        }

        class Segment : ReadOnlySequenceSegment<byte>
        {
            public Segment(Memory<byte> buffer)
            {
                Memory = buffer;
            }

            internal void SetRunningIndexAndNext(long runningIndex, Segment nextSegment)
            {
                RunningIndex = runningIndex;
                Next = nextSegment;
            }
        }
    }

    internal sealed unsafe class NativeMemoryArrayBufferWriter : IBufferWriter<byte>
    {
        readonly NativeByteArray nativeArray;
        PointerMemoryManager pointerMemoryManager;
        long written;

        internal NativeMemoryArrayBufferWriter(NativeByteArray nativeArray)
        {
            this.nativeArray = nativeArray;
            pointerMemoryManager = null;
        }

        public void Advance(int count)
        {
            written += count;
            if (pointerMemoryManager != null)
            {
                pointerMemoryManager.AllowReuse();
            }
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            if (sizeHint < 0) throw new InvalidOperationException($"sizeHint:{sizeHint} is invalid range.");
            long spaceRemaining = nativeArray.Length - written;
            if (spaceRemaining < sizeHint)
            {
                if (!nativeArray.Resize(written + sizeHint))
                {
                    throw new InvalidOperationException($"sizeHint:{sizeHint} is capacity:{nativeArray.Length} - written:{written} over");
                }
            }
            var length = (int)Math.Min(int.MaxValue, nativeArray.Length - written);

            if (length == 0) return Array.Empty<byte>();
            return new Span<byte>(nativeArray.buffer + written * Unsafe.SizeOf<byte>(), length);
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            if (sizeHint < 0) throw new InvalidOperationException($"sizeHint:{sizeHint} is invalid range.");
            long spaceRemaining = nativeArray.Length - written;
            if (spaceRemaining < sizeHint)
            {
                if (!nativeArray.Resize(written + sizeHint))
                {
                    throw new InvalidOperationException($"sizeHint:{sizeHint} is capacity:{nativeArray.Length} - written:{written} over");
                }
            }
            spaceRemaining = nativeArray.Length - written;

            var length = (int)Math.Min(int.MaxValue, spaceRemaining);
            if (length == 0) return Array.Empty<byte>();

            if (pointerMemoryManager == null)
            {
                pointerMemoryManager = new PointerMemoryManager(nativeArray.buffer + written, length);
            }
            else
            {
                pointerMemoryManager.Reset(nativeArray.buffer + written, length);
            }

            return pointerMemoryManager.Memory;
        }
    }

    internal sealed class NativeMemoryArrayDebugView
    {
        private readonly NativeByteArray array;

        public NativeMemoryArrayDebugView(NativeByteArray array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            this.array = array;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Span<byte> Items
        {
            get
            {
                if (array.TryGetFullSpan(out var span))
                {
                    return span;
                }
                else
                {
                    return array.AsSpan(0, 1000000); // limit
                }
            }
        }
    }
}
