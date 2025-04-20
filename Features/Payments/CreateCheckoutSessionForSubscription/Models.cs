using FastEndpoints;

namespace TrefingreGymControl.Features.Payments.CreateCheckoutSessionForSubscription;

sealed class Request
{
    public Guid SubscriptionTypeId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public string SessionId { get; internal set; }
}
