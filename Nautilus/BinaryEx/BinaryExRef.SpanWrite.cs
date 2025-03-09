// Copyright (c) 2019-2024 Matthew Sitton <matthewsitton@gmail.com>
// MIT License - See LICENSE in the project root for license information.
using System;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace BinaryEx
{
    public static partial class BinaryExRef
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt64BE(this Span<byte> buff, ref int offset, Int64 value)
        {
            BinaryEx.WriteInt64BE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt64LE(this Span<byte> buff, ref int offset, Int64 value)
        {
            BinaryEx.WriteInt64LE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt32BE(this Span<byte> buff, ref int offset, Int32 value)
        {
            BinaryEx.WriteInt32BE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt32LE(this Span<byte> buff, ref int offset, Int32 value)
        {
            BinaryEx.WriteInt32LE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt24BE(this Span<byte> buff, ref int offset, Int32 value)
        {
            BinaryEx.WriteInt24BE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt24LE(this Span<byte> buff, ref int offset, Int32 value)
        {
            BinaryEx.WriteInt24LE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt16BE(this Span<byte> buff, ref int offset, Int16 value)
        {
            BinaryEx.WriteInt16BE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt16LE(this Span<byte> buff, ref int offset, Int16 value)
        {
            BinaryEx.WriteInt16LE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteSByte(this Span<byte> buff, ref int offset, sbyte value)
        {
            BinaryEx.WriteSByte(buff, offset, value);
            offset += 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt64BE(this Span<byte> buff, ref int offset, UInt64 value)
        {
            BinaryEx.WriteUInt64BE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt64LE(this Span<byte> buff, ref int offset, UInt64 value)
        {
            BinaryEx.WriteUInt64LE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt32BE(this Span<byte> buff, ref int offset, UInt32 value)
        {
            BinaryEx.WriteUInt32BE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt32LE(this Span<byte> buff, ref int offset, UInt32 value)
        {
            BinaryEx.WriteUInt32LE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt24BE(this Span<byte> buff, ref int offset, UInt32 value)
        {
            BinaryEx.WriteUInt24BE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt24LE(this Span<byte> buff, ref int offset, UInt32 value)
        {
            BinaryEx.WriteUInt24LE(buff, offset, value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt16BE(this Span<byte> buff, ref int offset, UInt16 value)
        {
            BinaryEx.WriteUInt16BE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt16LE(this Span<byte> buff, ref int offset, UInt16 value)
        {
            BinaryEx.WriteUInt16LE(buff, offset, value);
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteFloatBE(this Span<byte> buff, ref int offset, float value)
        {
            BinaryEx.WriteFloatBE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteFloatLE(this Span<byte> buff, ref int offset, float value)
        {
            BinaryEx.WriteFloatLE(buff, offset, value);
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteDoubleBE(this Span<byte> buff, ref int offset, double value)
        {
            BinaryEx.WriteDoubleBE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteDoubleLE(this Span<byte> buff, ref int offset, double value)
        {
            BinaryEx.WriteDoubleLE(buff, offset, value);
            offset += 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteByte(this Span<byte> buff, ref int offset, byte value)
        {
            BinaryEx.WriteByte(buff, offset, value);
            offset += 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteBytes(this Span<byte> buff, ref int offset, byte[] input, int count)
        {
            offset += BinaryEx.WriteBytes(buff, offset, input, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteBytes(this Span<byte> buff, ref int offset, ReadOnlySpan<byte> input)
        {
            offset += BinaryEx.WriteBytes(buff, offset, input);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteCountLE<T>(this Span<byte> buff, ref int offset, T[] input, int count) where T : unmanaged
        {
            offset += BinaryEx.WriteCountLE(buff, offset, input, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteCountLE<T>(this Span<byte> buff, ref int offset, ReadOnlySpan<T> input) where T : unmanaged
        {
            offset += BinaryEx.WriteCountLE(buff, offset, input);
        }

    }
}