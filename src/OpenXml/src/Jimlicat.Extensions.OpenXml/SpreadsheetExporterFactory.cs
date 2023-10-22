using System;
using System.Collections.Generic;
using System.Dynamic;
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
        public static ISpreadsheetExporter Create<T>(IEnumerable<T> sourceDatas) where T : class
        {
            ISpreadsheetExporter exporter = new SpreadsheetExporter<T>(sourceDatas);
            return exporter;
        }

        /// <summary>
        /// 创建<see cref="ISpreadsheetExporter"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExporter Create<T>(IEnumerable<T> sourceDatas, IEnumerable<ColumnInfo> columns) where T : class
        {
            ISpreadsheetExporter exporter = new SpreadsheetExporter<T>(sourceDatas, columns);
            return exporter;
        }

        /// <summary>
        /// 创建<see cref="ISpreadsheetExporter"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExporter Create(ICollection<ExpandoObject> sourceDatas, IEnumerable<ColumnInfo> columns)
        {
            ISpreadsheetExporter exporter = new SpreadsheetExporter<ExpandoObject>(sourceDatas, columns);
            return exporter;
        }

        /// <summary>
        /// 创建<see cref="ISpreadsheetExporter"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExporter Create(object sourceDatas, IEnumerable<ColumnInfo> columns)
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
