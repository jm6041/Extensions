using System;

namespace Jimlicat.OpenXml
{
    /// <summary>
    /// 使用方法
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// 是否为数字类型，包括可为空类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNumericType(object obj)
        {
            return obj is int || obj is int?
                || obj is double || obj is double?
                || obj is long || obj is long?
                || obj is ulong || obj is ulong?
                || obj is uint || obj is uint?
                || obj is byte || obj is byte?
                || obj is decimal || obj is decimal?
                || obj is float || obj is float?
                || obj is short || obj is short?
                || obj is ushort || obj is ushort?                
                || obj is sbyte || obj is sbyte?;
        }
        /// <summary>
        /// 是否为数字类型，不包括可为空的类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsPureNumericType(object obj)
        {
            return obj is int
                || obj is double
                || obj is long
                || obj is ulong
                || obj is uint
                || obj is byte
                || obj is decimal
                || obj is float
                || obj is short
                || obj is ushort                
                || obj is sbyte;
        }
        /// <summary>
        /// 是否为可为空数字类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullableNumericType(object obj)
        {
            return obj is int?
                || obj is double?
                || obj is long?
                || obj is ulong?
                || obj is uint?
                || obj is byte?
                || obj is decimal?
                || obj is float?
                || obj is short?
                || obj is ushort?
                || obj is sbyte?;
        }
    }
}
