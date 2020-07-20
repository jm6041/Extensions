using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenXml.Attributes
{
    /// <summary>
    /// 不映射为OpenXml相关数据(不会在OpenXml中导出)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotOpenXmlAttribute : Attribute
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public NotOpenXmlAttribute() { }
    }

    /// <summary>
    /// 在OpenXml中显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DisplayOpenXmlAttribute : Attribute
    {
        private string _name;
        private string _formate;
        private string _boolTrueText;
        private string _boolFalseText;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DisplayOpenXmlAttribute() : this("") { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">显示名</param>
        public DisplayOpenXmlAttribute(string name) : this(name, "") { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">显示名</param>
        /// <param name="formate">格式字符串</param>
        public DisplayOpenXmlAttribute(string name, string formate)
        {
            this.Name = name;
            this.Formate = formate;
        }

        /// <summary>
        /// 显示名
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 格式字符串
        /// </summary>
        public string Formate
        {
            get
            {
                return _formate;
            }

            set
            {
                _formate = value;
            }
        }

        /// <summary>
        /// bool型为true时的显示文本
        /// </summary>
        public string BoolTrueText
        {
            get
            {
                return _boolTrueText;
            }

            set
            {
                _boolTrueText = value;
            }
        }

        /// <summary>
        /// bool型为false时的显示文本
        /// </summary>
        public string BoolFalseText
        {
            get
            {
                return _boolFalseText;
            }

            set
            {
                _boolFalseText = value;
            }
        }
    }

    /// <summary>
    /// OpenXml列设置，对应<see cref="DocumentFormat.OpenXml.Spreadsheet.Column"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnOpenXmlAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ColumnOpenXmlAttribute() { }

        /// <summary>
        /// 自定义宽度
        /// </summary>
        public double CustomWidth { get; set; }

        /// <summary>
        /// 是否隐藏
        /// <see cref="DocumentFormat.OpenXml.Spreadsheet.Column.Hidden"/>
        /// </summary>
        public bool Hidden { get; set; }

    }
}
