using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// OData 参数
    /// </summary>
    [Serializable]
    [DataContract]
    public class ODataParameter
    {
        /// <summary>
        /// $top
        /// </summary>
        [DataMember(Order = 10000)]
        public int Top { get; set; }
        /// <summary>
        /// $skip
        /// </summary>
        [DataMember(Order = 10001)]
        public int Skip { get; set; }
        /// <summary>
        /// $orderby
        /// </summary>
        [DataMember(Order = 10002)]
        public string OrderBy { get; set; }

        /// <summary>
        /// 排序清单
        /// </summary>
        [IgnoreDataMember]
        public Dictionary<string, Direction> Orderings
        {
            get
            {
                return ToOrderingDictionary(OrderBy);
            }
        }
        /// <summary>
        /// orderby 字符串转换为排序字典
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns>排序字典</returns>
        public static Dictionary<string, Direction> ToOrderingDictionary(string orderby)
        {
            if (orderby == null)
            {
                return null;
            }
            Dictionary<string, Direction> dic = new Dictionary<string, Direction>();
            if (!string.IsNullOrEmpty(orderby))
            {
                foreach (string order in orderby.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] arr = order.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 1)
                    {
                        dic.Add(arr[0], Direction.Asc);
                    }
                    else if (arr.Length == 2)
                    {
                        string k = arr[0];
                        string v = arr[1];
                        Direction dir;
                        if (v.Equals("asc", StringComparison.OrdinalIgnoreCase))
                        {
                            dir = Direction.Asc;
                        }
                        else if (v.Equals("desc", StringComparison.OrdinalIgnoreCase))
                        {
                            dir = Direction.Desc;
                        }
                        else
                        {
                            continue;
                        }
                        dic.Add(k, dir);
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// 排序清单是否为空
        /// </summary>
        public bool OrderingsIsNullOrEmpty()
        {
            return Orderings == null || (!Orderings.Any());
        }

        /// <summary>
        /// $filter 在数据具体查询中实现
        /// </summary>
        [DataMember(Order = 10003)]
        public string Filter { get; set; }
        /// <summary>
        /// $count 返回结果是否包含总数量
        /// </summary>
        [DataMember(Order = 1004)]
        public bool Count { get; set; } = true;
    }
}
