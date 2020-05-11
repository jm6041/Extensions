using System;
using System.Collections.Generic;
using System.Text;

namespace Gov.DocumentFormat.OpenXml
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class ColumnItem
    {
        /// <summary>
        /// 属性字段名
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string Show { get; set; }

        private double _width = 16.5;
        /// <summary>
        /// 长度
        /// </summary>
        public double Width
        {
            get
            {
                if (_width < 4)
                {
                    return 4;
                }
                return _width;
            }
            set => _width = value;
        }

        public override bool Equals(object obj)
        {
            if (!ReferenceEquals(this, obj))
            {
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is ColumnItem other))
                {
                    return false;
                }
                return PropertyName.Equals(other.PropertyName);
            }
            return true;
        }

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

    /// <summary>
    /// 列信息集合
    /// </summary>
    public class ColumnCollection
    {
        /// <summary>
        /// 列集合
        /// </summary>
        public List<ColumnItem> Items { get; set; } = new List<ColumnItem>();
    }
}
