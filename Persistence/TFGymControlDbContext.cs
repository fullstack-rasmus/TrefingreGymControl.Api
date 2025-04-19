using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Application.Common;
using TrefingreGymControl.Api.Domain.Notifications;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Api.Domain.Users;
using TrefingreGymControl.Api.Infrastructure.Extensions;
using TrefingreGymControl.Api.Persistence.Configurations;

namespace TrefingreGymControl.Api.Persistence
{
    public class TFGymControlDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<TFGymControlDbContext> _logger;

        public DbSet<TFGCUser> Users => Set<TFGCUser>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<SubscriptionType> SubscriptionTypes => Set<SubscriptionType>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Receipt> Receipts => Set<Receipt>();

        public TFGymControlDbContext(DbContextOptions<TFGymControlDbContext> options, IDomainEventDispatcher domainEventDispatcher, ILogger<TFGymControlDbContext> logger) : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entitiesWithEvents = this.GetDomainEntitiesWithEvents();

            var result = await base.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Saved initial changes. Dispatching domain events...");

            await _domainEventDispatcher.DispatchEventsAsync(entitiesWithEvents, cancellationToken);

            if (ChangeTracker.HasChanges())
            {
                _logger.LogInformation("Changes detected after event dispatch, saving again...");
                await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                _logger.LogInformation("No additional changes to save after domain event dispatch.");
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
    }
}