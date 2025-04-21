using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Domain.Fees
{
    public class FeeService : IFeeService
    {
        private TFGymControlDbContext _dbContext;
        public FeeService(TFGymControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateFeeAsync(string description, decimal amount, bool isRecurring, CancellationToken cancellationToken = default)
        {
            var fee = new Fee(amount, description, isRecurring);
            await _dbContext.Fees.AddAsync(fee);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Fee> GetFeeAsync(Guid feeId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Fees
                .Where(f => f.Id == feeId)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new Exception("Fee not found");
        }

        public async Task<List<Fee>> GetFeesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Fees.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task DeleteFeeAsync(Guid feeId, CancellationToken cancellationToken = default)
        {
            var fee = await GetFeeAsync(feeId, cancellationToken);
            var subscriptionTypes = await _dbContext.SubscriptionTypes
                .Where(c => c.Fees.Any(f => f.Id == fee.Id))
                .ToListAsync(cancellationToken);

            foreach (var subscriptionType in subscriptionTypes)
            {
                subscriptionType.Fees.Remove(fee);
            }

            _dbContext.Fees.Remove(fee);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}