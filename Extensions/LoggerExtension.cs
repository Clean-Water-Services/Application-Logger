using Serilog;
using Datadog.Trace;

namespace CWS.Application_Logger.Extensions
{
    public enum LogLevel { Debug, Information, Warning, Error, Fatal }

    public static class LoggerExtension
    {
        public static void Capture(this ILogger logger, LogLevel logLevel, string trace, params string[] contents)
        {
            using(var scope = Tracer.Instance.StartActive(trace))
                foreach(var content in contents)
                    Output(logLevel, scope, content);
        }

        public static void Capture(this ILogger logger, LogLevel logLevel, string trace, Exception exception)
        {
            using(var scope = Tracer.Instance.StartActive(trace))
            {
                Output(logLevel, scope, exception.Message);
                scope.Span.SetException(exception);
            }
        }

        #region Private:

        private static void Output(LogLevel logLevel, IScope scope, string content)
        {
            switch (logLevel)
            {
                case LogLevel.Warning:
                    Log.Warning(content);
                    scope.Span.SetTag("Warning", content);
                    break;

                case LogLevel.Error:
                    Log.Error(content);
                    scope.Span.SetTag("Error", content);
                    break;

                case LogLevel.Fatal:
                    Log.Fatal(content);
                    scope.Span.SetTag("Fatal", content);
                    break;

                case LogLevel.Debug:
                case LogLevel.Information:
                default:
                    Log.Information(content);
                    scope.Span.SetTag("Information", content);
                    break;
            }
        }

        #endregion
    }
}