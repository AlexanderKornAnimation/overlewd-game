using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Overlewd
{
    public static class LogCollector
    {
        public static void Initialize()
        {
            Application.logMessageReceived += Msg;
        }

        private static void Msg(string condition, string stackTrace, LogType type)
        {
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

            switch (type)
            {
                case LogType.Exception:
                    logException(newLog);
                    break;
            }
        }

        private static async void logException(AdminBRO.LogData logData)
        {
#if (UNITY_STANDALONE_WIN || UNITY_ANDROID) && !UNITY_EDITOR
            AdminBRO.logAsync(logData);

            var exceptionNotif = UIManager.MakeSystemNotif<RuntimeExceptionNotif>();
            exceptionNotif.message = $"{logData.condition}/n{logData.stackTrace}";
            var state = await exceptionNotif.WaitChangeState();
            await exceptionNotif.CloseAsync();
            UIManager.PeakSystemNotif();
            if (!UIManager.HasSystemNotif<RuntimeExceptionNotif>())
            {
                Game.Quit();
            }
#endif
            await Task.CompletedTask;
        }
    }

}
