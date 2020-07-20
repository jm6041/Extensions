using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenXml
{

    /// <summary>
    /// OpenXml与对象数据
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public interface IOpenXmlObject<T> where T : class
    {
        /// <summary>
        /// 导出为Stream
        /// </summary>
        /// <returns></returns>
        MemoryStream Export();

        /// <summary>
        /// 导出到文件
        /// </summary>
        /// <param name="exportFile">导出文件</param>
        void Export(string exportFile);
    }
}
