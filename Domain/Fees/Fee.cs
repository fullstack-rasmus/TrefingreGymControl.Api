using Stripe;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Api.Domain.Fees
{
    public class Fee(decimal amount, string description, bool isRecurringFee = false)
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Amount { get; set; } = amount;
        public string Description { get; set; } = description;
        public bool IsRecurringFee { get; set; } = isRecurringFee;
    }
}