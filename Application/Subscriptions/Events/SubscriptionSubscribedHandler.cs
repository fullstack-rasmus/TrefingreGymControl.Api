using FastEndpoints;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Domain.Subscriptions.Events;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Application.Subscriptions.Events
{
    public class SubscriptionSubscribedHandler : IEventHandler<SubscriptionSubscribedEvent>
    {
        private readonly TFGymControlDbContext _context;
        private readonly IReceiptService _receiptService;
        private readonly ILogger<SubscriptionSubscribedHandler> _logger;

        public SubscriptionSubscribedHandler(TFGymControlDbContext context, IReceiptService receiptService, ILogger<SubscriptionSubscribedHandler> logger)
        {
            _context = context;
            _receiptService = receiptService;
            _logger = logger;
        }

        public async Task HandleAsync(SubscriptionSubscribedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling SubscriptionSubscribedEvent for UserId: {UserId}, SubscriptionId: {SubscriptionId}, Price: {Price}", domainEvent.UserId, domainEvent.SubscriptionId, domainEvent.Price);
            var receipt = _receiptService.CreateSubscriptionReceipt(domainEvent.UserId, domainEvent.SubscriptionId, domainEvent.Price, "New subscription", cancellationToken);
            _context.Receipts.Add(receipt);
            _logger.LogInformation("Receipt {ReceiptId} created for UserId: {UserId}, SubscriptionId: {SubscriptionId}", receipt.Id, domainEvent.UserId, domainEvent.SubscriptionId);
            await Task.CompletedTask;
        }
    }
}