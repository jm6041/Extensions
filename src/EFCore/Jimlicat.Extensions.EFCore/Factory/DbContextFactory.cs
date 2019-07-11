using System;
using Microsoft.EntityFrameworkCore.Design;

namespace Microsoft.EntityFrameworkCore
{
    internal class DbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly DbContextOptions<TContext> _options;

        public DbContextFactory(DbContextOptions<TContext> options)
        {
            _options = options;
        }

        public TContext CreateDbContext()
        {
            return (TContext)Activator.CreateInstance(typeof(TContext), _options);
        }

        TContext IDesignTimeDbContextFactory<TContext>.CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }
    }
}
