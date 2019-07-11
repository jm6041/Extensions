using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="ModelBuilder"/>扩展
    /// <code>
    /// protected override void OnModelCreating(ModelBuilder modelBuilder)
    /// {
    ///     modelBuilder.AddEntityConfigurationsFromAssemblys(***Assembly);
    /// }
    /// </code>
    /// </summary>
    public static class ModelBuilderExtenions
    {
        private static Type EntityTypeConfigurationInterface = typeof(IEntityTypeConfiguration<>);

        /// <summary>
        /// 从程序集中添加实体映射配置
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/></param>
        /// <param name="assemblys">包含实体映射配置的<see cref="Assembly"/></param>
        public static void AddEntityConfigurationsFromAssemblys(this ModelBuilder modelBuilder, params Assembly[] assemblys)
        {
            AddEntityConfigurationsFromAssemblys(modelBuilder, (IEnumerable<Assembly>)assemblys);
        }

        /// <summary>
        /// 从程序集中添加实体映射配置
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/></param>
        /// <param name="assemblys">包含实体映射配置的<see cref="Assembly"/></param>
        public static void AddEntityConfigurationsFromAssemblys(this ModelBuilder modelBuilder, IEnumerable<Assembly> assemblys)
        {
            List<TypeInfo> ts = new List<TypeInfo>(1024);
            foreach (Assembly assembly in assemblys)
            {
                var mts = assembly.DefinedTypes.Where(x => !x.IsAbstract && !x.IsInterface && x.ImplementedInterfaces.Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == EntityTypeConfigurationInterface));
                ts.AddRange(mts);
            }
            foreach (var config in ts)
            {
                dynamic obj = Activator.CreateInstance(config);
                modelBuilder.ApplyConfiguration(obj);
            }
        }
    }
}
