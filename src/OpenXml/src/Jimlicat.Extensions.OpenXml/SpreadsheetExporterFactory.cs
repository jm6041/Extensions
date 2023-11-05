using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

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
            var cs = GetColumnInfos(typeof(T));
            SpreadsheetInfo info = new SpreadsheetInfo(cs);
            ISpreadsheetExporter exporter = new SpreadsheetExporter<T>(sourceDatas, info);
            return exporter;
        }
        private static IEnumerable<ColumnInfo> GetColumnInfos(Type type)
        {
            foreach (var p in type.GetRuntimeProperties().Where(x => x.CanRead))
            {
                ColumnInfo c = new ColumnInfo() { PropertyName = p.Name };
                var dn = p.GetCustomAttribute<DisplayNameAttribute>();
                if (dn != null)
                {
                    c.Show = dn.DisplayName;
                }
                else
                {
                    c.Show = p.Name;
                }
                yield return c;
            }
        }
        /// <summary>
        /// 创建<see cref="ISpreadsheetExporter"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExporter Create<T>(IEnumerable<T> sourceDatas, SpreadsheetInfo info) where T : class
        {
            ISpreadsheetExporter exporter = new SpreadsheetExporter<T>(sourceDatas, info);
            return exporter;
        }
        /// <summary>
        /// 创建<see cref="ISpreadsheetExporter"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExporter Create(ICollection<ExpandoObject> sourceDatas, SpreadsheetInfo info)
        {
            ISpreadsheetExporter exporter = new SpreadsheetExporter<ExpandoObject>(sourceDatas, info);
            return exporter;
        }
        /// <summary>
        /// 创建<see cref="ISpreadsheetExporter"/>
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetExporter Create(object sourceDatas, SpreadsheetInfo info)
        {
            var sdType = sourceDatas.GetType();
            if (sdType.IsGenericType && sdType.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                Type typeArgs0 = sdType.GetGenericArguments()[0];
                Type exportType = typeof(SpreadsheetExporter<>).MakeGenericType(typeArgs0);
                ISpreadsheetExporter? export = (ISpreadsheetExporter?)Activator.CreateInstance(exportType, sourceDatas, info);
                return export!;
            }
            else
            {
                throw new ArgumentException("Is not IEnumerable<> type", nameof(sourceDatas));
            }
        }
    }
}
