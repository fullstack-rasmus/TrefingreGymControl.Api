using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Domain.Subscriptions
{
    public class SubscriptionTypeService : ISubscriptionTypeService
    {
        private TFGymControlDbContext _dbContext;
        public SubscriptionTypeService(TFGymControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteSubscriptionTypeAsync(Guid subscriptionTypeId, CancellationToken cancellationToken = default)
        {
            var subscriptionType = await _dbContext.SubscriptionTypes.SingleOrDefaultAsync(x => x.Id == subscriptionTypeId, cancellationToken);
            if (subscriptionType == null)
            {
                throw new Exception("Subscription type not found");
            }
            subscriptionType.Delete();
            await _dbContext.SaveChangesAsync();
        }
    }
}