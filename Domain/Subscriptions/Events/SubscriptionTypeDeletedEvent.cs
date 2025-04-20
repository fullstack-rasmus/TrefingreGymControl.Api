using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrefingreGymControl.Api.Domain.Common;

namespace TrefingreGymControl.Api.Domain.Subscriptions.Events
{
    public class SubscriptionTypeDeletedEvent(Guid subscriptionTypeId) : IDomainEvent
    {
        public Guid SubscriptionTypeId { get; set; } = subscriptionTypeId;
    }
}