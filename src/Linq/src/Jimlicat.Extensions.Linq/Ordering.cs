using System.Runtime.Serialization;

namespace System.Linq
{
    /// <summary>
    /// 排序字段名和方向
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
            Dir = direction;
        }

        /// <summary>
        /// 克隆构造函数
        /// </summary>
        /// <param name="ordering"></param>
        public Ordering(Ordering ordering)
        {
            Name = ordering.Name;
            Dir = ordering.Dir;
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
        public Direction Dir { get; set; }

        /// <summary>
        /// 重写 ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + " " + Dir;
        }
    }
}
