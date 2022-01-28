using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Namotion.Reflection;

namespace Microsoft.EntityFrameworkCore.Comments
{
    /// <summary>
    /// 属性文档注释
    /// </summary>
    internal class PropertyDocCommentConvention : IPropertyAddedConvention
    {
        public void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
        {
            string? comment = propertyBuilder?.Metadata?.PropertyInfo?.GetXmlDocsSummary();
            if (!string.IsNullOrEmpty(comment))
            {
                propertyBuilder?.HasComment(comment);
            }
        }
    }
}
