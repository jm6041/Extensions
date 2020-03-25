using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 实体元数据配置扩展
    /// </summary>
    public static class EntityMetadataDbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// 使用实体元配置
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="metadataOptionsBuilderAction"></param>
        /// <returns></returns>
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
