using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Jimlicat.Functions
{
    /// <summary>
    /// 功能点方法信息
    /// </summary>
    public class FunctionMethod
    {
        /// <summary>
        /// 唯一名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 功能点对应的方法信息
        /// </summary>
        public MethodInfo FunctionMethodInfo { get; set; }
    }
}
