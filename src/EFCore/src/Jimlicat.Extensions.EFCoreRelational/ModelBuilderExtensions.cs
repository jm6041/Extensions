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
