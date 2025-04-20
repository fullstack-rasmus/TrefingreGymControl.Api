using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Persistence;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Api.Domain.Resources
{
    public class ResourceService : IResourceService
    {
        private readonly TFGymControlDbContext _dbContext;

        public ResourceService(TFGymControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddResourceAsync(string name, CancellationToken cancellationToken = default)
        {
            var resource = new Resource { Name = name };
            _dbContext.Resources.Add(resource);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Resource> GetResourceByIdAsync(ResourceDto resourceId, CancellationToken ct)
        {
            return await _dbContext.Resources
                .FirstOrDefaultAsync(x => x.Id == resourceId.Id, ct);
        }

        public async Task<List<Resource>> GetResourcesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Resources.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}