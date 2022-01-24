namespace Jimlicat.OfficeEx
{
    /// <summary>
    /// Excel 限制，适用于 office 2007 以上版本
    /// </summary>
    public class ExcelLimits
    {
        /// <summary>
        /// 最大行数
        /// </summary>
        public const int RowsMax = 1048576;
        /// <summary>
        /// 最大列数
        /// </summary>
        public const int ColumnsMax = 16384;
        /// <summary>
        /// 最大列名
        /// </summary>
        public const string ColumnsMaxName = "XFD";
    }
}
