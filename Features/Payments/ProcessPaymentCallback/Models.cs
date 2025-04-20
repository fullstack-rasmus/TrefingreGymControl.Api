using FastEndpoints;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.Payments.ProcessPaymentCallback;

sealed class Request
{
    [BindFrom("paymentId")]
    public Guid PaymentId { get; set; }
    public bool PaymentSuccessfull { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public SubscriptionDto Subscription { get; set; }
}
