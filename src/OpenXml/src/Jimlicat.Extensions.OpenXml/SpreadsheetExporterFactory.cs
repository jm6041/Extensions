using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jimlicat.OpenXml
{
    /// <summary>
    /// <see cref="ISpreadsheetExporter"/>工厂
    /// </summary>
    public class SpreadsheetExporterFactory
    {
        /// <summary>
        /// 创建<see cref="ISpreadsheetExporter"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExporter Create<T>(IEnumerable<T> sourceDatas, ColumnCollection columns) where T : class
        {
            ISpreadsheetExporter export = new SpreadsheetExporter<T>(sourceDatas, columns);
            return export;
        }

        /// <summary>
        /// 创建<see cref="ISpreadsheetExporter"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExporter Create(object sourceDatas, ColumnCollection columns)
        {
            var sdType = sourceDatas.GetType();
            if (sdType.IsGenericType && sdType.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                Type typeArgs0 = sdType.GetGenericArguments()[0];
                Type exportType = typeof(SpreadsheetExporter<>).MakeGenericType(typeArgs0);
                ISpreadsheetExporter export = (ISpreadsheetExporter)Activator.CreateInstance(exportType, sourceDatas, columns);
                return export;
            }
            else
            {
                throw new ArgumentException("Is not IEnumerable<> type", nameof(sourceDatas));
            }
        }
    }
}
