using System;
using System.Collections.Generic;
using System.Linq;

namespace Jimlicat.Utils
{
    /// <summary>
    /// 参数检查
    /// </summary>
    public static class Checker
    {
        private static readonly string PageIndexMustNonNegativeInteger = nameof(PageParameter.PageIndex) + " " + SR.MustNonNegativeInteger;
        private static readonly string PageSizeMustNonNegativeInteger = nameof(PageParameter.PageSize) + " " + SR.MustNonNegativeInteger;

        private static readonly string TopMustNonNegativeInteger = nameof(ODataParameter.Top) + " " + SR.MustNonNegativeInteger;
        private static readonly string SkipMustNonNegativeInteger = nameof(ODataParameter.Skip) + " " + SR.MustNonNegativeInteger;

        /// <summary>
        /// 检查 <see cref="PageParameter"/>
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="paraName"></param>
        public static void CheckPageParameter(PageParameter parameter, string paraName)
        {
            NotNull(parameter, paraName);
            if (parameter.PageIndex < 0)
            {
                throw new ArgumentException(PageIndexMustNonNegativeInteger);
            }
            if (parameter.PageSize < 0)
            {
                throw new ArgumentException(PageSizeMustNonNegativeInteger);
            }
        }

        /// <summary>
        /// 检查 <see cref="ODataParameter"/>
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="paraName"></param>
        public static void CheckODataParameter(ODataParameter parameter, string paraName)
        {
            NotNull(parameter, paraName);
            if (parameter.Top < 0)
            {
                throw new ArgumentException(TopMustNonNegativeInteger);
            }
            if (parameter.Skip < 0)
            {
                throw new ArgumentException(SkipMustNonNegativeInteger);
            }
        }

        private static string GetMustNonNegativeInteger(string paramName)
        {
            return paramName + " " + SR.MustNonNegativeInteger;
        }

        /// <summary>
        /// 整型必须非负
        /// </summary>
        /// <param name="num"></param>
        /// <param name="paraName"></param>
        public static void CheckMustNonNegativeInteger(int num, string paraName)
        {
            if (num < 0)
            {
                throw new ArgumentException(GetMustNonNegativeInteger(paraName));
            }
        }

        /// <summary>
        /// Not Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public static bool NotNull<T>(T data, string paraName)
        {
            if (data == null)
            {
                throw new ArgumentNullException(paraName);
            }
            return true;
        }

        /// <summary>
        /// Not Empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public static bool NotEmpty<T>(IEnumerable<T> data, string paraName)
        {
            if (data == null)
            {
                throw new ArgumentNullException(paraName);
            }
            if (!data.Any())
            {
                throw new ArgumentException(paraName + " " + SR.IsEmpty);
            }
            return true;
        }
    }
}
