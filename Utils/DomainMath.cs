using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Api.Utils
{
    public static class DomainMath
    {
        public static DateTimeOffset EndDateCalculator(SubscriptionType subscriptionType, DateTimeOffset start)
        {
            DateTimeOffset expiration;
            switch (subscriptionType.SubscriptionDurationUnit)
            {
                case SubscriptionDurationUnit.Days:
                    expiration = start.AddDays(subscriptionType.DurationValue);
                    break;
                case SubscriptionDurationUnit.Weeks:
                    expiration = start.AddDays(subscriptionType.DurationValue * 7);
                    break;
                case SubscriptionDurationUnit.Months:
                    expiration = start.AddMonths(subscriptionType.DurationValue);
                    break;
                case SubscriptionDurationUnit.Years:
                    expiration = start.AddYears(subscriptionType.DurationValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(subscriptionType.SubscriptionDurationUnit), "Unknown subscription duration unit.");
            }

            return expiration;
        }
    }
}