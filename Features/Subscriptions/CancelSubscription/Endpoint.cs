using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Features.Subscriptions.CancelSubscription;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly ISubscriptionService _subscriptionService;

    public Endpoint(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public override void Configure()
    {
        Delete("/users/{userId}/subscriptions/{subscriptionId}");
        Policies("SelfOnly", "UserOrAbove");
        Description(x =>
        {
            x.AutoTagOverride("Subscriptions");
        });
        Summary(s =>
        {
            s.Summary = "Cancel a subscription";
            s.Description = "This endpoint allows a user to cancel their subscription.";
            s.Response(200, "Subscription cancelled successfully.");
            s.Response(404, "Subscription not found.");
            s.Response(403, "Forbidden");
        });

    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            await _subscriptionService.CancelSubscriptionAsync(req.SubscriptionId, ct);
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(cancellation: ct);
            return;
        }
    }
}