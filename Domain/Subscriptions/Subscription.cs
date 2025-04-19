using TrefingreGymControl.Api.Domain.Common;
using TrefingreGymControl.Api.Domain.Receipts.Events;

namespace TrefingreGymControl.Api.Domain.Subscriptions
{
    public class Subscription : AggregateRoot
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsDeleted { get; set; }

        public static Subscription RegisterNewSubscription(Guid userId, Guid subscriptionTypeId, decimal price, bool isResubscription)
        {
            var newSub = new Subscription();
            var description = isResubscription ? "Resubscription" : "New subscription";
            newSub.SetUser(userId);
            newSub.SetSubscriptionType(subscriptionTypeId);
            newSub.SetPrice(price);

            newSub.AddDomainEvent(new ReceiptRequestedEvent(userId, newSub.Id, description));

            return newSub;
        }

        public void Cancel()
        {
            if (EndDate < DateTimeOffset.UtcNow)
                throw new InvalidOperationException("Cannot cancel a subscription that has already expired.");
            if (!IsActive)
                throw new InvalidOperationException("Subscription is already inactive.");
            if (IsCanceled)
                throw new InvalidOperationException("Subscription is already canceled.");

            IsCanceled = true;
        }

        public void DeactivateIfExpired()
        {
            if (IsCanceled && IsActive && EndDate < DateTimeOffset.UtcNow)
            {
                IsActive = false;
            }
        }

        public void Activate(bool isRenewed)
        {
            if (Price <= 0)
                throw new InvalidOperationException("Price must be greater than 0 to activate the subscription.");
            if (UserId == default)
                throw new InvalidOperationException("UserId must be set to activate the subscription.");
            if (SubscriptionTypeId == default)
                throw new InvalidOperationException("SubscriptionTypeId must be set to activate the subscription.");
            if (StartDate == default)
                throw new InvalidOperationException("StartDate must be set to activate the subscription.");
            if (EndDate == default)
                throw new InvalidOperationException("EndDate must be set to activate the subscription.");
            if (EndDate < StartDate)
                throw new InvalidOperationException("EndDate must be greater than StartDate to activate the subscription.");

            IsActive = true;
        }

        public void SetEndDate(DateTimeOffset endDate)
        {
            EndDate = endDate;
        }

        public void SetPrice(decimal price)
        {
            if (price <= 0)
                throw new InvalidOperationException("Price must be greater than 0 to set the price.");
            Price = price;
        }

        public void SetStartDate(DateTimeOffset startDate)
        {
            StartDate = startDate;
        }

        public void SetSubscriptionType(Guid subscriptionTypeId)
        {
            if (subscriptionTypeId == Guid.Empty)
                throw new InvalidOperationException("SubscriptionType must be set to register a new subscription.");
            SubscriptionTypeId = subscriptionTypeId;
        }

        public void SetUser(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new InvalidOperationException("UserId must be set to register a new subscription.");
            UserId = userId;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}