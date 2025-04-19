using FastEndpoints;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Features.SubscriptionTypes.AddSubscriptionType;

sealed class Mapper : Mapper<Request, Response, SubscriptionType>
{
    public override SubscriptionType ToEntity(Request r)
    {
        return new SubscriptionType(r.Name, r.Price, r.DurationUnit, r.DurationValue);
    }

}