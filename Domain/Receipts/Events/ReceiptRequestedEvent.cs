using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrefingreGymControl.Api.Domain.Common;

namespace TrefingreGymControl.Api.Domain.Receipts.Events
{
    public class ReceiptRequestedEvent : IDomainEvent
    {
        public Guid UserId { get; set; }
        public Guid SubscriptionId { get; set; }
        public string Description { get; set; }

        public ReceiptRequestedEvent(Guid userId, Guid subscriptionId, string description = "", decimal price = 0)
        {
            UserId = userId;
            SubscriptionId = subscriptionId;
            Description = description;
        }

    }
}