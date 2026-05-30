using UnityEngine;
namespace Duskvern
{
    public class LogType
    {
        public const string AdLog = "AdLog";
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
    }
}
