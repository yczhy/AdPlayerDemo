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
            
        }

        public static void LogAd(string message)
        {
            Log(LogType.AdLog, message);
        }

        public static void LogPool(string message)
        {
            Log(LogType.PoolLog, message);
        }

        public static void LogUI(string message)
        {
            Log(LogType.UI, message);
        }

        public static void LogWarning(string Tag, string message)
        {
            Debug.LogWarning($"[{Tag}] {message}");
        }
    }
}
