using FastEndpoints;
using TrefingreGymControl.Api.Domain.Users;
using TrefingreGymControl.Api.Domain.Users.Dto;

namespace TrefingreGymControl.Features.Admin.Users.GetAllUsers;

sealed class Mapper : ResponseMapper<Response, List<TFGCUser>>
{
    public override Response FromEntity(List<TFGCUser> users)
    {
        var response = new Response
        {
            Users = users.Select(u => new UserDto
            {
                Id = u.Id,
                Fullname = u.Fullname,
                Email = u.Email,
                Role = u.Role.ToString(),
                IsDeleted = u.IsDeleted
            }).ToList()
        };
        return response;
    }

}