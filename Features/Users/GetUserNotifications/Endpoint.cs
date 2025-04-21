using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Notifications;
using TrefingreGymControl.Api.Utils;
using YamlDotNet.Core.Tokens;

namespace TrefingreGymControl.Features.Users.GetUserNotifications;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly INotificationService _notificationService;
    public Endpoint(INotificationService notificationService)
    {
        _notificationService = notificationService;

    }

    public override void Configure()
    {
        Get("/users/{userId}/notifications");
        Policies("UserOrAbove");
        Description(x =>
        {
            x.AutoTagOverride("Notifications");
        });
        Summary(x =>
        {
            x.Summary = "Get user notifications";
            x.Description = "This endpoint retrieves the notifications for the authenticated user.";
            x.Response(200, "Notifications retrieved successfully.");
            x.Response(403, "Forbidden: You do not have permission to access this resource.");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var notifications = await _notificationService.GetUserNotificationsAsync(req.UserId, ct);
        if (notifications is null || !notifications.Any())
        {
            AddError("No notifications found for the user.");
            await SendNotFoundAsync(cancellation: ct);
        }
        else
        {
            await SendOkAsync(Map.FromEntity(notifications), cancellation: ct);
        }
    }
}