using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Features.Subscriptions.BuySubscription;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private ISubscriptionService _subscriptionService;
    public Endpoint(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    public override void Configure()
    {
        Post("/users/{userId}/subscriptions/{subscriptionTypeId}");
        Policies("SelfOnly", "UserOrAbove");
        Description(x=> {
            x.AutoTagOverride("Subscriptions");
        });
        Summary(
            s =>
            {
                s.Summary = "Buy a subscription for a user";
                s.Description = "This endpoint allows a user to buy a subscription.";
                s.Response<Response>(200, "Subscription bought successfully");
                s.Response(400, "Bad request");
                s.Response(401, "Unauthorized");
                s.Response(403, "Forbidden");
            }
        );
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            await _subscriptionService.BuySubscriptionAsync(req.UserId, req.SubscriptionTypeId, req.StartSubscriptionAt, ct);
            await SendOkAsync(new Response(), ct);
        }
        catch (NoUserWithIdFoundException ex)
        {
            await SendNotFoundAsync(cancellation: ct);
            return;
        }
        catch (SubscriptionTypeNotFoundException ex)
        {
            await SendNotFoundAsync(cancellation: ct);
            return;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(cancellation: ct);
            return;
        }
    }
}