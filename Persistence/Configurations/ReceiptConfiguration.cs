using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Api.Persistence.Configurations
{
    public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
    {
        public void Configure(EntityTypeBuilder<Receipt> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.SubscriptionId).IsRequired();
            builder.Property(u => u.Description).IsRequired().HasMaxLength(500);
            builder.Property(u => u.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(u => u.ReceiptType).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}