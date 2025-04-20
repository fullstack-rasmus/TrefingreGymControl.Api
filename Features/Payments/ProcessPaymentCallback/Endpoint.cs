using FastEndpoints;
using TrefingreGymControl.Api.Domain.Payments;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Features.Payments.ProcessPaymentCallback;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IPaymentService _paymentService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly TFGymControlDbContext _dbContext;


    public Endpoint(IPaymentService paymentService, ISubscriptionService subscriptionService, TFGymControlDbContext dbContext)
    {
        _paymentService = paymentService;
        _subscriptionService = subscriptionService;
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/payments/{paymentId}/callback");
        Policies("UserOrAbove");
    }

    public async override Task HandleAsync(Request req, CancellationToken ct)
    {
        var payment = await _paymentService.GetPaymentAsync(req.PaymentId, ct);

        if (req.PaymentSuccessfull)
        {
            payment.Status = PaymentStatus.Completed;
            if (payment is SubscriptionPayment subscriptionPayment)
            {
                await _subscriptionService.BuySubscriptionAsync(payment.UserId, subscriptionPayment.SubscriptionTypeId, subscriptionPayment.StartDate, ct);
            }
        }
        else
        {
            payment.Status = PaymentStatus.Failed;
        }

        await _dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }
}