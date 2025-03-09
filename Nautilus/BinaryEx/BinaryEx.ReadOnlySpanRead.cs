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
        public static UInt32 ReadUInt24LE(this ReadOnlySpan<byte> buff, int offset)
        {
            var remaining = buff.Length - offset;
            Debug.Assert(remaining >= 3);

            if (remaining >= 4)
            {
                UInt32 val = Unsafe.ReadUnaligned<UInt32>(ref Unsafe.AsRef(in buff[offset]));
                return val & 0x00FFFFFF;
            }
            else
            {
                return buff[offset] | (UInt32)buff[offset + 1] << 8 | (UInt32)buff[offset + 2] << 16;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt32 ReadUInt24BE(this ReadOnlySpan<byte> buff, int offset)
        {
            var remaining = buff.Length - offset;
            Debug.Assert(remaining >= 3);

            if (remaining >= 4)
            {
                UInt32 val = Unsafe.ReadUnaligned<UInt32>(ref Unsafe.AsRef(in buff[offset]));
                return Endian.SwapEndianess(val << 8);
            }
            else
            {
                return (UInt32)buff[offset] << 16 | (UInt32)buff[offset + 1] << 8 | buff[offset + 2];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int32 ReadInt24LE(this ReadOnlySpan<byte> buff, int offset)
        {
            Int32 val = (Int32)ReadUInt24LE(buff, offset);
            return val - (val >> 23 << 24);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int32 ReadInt24BE(this ReadOnlySpan<byte> buff, int offset)
        {
            Int32 val = (Int32)ReadUInt24BE(buff, offset);
            return val - (val >> 23 << 24);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int64 ReadInt64LE(this ReadOnlySpan<byte> buff, int offset)
        {
            return (Int64)ReadUInt64LE(buff, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int64 ReadInt64BE(this ReadOnlySpan<byte> buff, int offset)
        {
            return (Int64)ReadUInt64BE(buff, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int32 ReadInt32LE(this ReadOnlySpan<byte> buff, int offset)
        {
            return (Int32)ReadUInt32LE(buff, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int32 ReadInt32BE(this ReadOnlySpan<byte> buff, int offset)
        {
            return (Int32)ReadUInt32BE(buff, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int16 ReadInt16LE(this ReadOnlySpan<byte> buff, int offset)
        {
            return (Int16)ReadUInt16LE(buff, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static Int16 ReadInt16BE(this ReadOnlySpan<byte> buff, int offset)
        {
            return (Int16)ReadUInt16BE(buff, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static sbyte ReadSByte(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= Unsafe.SizeOf<byte>());
            return (sbyte)buff[offset];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt64 ReadUInt64LE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt64>());
            return Unsafe.As<byte, UInt64>(ref Unsafe.AsRef(in buff[offset]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt64 ReadUInt64BE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt64>());
            return Endian.SwapEndianess(Unsafe.As<byte, UInt64>(ref Unsafe.AsRef(in buff[offset])));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt32 ReadUInt32LE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt32>());
            return Unsafe.As<byte, UInt32>(ref Unsafe.AsRef(in buff[offset]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt32 ReadUInt32BE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt32>());
            return Endian.SwapEndianess(Unsafe.As<byte, UInt32>(ref Unsafe.AsRef(in buff[offset])));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt16 ReadUInt16LE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt16>());
            return Unsafe.As<byte, UInt16>(ref Unsafe.AsRef(in buff[offset]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static UInt16 ReadUInt16BE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<UInt16>());
            return Endian.SwapEndianess(Unsafe.As<byte, UInt16>(ref Unsafe.AsRef(in buff[offset])));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static float ReadFloatLE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<float>());
            return Unsafe.ReadUnaligned<float>(ref Unsafe.AsRef(in buff[offset]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static float ReadFloatBE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<float>());
            var data = Endian.SwapEndianess(Unsafe.ReadUnaligned<UInt32>(ref Unsafe.AsRef(in buff[offset])));
            return Unsafe.As<UInt32, float>(ref data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static double ReadDoubleLE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<double>());
            return Unsafe.ReadUnaligned<double>(ref Unsafe.AsRef(in buff[offset]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static double ReadDoubleBE(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<double>());
            var data = Endian.SwapEndianess(Unsafe.ReadUnaligned<UInt64>(ref Unsafe.AsRef(in buff[offset])));
            return Unsafe.As<UInt64, double>(ref data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static byte ReadByte(this ReadOnlySpan<byte> buff, int offset)
        {
            Debug.Assert(buff.Length >= offset + Unsafe.SizeOf<byte>());
            return buff[offset];
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static int ReadBytes(this ReadOnlySpan<byte> buff, int offset, byte[] output, int count)
        {
            Debug.Assert(count > 0);
            Debug.Assert(buff.Length >= offset + count);
            Unsafe.CopyBlockUnaligned(ref output[0], ref Unsafe.AsRef(in buff[offset]), (uint)count);
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static int ReadBytes(this ReadOnlySpan<byte> buff, int offset, Span<byte> output)
        {
            Debug.Assert(buff.Length >= offset + output.Length);
            Unsafe.CopyBlockUnaligned(ref output[0], ref Unsafe.AsRef(in buff[offset]), (uint)output.Length);
            return output.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static int ReadCountLE<T>(this ReadOnlySpan<byte> buff, int offset, T[] output, int count) where T : unmanaged
        {
            var bytes = MemoryMarshal.AsBytes(output.AsSpan(0, count));
            Debug.Assert(count > 0);
            Debug.Assert(buff.Length >= offset + bytes.Length);
            Unsafe.CopyBlockUnaligned(ref bytes[0], ref Unsafe.AsRef(in buff[offset]), (uint)bytes.Length);
            return bytes.Length;
        }

        // only implemented for LE because we don't know what the layout of the struct/objet is
        // so there is no way to safely swap the endianness
        [MethodImpl(MethodImplOptions.AggressiveInlining), TargetedPatchingOptOut("Inline across assemplies")]
        public static int ReadCountLE<T>(this ReadOnlySpan<byte> buff, int offset, Span<T> output) where T : unmanaged
        {
            var bytes = MemoryMarshal.AsBytes(output);
            Debug.Assert(buff.Length >= offset + bytes.Length);
            Unsafe.CopyBlockUnaligned(ref bytes[0], ref Unsafe.AsRef(in buff[offset]), (uint)bytes.Length);
            return bytes.Length;
        }
    }
}