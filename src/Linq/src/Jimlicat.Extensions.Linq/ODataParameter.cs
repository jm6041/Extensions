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
    public class ODataParameter : IODataParameter
    {
        /// <summary>
        /// $top 返回数据数量，如果返回所有数据，传递 <see cref="int.MaxValue"/>
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
        // 排序字段字典
        private Dictionary<string, Direction> _orderings = new Dictionary<string, Direction>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 添加排序属性
        /// </summary>
        /// <param name="prop">属性或者字段名</param>
        /// <param name="dir"></param>
        public void AddOrder(string prop, Direction dir)
        {
            if (prop == null)
            {
                return;
            }
            string p = prop.Trim();
            if (string.IsNullOrEmpty(p))
            {
                return;
            }
            if (!_orderings.ContainsKey(p))
            {
                _orderings.Add(p, dir);
            }
        }
        /// <summary>
        /// 获得排序清单
        /// </summary>
        [IgnoreDataMember]
        public IReadOnlyDictionary<string, Direction> Orderings
        {
            get
            {
                var dic = ToOrderingDictionary(OrderBy);
                if (dic != null)
                {
                    foreach (var kv in dic)
                    {
                        if (!_orderings.ContainsKey(kv.Key))
                        {
                            _orderings.Add(kv.Key, kv.Value);
                        }
                    }
                }
                return _orderings;
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
            var orderings = Orderings;
            return orderings == null || (!orderings.Any());
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

    /// <summary>
    /// <see cref="IODataParameter"/>扩展
    /// </summary>
    public static class IODataParameterExtensions
    {
        /// <summary>
        /// 获得排序字段集合
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, Direction> GetOrderings(this IODataParameter para)
        {
            if (para is ODataParameter opara)
            {
                return opara.Orderings;
            }
            return ODataParameter.ToOrderingDictionary(para.OrderBy);
        }
    }
}
