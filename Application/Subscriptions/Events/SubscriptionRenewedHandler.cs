using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Domain.Subscriptions.Events;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Application.Subscriptions.Events
{
    public class SubscriptionRenewedHandler : IEventHandler<SubscriptionRenewedEvent>
    {
        private readonly TFGymControlDbContext _context;
        private readonly IReceiptService _receiptService;
        private readonly ILogger<SubscriptionRenewedHandler> _logger;

        public SubscriptionRenewedHandler(TFGymControlDbContext context, IReceiptService receiptService, ILogger<SubscriptionRenewedHandler> logger)
        {
            _context = context;
            _receiptService = receiptService;
            _logger = logger;
        }

        public async Task HandleAsync(SubscriptionRenewedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling SubscriptionRenewedEvent for UserId: {UserId}, SubscriptionId: {SubscriptionId}, Price: {Price}", domainEvent.UserId, domainEvent.SubscriptionId, domainEvent.Price);
            var receipt = _receiptService.CreateSubscriptionReceipt(domainEvent.UserId, domainEvent.SubscriptionId, domainEvent.Price, "New subscription", cancellationToken);
            await _context.Receipts.AddAsync(receipt, cancellationToken);
            _logger.LogInformation("Receipt created for UserId: {UserId}, SubscriptionId: {SubscriptionId}", domainEvent.UserId, domainEvent.SubscriptionId);
        }
    }
}