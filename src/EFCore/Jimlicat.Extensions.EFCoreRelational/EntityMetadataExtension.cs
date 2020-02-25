using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 实体元数据配置扩展
    /// </summary>
    public class EntityMetadataExtension : IDbContextOptionsExtension
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EntityMetadataExtension()
        {
        }
        /// <summary>
        /// 克隆构造函数
        /// </summary>
        /// <param name="copyFrom"></param>
        protected EntityMetadataExtension(EntityMetadataExtension copyFrom)
        {
            if (copyFrom == null)
            {
                throw new ArgumentNullException(nameof(copyFrom));
            }
            DefaultTableNameType = copyFrom.DefaultTableNameType;
            DetaultColumnNameType = copyFrom.DetaultColumnNameType;
            DetaultStringLength = copyFrom.DetaultStringLength;
        }

        /// <summary>
        /// 添加依赖
        /// </summary>
        /// <param name="services"></param>
        public void ApplyServices(IServiceCollection services)
        {
            services.TryAddSingleton<EntityMetadataExtension>();
        }

        /// <summary>
        /// 配置默认表名类型
        /// </summary>
        /// <param name="nt"></param>
        /// <returns></returns>
        public EntityMetadataExtension WithDefaultTableNameType(NameType nt)
        {
            var clone = Clone();
            clone.DefaultTableNameType = nt;
            return clone;
        }
        /// <summary>
        /// 配置默认列名类型
        /// </summary>
        /// <param name="nt"></param>
        /// <returns></returns>
        public EntityMetadataExtension WithDetaultColumnNameType(NameType nt)
        {
            var clone = Clone();
            clone.DetaultColumnNameType = nt;
            return clone;
        }
        /// <summary>
        /// 配置默认字符串长度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public EntityMetadataExtension WithDetaultStringLength(int? length)
        {
            if (length.HasValue && length <= 0)
            {
                throw new InvalidOperationException("长度必须大于等于0");
            }
            var clone = Clone();
            clone.DetaultStringLength = length;
            return clone;
        }

        /// <summary>
        /// 默认表名类型
        /// </summary>
        public NameType DefaultTableNameType { get; private set; }
        /// <summary>
        /// 默认列名类型
        /// </summary>
        public NameType DetaultColumnNameType { get; private set; }
        /// <summary>
        /// 默认字符串长度，默认值 2000
        /// </summary>
        public int? DetaultStringLength { get; private set; } = 2000;
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        protected EntityMetadataExtension Clone()
        {
            return new EntityMetadataExtension(this);
        }
        /// <summary>
        /// 校验 <see cref="IDbContextOptions"/>
        /// </summary>
        /// <param name="options"></param>
        public virtual void Validate(IDbContextOptions options)
        {
        }

        private DbContextOptionsExtensionInfo _info;
        /// <summary>
        /// <see cref="DbContextOptionsExtensionInfo"/>
        /// </summary>
        public DbContextOptionsExtensionInfo Info
        {
            get
            {
                if (_info == null)
                {
                    _info = new MetadataExtensionInfo(this);
                }
                return _info;
            }
        }
    }

    /// <summary>
    /// 元扩展信息
    /// </summary>
    internal class MetadataExtensionInfo : DbContextOptionsExtensionInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="extension"></param>
        public MetadataExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
        {
        }
        /// <summary>
        /// 不是 DatabaseProvider
        /// </summary>
        public override bool IsDatabaseProvider => false;
        public override string LogFragment => "using EntityMetadata ";
        public override long GetServiceProviderHashCode()
        {
            return 0;
        }
        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }
    }
}
