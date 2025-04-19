using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Api.Domain.Receipts
{
    public interface IReceiptService
    {
        Receipt CreateSubscriptionReceipt(Guid userId, Subscription subscription, decimal price, string description, CancellationToken cancellationToken = default);
        Task<List<Receipt>> GetReceiptsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}