using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Common;

namespace TrefingreGymControl.Api.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static IEnumerable<Entity> GetDomainEntitiesWithEvents(this DbContext context)
        {
            return context.ChangeTracker.Entries<Entity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity).ToList();
        }
    }
}