// Copyright (c) 2019-2024 Matthew Sitton <matthewsitton@gmail.com>
// MIT License - See LICENSE in the project root for license information.
using System;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace BinaryEx
{
    public static partial class BinaryExRef
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt64BE(byte* buff, ref int offset, Int64 value)
        {
            BinaryEx.WriteInt64BE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt64LE(byte* buff, ref int offset, Int64 value)
        {
            BinaryEx.WriteInt64LE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt32BE(byte* buff, ref int offset, Int32 value)
        {
            BinaryEx.WriteInt32BE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt32LE(byte* buff, ref int offset, Int32 value)
        {
            BinaryEx.WriteInt32LE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt24BE(byte* buff, ref int offset, Int32 value)
        {
            BinaryEx.WriteInt24BE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt24LE(byte* buff, ref int offset, Int32 value)
        {
            BinaryEx.WriteInt24LE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt16BE(byte* buff, ref int offset, Int16 value)
        {
            BinaryEx.WriteInt16BE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt16LE(byte* buff, ref int offset, Int16 value)
        {
            BinaryEx.WriteInt16LE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteSByte(byte* buff, ref int offset, sbyte value)
        {
            BinaryEx.WriteSByte(buff, offset, value);
            offset += 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt64BE(byte* buff, ref int offset, UInt64 value)
        {
            BinaryEx.WriteUInt64BE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt64LE(byte* buff, ref int offset, UInt64 value)
        {
            BinaryEx.WriteUInt64LE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt32BE(byte* buff, ref int offset, UInt32 value)
        {
            BinaryEx.WriteUInt32BE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt32LE(byte* buff, ref int offset, UInt32 value)
        {
            BinaryEx.WriteUInt32LE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt24BE(byte* buff, ref int offset, UInt32 value)
        {
            BinaryEx.WriteUInt24BE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt24LE(byte* buff, ref int offset, UInt32 value)
        {
            BinaryEx.WriteUInt24LE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt16BE(byte* buff, ref int offset, UInt16 value)
        {
            BinaryEx.WriteUInt16BE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt16LE(byte* buff, ref int offset, UInt16 value)
        {
            BinaryEx.WriteUInt16LE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteFloatBE(byte* buff, ref int offset, float value)
        {
            BinaryEx.WriteFloatBE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteFloatLE(byte* buff, ref int offset, float value)
        {
            BinaryEx.WriteFloatLE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteDoubleBE(byte* buff, ref int offset, double value)
        {
            BinaryEx.WriteDoubleBE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteDoubleLE(byte* buff, ref int offset, double value)
        {
            BinaryEx.WriteDoubleLE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteByte(byte* buff, ref int offset, byte value)
        {
            BinaryEx.WriteByte(buff, offset, value);
            offset += 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteBytes(byte* buff, ref int offset, byte[] input, int count)
        {
            offset += BinaryEx.WriteBytes(buff, offset, input, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteBytes(byte* buff, ref int offset, ReadOnlySpan<byte> input)
        {
            offset += BinaryEx.WriteBytes(buff, offset, input);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteCountLE<T>(byte* buff, ref int offset, T[] input, int count) where T : unmanaged
        {
            offset += BinaryEx.WriteCountLE(buff, offset, input, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteCountLE<T>(byte* buff, ref int offset, ReadOnlySpan<T> input) where T : unmanaged
        {
            offset += BinaryEx.WriteCountLE(buff, offset, input);
        }
    }
}