using FastEndpoints;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.Admin.Resource.GetResources;

sealed class Mapper : ResponseMapper<Response, List<Api.Domain.Resources.Resource>>
{
    public override Response FromEntity(List<Api.Domain.Resources.Resource> e)
    {
        return new Response
        {
            Resources = e.Select(r => new ResourceDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList()
        };
    }
}