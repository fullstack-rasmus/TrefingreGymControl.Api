using FastEndpoints;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.SubsciptionTypes.GetActiveSubscriptionTypes;

sealed class Response
{
    public List<SubscriptionTypeDto> SubscriptionTypes { get; set; } = new();
}
