using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jimlicat.Functions
{
    /// <summary>
    /// 程序集访问器
    /// </summary>
    public interface IAssembliesAccessor
    {
        Assembly[] GetAssemblies();
    }
}
