using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="DbContext"/> Factory 依赖注入扩展
    /// </summary>
    public static class DbContextFactoryServiceCollectionExtensions
    {
        /// <summary>
        /// 添加<see cref="DbContext"/> Factory
        /// </summary>
        /// <typeparam name="TContext"><see cref="DbContext"/></typeparam>
        /// <param name="serviceCollection"><see cref="IServiceCollection"/></param>
        /// <param name="optionsAction"><see cref="DbContextOptionsBuilder"/></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContextFactory<TContext>(
            this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
        {
            DbContextOptions<TContext> options = DbContextOptionsFactory<TContext>(optionsAction);
            DbContextFactory<TContext> factory = new DbContextFactory<TContext>(options);
            serviceCollection.AddSingleton(factory);
            serviceCollection.AddSingleton<EntityFrameworkCore.IDbContextFactory<TContext>>((sp) => sp.GetRequiredService<DbContextFactory<TContext>>());
            serviceCollection.AddSingleton<IDesignTimeDbContextFactory<TContext>>((sp) => sp.GetRequiredService<DbContextFactory<TContext>>());
            return serviceCollection;
        }

        private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(
            Action<DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TContext>(
                new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));
            optionsAction?.Invoke(builder);
            return builder.Options;
        }
    }
}
