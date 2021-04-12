using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Omega.Package.Internal
{
    internal static class HexSupport
    {
        private static readonly char[] Symbols =
        {
            '0', '1', '2', '3',
            '4', '5', '6', '7',
            '8', '9', 'A', 'B',
            'C', 'D', 'E', 'F'
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ValueToHex(int value)
        {
            if (value < 0 || value >= Symbols.Length)
                ValueToHexFail_ValueOutOfRange(); // terminate

            return Symbols[value];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char HighByteToChar(byte b)
        {
            var value = (b & 0xF0) >> 4;
            return ValueToHex(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char LowByteToChar(byte b)
        {
            var value = b & 0xF;
            return ValueToHex(value);
        }

        [TerminatesProgram]
        private static void ValueToHexFail_ValueOutOfRange()
            => throw new ArgumentOutOfRangeException();
    }
}