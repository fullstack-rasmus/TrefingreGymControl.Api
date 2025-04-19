using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using TrefingreGymControl.Api.Domain.Common;

namespace TrefingreGymControl.Api.Application.Common
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(IEnumerable<Entity> entities, CancellationToken cancellationToken = default);
    }

    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchEventsAsync(IEnumerable<Entity> entities, CancellationToken cancellationToken = default)
        {
            var domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());

                var handlers = _serviceProvider
                    .GetServices(handlerType)
                    .Cast<object>();

                foreach (var handler in handlers)
                {
                    var method = handlerType.GetMethod("HandleAsync");
                    if (method != null)
                    {
                        await (Task)method.Invoke(handler, [domainEvent, cancellationToken])!;
                    }
                }
            }

            foreach (var entity in entities)
            {
                entity.ClearDomainEvents();
            }
        }
    }
}