using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jimlicat.Functions
{
    /// <summary>
    /// 功能仓储
    /// </summary>
    public class FunctionStore: IFunctionStore
    {
        /// <summary>
        /// 根据名字获得功能点信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FunctionInfo GetFunction(string name)
        {
            return new FunctionInfo(name);
        }
    }
}
