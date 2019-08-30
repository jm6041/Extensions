using System;

namespace System
{
    /// <summary>
    /// <see cref="Guid"/>扩展
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Guid转换为url友好的Base64
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ToBase64Url(this Guid guid)
        {
            return GuidHelper.ToBase64Url(guid);
        }

        /// <summary>
        /// Guid转换为Base32
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string ToBase32(this Guid guid)
        {
            return GuidHelper.ToBase32(guid);
        }
    }
}
