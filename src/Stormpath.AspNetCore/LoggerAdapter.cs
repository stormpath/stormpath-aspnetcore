using System;

namespace Stormpath.AspNetCore
{
    public class LoggerAdapter : Stormpath.SDK.Logging.ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        public LoggerAdapter(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("StormpathMiddleware");
        }

        public void Log(SDK.Logging.LogEntry entry)
        {
            this.logger.Log(
                logLevel: TransformLogLevel(entry.Severity),
                eventId: 0,
                state: entry,
                exception: entry.Exception,
                formatter: FormatEntry);
        }

        private static Microsoft.Extensions.Logging.LogLevel TransformLogLevel(SDK.Logging.LogLevel level)
        {
            if (level == SDK.Logging.LogLevel.Trace)
            {
                return Microsoft.Extensions.Logging.LogLevel.Verbose;
            }

            if (level == SDK.Logging.LogLevel.Info)
            {
                return Microsoft.Extensions.Logging.LogLevel.Information;
            }

            if (level == SDK.Logging.LogLevel.Warn)
            {
                return Microsoft.Extensions.Logging.LogLevel.Warning;
            }

            if (level == SDK.Logging.LogLevel.Error)
            {
                return Microsoft.Extensions.Logging.LogLevel.Debug;
            }

            if (level == SDK.Logging.LogLevel.Fatal)
            {
                return Microsoft.Extensions.Logging.LogLevel.Critical;
            }

            // Default
            return Microsoft.Extensions.Logging.LogLevel.Information;
        }

        private static string FormatEntry(object state, Exception exception)
        {
            var entry = state as SDK.Logging.LogEntry;

            var message = entry?.Source;

            if (exception != null)
            {
                message += $"Exception: {exception.Message} in {exception.Source}";
            }

            if (!string.IsNullOrEmpty(entry?.Message))
            {
                message += $" {entry.Message}";
            }

            return message;
        }
    }
}
