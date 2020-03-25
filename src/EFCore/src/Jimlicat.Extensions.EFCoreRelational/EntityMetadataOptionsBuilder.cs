using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="EntityMetadataExtension"/>配置Builder
    /// </summary>
    public class EntityMetadataOptionsBuilder : IRelationalDbContextOptionsBuilderInfrastructure
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="optionsBuilder"></param>
        public EntityMetadataOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
            OptionsBuilder = optionsBuilder;
        }

        /// <summary>
        /// <see cref="DbContextOptionsBuilder"/>
        /// </summary>
        public virtual DbContextOptionsBuilder OptionsBuilder { get; }
        /// <summary>
        /// 配置默认表名类型
        /// </summary>
        /// <param name="nt"></param>
        /// <returns></returns>
        public EntityMetadataOptionsBuilder DefaultTableNameType(NameType nt) => WithOption(e => e.WithDefaultTableNameType(nt));
        /// <summary>
        /// 配置默认列名类型
        /// </summary>
        /// <param name="nt"></param>
        /// <returns></returns>
        public EntityMetadataOptionsBuilder DetaultColumnNameType(NameType nt) => WithOption(e => e.WithDetaultColumnNameType(nt));
        /// <summary>
        /// 配置默认字符串长度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public EntityMetadataOptionsBuilder DetaultStringLength(int? length) => WithOption(e => e.WithDetaultStringLength(length));
        /// <summary>
        /// 添加或者更新扩展信息
        /// </summary>
        /// <param name="setAction"></param>
        /// <returns></returns>
        protected virtual EntityMetadataOptionsBuilder WithOption(Func<EntityMetadataExtension, EntityMetadataExtension> setAction)
        {
            ((IDbContextOptionsBuilderInfrastructure)OptionsBuilder).AddOrUpdateExtension(
                setAction(OptionsBuilder.Options.FindExtension<EntityMetadataExtension>() ?? new EntityMetadataExtension()));
            return this;
        }
    }
}
