using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Notifications
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task MarkNotificationAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default);
        Task DeleteNotificationAsync(Guid notificationId, CancellationToken cancellationToken = default);
    }
}