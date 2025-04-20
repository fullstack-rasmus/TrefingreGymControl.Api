using FastEndpoints;
using TrefingreGymControl.Api.Domain.Resources;

namespace TrefingreGymControl.Features.Admin.Resource.GetResources;

sealed class Endpoint : EndpointWithoutRequest<Response, Mapper>
{
    private IResourceService _resourceService;
    public Endpoint(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    public override void Configure()
    {
        Get("/admin/resources");
        Policies("AdminOrAbove");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(Map.FromEntity(await _resourceService.GetResourcesAsync(cancellationToken: ct)), ct);
    }
}