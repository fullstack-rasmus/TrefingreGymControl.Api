using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Api.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.UserId).IsRequired();
            builder.Property(s => s.SubscriptionTypeId).IsRequired();
            builder.Property(s => s.StartDate).IsRequired();
            builder.Property(s => s.EndDate).IsRequired();
            builder.Property(s => s.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(s => s.IsActive).IsRequired().HasDefaultValue(false);
            builder.Property(s => s.IsCanceled).IsRequired().HasDefaultValue(false);
            builder.HasOne(s => s.SubscriptionType)
                .WithMany()
                .HasForeignKey(s => s.SubscriptionTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}