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
            switch (type)
            {
                case LogType.Exception:
                    sendLogData(condition, stackTrace, type);
                    showExceptionNotif(condition, stackTrace, type);
                    break;
            }
        }

        private static void sendLogData(string condition, string stackTrace, LogType type)
        {
#if (UNITY_STANDALONE_WIN || UNITY_ANDROID) && !UNITY_EDITOR
            AdminBRO.logAsync(makeLogData(condition, stackTrace, type));
#endif
        }

        private static async void showExceptionNotif(string condition, string stackTrace, LogType type)
        {
            var exceptionNotif = UIManager.MakeSystemNotif<RuntimeExceptionNotif>();
            exceptionNotif.message = $"{condition}\n{stackTrace}";
            var state = await exceptionNotif.WaitChangeState();
            await exceptionNotif.CloseAsync();
            UIManager.PeakSystemNotif();
#if !UNITY_EDITOR
            if (!UIManager.HasSystemNotif<RuntimeExceptionNotif>())
            {
                Game.Quit();
            }
#endif
        }

        private static AdminBRO.LogData makeLogData(string condition, string stackTrace, LogType type)
        {
            return new AdminBRO.LogData
            {
                platform = Application.platform switch
                {
                    RuntimePlatform.Android => "android",
                    RuntimePlatform.WindowsEditor => "windows_editor",
                    RuntimePlatform.WindowsPlayer => "windows",
                    RuntimePlatform.WebGLPlayer => "webgl",
                    _ => "none"
                },
                condition = condition,
                stackTrace = stackTrace,
                type = type switch
                {
                    LogType.Assert => "assert",
                    LogType.Error => "error",
                    LogType.Exception => "exception",
                    LogType.Log => "log",
                    LogType.Warning => "warning",
                    _ => "none"
                }
            };
        }
    }

}
