using FastEndpoints;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.SubscriptionTypes.GetSubscriptionTypes;

sealed class Response
{
    public string Message => "Returning all subscription types";
    public List<SubscriptionTypeDto> SubscriptionTypes { get; set; } = new();
}
