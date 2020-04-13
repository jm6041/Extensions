using System;
using System.Collections.Generic;
using System.Text;

namespace Jimlicat.Functions
{
    /// <summary>
    /// 功能点信息
    /// </summary>
    public class FunctionInfo
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FunctionInfo()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">功能点名字</param>
        public FunctionInfo(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 唯一名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 授权是否启用，默认为启用
        /// </summary>
        public bool AuthorizationEnable { get; set; }
        /// <summary>
        /// 日志是否启用，默认为启用
        /// </summary>
        public bool LoggingEnable { get; set; } = true;
        /// <summary>
        /// 是否启用，默认为 true
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 重写 Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            FunctionInfo fun = obj as FunctionInfo;
            if (fun != null)
            {
                return fun.Name == Name;
            }
            return false;
        }

        /// <summary>
        /// 重写 GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name == null ? 0 : Name.GetHashCode();
        }
    }
}
