using FastEndpoints;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Features.Users.DeleteUser;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IUserService _userService;
    public Endpoint(IUserService userService)
    {
        _userService = userService;

    }
    public override void Configure()
    {
        Delete("/users/{userId}");
        Policies("SelfOrAdminOnly", "UserOrAbove");
        Summary(x =>
        {
            x.Summary = "Delete a user";
            x.Description = "This endpoint allows an admin or the user themselves to delete a user account.";
            x.Response(200, "User deleted successfully.");
            x.Response(404, "User not found.");
            x.Response(403, "Forbidden: You do not have permission to delete this user.");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            // Check if the user is an admin or the user themselves
            var isAdmin = User.IsInRole("Admin");
            await _userService.DeleteUserAsync(req.UserId, isAdmin, ct);
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