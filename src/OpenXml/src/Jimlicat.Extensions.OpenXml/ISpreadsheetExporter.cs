using System.IO;

namespace Jimlicat.OpenXml
{
    /// <summary>
    /// 电子表格导出器接口
    /// </summary>
    public interface ISpreadsheetExporter
    {
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        MemoryStream Export();
    }
}