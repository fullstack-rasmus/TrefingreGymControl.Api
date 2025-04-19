namespace TrefingreGymControl.Api.Domain.Exceptions
{
    public class LoggedException : Exception
    {
        private readonly ILogger _logger;

        public LoggedException(string message, ILogger logger) : base(message)
        {
            _logger = logger;
            _logger.LogError(this, message);
        }
    }
}