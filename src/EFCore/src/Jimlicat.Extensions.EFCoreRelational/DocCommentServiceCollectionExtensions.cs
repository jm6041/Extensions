using Microsoft.EntityFrameworkCore.Comments;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 扩展<see cref="IServiceCollection"/>
    /// </summary>
    public static class DocCommentServiceCollectionExtensions
    {
        /// <summary>
        /// 添加文档注释插件
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityFrameworkDocComment(
            this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            new EntityFrameworkServicesBuilder(services)
                .TryAdd<IConventionSetPlugin, DocCommentSetPlugin>();

            return services;
        }
    }
}
