using FastEndpoints;

namespace TrefingreGymControl.Features.Admin.SubscriptionTypes.DeleteSubscriptionType;

sealed class Request
{
    public Guid Id { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public string Message => "Subscription type deleted successfully.";
}
