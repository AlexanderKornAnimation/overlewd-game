using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overlewd
{
    public static class LogCollector
    {
        public static void Initialize()
        {
#if (UNITY_STANDALONE_WIN || UNITY_ANDROID) && !UNITY_EDITOR_WIN
            Application.logMessageReceived += Msg;
#endif
        }

        private static void Msg(string condition, string stackTrace, LogType type)
        {
            if (type != LogType.Error)
                return;

            var newLog = new AdminBRO.LogData();
            newLog.platform = Application.platform switch
            {
                RuntimePlatform.Android => "android",
                RuntimePlatform.WindowsEditor => "windows_editor",
                RuntimePlatform.WindowsPlayer => "windows",
                RuntimePlatform.WebGLPlayer => "webgl",
                _ => "none"
            };
            newLog.condition = condition;
            newLog.stackTrace = stackTrace;
            newLog.type = type switch
            {
                LogType.Assert => "assert",
                LogType.Error => "error",
                LogType.Exception => "exception",
                LogType.Log => "log",
                LogType.Warning => "warning",
                _ => "none"
            };
            AdminBRO.logAsync(newLog);
        }
    }

}
