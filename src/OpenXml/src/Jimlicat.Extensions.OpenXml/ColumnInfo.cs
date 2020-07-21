using System;
using System.Collections.Generic;
using System.Text;

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
        public string PropertyName { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string Show { get; set; }
        /// <summary>
        /// 格式字符串
        /// </summary>
        public string FormatString { get; set; }

        private double? _width = null;
        /// <summary>
        /// 长度
        /// </summary>
        public double? Width
        {
            get
            {
                if (_width != null && _width.Value < 1)
                {
                    return 1;
                }
                return _width;
            }
            set => _width = value;
        }
        /// <summary>
        /// 是否自动确定长度
        /// </summary>
        public bool AutoWidth { get; set; } = true;
        /// <summary>
        /// 重写 Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!ReferenceEquals(this, obj))
            {
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is ColumnInfo other))
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
