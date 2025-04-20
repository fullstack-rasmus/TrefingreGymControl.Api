using FastEndpoints;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.Admin.Resource.GetResources;

sealed class Response
{
    public List<ResourceDto> Resources { get; set; }
}
