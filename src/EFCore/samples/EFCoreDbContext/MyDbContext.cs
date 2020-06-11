using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EFCoreEntities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreData
{
    /// <summary>
    /// 上下文
    /// </summary>
    public class MyDbContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// <see cref="OnModelCreating(ModelBuilder)"/>
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 表名使用类型名表示
            //foreach (var entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    Regex regex = new Regex(@"`\d+$");
            //    string tableName = regex.Replace(entity.ClrType.Name, string.Empty);
            //    modelBuilder.Entity(entity.ClrType).ToTable(tableName);
            //}
            modelBuilder.DetaultStringMaxLength(2000).DefaultDeleteBehavior();

            //// 限制一对多级联删除
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            //foreach (var property in modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(t => t.GetProperties())
            //    .Where(p => p.ClrType == typeof(decimal)))
            //{
            //    property.Relational().ColumnType = "numeric(18, 6)";
            //}

            // 字符串默认长度设置为 2000
            //foreach (var property in modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(t => t.GetProperties())
            //    .Where(p => p.ClrType == typeof(string)))
            //{
            //    if (property.GetMaxLength() == null)
            //    {
            //        property.SetMaxLength(2000);
            //    }
            //}

            modelBuilder.ApplyConfigurationsFromAssembly(EFCoreEntities.AssemblyInfo.Assembly);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual DbSet<User> Users { get; set; }
    }
}
