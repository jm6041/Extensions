using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// 分页参数
    /// </summary>
    [Serializable]
    public class PageParameter
    {
        /// <summary>
        /// 加载数据的页号  从0开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小，默认 20
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 排序清单
        /// </summary>
        public ICollection<Ordering> Orderings { get; set; }
        /// <summary>
        /// 排序清单是否为空
        /// </summary>
        public bool OrderingsIsNullOrEmpty
        {
            get
            {
                return Orderings == null || (!Orderings.Any());
            }
        }
    }
}
