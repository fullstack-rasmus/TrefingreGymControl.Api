using FastEndpoints;
using TrefingreGymControl.Api.Domain.Users;
using TrefingreGymControl.Api.Utils;

namespace TrefingreGymControl.Features.Auth.Me;

sealed class Endpoint : Endpoint<Request,Response>
{
    private readonly IUserService _userService;
    private readonly ILogger<Endpoint> _logger;
    public Endpoint(IUserService userService, ILogger<Endpoint> logger)
    {
        _logger = logger;
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/auth/me");
        
        Summary(s=> {
            s.Summary = "Get the current authenticated user";
            s.Description = "Returns the details of the currently authenticated user.";
            s.Response<Response>(200, "User details");
            s.Response(404, "User not found");
            s.Response(401, "Unauthorized");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var userId = req.UserId;
        _logger.LogInformation("AuthMe UserId: {UserId}", userId);
        var user = await _userService.GetUserByIdAsync(userId, ct);
        _logger.LogInformation("AuthMe success {UserId}", user.Id);
        await SendAsync(new Response
        {
            UserId = user.Id,
            Fullname = user.Fullname,
            Email = user.Email,
            Role = user.Role
        }, cancellation: ct);
    }
}