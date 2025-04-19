using FastEndpoints;
using TrefingreGymControl.Api.Domain.Users.Dto;

namespace TrefingreGymControl.Features.Admin.Users.GetAllUsers;

sealed class Response
{
    public List<UserDto> Users { get; set; }
}

sealed class Request 
{
    public bool IncludeDeleted { get; set; }
}
