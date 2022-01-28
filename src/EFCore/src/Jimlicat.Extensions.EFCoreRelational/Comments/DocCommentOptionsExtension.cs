using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 文档注释扩展
    /// </summary>
    internal class DocCommentOptionsExtension : IDbContextOptionsExtension
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DocCommentOptionsExtension()
        {
        }
        /// <summary>
        /// 克隆构造函数
        /// </summary>
        /// <param name="copyFrom"></param>
        protected DocCommentOptionsExtension(DocCommentOptionsExtension copyFrom)
        {
            if (copyFrom == null)
            {
                throw new ArgumentNullException(nameof(copyFrom));
            }
            UseDocComment = copyFrom.UseDocComment;
        }

        /// <summary>
        /// 添加依赖
        /// </summary>
        /// <param name="services"></param>
        public void ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkDocComment();
        }

        /// <summary>
        /// 使用文档注释
        /// </summary>
        /// <returns></returns>
        public DocCommentOptionsExtension WithDocComment()
        {
            var clone = Clone();
            clone.UseDocComment = true;
            return clone;
        }
        /// <summary>
        /// 是否使用文档注释
        /// </summary>
        public bool UseDocComment { get; private set; } = false;
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        protected DocCommentOptionsExtension Clone()
        {
            return new DocCommentOptionsExtension(this);
        }
        /// <summary>
        /// 校验 <see cref="IDbContextOptions"/>
        /// </summary>
        /// <param name="options"></param>
        public virtual void Validate(IDbContextOptions options)
        {
        }

        private DbContextOptionsExtensionInfo? _info;
        /// <summary>
        /// <see cref="DbContextOptionsExtensionInfo"/>
        /// </summary>
        public DbContextOptionsExtensionInfo Info
        {
            get
            {
                if (_info == null)
                {
                    _info = new DocCommentExtensionInfo(this);
                }
                return _info;
            }
        }
    }

    /// <summary>
    /// 元扩展信息
    /// </summary>
    internal class DocCommentExtensionInfo : DbContextOptionsExtensionInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="extension"></param>
        public DocCommentExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
        {
        }

        /// <summary>
        /// 不是 DatabaseProvider
        /// </summary>
        public override bool IsDatabaseProvider => false;
        public override string LogFragment => "using DocComment ";
#if NETSTANDARD2_0
        public override long GetServiceProviderHashCode()
#else
        public override int GetServiceProviderHashCode()
#endif
        {
            var ext = (DocCommentOptionsExtension)base.Extension;
            return ext.UseDocComment.GetHashCode() ^ 7;
        }
        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }
#if NET6_0_OR_GREATER
        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return true;
        }
#endif
    }
}
