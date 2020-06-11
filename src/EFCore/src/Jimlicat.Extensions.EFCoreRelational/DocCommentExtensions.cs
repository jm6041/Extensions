using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 文档注释扩展
    /// </summary>
    public static class DocCommentExtensions
    {
        /// <summary>
        /// 使用文档注释
        /// </summary>
        /// <param name="optionsBuilder"><see cref="DbContextOptionsBuilder"/></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseDocCommentConvention(this DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
            var extension = (optionsBuilder.Options.FindExtension<DocCommentOptionsExtension>() ?? new DocCommentOptionsExtension())
                .WithDocComment();

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }

        /// <summary>
        /// 使用文档注释
        /// </summary>
        /// <typeparam name="TContext">上下文</typeparam>
        /// <param name="optionsBuilder"><see cref="DbContextOptionsBuilder"/></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder<TContext> UseDocCommentConvention<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder)
            where TContext : DbContext
        {
            return (DbContextOptionsBuilder<TContext>)UseDocCommentConvention((DbContextOptionsBuilder)optionsBuilder);
        }
    }
}
