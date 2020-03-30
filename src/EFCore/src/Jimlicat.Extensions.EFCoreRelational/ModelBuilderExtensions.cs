using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="ModelBuilder"/> 扩展
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// 设置默认的实体对应数据表名
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="nt">名字类型</param>
        /// <param name="schema">数据库 schema</param>
        /// <returns></returns>
        public static ModelBuilder DefalutTableName(this ModelBuilder modelBuilder, NameType nt = NameType.Normal, string schema = null)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                Regex regex = new Regex(@"`\d+$");
                // 实体类名
                string typeName = regex.Replace(entity.ClrType.Name, string.Empty);
                // 数据表名
                string tableName = NameTypeHelper.GetName(typeName, nt);
                if (string.IsNullOrWhiteSpace(schema))
                {
                    modelBuilder.Entity(entity.ClrType).ToTable(tableName);
                }
                else
                {
                    modelBuilder.Entity(entity.ClrType).ToTable(tableName, schema);
                }
            }
            return modelBuilder;
        }

        /// <summary>
        /// 设置默认的实体属性对应数据表列名
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="nt">名字类型</param>
        /// <returns></returns>
        public static ModelBuilder DefalutColumnName(this ModelBuilder modelBuilder, NameType nt = NameType.Normal)
        {
            if (nt != NameType.Normal)
            {
                foreach (var entity in modelBuilder.Model.GetEntityTypes())
                {
                    foreach (var property in entity.GetProperties())
                    {
                        string cn = NameTypeHelper.GetName(property.Name, nt);
                        property.SetColumnName(cn);
                    }
                }
            }
            return modelBuilder;
        }

        /// <summary>
        /// 设置默认的字符串属性对应数据表列的最大长度
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>
        public static ModelBuilder DetaultStringMaxLength(this ModelBuilder modelBuilder, int? maxLength)
        {
            if (maxLength != null)
            {
                foreach (var property in modelBuilder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(string)))
                {
                    if (property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(maxLength);
                    }
                }
            }
            return modelBuilder;
        }

        /// <summary>
        /// 属性的<see cref="DescriptionAttribute"/>作为默认注释
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder DefaultComment(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    var attr = property.PropertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                    if(attr != null)
                    {
                        DescriptionAttribute descAttr = attr as DescriptionAttribute;
                        if (!string.IsNullOrWhiteSpace(descAttr.Description))
                        {
                            property.SetComment(descAttr.Description);
                        }
                    }
                }
            }
            return modelBuilder;
        }
    }
}
