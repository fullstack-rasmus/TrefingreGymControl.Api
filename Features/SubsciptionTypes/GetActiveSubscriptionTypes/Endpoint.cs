using FastEndpoints;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Features.SubsciptionTypes.GetActiveSubscriptionTypes;

sealed class Endpoint : EndpointWithoutRequest<Response, Mapper>
{
    private ISubscriptionService _subscriptionService;

    public Endpoint(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    public override void Configure()
    {
        Get("/subscription-types/active");
        Summary(s =>
        {
            s.Summary = "Get active subscription types";
            s.Description = "Get active subscription types";
            s.Response(200, "Active subscription types");
            s.Response(404, "No active subscription types found");
            s.Response(403, "Forbidden");
        });
        Policies("UserOrAbove");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var subscriptionTypes = await _subscriptionService.GetSubscriptionTypesAsync(true, ct);
        await SendOkAsync(Map.FromEntity(subscriptionTypes), ct);
    }
}