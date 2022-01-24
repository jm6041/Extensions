using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jimlicat.OfficeEx
{
    /// <summary>
    /// 单元格值
    /// </summary>
    public class CellValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="coloumnIndex"></param>
        /// <param name="value"></param>
        public CellValue(int coloumnIndex, object value)
        {
            ColoumnIndex = coloumnIndex;
            Value = value;
        }
        /// <summary>
        /// 列索引
        /// </summary>
        public int ColoumnIndex { get; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; }
        /// <summary>
        /// 重写 ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format($"{ColoumnIndex}, {Value}");
        }
    }

    /// <summary>
    /// 行内容
    /// </summary>
    public class RowContent
    {
        private readonly SortedDictionary<int, CellValue> cellIndexDic;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="cvs">单元格值</param>
        public RowContent(int rowIndex, IEnumerable<CellValue> cvs)
        {
            RowIndex = rowIndex;
            cellIndexDic = new SortedDictionary<int, CellValue>();
            if (cvs != null)
            {
                foreach (var cv in cvs)
                {
                    AddInner(cv);
                }
            }
        }

        private void AddInner(CellValue cv)
        {
            if (!cellIndexDic.ContainsKey(cv.ColoumnIndex))
            {
                cellIndexDic.Add(cv.ColoumnIndex, cv);
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="coloumnIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(int coloumnIndex, out CellValue value)
        {
            bool s = cellIndexDic.TryGetValue(coloumnIndex, out CellValue v);
            value = v;
            return s;
        }
        /// <summary>
        /// 单元格值数量
        /// </summary>
        public int Count => cellIndexDic.Count;
        /// <summary>
        /// 最大列索引，最大值不超过2000列
        /// </summary>
        public int MaxColoumnIndex
        {
            get
            {
                if (cellIndexDic.Count == 0)
                {
                    return 1;
                }
                var max = cellIndexDic.Keys.Max();
                if (max >= 2000)
                {
                    return 2000;
                }
                return max;
            }
        }
        /// <summary>
        /// 最小列索引
        /// </summary>
        public int MinColoumnIndex
        {
            get
            {
                if (cellIndexDic.Count == 0)
                {
                    return 1;
                }
                return cellIndexDic.Keys.Min();
            }
        }
        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get; }
        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="cv"></param>
        public void Add(CellValue cv)
        {
            AddInner(cv);
        }
    }
}
