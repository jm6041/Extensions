using Microsoft.EntityFrameworkCore.Design;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="DbContext"/> Factory 接口定义
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IDbContextFactory<out TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// 创建 <see cref="DbContext"/>，优先使用依赖注入的 <see cref="DbContext"/> 对象，此方法仅仅用于新的线程中，创建新的 <see cref="DbContext"/> 对象
        /// </summary>
        /// <returns></returns>
        TContext CreateDbContext();
    }
}