using FastEndpoints;

namespace TrefingreGymControl.Features.Users.GetUserNotifications;

sealed class Request
{
    public Guid UserId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {

    }
}

sealed class Response
{
    public List<NotificationDto> Notifications { get; set; } = new();
    public string Message => "Notifications retrieved successfully.";
}

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool IsRead { get; set; }
}


