using FastEndpoints;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Users;
using TrefingreGymControl.Api.Infrastructure.Auth.Permissions;

namespace TrefingreGymControl.Features.Users.UpdateUser;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/users/{userId}");
        Summary(s =>
        {
            s.Summary = "Update user";
            s.Description = "Update user information, can only be done by the user himself or an admin";
        });
        Policies("SelfOnly", "UserOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            await _userService.UpdateUserAsync(req.UserId, req.Fullname, ct);
        }
        catch (NoUserWithIdFoundException ex)
        {
            AddError(ex.Message);
            await SendNotFoundAsync(cancellation: ct);
            return;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendOkAsync(Response, ct);
    }
}