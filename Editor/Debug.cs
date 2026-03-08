using UnityEngine;

namespace YAMLMergeInEditor
{
    public static class Debug
    {

        public enum LogLevel
        {
            Trace,
            Debug,
            Info,
            Warn,
            Error,
            None,
        }

        public static void Log(LogLevel level, string message, UnityEngine.GameObject attachedGameObject = null)
        {

            if (level < Settings.LogLevel)
                return;

            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Info:
                    UnityEngine.Debug.Log(message, attachedGameObject);
                    break;
                case LogLevel.Warn:
                    UnityEngine.Debug.LogWarning(message, attachedGameObject);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message, attachedGameObject);
                    break;
            }

            
        }


    }

}