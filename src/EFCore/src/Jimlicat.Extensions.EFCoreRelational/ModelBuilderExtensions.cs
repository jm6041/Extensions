using Microsoft.EntityFrameworkCore.Metadata;
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
        /// 设置实体对应数据表名
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="nameFunc">名字委托</param>
        /// <param name="schema">数据库 schema</param>
        /// <returns></returns>
        public static ModelBuilder SetTableName(this ModelBuilder modelBuilder, Func<IMutableEntityType, string> nameFunc, string schema = null)
        {
            if (nameFunc == null)
            {
                return modelBuilder;
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // 数据表名
                string name = nameFunc.Invoke(entity);
                if (!string.IsNullOrEmpty(name))
                {
                    if (string.IsNullOrWhiteSpace(schema))
                    {
                        modelBuilder.Entity(entity.ClrType).ToTable(name);
                    }
                    else
                    {
                        modelBuilder.Entity(entity.ClrType).ToTable(name, schema);
                    }
                }
            }
            return modelBuilder;
        }

        /// <summary>
        /// 设置实体属性对应数据表列名
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="nameFunc">名字委托</param>
        /// <returns></returns>
        public static ModelBuilder SetColumnName(this ModelBuilder modelBuilder, Func<IMutableProperty, string> nameFunc)
        {
            if (nameFunc != null)
            {
                foreach (var entity in modelBuilder.Model.GetEntityTypes())
                {
                    foreach (var property in entity.GetProperties())
                    {
                        string name = nameFunc.Invoke(property);
                        property.SetColumnName(name);
                    }
                }
            }
            return modelBuilder;
        }

        /// <summary>
        /// 设置实体索引名
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="nameFunc">名字委托</param>
        /// <returns></returns>
        public static ModelBuilder SetIndexName(this ModelBuilder modelBuilder, Func<IMutableIndex, string> nameFunc)
        {
            if (nameFunc != null)
            {
                foreach (var entity in modelBuilder.Model.GetEntityTypes())
                {
                    foreach (var index in entity.GetIndexes())
                    {
                        string name = nameFunc.Invoke(index);
                        index.SetDatabaseName(name);
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
        /// 设置默认的一对多级联删除行为
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="deleteBehavior">删除行为，默认为限制级联删除<seealso cref="DeleteBehavior.Restrict"/></param>
        /// <returns></returns>
        public static ModelBuilder DefaultDeleteBehavior(this ModelBuilder modelBuilder, DeleteBehavior deleteBehavior = DeleteBehavior.Restrict)
        {
            // 限制一对多级联删除
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = deleteBehavior;
            }
            return modelBuilder;
        }
    }
}
