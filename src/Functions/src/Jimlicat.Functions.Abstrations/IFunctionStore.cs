using System;
using System.Collections.Generic;
using System.Text;

namespace Jimlicat.Functions
{
    /// <summary>
    /// 功能仓储
    /// </summary>
    public interface IFunctionStore
    {
        /// <summary>
        /// 根据名字获得功能点信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        FunctionInfo GetFunction(string name);
    }
}
