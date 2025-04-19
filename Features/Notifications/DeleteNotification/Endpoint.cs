using FastEndpoints;
using FastEndpoints.Swagger;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Notifications;

namespace TrefingreGymControl.Features.Notifications.DeleteNotification;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly INotificationService _notificationService;
    public Endpoint(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override void Configure()
    {
        Delete("/users/{userId}/notifications/{notificationId}");
        Policies("SelfOnly", "UserOrAbove");
        Description(x=> {
            x.AutoTagOverride("Notifications");
        });
        Summary(x =>
        {
            x.Summary = "Delete notification";
            x.Description = "This endpoint deletes a notification for the authenticated user.";
            x.Response(200, "Notification deleted successfully.");
            x.Response(403, "Forbidden: You do not have permission to access this resource.");
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            await _notificationService.DeleteNotificationAsync(req.NotificationId, ct);
            await SendOkAsync(cancellation: ct);
        }
        catch (NoNotificationWithIdFoundException ex)
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