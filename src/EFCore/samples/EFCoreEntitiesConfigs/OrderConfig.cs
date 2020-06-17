using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreEntities
{
    internal class OrderConfig : IEntityTypeConfiguration<Order<string>>
    {
        public void Configure(EntityTypeBuilder<Order<string>> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(32).IsFixedLength().IsRequired();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired().HasComment("名字");
            builder.HasIndex(x => x.Name).IsUnique().HasName("OrderNameIndex");
        }
    }
}
