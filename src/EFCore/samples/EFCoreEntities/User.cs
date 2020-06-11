using System;
using System.ComponentModel;

namespace EFCoreEntities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }
        /// <summary>
        /// 整形
        /// </summary>
        public int IntV { get; set; }
        /// <summary>
        /// 浮点数
        /// </summary>
        public double DouV { get; set; }        
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 男性
        /// </summary>
        Male = 0,
        /// <summary>
        /// 女性
        /// </summary>
        FeMale = 1,
    }
}
