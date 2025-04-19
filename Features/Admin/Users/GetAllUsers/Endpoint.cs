using FastEndpoints;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Features.Admin.Users.GetAllUsers;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }
    public override void Configure()
    {
        Get("/admin/users");
        Policies("AdminOrAbove");
        Summary(s => {
            s.Summary = "Get all users";
            s.Description = "Get all users in the system";
            s.Response<Response>(200, "Success");
            s.Response(401, "Unauthorized");
            s.Response(403, "Forbidden");
            s.Response(500, "Internal Server Error");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var users = await _userService.GetAllUsersAsync(req.IncludeDeleted, ct);
        await SendAsync(Map.FromEntity(users), cancellation: ct);
    }
}