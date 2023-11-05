using System.Reflection;

namespace Jimlicat.OpenXml
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// 属性字段名
        /// </summary>
        public string PropertyName { get; set; } = "";
        /// <summary>
        /// 显示名
        /// </summary>
        public string Show { get; set; } = "";
        /// <summary>
        /// 格式字符串
        /// </summary>
        public string FormatString { get; set; } = "";

        private double? _width = null;
        /// <summary>
        /// 长度，如果小于0，值会设置为0
        /// </summary>
        public double? Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (value is not null && value < 0)
                {
                    _width = 0;
                }
                else
                {
                    _width = value;
                }
            }
        }
        /// <summary>
        /// <see cref="bool"/> true 文本
        /// </summary>
        public string BoolTrueText { get; set; } = "";
        /// <summary>
        /// <see cref="bool"/> false 文本
        /// </summary>
        public string BoolFalseText { get; set; } = "";
        /// <summary>
        /// 属性
        /// </summary>
        internal PropertyInfo? PropertyInfo { get; set; }
        /// <summary>
        /// 重写 Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (!ReferenceEquals(this, obj))
            {
                if (obj == null)
                {
                    return false;
                }
                if (obj is not ColumnInfo other)
                {
                    return false;
                }
                return PropertyName.Equals(other.PropertyName);
            }
            return true;
        }

        /// <summary>
        /// 重写 GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int h = 0;
            if (PropertyName != null)
            {
                h = PropertyName.GetHashCode();
            }
            return h ^ 17;
        }
    }
}
