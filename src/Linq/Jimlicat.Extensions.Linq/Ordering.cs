using System.Runtime.Serialization;

namespace System.Linq
{
    /// <summary>
    /// 分页排序
    /// </summary>
    [Serializable]
    [DataContract]
    public class Ordering
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Ordering() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">排序属性、字段名</param>
        /// <param name="direction">排序方向</param>
        public Ordering(string name, Direction direction)
        {
            Name = name;
            Direction = direction;
        }

        /// <summary>
        /// 克隆构造函数
        /// </summary>
        /// <param name="ordering"></param>
        public Ordering(Ordering ordering)
        {
            Name = ordering.Name;
            Direction = ordering.Direction;
        }

        /// <summary>
        /// 排序属性、字段名
        /// </summary>
        [DataMember(Order = 10)]
        public string Name { get; set; }
        /// <summary>
        /// 排序方向
        /// </summary>
        [DataMember(Order = 20)]
        public Direction Direction { get; set; }
    }
}
