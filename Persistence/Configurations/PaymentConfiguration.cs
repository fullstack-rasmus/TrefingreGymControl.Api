using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrefingreGymControl.Api.Domain.Payments;

namespace TrefingreGymControl.Api.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasDiscriminator<string>("PaymentType")
                     .HasValue<SubscriptionPayment>("SubscriptionPayment")
                     .HasValue<KioskPayment>("KioskPayment");
        }
    }
}