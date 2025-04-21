using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Fees;
using TrefingreGymControl.Api.Domain.Resources;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Features.SubscriptionTypes.AddSubscriptionType;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IResourceService _resourceService;
    private readonly IFeeService _feeService;


    public Endpoint(ISubscriptionService subscriptionService, IResourceService resourceService, IFeeService feeService)
    {
        _resourceService = resourceService;
        _feeService = feeService;
        _subscriptionService = subscriptionService;
    }

    public override void Configure()
    {
        Post("/subscription-types");
        Policies("AdminOrAbove");
        Summary(x =>
        {
            x.Summary = "Add a new subscription type";
            x.Description = "This endpoint allows an admin to add a new subscription type.";
            x.Response(200, "Subscription type added successfully.");
            x.Response(400, "Invalid request data.");
            x.Response(401, "Unauthorized.");
            x.Response(403, "Forbidden.");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var subscriptionType = Map.ToEntity(req);
        
        if(req.IsRecurring)
            subscriptionType.MakeRecurring();
        
        foreach (var resourceId in req.Resources)
        {
            var resource = await _resourceService.GetResourceByIdAsync(resourceId, ct);
            subscriptionType.AddResource(resource);
        }

        foreach (var feeDto in req.Fees)
        {
            var fee = await _feeService.GetFeeAsync(feeDto.Id, ct);
            subscriptionType.AddFee(fee);
        }
        
        try
        {
            await _subscriptionService.AddSubscriptionTypeAsync(subscriptionType, ct);
            await SendAsync(Response, cancellation: ct);
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(cancellation: ct);
        }
    }
}