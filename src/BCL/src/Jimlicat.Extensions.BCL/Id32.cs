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
        /// <summary>
        /// 最大年号，36进制"ZZZ"对应整数是46655
        /// </summary>
        private const int MaxYear = 46655;
        /// <summary>
        /// 生成全局唯一32位字符串Id，格式中年月日小时采用36进制编码，guid采用Base32，排列顺序：年号3，月号1，日号1，小时1，GUID26
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            DateTime now = DateTime.Now;
            int y = now.Year;
            if (y > MaxYear)
            {
                throw new InvalidOperationException($"Current year can't be greater than {MaxYear}");
            }            
            // 3个字符
            string year = XMath.XConvert36(y).PadLeft(3, '0');
            // 1个字符
            string month = XMath.XConvert36(now.Month);
            // 1个字符
            string day = XMath.XConvert36(now.Day);
            // 1个字符
            string hour = XMath.XConvert36(now.Hour);
            // Guid部分，26个字符
            string guid = Base32.ToBase32NP(Guid.NewGuid().ToByteArray());

            List<char> idcs = new List<char>(32);
            idcs.AddRange(year);
            idcs.AddRange(month);
            idcs.AddRange(day);
            idcs.AddRange(hour);
            idcs.AddRange(guid);

            return new string(idcs.ToArray());
        }
    }
}
