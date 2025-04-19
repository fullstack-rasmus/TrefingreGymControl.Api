using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Features.Users.GetUser;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IUserService _userService;
    public Endpoint(IUserService userService)
    {
        _userService = userService;

    }
    public override void Configure()
    {
        Get("/users/{userid}");
        Policies("UserOrAbove");
        Summary(s =>
        {
            s.Summary = "Get user details";
            s.Description = "Retrieve the details of the authenticated user.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(req.UserId, ct);
            await SendOkAsync(Map.FromEntity(user), ct);
        }
        catch (NoUserWithIdFoundException ex)
        {
            AddError(ex.Message);
            await SendNotFoundAsync(cancellation: ct);
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(cancellation: ct);
        }
    }
}