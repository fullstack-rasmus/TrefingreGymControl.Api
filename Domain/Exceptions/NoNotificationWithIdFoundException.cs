using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Exceptions
{
    public class NoNotificationWithIdFoundException : LoggedException
    {
        public NoNotificationWithIdFoundException(Guid notificationId, ILogger logger) : base($"No notification with id {notificationId} found.", logger)
        {
        }
    }
}