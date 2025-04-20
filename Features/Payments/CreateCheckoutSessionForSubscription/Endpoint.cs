using FastEndpoints;
using Stripe;
using Stripe.Checkout;
using TrefingreGymControl.Api.Domain.Payments;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Api.Utils;

namespace TrefingreGymControl.Features.Payments.CreateCheckoutSessionForSubscription;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IPaymentService _paymentService;

    public Endpoint(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public override void Configure()
    {
        Post("/checkout/create-session");
        Policies("SelfOnly", "UserOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var payment = await _paymentService.CreatePaymentAsync(req.SubscriptionTypeId, User.GetUserId().Value, ct);
        if (payment != null)
        {
            Response.SessionId = payment.PaymentProviderSessionId;
            await SendAsync(Response, cancellation: ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}