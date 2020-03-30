using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 生成32位全局唯一字符串Id
    /// </summary>
    public static class Id32
    {
        private static readonly SortedDictionary<int, char> NumberCharDic = new SortedDictionary<int, char>()
        {
            { 0, '0' },
            { 1, '1' },
            { 2, '2' },
            { 3, '3' },
            { 4, '4' },
            { 5, '5' },
            { 6, '6' },
            { 7, '7' },
            { 8, '8' },
            { 9, '9' },
            { 10, 'A' },
            { 11, 'B' },
            { 12, 'C' },
            { 13, 'D' },
            { 14, 'E' },
            { 15, 'F' },
            { 16, 'G' },
            { 17, 'H' },
            { 18, 'I' },
            { 19, 'J' },
            { 20, 'K' },
            { 21, 'L' },
            { 22, 'M' },
            { 23, 'N' },
            { 24, 'O' },
            { 25, 'P' },
            { 26, 'Q' },
            { 27, 'R' },
            { 28, 'S' },
            { 29, 'T' },
            { 30, 'U' },
            { 31, 'V' },
            { 32, 'W' },
            { 33, 'X' },
            { 34, 'Y' },
            { 35, 'Z' },
            { 36, '_' },
            { 37, 'a' },
            { 38, 'b' },
            { 39, 'c' },
            { 40, 'd' },
            { 41, 'e' },
            { 42, 'f' },
            { 43, 'g' },
            { 44, 'h' },
            { 45, 'i' },
            { 46, 'j' },
            { 47, 'k' },
            { 48, 'l' },
            { 49, 'm' },
            { 50, 'n' },
            { 51, 'o' },
            { 52, 'p' },
            { 53, 'q' },
            { 54, 'r' },
            { 55, 's' },
            { 56, 't' },
            { 57, 'u' },
            { 58, 'v' },
            { 59, 'w' },
            { 60, 'x' },
            { 61, 'y' },
            { 62, 'z' },
        };
        /// <summary>
        /// 最大年号，3个字节的最大整数是16777215
        /// </summary>
        private const int MaxYear = 9999;
        /// <summary>
        /// 生成全局唯一32位字符串Id
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            DateTime now = DateTime.Now;
            if (now.Year > MaxYear)
            {
                throw new InvalidOperationException($"Current year can't be greater than {MaxYear}");
            }
            // 4个字符
            string year = now.Year.ToString().PadLeft(4, '0');
            char month = NumberCharDic[now.Month];
            char day = NumberCharDic[now.Day];
            // Guid部分，26个字符
            string guid = OrderBase32.ToBase32(Guid.NewGuid().ToByteArray());
            StringBuilder sb = new StringBuilder(32);
            sb.Append(year).Append(month).Append(day).Append(guid);
            return sb.ToString();
        }
    }
}
