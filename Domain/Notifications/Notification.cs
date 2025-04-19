using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? ReadAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }

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