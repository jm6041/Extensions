using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jimlicat.OfficeEx;
using Excel = Microsoft.Office.Interop.Excel;

namespace Jimlicat.ExcelAddIns.ViewData
{
    /// <summary>
    /// <see cref="Excel.Worksheet"/> 帮助类
    /// </summary>
    public static class WorksheetHelper
    {
        // 行连续为空的数量，超过此数量，不再读取
        private const int NullRowCount = 5;
        // 列连续为空的数量，超过此数量，不再读取
        private const int NullColumnCount = 5;
        /// <summary>
        /// 一批读取的行数，必须被 <see cref="ExcelLimits.RowsMax"/> 整除
        /// </summary>
        private const int perRowCount = 128;
        // 行读取循环次数
        private const int rowCycleCount = ExcelLimits.RowsMax / perRowCount;
        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="ws">电子表格</param>
        /// <param name="rowStartIndex">数据行开始索引，如果小于等于0，值为1</param>
        /// <param name="colEndIndex">结束列名，A，BZ等</param>
        /// <returns></returns>
        internal static SortedDictionary<int, RowContent> GetRowContent(Excel.Worksheet ws, int rowStartIndex, string endColName)
        {
            SortedDictionary<int, RowContent> dic = new SortedDictionary<int, RowContent>();
            if (rowStartIndex <= 0)
            {
                rowStartIndex = 1;
            }
            int nrc = 0;   // 连续为空的行数
            for (int i = 0; i < rowCycleCount; i++)
            {
                if (nrc > NullRowCount)   // 超过最大为空行数，跳出循环
                {
                    break;
                }
                int startRow = i * perRowCount + rowStartIndex;
                int endRow = startRow + perRowCount - 1;
                string start = $"A{startRow}";
                string end = $"{endColName}{endRow}";
                Excel.Range dataRange = ws.Range[start, end];
                // 表格的值组成的二维数组
                //object[,] array = rows.Value2;
                object[,] array = dataRange.Value;
                // 列数
                int cc = array.GetLength(1);
                // 行循环
                for (int ri = 1; ri <= perRowCount; ri++)
                {
                    // 行索引
                    int rowIndex = startRow + ri - 1;
                    int ncc = 0;   // 连续为空的列数
                    // 保存列值
                    List<CellValue> cvlist = new List<CellValue>();
                    // 列循环
                    for (int ci = 1; ci <= cc; ci++)
                    {
                        if (ncc > NullColumnCount)
                        {
                            break;
                        }
                        object v = array[ri, ci];
                        if (v != null)
                        {
                            CellValue cv = new CellValue(ci, v);
                            cvlist.Add(cv);
                            ncc = 0;
                        }
                        else
                        {
                            ncc++;
                        }
                    }
                    if (cvlist.Any())
                    {
                        dic.Add(rowIndex, new RowContent(rowIndex, cvlist));
                        nrc = 0;
                    }
                    else
                    {
                        nrc++;
                    }
                }
            }
            return dic;
        }
    }
}
