using System;

namespace Gov.DocumentFormat.OpenXml
{
    /// <summary>
    /// 列引用标记实用方法
    /// </summary>
    public static class CellReferenceUtil
    {
        private static readonly char[] columnNames = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

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
