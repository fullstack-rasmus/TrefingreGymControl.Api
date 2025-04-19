
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Domain.Receipts
{
    public class ReceiptService : IReceiptService
    {
        private readonly TFGymControlDbContext _dbContext;

        public ReceiptService(TFGymControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Receipt CreateSubscriptionReceipt(Guid userId, Guid subscriptionId, decimal price, string description, CancellationToken cancellationToken = default)
        {
            var subscription = _dbContext.Subscriptions
                .FirstOrDefault(x => x.Id == subscriptionId && x.UserId == userId);
            
            var receipt = Receipt.NewReceipt(ReceiptType.Subscription);
            receipt.AttachToUser(userId);
            receipt.AttachToSubscription(subscription);
            receipt.SetPrice(price);
            receipt.SetDescription(description);
            return receipt;
        }

        public async Task<List<Receipt>> GetReceiptsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Receipts
                .Include(x => x.Subscription)
                .Include(x => x.Subscription.SubscriptionType)
                .Where(r => r.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}