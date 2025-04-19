using FastEndpoints;

namespace TrefingreGymControl.Features.SubsciptionTypes.ToogleSubscriptionTypeActivationState;

sealed class Request
{
    [BindFrom("subscriptionTypeId")]
    public Guid SubscriptionTypeId { get; set; }
    public bool IsActive { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public string Message => "Subscription type activated";
}
