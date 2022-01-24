using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Jimlicat.OfficeEx
{
    /// <summary>
    /// 表格引用标记助手
    /// </summary>
    public class CellReferenceHelper
    {
        /// <summary>
        /// 根据列引用标记获得列索引(1：第一列)
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
            int columnIndex = 0;
            int m = 1;
            int i = ss.Length;
            for (; i > 0; i--)
            {
                char c = ss[i - 1];
                columnIndex += m * (c - 64);
                m *= 26;
            }
            return columnIndex;
        }

        private static readonly char[] columnNames = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        /// <summary>
        /// 根据列索引获得列引用标记
        /// </summary>
        /// <param name="columnIndex">列索引，从0开始</param>
        /// <returns>列引用标记,例如A,AB</returns>
        public static string GetColumnReference(int columnIndex)
        {
            string name = string.Empty;
            int num = columnIndex;
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
        public static string GetCellReference(int rowIndex, int columnIndex)
        {
            string cc = GetColumnReference(columnIndex);
            return string.Format("{0}{1}", cc, rowIndex + 1);
        }
    }
}
