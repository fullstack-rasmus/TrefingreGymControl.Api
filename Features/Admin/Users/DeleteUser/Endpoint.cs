using FastEndpoints;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Features.Admin.Users.DeleteUser;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IUserService _userService;
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }
    public override void Configure()
    {
        Delete("/admin/users/{userId}");
        Policies("AdminOrAbove");
        Summary(s =>
        {
            s.Summary = "Delete a user";
            s.Description = "Delete a user from the system";
            s.Response<Response>(200, "Success");
            s.Response(401, "Unauthorized");
            s.Response(403, "Forbidden");
            s.Response(500, "Internal Server Error");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _userService.DeleteUserAsync(req.UserId, ct);
        await SendAsync(new Response(), cancellation: ct);
    }
}