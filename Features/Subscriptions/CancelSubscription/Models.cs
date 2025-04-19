using FastEndpoints;

namespace TrefingreGymControl.Features.Subscriptions.CancelSubscription;

sealed class Request
{
    [BindFrom("subscriptionId")]
    public Guid SubscriptionId { get; set; }
    [BindFrom("userId")]
    public Guid UserId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {

    }
}

sealed class Response
{
    public string Message => "This endpoint hasn't been implemented yet!";
}
