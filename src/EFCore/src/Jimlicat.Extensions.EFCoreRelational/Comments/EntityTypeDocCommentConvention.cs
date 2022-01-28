using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Namotion.Reflection;

namespace Microsoft.EntityFrameworkCore.Comments
{
    /// <summary>
    /// 实体类文档注释
    /// </summary>
    internal class EntityTypeDocCommentConvention : IEntityTypeAddedConvention
    {
        public void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
        {
            string? comment = entityTypeBuilder?.Metadata?.ClrType?.GetXmlDocsSummary();
            if (!string.IsNullOrEmpty(comment))
            {
                entityTypeBuilder?.HasComment(comment);
            }
        }
    }
}
