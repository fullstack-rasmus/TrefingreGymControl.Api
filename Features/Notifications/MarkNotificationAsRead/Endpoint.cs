using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Notifications;

namespace TrefingreGymControl.Features.Notifications.MarkNotificationAsRead;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly INotificationService _notificationService;
    public Endpoint(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override void Configure()
    {
        Put("/users/{userId}/notifications/{notificationId}/read");
        Policies("SelfOnly", "UserOrAbove");
        Description(x =>
        {
            x.AutoTagOverride("Notifications");
        });
        Summary(x =>
        {
            x.Summary = "Mark notification as read";
            x.Description = "This endpoint marks a notification as read for the authenticated user.";
            x.Response(200, "Notification marked as read successfully.");
            x.Response(403, "Forbidden: You do not have permission to access this resource.");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            await _notificationService.MarkNotificationAsReadAsync(req.NotificationId, ct);
            await SendOkAsync(cancellation: ct);
        }
        catch (NoNotificationWithIdFoundException ex)
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
    }
}