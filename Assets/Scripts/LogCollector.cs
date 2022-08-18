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
            Application.logMessageReceived += Msg;
        }

        private static void Msg(string condition, string stackTrace, LogType type)
        {

        }
    }

}
