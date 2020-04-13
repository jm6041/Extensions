using System;
using System.ComponentModel;

namespace Jimlicat.Functions
{
    /// <summary>
    /// 功能点特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FunctionAttribute : Attribute
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
        /// 默认构造函数
        /// </summary>
        public FunctionAttribute()
            : this(string.Empty)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">功能名</param>
        public FunctionAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">功能名</param>
        /// <param name="desc">描述</param>
        public FunctionAttribute(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

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
            FunctionAttribute attr = obj as FunctionAttribute;
            if (attr != null)
            {
                return attr.Name == Name;
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
