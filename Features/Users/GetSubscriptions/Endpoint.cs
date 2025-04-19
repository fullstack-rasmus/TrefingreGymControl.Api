using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.Users.GetSubscriptions;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly ISubscriptionService _subscriptionService;

    public Endpoint(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public override void Configure()
    {
        Get("/users/{userId}/subscriptions");
        Description(x=> {
            x.AutoTagOverride("Subscriptions");
        });
        Policies("SelfOnly", "UserOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var subscriptions = await _subscriptionService.GetUserSubscriptionsAsync(req.UserId, ct);
        if (subscriptions == null || subscriptions.Count == 0)
        {
            Response.Subscriptions = new List<SubscriptionDto>();
            await SendAsync(Response, cancellation: ct);
            return;
        }
        await SendAsync(Map.FromEntity(subscriptions), cancellation: ct);
    }
}