using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    /// <summary>
    /// 类型相关实用方法
    /// </summary>
    public static class TypeUtil
    {
        /// <summary>
        /// 是否为数字类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>数字为true，否则为false</returns>
        public static bool IsNumericType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            TypeCode tc = GetTypeCode(type);
            return IsNumericTypeCode(tc);
        }

        /// <summary>
        /// 是否为数字类型编号
        /// </summary>
        /// <param name="tc"><see cref="TypeCode"/></param>
        /// <returns>数字为true，否则为false</returns>
        public static bool IsNumericTypeCode(TypeCode tc)
        {

            switch (tc)
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获得TypeCode
        /// </summary>
        /// <param name="type"><see cref="Type"/></param>
        /// <returns><see cref="TypeCode"/></returns>
        public static TypeCode GetTypeCode(Type type)
        {
            if (type == null)
            {
                return TypeCode.Empty;
            }
            TypeCode tc = Type.GetTypeCode(type);
            if (tc == TypeCode.Object)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return Type.GetTypeCode(Nullable.GetUnderlyingType(type));
                }
            }
            return tc;
        }
    }

    /// <summary>
    /// 相等相关实用方法
    /// </summary>
    public static class EqualUtil
    {
        /// <summary>
        /// 对象的所有公共属性相等
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="self">对象</param>
        /// <param name="to">比较的对象</param>
        /// <param name="ignore">忽略属性的名字</param>
        /// <returns>所有公共属性相等为true，否则为false</returns>
        public static bool PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof(T);
                List<string> ignoreList = new List<string>(ignore);
                var ups = from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          where !ignoreList.Contains(pi.Name)
                          let selfValue = type.GetProperty(pi.Name).GetValue(self, null)
                          let toValue = type.GetProperty(pi.Name).GetValue(to, null)
                          where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
                          select selfValue;
                return !ups.Any();
            }
            return self == to;
        }

        /// <summary>
        /// 对象的所有公共属性的HashCode执行Xor计算
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="self">对象</param>
        /// <param name="ignore">忽略属性的名字</param>
        /// <returns>执行Xor计算后的值</returns>
        public static int PublicInstancePropertiesHashCodeXor<T>(T self, params string[] ignore) where T : class
        {
            int hash = 0;
            if (self != null)
            {
                hash = self.GetHashCode();
                Type type = typeof(T);
                List<string> ignoreList = new List<string>(ignore);
                var objs = from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           where !ignoreList.Contains(pi.Name)
                           select type.GetProperty(pi.Name).GetValue(self, null);
                foreach (object obj in objs)
                {
                    if (obj != null)
                    {
                        int h = obj.GetHashCode();
                        hash ^= h;
                    }
                }
            }
            return hash;
        }
    }

    /// <summary>
    /// 列引用标记实用方法
    /// </summary>
    public static class CellReferenceUtil
    {
        /// <summary>
        /// 根据列引用标记获得列索引(0：第一列)
        /// </summary>
        /// <param name="cellReference">列引用标记,例如A2，AB56</param>
        /// <returns>0开始的列索引，-1表示传入的参数不能转换为列标记</returns>
        public static int GetColumnIndex(string cellReference)
        {
            if (string.IsNullOrEmpty(cellReference))
            {
                return -1;
            }
            string cr = Regex.Replace(cellReference.ToUpper(), @"[\d]", string.Empty);
            char[] ss = cr.ToCharArray();
            int columnIndex = -1;
            int m = 1;
            int iA = 'A';   // A为65
            int i = ss.Length;
            for (; i > 0; i--)
            {
                char c = ss[i - 1];
                int d = (c - iA + 1);
                columnIndex += m * (c - 64);
                m = m * 26;
            }
            return columnIndex;
        }

        private static char[] columnNames = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        /// <summary>
        /// 根据列索引获得列引用标记
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <returns>列引用标记,例如A,AB</returns>
        public static string GetColumnReference(uint columnIndex)
        {
            string name = string.Empty;
            int num = (int)columnIndex;
            do
            {
                int i = num % 26;
                name = columnNames[i] + name;
                num = num / 26 - 1;
            } while (num > -1);

            if (string.IsNullOrEmpty(name))
            {
                name = "A";
            }
            return name;
        }

        /// <summary>
        /// 根据行索引，列索引获得单元格引用标记
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <returns>单元格引用标记，例如A1,BA54</returns>
        public static string GetCellReference(uint rowIndex, uint columnIndex)
        {
            string cc = GetColumnReference(columnIndex);
            return string.Format("{0}{1}", cc, rowIndex + 1);
        }
    }
}