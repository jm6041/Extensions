using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreEntities
{
    /// <summary>
    /// 订单
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    public class Order<TK> where TK : IEquatable<TK>
    {
        /// <summary>
        /// Id
        /// </summary>
        public TK Id { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
    }
}
