using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 数学计算
    /// </summary>
    public static class XMath
    {
        // 数字+字母
        private const string c36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string xrangerr = "must greater than or equal to 2 and less than or equal to 36";

        /// <summary>
        /// 36进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert36(int n)
        {
            return XConvert(n, 36);
        }
        /// <summary>
        /// 36进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert36(uint n)
        {
            return XConvert(n, 36);
        }

        /// <summary>
        /// 36进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert36(long n)
        {
            return XConvert(n, 36);
        }
        /// <summary>
        /// 36进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert36(ulong n)
        {
            return XConvert(n, 36);
        }

        /// <summary>
        /// 16进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert16(int n)
        {
            return XConvert(n, 16);
        }

        /// <summary>
        /// 16进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert16(long n)
        {
            return XConvert(n, 16);
        }

        /// <summary>
        /// 8进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert8(int n)
        {
            return XConvert(n, 8);
        }

        /// <summary>
        /// 8进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert8(long n)
        {
            return XConvert(n, 8);
        }

        /// <summary>
        /// 2进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert2(int n)
        {
            return XConvert(n, 2);
        }

        /// <summary>
        /// 2进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert2(long n)
        {
            return XConvert(n, 2);
        }

        /// <summary>
        /// 进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <param name="x">进制，2到36</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert(int n, int x)
        {
            if (x < 2 || x > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(x), xrangerr);
            }
            if (n == 0)
            {
                return "0";
            }
            List<char> rs = new List<char>(32);
            int tn = n;
            while (tn != 0)
            {
                int y = Math.Abs(tn % x);
                rs.Add(c36[y]);
                tn /= x;
            }
            rs.Reverse();
            string cs = new string(rs.ToArray());
            if (n < 0)
            {
                cs = "-" + cs;
            }
            return cs;
        }

        /// <summary>
        /// 进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <param name="x">进制，2到36</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert(uint n, int x)
        {
            if (x < 2 || x > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(x), xrangerr);
            }
            if (n == 0)
            {
                return "0";
            }
            List<char> rs = new List<char>(32);
            uint tn = n;
            uint tx = (uint)x;
            while (tn != 0)
            {
                int y = Math.Abs((int)(tn % tx));
                rs.Add(c36[y]);
                tn /= tx;
            }
            rs.Reverse();
            string cs = new string(rs.ToArray());
            if (n < 0)
            {
                cs = "-" + cs;
            }
            return cs;
        }

        /// <summary>
        /// 进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <param name="x">进制，2到36</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert(long n, int x)
        {
            if (x < 2 || x > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(x), xrangerr);
            }
            if (n == 0)
            {
                return "0";
            }
            List<char> rs = new List<char>(64);
            long tn = n;
            while (tn != 0)
            {
                int y = Math.Abs((int)(tn % x));
                rs.Add(c36[y]);
                tn /= x;
            }
            rs.Reverse();
            string cs = new string(rs.ToArray());
            if (n < 0)
            {
                cs = "-" + cs;
            }
            return cs;
        }
        /// <summary>
        /// 进制转换
        /// </summary>
        /// <param name="n">数字</param>
        /// <param name="x">进制，2到36</param>
        /// <returns>转换后的字符串</returns>
        public static string XConvert(ulong n, int x)
        {
            if (x < 2 || x > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(x), xrangerr);
            }
            if (n == 0)
            {
                return "0";
            }
            List<char> rs = new List<char>(64);
            ulong tn = n;
            ulong tx = (ulong)x;
            while (tn != 0)
            {
                int y = Math.Abs((int)(tn % tx));
                rs.Add(c36[y]);
                tn /= tx;
            }
            rs.Reverse();
            string cs = new string(rs.ToArray());
            return cs;
        }
    }
}
