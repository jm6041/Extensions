using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jimlicat.Functions
{
    /// <summary>
    /// 功能点数据源
    /// </summary>
    internal sealed class FunctionSource
    {
        public static Dictionary<string, FunctionInfo> NameMethodsDic = new Dictionary<string, FunctionInfo>();

        public static void Initalize(Assembly[] assemblies)
        {
            var scanner = FunctionScanner.FindFunctionsInAssemblies(assemblies);
            foreach (var gitem in scanner.GroupBy(x => x.Name))
            {
                FunctionInfo fun = new FunctionInfo()
                {
                    Name = gitem.Key,
                };
                fun.Description = string.Join(",", gitem.Select(x => x.Description));
                NameMethodsDic.Add(gitem.Key, fun);
            }
        }

        public static FunctionInfo GetFunction(string name)
        {
            return new FunctionInfo();
        }
    }
}
