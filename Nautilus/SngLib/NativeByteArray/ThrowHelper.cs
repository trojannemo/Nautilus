using System;

namespace Nautilus.Cysharp.Collections
{
    internal static class ThrowHelper
    {
        public static void ThrowIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException();
        }

        public static void ThrowArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }

    }
}
