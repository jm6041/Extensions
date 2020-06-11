using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Namotion.Reflection;

namespace Microsoft.EntityFrameworkCore.Internal
{
    /// <summary>
    /// 文档注释
    /// </summary>
    class DocComment : IPropertyAddedConvention
    {
        public void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
        {
            propertyBuilder.HasComment(propertyBuilder.Metadata.PropertyInfo.GetXmlDocsSummary());
        }
    }
}
