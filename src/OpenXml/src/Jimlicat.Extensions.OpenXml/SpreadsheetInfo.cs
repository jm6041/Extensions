using System.Collections.Generic;
using System.Globalization;

namespace Jimlicat.OpenXml
{
    /// <summary>
    /// 电子表格信息
    /// </summary>
    public class SpreadsheetInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SpreadsheetInfo() : this(new List<ColumnInfo>())
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public SpreadsheetInfo(IEnumerable<ColumnInfo> columns)
        {
            var s = string.Empty;
            SheetName = s;
            BoolValueTranslate = true;
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "zh")
            {
                BoolTrueText = "是";
                BoolFalseText = "否";
            }
            else if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en")
            {
                BoolTrueText = "Yes";
                BoolFalseText = "No";
            }
            else
            {
                BoolTrueText = s;
                BoolFalseText = s;
            }
            Columns = new List<ColumnInfo>(columns);
        }
        /// <summary>
        /// Sheet名
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// <see cref="bool"/> 值是否翻译，默认为 true
        /// </summary>
        public bool BoolValueTranslate { get; set; }
        /// <summary>
        /// <see cref="bool"/> true 文本
        /// </summary>
        public string BoolTrueText { get; set; }
        /// <summary>
        /// <see cref="bool"/> false 文本
        /// </summary>
        public string BoolFalseText { get; set; }
        /// <summary>
        /// <see cref="ColumnInfo"/> 集合
        /// </summary>
        public IList<ColumnInfo> Columns { get; set; }
    }
}
