using System;
using System.Text;

namespace System
{
    /// <summary>
    /// 顺序Base32，字符串"234567ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    /// </summary>
    internal static class OrderBase32
    {
        private static readonly string _base32Chars = "234567ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 转换为Base32字符串，保持顺序，没有"="结尾
        /// </summary>
        /// <param name="input">输入的byte数组</param>
        /// <returns>Base32字符串，没有"="</returns>
        public static string ToBase32(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            StringBuilder sb = new StringBuilder();
            for (int offset = 0; offset < input.Length;)
            {
                byte a, b, c, d, e, f, g, h;
                int numCharsToOutput = GetNextGroup(input, ref offset, out a, out b, out c, out d, out e, out f, out g, out h);

                if (numCharsToOutput >= 1)
                {
                    sb.Append(_base32Chars[a]);
                }
                if (numCharsToOutput >= 2)
                {
                    sb.Append(_base32Chars[b]);
                }
                if (numCharsToOutput >= 3)
                {
                    sb.Append(_base32Chars[c]);
                }
                if (numCharsToOutput >= 4)
                {
                    sb.Append(_base32Chars[d]);
                }
                if (numCharsToOutput >= 5)
                {
                    sb.Append(_base32Chars[e]);
                }
                if (numCharsToOutput >= 6)
                {
                    sb.Append(_base32Chars[f]);
                }
                if (numCharsToOutput >= 7)
                {
                    sb.Append(_base32Chars[g]);
                }
                if (numCharsToOutput >= 8)
                {
                    sb.Append(_base32Chars[h]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Base32字符串转换为字节数组
        /// </summary>
        /// <param name="input">Base32字符串，可以传入非"="结尾的字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] FromBase32(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            input = input.TrimEnd('=').ToUpperInvariant();
            if (input.Length == 0)
            {
                return new byte[0];
            }

            var output = new byte[input.Length * 5 / 8];
            var bitIndex = 0;
            var inputIndex = 0;
            var outputBits = 0;
            var outputIndex = 0;
            while (outputIndex < output.Length)
            {
                var byteIndex = _base32Chars.IndexOf(input[inputIndex]);
                if (byteIndex < 0)
                {
                    throw new FormatException();
                }

                var bits = Math.Min(5 - bitIndex, 8 - outputBits);
                output[outputIndex] <<= bits;
                output[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

                bitIndex += bits;
                if (bitIndex >= 5)
                {
                    inputIndex++;
                    bitIndex = 0;
                }

                outputBits += bits;
                if (outputBits >= 8)
                {
                    outputIndex++;
                    outputBits = 0;
                }
            }
            return output;
        }

        // returns the number of bytes that were output
        private static int GetNextGroup(byte[] input, ref int offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
        {
            uint b1, b2, b3, b4, b5;

            int retVal;
            switch (input.Length - offset)
            {
                case 1: retVal = 2; break;
                case 2: retVal = 4; break;
                case 3: retVal = 5; break;
                case 4: retVal = 7; break;
                default: retVal = 8; break;
            }

            b1 = (offset < input.Length) ? input[offset++] : 0U;
            b2 = (offset < input.Length) ? input[offset++] : 0U;
            b3 = (offset < input.Length) ? input[offset++] : 0U;
            b4 = (offset < input.Length) ? input[offset++] : 0U;
            b5 = (offset < input.Length) ? input[offset++] : 0U;

            a = (byte)(b1 >> 3);
            b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6));
            c = (byte)((b2 >> 1) & 0x1f);
            d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4));
            e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7));
            f = (byte)((b4 >> 2) & 0x1f);
            g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
            h = (byte)(b5 & 0x1f);

            return retVal;
        }
    }
}
