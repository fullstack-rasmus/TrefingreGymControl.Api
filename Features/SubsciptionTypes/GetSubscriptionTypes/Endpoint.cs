using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Features.SubscriptionTypes.GetSubscriptionTypes;

sealed class Endpoint : EndpointWithoutRequest<Response, Mapper>
{
    private ISubscriptionService _subscriptionService;
    public Endpoint(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public override void Configure()
    {
        Get("/subscription-types");
        Summary(s =>
        {
            s.Summary = "Get all subscription types";
            s.Description = "Get all subscription types";
            s.Response(200, "Subscription types retrieved successfully");
            s.Response(404, "No subscription types found");
            s.Response(403, "Forbidden");
        });
        Policies("AdminOrAbove");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var subscriptionTypes = await _subscriptionService.GetSubscriptionTypesAsync(ct: ct);
        await SendOkAsync(Map.FromEntity(subscriptionTypes), ct);
    }
}