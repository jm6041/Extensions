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
