using FastEndpoints;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Features.Users.GetUser;

sealed class Mapper : Mapper<Request, Response, object>
{
    public override Response FromEntity(object e)
    {
        var user = e as TFGCUser;
        if (user == null)
        {
            throw new ArgumentException("Invalid entity type");
        }

        return new Response
        {
            Fullname = user.Fullname,
            Email = user.Email,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Role = user.Role.ToString(),
        };
    }

}