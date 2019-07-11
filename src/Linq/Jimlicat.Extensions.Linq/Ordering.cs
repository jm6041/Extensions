namespace System.Linq
{
    /// <summary>
    /// 分页排序
    /// </summary>
    [Serializable]
    public class Ordering
    {
        /// <summary>
        /// 排序属性、字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序方向
        /// </summary>
        public OrderingDirection Direction { get; set; }
    }
}
