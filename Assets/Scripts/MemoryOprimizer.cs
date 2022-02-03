using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overlewd
{
    public static class MemoryOprimizer
    {
        public static void ChangeScreen()
        {
            Resources.UnloadUnusedAssets();
            ResourceManager.UnloadAssetBundles();
        }

        private static void ManualCallGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

}
