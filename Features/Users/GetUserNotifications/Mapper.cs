using FastEndpoints;
using TrefingreGymControl.Api.Domain.Notifications;

namespace TrefingreGymControl.Features.Users.GetUserNotifications;

sealed class Mapper : Mapper<Request, Response, object>
{
    public override Response FromEntity(object e)
    {
        if (e is not List<Notification> notifications)
        {
            throw new ArgumentException("Invalid entity type");
        }

        var response = new Response
        {
            Notifications = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                Created = n.Created,
                IsRead = n.IsRead
            }).ToList()
        };

        return response;
    }

}