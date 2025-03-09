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
        public static void WriteInt64BE(this Span<byte> buff, int offset, Int64 value)
        {
            WriteUInt64BE(buff, offset, (UInt64)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt64LE(this Span<byte> buff, int offset, Int64 value)
        {
            WriteUInt64LE(buff, offset, (UInt64)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt32BE(this Span<byte> buff, int offset, Int32 value)
        {
            WriteUInt32BE(buff, offset, (UInt32)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt32LE(this Span<byte> buff, int offset, Int32 value)
        {
            WriteUInt32LE(buff, offset, (UInt32)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt24BE(this Span<byte> buff, int offset, Int32 value)
        {
            Debug.Assert(value <= 0x7FFFFF && value >= -0x7FFFFF);
            Debug.Assert(buff.Length >= offset + 3);
            WriteUInt24BE(buff, offset, (UInt32)(value & 0xFFFFFF));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt24LE(this Span<byte> buff, int offset, Int32 value)
        {
            Debug.Assert(value <= 0x7FFFFF && value >= -0x7FFFFF);
            Debug.Assert(buff.Length >= offset + 3);
            WriteUInt24LE(buff, offset, (UInt32)(value & 0xFFFFFF));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt16BE(this Span<byte> buff, int offset, Int16 value)
        {
            WriteUInt16BE(buff, offset, (UInt16)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteInt16LE(this Span<byte> buff, int offset, Int16 value)
        {
            WriteUInt16LE(buff, offset, (UInt16)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteSByte(this Span<byte> buff, int offset, sbyte value)
        {
            WriteByte(buff, offset, (byte)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt64BE(this Span<byte> buff, int offset, UInt64 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt64>());
            Unsafe.WriteUnaligned<UInt64>(ref buff[offset], Endian.SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt64LE(this Span<byte> buff, int offset, UInt64 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt64>());
            Unsafe.WriteUnaligned<UInt64>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt32BE(this Span<byte> buff, int offset, UInt32 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt32>());
            Unsafe.WriteUnaligned<UInt32>(ref buff[offset], Endian.SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt32LE(this Span<byte> buff, int offset, UInt32 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt32>());
            Unsafe.WriteUnaligned<UInt32>(ref buff[offset], value);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt24LE(this Span<byte> buff, int offset, UInt32 value)
        {
            Debug.Assert(value <= 0xFFFFFF);
            Debug.Assert(buff.Length >= offset + 3);

            buff[offset] = (byte)value;
            buff[offset + 1] = (byte)(value >> 8);
            buff[offset + 2] = (byte)(value >> 16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt24BE(this Span<byte> buff, int offset, UInt32 value)
        {
            Debug.Assert(value <= 0xFFFFFF);
            Debug.Assert(buff.Length >= offset + 3);

            buff[offset] = (byte)(value >> 16);
            buff[offset + 1] = (byte)(value >> 8);
            buff[offset + 2] = (byte)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt16BE(this Span<byte> buff, int offset, UInt16 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt16>());
            Unsafe.WriteUnaligned<UInt16>(ref buff[offset], Endian.SwapEndianess(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteUInt16LE(this Span<byte> buff, int offset, UInt16 value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt16>());
            Unsafe.WriteUnaligned<UInt16>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteFloatLE(this Span<byte> buff, int offset, float value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<float>());
            Unsafe.WriteUnaligned<float>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteFloatBE(this Span<byte> buff, int offset, float value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<float>());
            var swapped = Endian.SwapEndianess(Unsafe.As<float, UInt32>(ref value));
            Unsafe.WriteUnaligned<UInt32>(ref buff[offset], swapped);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteDoubleLE(this Span<byte> buff, int offset, double value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<double>());
            Unsafe.WriteUnaligned<double>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteDoubleBE(this Span<byte> buff, int offset, double value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<double>());
            var swapped = Endian.SwapEndianess(Unsafe.As<double, UInt64>(ref value));
            Unsafe.WriteUnaligned<UInt64>(ref buff[offset], swapped);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static void WriteByte(this Span<byte> buff, int offset, byte value)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<byte>());
            Unsafe.WriteUnaligned<byte>(ref buff[offset], value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static int WriteBytes(this Span<byte> buff, int offset, byte[] input, int count)
        {
            Debug.Assert(count > 0);
            Debug.Assert(buff.Length >= offset + count && count >= 0);
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref input[0], (uint)count);
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static int WriteBytes(this Span<byte> buff, int offset, ReadOnlySpan<byte> input)
        {
            Debug.Assert(buff.Length >= offset + input.Length);
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref Unsafe.AsRef(in input[0]), (uint)input.Length);
            return input.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static int WriteCountLE<T>(this Span<byte> buff, int offset, T[] input, int count) where T : unmanaged
        {
            var bytes = MemoryMarshal.AsBytes(input.AsSpan(0, count));
            Debug.Assert(count > 0);
            Debug.Assert(buff.Length >= offset + bytes.Length);
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref bytes[0], (uint)bytes.Length);
            return bytes.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static int WriteCountLE<T>(this Span<byte> buff, int offset, ReadOnlySpan<T> input) where T : unmanaged
        {
            var bytes = MemoryMarshal.AsBytes(input);
            Debug.Assert(buff.Length >= offset + bytes.Length);
            Unsafe.CopyBlockUnaligned(ref buff[offset], ref Unsafe.AsRef(in bytes[0]), (uint)bytes.Length);
            return bytes.Length;
        }
    }
}