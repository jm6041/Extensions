using System.IO;
using DocumentFormat.OpenXml;

namespace Jimlicat.OpenXml
{
    /// <summary>
    /// 电子表格导出接口
    /// </summary>
    public interface ISpreadsheetExporter
    {
        /// <summary>
        /// Sheet名
        /// </summary>
        string SheetName { get; set; }
        /// <summary>
        /// <see cref="bool"/> true 文本
        /// </summary>
        string BoolTrueText { get; set; }
        /// <summary>
        /// <see cref="bool"/> false 文本
        /// </summary>
        string BoolFalseText { get; set; }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        void Export(string path, SpreadsheetDocumentType type = SpreadsheetDocumentType.Workbook);
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        void Export(Stream stream, SpreadsheetDocumentType type = SpreadsheetDocumentType.Workbook);
    }
}
