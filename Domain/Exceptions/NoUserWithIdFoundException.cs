using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Exceptions
{
    public class NoUserWithIdFoundException : LoggedException
    {
        public NoUserWithIdFoundException(string id, ILogger logger) : base($"User with id '{id}' was not found.", logger)
        {
        }
    }
}