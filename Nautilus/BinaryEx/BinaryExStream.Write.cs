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
        [ThreadStatic] static byte[] scratchData;

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        private static byte[] EnsureScratch()
        {
            if (scratchData == null)
            {
                scratchData = new byte[8];
            }

            return scratchData;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt24BE(this Stream data, Int32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt24BE(0, value);
            data.Write(scratch, 0, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt24LE(this Stream data, Int32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt24LE(0, value);
            data.Write(scratch, 0, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt64BE(this Stream data, Int64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt64BE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt64LE(this Stream data, Int64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt64LE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt32BE(this Stream data, Int32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt32BE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt32LE(this Stream data, Int32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt32LE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt16BE(this Stream data, Int16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt16BE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt16LE(this Stream data, Int16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteInt16LE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteSByte(this Stream data, sbyte value)
        {
            Debug.Assert(data.CanWrite);
            data.WriteByte((byte)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt24BE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt24BE(0, value);
            data.Write(scratch, 0, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt24LE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt24LE(0, value);
            data.Write(scratch, 0, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt64BE(this Stream data, UInt64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt64BE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt64LE(this Stream data, UInt64 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt64LE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt32BE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt32BE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt32LE(this Stream data, UInt32 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt32LE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt16BE(this Stream data, UInt16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt16BE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt16LE(this Stream data, UInt16 value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteUInt16LE(0, value);
            data.Write(scratch, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteFloatBE(this Stream data, float value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteFloatBE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteFloatLE(this Stream data, float value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteFloatLE(0, value);
            data.Write(scratch, 0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteDoubleBE(this Stream data, double value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteDoubleBE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteDoubleLE(this Stream data, double value)
        {
            Debug.Assert(data.CanWrite);

            byte[] scratch = EnsureScratch();

            scratch.WriteDoubleLE(0, value);
            data.Write(scratch, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteByte(this Stream data, byte value)
        {
            Debug.Assert(data.CanWrite);
            data.WriteByte(value);
        }

#if NETSTANDARD2_1_OR_GREATER
        // only implemented for LE because we don't know what the layout of the struct/object is
        // so there is no way to safely swap the endianness
        public static void WriteCountLE<T>(this Stream data, ReadOnlySpan<T> input) where T : unmanaged
        {
            Debug.Assert(data.CanWrite);
            var bytes = MemoryMarshal.AsBytes(input);
            data.Write(bytes);
        }

        public static void WriteCountLE<T>(this Stream data, T[] input, int count) where T : unmanaged
        {
            Debug.Assert(data.CanWrite);
            Debug.Assert(count > 0);
            var bytes = MemoryMarshal.AsBytes<T>(input);
            data.Write(bytes);
        }
#endif
    }
}