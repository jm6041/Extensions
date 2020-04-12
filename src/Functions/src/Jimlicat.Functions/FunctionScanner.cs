using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;

namespace Jimlicat.Functions
{
    /// <summary>
    /// <see cref="FunctionAttribute"/>扫描
    /// </summary>
    public class FunctionScanner : IEnumerable<FunctionMethod>
    {
        private readonly IEnumerable<FunctionMethod> _source;

        public FunctionScanner(IEnumerable<FunctionMethod> funMethods)
        {
            _source = funMethods;
        }

        /// <summary>
        /// 查找程序集中的功能点
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static FunctionScanner FindFunctionsInAssembly(Assembly assembly)
        {
            var data = FindMethodsInAssembly(assembly);
            return new FunctionScanner(data);
        }
        /// <summary>
        /// 查找程序集中的功能点
        /// </summary>
        public static FunctionScanner FindFunctionsInAssemblies(IEnumerable<Assembly> assemblies)
        {
            List<FunctionMethod> list = new List<FunctionMethod>();
            foreach (var assembly in assemblies)
            {
                var data = FindMethodsInAssembly(assembly);
                list.AddRange(data);
            }
            return new FunctionScanner(list);
        }

        /// <summary>
        /// 查找程序集中的功能点方法
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static ICollection<FunctionMethod> FindMethodsInAssembly(Assembly assembly)
        {
            List<FunctionMethod> list = new List<FunctionMethod>(64);
            foreach (var method in assembly.ExportedTypes.Where(x => x.IsInterface).Distinct().SelectMany(x => x.GetRuntimeMethods()))
            {
                var funAttr = method.GetCustomAttribute<FunctionAttribute>(false);
                if (funAttr != null)
                {
                    FunctionMethod fm = new FunctionMethod()
                    {
                        Name = funAttr.Name,
                        Description = funAttr.Description,
                        FunctionMethodInfo = method,
                    };
                    list.Add(fm);
                }
            }
            return list;
        }
        public IEnumerator<FunctionMethod> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
