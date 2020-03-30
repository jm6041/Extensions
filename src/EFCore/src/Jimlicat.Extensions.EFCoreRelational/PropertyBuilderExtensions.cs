using System;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders
{
    /// <summary>
    /// <see cref="PropertyBuilder{TProperty}"/> 扩展
    /// </summary>
    public static class PropertyBuilderExtensions
    {
        /// <summary>
        /// 32固定长度
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyBuilder"></param>
        /// <returns></returns>
        public static PropertyBuilder<TProperty> HasFixedLength32<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
        {
            return propertyBuilder.HasFixedLength(32);
        }

        /// <summary>
        /// 固定长度
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyBuilder">扩展的属性构造器长度</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static PropertyBuilder<TProperty> HasFixedLength<TProperty>(this PropertyBuilder<TProperty> propertyBuilder, int length)
        {
            return propertyBuilder.HasMaxLength(length).IsFixedLength();
        }
    }
}
