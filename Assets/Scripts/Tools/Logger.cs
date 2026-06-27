using UnityEngine;
namespace Duskvern
{
    public class LogType
    {
        public const string AdLog = "AdLog";
        public const string PoolLog = "PoolLog";
        public const string UI = "UI";
    }

    public static class Logger
    {
        public const string DEBUGSTATE = "DEBUG_STATE";

        public static void Log(string logType, string message)
        {
            Debug.Log($"[{logType}] {message}");
        }

        public static void LogWarning(string logType, string message)
        {
            Debug.LogWarning($"[{logType}] {message}");
        }

        public static void LogAd(string message)
        {
            Log(LogType.AdLog, message);
        }

        public static void LogPool(string message)
        {
            Log(LogType.PoolLog, message);
        }

        public static void LogPoolWarning(string message)
        {
            LogWarning(LogType.PoolLog, message);
        }

        public static void LogUI(string message)
        {
            Log(LogType.UI, message);
        }

        public static void LogUIWarning(string message)
        {
            LogWarning(LogType.UI, message);
        }

        public static void LogError(string tag, string message)
        {
            Debug.LogError($"[{tag}] {message}");
        }
    }
}
