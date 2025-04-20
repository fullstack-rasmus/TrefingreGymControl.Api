using FastEndpoints;
using TrefingreGymControl.Api.Domain.Payments;

namespace TrefingreGymControl.Features.Payments.GetPayment;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IPaymentService _paymentService;

    public Endpoint(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    public override void Configure()
    {
        Get("/payments/{paymentId}");
        Policies("UserOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var payment = await _paymentService.GetPaymentAsync(req.PaymentId, ct);
        if (payment != null)
        {
            await SendAsync(Map.FromEntity(payment), cancellation: ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}