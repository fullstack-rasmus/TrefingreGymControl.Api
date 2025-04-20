using FastEndpoints;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Features.Admin.SubscriptionTypes.DeleteSubscriptionType;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly ISubscriptionTypeService _subscriptionTypeService;
    public Endpoint(ISubscriptionTypeService subscriptionTypeService)
    {
        _subscriptionTypeService = subscriptionTypeService;
    }
    public override void Configure()
    {
        Delete("/admin/subscription-types/{id}");
        Policies("AdminOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _subscriptionTypeService.DeleteSubscriptionTypeAsync(req.Id, ct);
        await SendOkAsync(Response, ct);
    }
}