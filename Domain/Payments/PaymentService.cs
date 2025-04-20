using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Domain.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionService _subscriptionService;
        private readonly TFGymControlDbContext _dbContext;

        public PaymentService(IConfiguration configuration, ISubscriptionService subscriptionService, TFGymControlDbContext dbContext)
        {
            _configuration = configuration;
            _subscriptionService = subscriptionService;
            _dbContext = dbContext;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<Payment> CreatePaymentAsync(Guid subscriptionTypeId, Guid userId, CancellationToken cancellationToken = default)
        {
            var subscriptionTypes = await _subscriptionService.GetSubscriptionTypesAsync();
            var subscriptionType = subscriptionTypes.SingleOrDefault(x => x.Id == subscriptionTypeId) ?? throw new Exception($"Subscription type with id {subscriptionTypeId} not found.");

            var payment = SubscriptionPayment.Create(subscriptionType.Price, subscriptionTypeId, userId);

            var options = new SessionCreateOptions()
            {
                PaymentMethodTypes = new List<string>
            {
                "card","mobilepay"
            },
                LineItems = new List<SessionLineItemOptions>()
            {
                new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        Currency = "dkk",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = $"Abonnement: {subscriptionType.Name}"
                        },
                        UnitAmountDecimal = subscriptionType.Price * 100,
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = $"{_configuration["Stripe:SuccessUrl"]}/{payment.Id}",
                CancelUrl = $"{_configuration["Stripe:CancelUrl"]}/{payment.Id}",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options, cancellationToken: cancellationToken);

            payment.SetSessionId(session.Id);

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return payment;
        }

        public async Task<Payment> GetPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default)
        {
            var payment = await _dbContext.Payments
                .FirstOrDefaultAsync(x => x.Id == paymentId, cancellationToken: cancellationToken);

            if (payment == null)
            {
                throw new Exception($"Payment with id {paymentId} not found.");
            }

            return payment;
        }
    }
}