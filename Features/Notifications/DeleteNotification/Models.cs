using FastEndpoints;

namespace TrefingreGymControl.Features.Notifications.DeleteNotification;

sealed class Request
{
    [BindFrom("userId")]
    public Guid UserId { get; set; }

    [BindFrom("notificationId")]
    public Guid NotificationId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public string Message => "Notification deleted successfully.";
}
