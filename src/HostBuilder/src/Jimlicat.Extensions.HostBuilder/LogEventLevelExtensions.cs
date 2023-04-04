using Microsoft.Extensions.Logging;

namespace Serilog.Events
{
    /// <summary>
    /// LogLevel 扩展
    /// </summary>
    public static class LogEventLevelExtensions
    {
        /// <summary>
        /// <see cref="LogEvent"/>转换为<see cref="LogEventLevel"/>
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public static LogEventLevel ToLogEventLevel(this LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => LogEventLevel.Verbose,
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Critical => LogEventLevel.Fatal,
                _ => LogEventLevel.Information,
            };
        }
    }
}
