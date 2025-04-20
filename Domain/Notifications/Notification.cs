using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ReadAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }

        public Notification(Guid userId, string message)
        {
            UserId = userId;
            Message = message;
        }
        public void MarkAsRead()
        {
            IsRead = true;
            ReadAt = DateTimeOffset.UtcNow;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}