// Copyright (c) 2019-2024 Matthew Sitton <matthewsitton@gmail.com>
// MIT License - See LICENSE in the project root for license information.
using System;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BinaryEx
{
    public static partial class BinaryEx
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt64BE(byte* buff, int offset, Int64 value)
        {
            WriteUInt64BE(buff, offset, (UInt64)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt64LE(byte* buff, int offset, Int64 value)
        {
            WriteUInt64LE(buff, offset, (UInt64)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt32BE(byte* buff, int offset, Int32 value)
        {
            WriteUInt32BE(buff, offset, (UInt32)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt32LE(byte* buff, int offset, Int32 value)
        {
            WriteUInt32LE(buff, offset, (UInt32)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt24BE(byte* buff, int offset, Int32 value)
        {
            Debug.Assert(value <= 0x7FFFFF && value >= -0x7FFFFF);
            WriteUInt24BE(buff, offset, (UInt32)(value & 0xFFFFFF));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt24LE(byte* buff, int offset, Int32 value)
        {
            Debug.Assert(value <= 0x7FFFFF && value >= -0x7FFFFF);
            WriteUInt24LE(buff, offset, (UInt32)(value & 0xFFFFFF));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt16BE(byte* buff, int offset, Int16 value)
        {
            WriteUInt16BE(buff, offset, (UInt16)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteInt16LE(byte* buff, int offset, Int16 value)
        {
            WriteUInt16LE(buff, offset, (UInt16)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteSByte(byte* buff, int offset, sbyte value)
        {
            WriteByte(buff, offset, (byte)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt64BE(byte* buff, int offset, UInt64 value)
        {
            Unsafe.WriteUnaligned<UInt64>(ref buff[offset], Endian.SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt64LE(byte* buff, int offset, UInt64 value)
        {
            Unsafe.WriteUnaligned<UInt64>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt32BE(byte* buff, int offset, UInt32 value)
        {
            Unsafe.WriteUnaligned<UInt32>(ref buff[offset], Endian.SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt32LE(byte* buff, int offset, UInt32 value)
        {
            Unsafe.WriteUnaligned<UInt32>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt24LE(byte* buff, int offset, UInt32 value)
        {
            Debug.Assert(value <= 0xFFFFFF);

            buff[offset] = (byte)value;
            buff[offset + 1] = (byte)(value >> 8);
            buff[offset + 2] = (byte)(value >> 16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt24BE(byte* buff, int offset, UInt32 value)
        {
            Debug.Assert(value <= 0xFFFFFF);

            buff[offset] = (byte)(value >> 16);
            buff[offset + 1] = (byte)(value >> 8);
            buff[offset + 2] = (byte)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt16BE(byte* buff, int offset, UInt16 value)
        {
            Unsafe.WriteUnaligned<UInt16>(ref buff[offset], Endian.SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteUInt16LE(byte* buff, int offset, UInt16 value)
        {
            Unsafe.WriteUnaligned<UInt16>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteFloatLE(byte* buff, int offset, float value)
        {
            Unsafe.WriteUnaligned<float>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteFloatBE(byte* buff, int offset, float value)
        {
            var swapped = Endian.SwapEndianess(Unsafe.As<float, UInt32>(ref value));
            Unsafe.WriteUnaligned<UInt32>(ref buff[offset], swapped);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteDoubleLE(byte* buff, int offset, double value)
        {
            Unsafe.WriteUnaligned<double>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteDoubleBE(byte* buff, int offset, double value)
        {
            var swapped = Endian.SwapEndianess(Unsafe.As<double, UInt64>(ref value));
            Unsafe.WriteUnaligned<UInt64>(ref buff[offset], swapped);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static void WriteByte(byte* buff, int offset, byte value)
        {
            Unsafe.WriteUnaligned<byte>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static int WriteBytes(byte* buff, int offset, byte[] input, int count)
        {
            Debug.Assert(count > 0);
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref input[0], (uint)count);
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static int WriteBytes(byte* buff, int offset, ReadOnlySpan<byte> input)
        {
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref Unsafe.AsRef(in input[0]), (uint)input.Length);
            return input.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static int WriteCountLE<T>(byte* buff, int offset, T[] input, int count) where T : unmanaged
        {
            var bytes = MemoryMarshal.AsBytes(input.AsSpan(0, count));
            Debug.Assert(count > 0);
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref bytes[0], (uint)bytes.Length);
            return bytes.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public unsafe static int WriteCountLE<T>(byte* buff, int offset, ReadOnlySpan<T> input) where T : unmanaged
        {
            var bytes = MemoryMarshal.AsBytes(input);
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref Unsafe.AsRef(in bytes[0]), (uint)bytes.Length);
            return bytes.Length;
        }
    }
}