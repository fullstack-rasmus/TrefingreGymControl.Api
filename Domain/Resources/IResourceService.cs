using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Api.Domain.Resources
{
    public interface IResourceService
    {
        Task<List<Resource>> GetResourcesAsync(CancellationToken cancellationToken = default);
        Task AddResourceAsync(string name, CancellationToken cancellationToken = default);
        Task<Resource> GetResourceByIdAsync(ResourceDto resourceId, CancellationToken ct);
    }
}