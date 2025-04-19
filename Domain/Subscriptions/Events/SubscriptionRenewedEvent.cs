using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrefingreGymControl.Api.Domain.Common;

namespace TrefingreGymControl.Api.Domain.Subscriptions.Events
{
    public class SubscriptionRenewedEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public Guid SubscriptionTypeId { get; }
        public Guid SubscriptionId { get; set; }
        public decimal Price { get; }

        public SubscriptionRenewedEvent(Guid userId, Guid subscriptionTypeId, Guid subscriptionId, decimal price)
        {
            UserId = userId;
            SubscriptionTypeId = subscriptionTypeId;
            SubscriptionId = subscriptionId;
            Price = price;
        }

    }
}