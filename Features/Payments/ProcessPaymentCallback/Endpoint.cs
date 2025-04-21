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

        if (payment.Status == PaymentStatus.Completed)
        {
            if (payment is SubscriptionPayment subscriptionPayment)
            {
                var subscription = await _subscriptionService.GetSubscriptionAsync(subscriptionPayment.SubscriptionId, ct);
                await SendAsync(Map.FromEntity(subscription), cancellation: ct);
                return;
            }
        }

        if (req.PaymentSuccessfull)
        {
            payment.Status = PaymentStatus.Completed;
            if (payment is SubscriptionPayment subscriptionPayment)
            {
                var subscription = await _subscriptionService.BuySubscriptionAsync(payment.UserId, subscriptionPayment.SubscriptionTypeId, subscriptionPayment.StartDate, ct);
                subscriptionPayment.SetSubscriptionId(subscription.Id);
                await _dbContext.SaveChangesAsync(ct);
                await SendAsync(Map.FromEntity(subscription), cancellation: ct);
                return;
            }
        }
        else
        {
            payment.Status = PaymentStatus.Failed;
            await SendNotFoundAsync(ct);
        }
    }
}