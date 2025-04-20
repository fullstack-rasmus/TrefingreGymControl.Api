using FastEndpoints;
using TrefingreGymControl.Api.Domain.Resources;

namespace TrefingreGymControl.Features.Admin.Resource.AddResources;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private IResourceService _resourceService;

    public Endpoint(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }
    public override void Configure()
    {
        Post("/admin/resources");
        Policies("AdminOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _resourceService.AddResourceAsync(req.Name, ct);
        await SendAsync(Response, cancellation: ct);
    }
}