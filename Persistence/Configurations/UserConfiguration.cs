using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrefingreGymControl.Api.Domain.Fees;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Api.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<TFGCUser>
    {
        public void Configure(EntityTypeBuilder<TFGCUser> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Fullname).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Role).IsRequired().HasDefaultValue("User");
            builder.Property(u => u.ProfilePictureUrl).IsRequired().HasDefaultValue("");
            builder.HasMany(u => u.Notifications)
                .WithOne()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}