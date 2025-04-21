
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Subscriptions;
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

        public Receipt CreateSubscriptionReceipt(Guid userId, Subscription subscription, decimal price, string description, CancellationToken cancellationToken = default)
        {
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
                .Include(x=> x.Subscription.SubscriptionType.Fees)
                .Where(r => r.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Receipt>> GetAllPendingReceiptsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Receipts
                .Include(x => x.Subscription)
                .Include(x => x.Subscription.SubscriptionType)
                .Where(r => r.Status == ReceiptPaymentStatus.Pending)
                .ToListAsync(cancellationToken);
        }
    }
}