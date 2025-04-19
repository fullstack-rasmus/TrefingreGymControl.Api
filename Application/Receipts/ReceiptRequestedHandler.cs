using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Domain.Receipts.Events;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Application.Receipts
{
    public class ReceiptRequestedHandler : IEventHandler<ReceiptRequestedEvent>
    {
        private readonly TFGymControlDbContext _context;
        private readonly ILogger<ReceiptRequestedHandler> _logger;

        public ReceiptRequestedHandler(TFGymControlDbContext context, ILogger<ReceiptRequestedHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task HandleAsync(ReceiptRequestedEvent receiptEvent, CancellationToken ct)
        {
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == receiptEvent.SubscriptionId, ct);

            if (subscription is null)
            {
                _logger.LogWarning("Could not find subscription {Id}", receiptEvent.SubscriptionId);
                return;
            }

            var receipt = Receipt.NewReceipt(ReceiptType.Subscription);
            receipt.AttachToUser(receiptEvent.UserId);
            receipt.SetDescription(receiptEvent.Description);
            receipt.SetPrice(subscription.Price);
            receipt.Subscription = subscription;

            await _context.Receipts.AddAsync(receipt, ct);
            _logger.LogInformation("Created receipt for subscription {Id}", receiptEvent.SubscriptionId);
        }
    }
}