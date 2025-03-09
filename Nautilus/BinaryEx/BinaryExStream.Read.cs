// Copyright (c) 2019-2024 Matthew Sitton <matthewsitton@gmail.com>
// MIT License - See LICENSE in the project root for license information.
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BinaryEx
{
    public static partial class BinaryExStream
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt32 ReadUInt24LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 3);

            return scratch.ReadUInt24LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt32 ReadUInt24BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 3);

            return scratch.ReadUInt24BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int32 ReadInt24LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 3);

            return scratch.ReadInt24LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int32 ReadInt24BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 3);

            return scratch.ReadInt24BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int64 ReadInt64LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadInt64LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int64 ReadInt64BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadInt64BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int32 ReadInt32LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadInt32LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int32 ReadInt32BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadInt32BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int16 ReadInt16LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 2);

            return scratch.ReadInt16LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int16 ReadInt16BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 2);

            return scratch.ReadInt16BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static sbyte ReadSByte(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 1);

            return scratch.ReadSByte(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt64 ReadUInt64LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadUInt64LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt64 ReadUInt64BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadUInt64BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt32 ReadUInt32LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadUInt32LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt32 ReadUInt32BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadUInt32BE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt16 ReadUInt16LE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 2);

            return scratch.ReadUInt16LE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt16 ReadUInt16BE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 2);

            return scratch.ReadUInt16BE(0);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static float ReadFloatLE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadFloatLE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static float ReadFloatBE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 4);

            return scratch.ReadFloatBE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static double ReadDoubleLE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadDoubleLE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static double ReadDoubleBE(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 8);

            return scratch.ReadDoubleBE(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static byte ReadByte(this Stream data)
        {
            Debug.Assert(data.CanRead);

            byte[] scratch = EnsureScratch();

            data.Read(scratch, 0, 1);

            return scratch.ReadByte(0);
        }

#if NETSTANDARD2_1_OR_GREATER

        // only implemented for LE because we don't know what the layout of the struct/object is
        // so there is no way to safely swap the endianness
        public static int ReadCountLE<T>(this Stream data, Span<T> output) where T : unmanaged
        {
            Debug.Assert(data.CanRead);
            var bytes = MemoryMarshal.AsBytes(output);
            return data.Read(bytes);
        }

        public static int ReadCountLE<T>(this Stream data, T[] output, int count) where T : unmanaged
        {
            Debug.Assert(data.CanRead);
            var bytes = MemoryMarshal.AsBytes(output.AsSpan(0, count));
            return data.Read(bytes);
        }
#endif
    }
}