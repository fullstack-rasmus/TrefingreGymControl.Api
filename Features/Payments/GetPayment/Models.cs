using FastEndpoints;
using TrefingreGymControl.Api.Domain.Payments.Dto;

namespace TrefingreGymControl.Features.Payments.GetPayment;

sealed class Request
{
    [BindFrom("paymentId")]
    public Guid PaymentId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public PaymentDto Payment { get; set; }
}
