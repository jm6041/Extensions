using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// <see cref="Guid"/> 帮助类
    /// </summary>
    public class GuidHelper
    {
        /// <summary>
        /// Guid转换为Base64，去除末尾'=='
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ToBase64(Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray()).TrimEnd('=');
        }

        /// <summary>
        /// Guid转换为Base32
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ToBase32(Guid guid)
        {
            return Base32.ToBase32NP(guid.ToByteArray());
        }

        /// <summary>
        /// Base64的Guid字符串转换为Guid
        /// </summary>
        /// <param name="str">Base64的Guid字符串</param>
        /// <param name="guid">Guid</param>
        /// <returns></returns>
        public static bool TryFromBase64(string str, out Guid guid)
        {
            guid = Guid.Empty;
            try
            {
                string gs = str.PadRight(24, '=');
                byte[] gb = Convert.FromBase64String(gs);
                guid = new Guid(gb);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Base32的Guid字符串转换为Guid
        /// </summary>
        /// <param name="str">Base32的Guid字符串</param>
        /// <param name="guid">Guid</param>
        /// <returns></returns>
        public static bool TryFromBase32(string str, out Guid guid)
        {
            guid = Guid.Empty;
            try
            {
                byte[] gb = Base32.FromBase32(str);
                guid = new Guid(gb);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
