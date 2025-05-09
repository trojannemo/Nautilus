﻿using System;
using System.Buffers;

namespace Nautilus.Cysharp.Collections
{
    internal sealed unsafe class PointerMemoryManager : MemoryManager<byte>
    {
        byte* pointer;
        int length;
        bool usingMemory;

        internal PointerMemoryManager(byte* pointer, int length)
        {
            this.pointer = pointer;
            this.length = length;
            usingMemory = false;
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override Span<byte> GetSpan()
        {
            usingMemory = true;
            return new Span<byte>(pointer, length);
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            if ((uint)elementIndex >= (uint)length) ThrowHelper.ThrowIndexOutOfRangeException();
            return new MemoryHandle(pointer + elementIndex, default, this);
        }

        public override void Unpin()
        {
        }

        public void AllowReuse()
        {
            usingMemory = false;
        }

        public void Reset(byte* pointer, int length)
        {
            if (usingMemory) throw new InvalidOperationException("Memory is using, can not reset.");
            this.pointer = pointer;
            this.length = length;
        }
    }
}