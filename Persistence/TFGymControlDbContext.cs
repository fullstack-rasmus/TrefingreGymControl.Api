using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Application.Common;
using TrefingreGymControl.Api.Domain.Notifications;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Domain.Resources;
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
        public DbSet<Resource> Resources => Set<Resource>();

        public TFGymControlDbContext(DbContextOptions<TFGymControlDbContext> options, IDomainEventDispatcher domainEventDispatcher, ILogger<TFGymControlDbContext> logger) : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            var entitiesWithEvents = this.GetDomainEntitiesWithEvents();
            await _domainEventDispatcher.DispatchEventsAsync(entitiesWithEvents, cancellationToken);
            if(ChangeTracker.HasChanges())
            {
                _logger.LogInformation("Changes detected in the context. Saving changes to the database.");
                await base.SaveChangesAsync(cancellationToken);
            }
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
    }
}