using FastEndpoints;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Features.SubsciptionTypes.ToogleSubscriptionTypeActivationState;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly ISubscriptionService _subscriptionService;
    
    public Endpoint(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    public override void Configure()
    {
        Post("/subscription-types/{subscriptionTypeId}");
        Policies("AdminOrAbove");
        Summary(s =>
        {
            s.Summary = "Activate a subscription type";
            s.Description = "Activate a subscription type";
            s.Response(200, "Subscription type activated");
            s.Response(404, "Subscription type not found");
            s.Response(403, "Forbidden");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _subscriptionService.ToogleSubscriptionTypeActivationStateAsync(req.SubscriptionTypeId, req.IsActive, ct);
        await SendOkAsync(new Response(), ct);
    }
}