using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Domain.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly TFGymControlDbContext _dbContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(TFGymControlDbContext dbContext, ILogger<NotificationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task AddNotificationAsync(Guid userId, string message, CancellationToken cancellationToken = default)
        {
            var notification = new Notification(userId, message);
            _dbContext.Notifications.Add(notification);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteNotificationAsync(Guid notificationId, CancellationToken cancellationToken = default)
        {
            var notification = await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);
            if (notification is null)
                throw new NoNotificationWithIdFoundException(notificationId, _logger);
            
            notification.Delete();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Notification with ID {NotificationId} deleted successfully.", notificationId);
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var notifications = await _dbContext.Notifications
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .ToListAsync(cancellationToken);

            if (notifications is null || !notifications.Any())
            {
                _logger.LogWarning("No notifications found for user with ID {UserId}.", userId);
                return new List<Notification>();
            }

            return notifications;
        }

        public async Task MarkNotificationAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
        {
            var notification = await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);
            if (notification is null)
                throw new NoNotificationWithIdFoundException(notificationId, _logger);

            notification.MarkAsRead();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Notification with ID {NotificationId} marked as read.", notificationId);
        }
    }

}