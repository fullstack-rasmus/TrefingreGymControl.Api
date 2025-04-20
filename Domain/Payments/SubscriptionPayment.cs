using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Api.Domain.Payments
{
    public class SubscriptionPayment : Payment
    {
        public Guid SubscriptionTypeId { get; set; }
        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow; //This will change in the future, letting people set this from the ui.

        public SubscriptionPayment(decimal amount, Guid subscriptionTypeId, Guid userId) : base(amount, userId)
        {
            SubscriptionTypeId = subscriptionTypeId;
        }

        public static SubscriptionPayment Create(decimal amount, Guid subscriptionTypeId, Guid userId)
        {
            return new SubscriptionPayment(amount, subscriptionTypeId, userId);
        }
    }
}