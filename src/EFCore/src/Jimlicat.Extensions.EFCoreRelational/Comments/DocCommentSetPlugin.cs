using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Comments
{
    /// <summary>
    /// 文档注释插件
    /// </summary>
    internal class DocCommentSetPlugin : IConventionSetPlugin
    {
        private readonly IDbContextOptions _options;
        public DocCommentSetPlugin(IDbContextOptions options)
        {
            _options = options;
        }

        public ConventionSet ModifyConventions(ConventionSet conventionSet)
        {
            var extension = _options.FindExtension<DocCommentOptionsExtension>();            
            if (extension.UseDocComment)
            {
                DocCommentConvention docComment = new DocCommentConvention();
                conventionSet.PropertyAddedConventions.Add(docComment);
            }
            return conventionSet;
        }
    }
}
