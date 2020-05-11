using System.IO;

namespace Gov.DocumentFormat.OpenXml
{
    /// <summary>
    /// 电子表格导出接口
    /// </summary>
    public interface ISpreadsheetExport
    {
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        MemoryStream Export();
    }
}