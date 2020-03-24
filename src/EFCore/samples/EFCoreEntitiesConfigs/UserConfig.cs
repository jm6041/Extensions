using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreEntities
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        private DateTimeOffset Created = new DateTimeOffset(2020, 3, 12, 12, 33, 12, TimeSpan.FromHours(8));

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired().HasComment("名字");
            builder.Property(x => x.Sex).HasComment("性别");
            builder.Property(x => x.Created).HasComment("创建时间");

            int count = 100;
            User[] users = new User[count];
            for (int i = 0; i < count; i++)
            {
                User u = new User()
                {
                    Id = i + 1,
                    IntV = i,
                    DouV = i + 0.1,
                    Name = "Test" + i.ToString(),
                    Created = Created,
                };
                users[i] = u;
            }
            builder.HasData(users);
        }
    }
}
