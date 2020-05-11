using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gov.DocumentFormat.OpenXml
{
    /// <summary>
    /// <see cref="ISpreadsheetExport"/>工厂
    /// </summary>
    public class SpreadsheetExportFactory
    {
        /// <summary>
        /// 创建<see cref="ISpreadsheetExport"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExport Create<T>(IEnumerable<T> sourceDatas, ColumnCollection columns, string sheetName = "Data") where T : class
        {
            ISpreadsheetExport export = new SpreadsheetExport<T>(sourceDatas, columns, sheetName);
            return export;
        }

        /// <summary>
        /// 创建<see cref="ISpreadsheetExport"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExport Create(object sourceDatas, ColumnCollection columns, string sheetName = "Data")
        {
            var sdType = sourceDatas.GetType();
            if (sdType.IsGenericType && sdType.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {                
                Type typeArgs0 = sdType.GetGenericArguments()[0];
                Type exportType = typeof(SpreadsheetExport<>).MakeGenericType(typeArgs0);
                ISpreadsheetExport export = (ISpreadsheetExport)Activator.CreateInstance(exportType, sourceDatas, columns, sheetName);
                return export;
            }
            else
            {
                throw new ArgumentException("Is not IEnumerable<> type", nameof(sourceDatas));
            }
        }
    }
}
