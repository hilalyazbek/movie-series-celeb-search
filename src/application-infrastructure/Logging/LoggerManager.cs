using WatchDog;

namespace application_infrastructure.Logging;

public class LoggerManager : ILoggerManager
{
    public void LogDebug(string message) => WatchLogger.Log(message);
    public void LogError(string message) => WatchLogger.LogError(message);
    public void LogInfo(string message) => WatchLogger.Log(message);
    public void LogWarn(string message) => WatchLogger.LogWarning(message);
}
