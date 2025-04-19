using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Exceptions
{
    public class EmailAlreadyInUseException : LoggedException
    {
        public EmailAlreadyInUseException(string email, ILogger logger) : base($"Email '{email}' is already in use.", logger)
        {
        }
    }
}