using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 名字类型
    /// </summary>
    public enum NameType
    {
        /// <summary>
        /// 正常，表名与实体类名一样，字段名与实体类属性名一样，适用于 mssql
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 小写，用下划线'_'隔开，适用于 mysql, postgresql
        /// </summary>
        Lower = 1,
        /// <summary>
        /// 大写，用下划线'_'隔开，适用于 oracle
        /// </summary>
        Upper = 2,
    }
    /// <summary>
    /// <see cref="NameType"/> 帮助类
    /// </summary>
    public static class NameTypeHelper
    {
        /// <summary>
        /// 获得正确的名字
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nt"></param>
        /// <returns></returns>
        public static string GetName(string name, NameType nt)
        {
            switch (nt)
            {
                case NameType.Normal:
                    return name;
                case NameType.Lower:
                    return GetLower(name);
                case NameType.Upper:
                    return GetUpper(name);
                default:
                    return name;
            }
        }
        /// <summary>
        /// 获得小写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetLower(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            List<char> nl = new List<char>(name.Length + 10)
            {
                char.ToLower(name[0])
            };
            for (int i = 1; i < name.Length; i++)
            {
                char ci = name[i];
                if (char.IsUpper(ci))
                {
                    if (char.IsUpper(name[i - 1]))
                    {
                        nl.Add(char.ToLower(ci));
                    }
                    else
                    {
                        nl.Add('_');
                        nl.Add(char.ToLower(ci));
                    }
                }
                else
                {
                    nl.Add(ci);
                }
            }
            return new string(nl.ToArray());
        }
        /// <summary>
        /// 获得大写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetUpper(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            List<char> nl = new List<char>(name.Length + 10)
            {
                char.ToUpper(name[0])
            };
            for (int i = 1; i < name.Length; i++)
            {
                char ci = name[i];
                if (char.IsLower(ci))
                {
                    if (char.IsLower(name[i - 1]))
                    {
                        nl.Add(char.ToUpper(ci));
                    }
                    else
                    {
                        nl.Add('_');
                        nl.Add(char.ToUpper(ci));
                    }
                }
                else
                {
                    nl.Add(ci);
                }
            }
            return new string(nl.ToArray());
        }
    }

    public class EntityMetadataExtension : IDbContextOptionsExtension
    {
        private NameType _defaultTableNameType { get; set; }
        private NameType _detaultColumnNameType { get; set; }
        private int?  _detaultStringLength { get; set; } = 2000;

        public void ApplyServices(IServiceCollection services)
        {
            services.TryAddSingleton<EntityMetadataExtension>();
        }

        public EntityMetadataExtension WithDefaultTableNameType(NameType nt)
        {
            var clone = Clone();
            clone._defaultTableNameType = nt;
            return clone;
        }

        public EntityMetadataExtension WithDetaultColumnNameType(NameType nt)
        {
            var clone = Clone();
            clone._detaultColumnNameType = nt;
            return clone;
        }

        public EntityMetadataExtension WithDetaultStringLength(int? length)
        {
            if (length.HasValue                 && length <= 0)
            {
                throw new InvalidOperationException("长度必须大于等于0");
            }
            var clone = Clone();
            clone._detaultStringLength = length;
            return clone;
        }

        /// <summary>
        /// 默认表名类型
        /// </summary>
        public NameType DefaultTableNameType => _defaultTableNameType;
        /// <summary>
        /// 默认列名类型
        /// </summary>
        public NameType DetaultColumnNameType => _detaultColumnNameType;
        /// <summary>
        /// 默认字符串长度，默认值 2000
        /// </summary>
        public int? DetaultStringLength => _detaultStringLength;

        protected EntityMetadataExtension Clone()
        {
            return new EntityMetadataExtension(this);
        }

        public virtual void Validate(IDbContextOptions options)
        {
        }

        public EntityMetadataExtension()
        {
        }

        protected EntityMetadataExtension(EntityMetadataExtension copyFrom)
        {
            if (copyFrom == null)
            {
                throw new ArgumentNullException(nameof(copyFrom));
            }
            _defaultTableNameType = copyFrom._defaultTableNameType;
            _detaultColumnNameType = copyFrom._detaultColumnNameType;
            _detaultStringLength = copyFrom._detaultStringLength;
        }

        private DbContextOptionsExtensionInfo _info;

        public DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);
    }

    public class ExtensionInfo : DbContextOptionsExtensionInfo
    {

        public ExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
        {
        }

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

    public class EntityMetadataOptionsBuilder : IRelationalDbContextOptionsBuilderInfrastructure
    {
        public EntityMetadataOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
            OptionsBuilder = optionsBuilder;
        }


        public virtual DbContextOptionsBuilder OptionsBuilder { get; }

        public EntityMetadataOptionsBuilder DefaultTableNameType(NameType nt) => WithOption(e => e.WithDefaultTableNameType(nt));

        public EntityMetadataOptionsBuilder DetaultColumnNameType(NameType nt) => WithOption(e => e.WithDetaultColumnNameType(nt));

        public EntityMetadataOptionsBuilder DetaultStringLength(int? length) => WithOption(e => e.WithDetaultStringLength(length));

        protected virtual EntityMetadataOptionsBuilder WithOption(Func<EntityMetadataExtension, EntityMetadataExtension> setAction)
        {
            ((IDbContextOptionsBuilderInfrastructure)OptionsBuilder).AddOrUpdateExtension(
                setAction(OptionsBuilder.Options.FindExtension<EntityMetadataExtension>() ?? new EntityMetadataExtension()));
            return this;
        }
    }

    public static class EntityMetadataDbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseEntityMetadata(this DbContextOptionsBuilder optionsBuilder, Action<EntityMetadataOptionsBuilder> metadataOptionsBuilderAction)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(GetOrCreateExtension(optionsBuilder));
            ConfigureWarnings(optionsBuilder);
            metadataOptionsBuilderAction?.Invoke(new EntityMetadataOptionsBuilder(optionsBuilder));
            return optionsBuilder;
        }

        private static EntityMetadataExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.Options.FindExtension<EntityMetadataExtension>()
                  ?? new EntityMetadataExtension();
        }

        private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
        {
            var coreOptionsExtension
                = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()
                ?? new CoreOptionsExtension();

            coreOptionsExtension = coreOptionsExtension.WithWarningsConfiguration(
                coreOptionsExtension.WarningsConfiguration.TryWithExplicit(
                    RelationalEventId.AmbientTransactionWarning, WarningBehavior.Throw));

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
        }
    }
}
