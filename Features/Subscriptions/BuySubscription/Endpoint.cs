using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Features.Subscriptions.BuySubscription;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private ISubscriptionService _subscriptionService;
    private readonly TFGymControlDbContext _dbContext;


    public Endpoint(ISubscriptionService subscriptionService, TFGymControlDbContext dbContext)
    {
        _subscriptionService = subscriptionService;
        _dbContext = dbContext;

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
            await _dbContext.SaveChangesAsync();
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