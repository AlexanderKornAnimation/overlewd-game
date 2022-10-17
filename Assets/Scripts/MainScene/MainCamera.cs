using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Overlewd
{
    public class MainCamera : MonoBehaviour
    {
#if UNITY_EDITOR
        private Vector2 resolution;
#endif

        void Awake()
        {
#if UNITY_EDITOR
            resolution = new Vector2(Screen.width, Screen.height);
#endif

            LogCollector.Initialize();
            UIManager.Initialize();
            ResourceManager.Initialize();

#if !UNITY_EDITOR && UNITY_ANDROID && !DEV_BUILD
            Nutaku.Unity.SdkPlugin.Initialize();
#endif
        }

        IEnumerator Start()
        {
            /*
            This delay is needed so that Unity finishes initialization
            and does't interfere with the smooth animation of showing
            the window
            */
            yield return new WaitForSeconds(0.5f);

            //Sound manager init
            yield return SoundManager.PreloadBanks();
            SoundManager.Initialize();

            UIManager.ShowScreen<LoadingScreen>();
        }

        void Update()
        {
#if UNITY_EDITOR
            if (resolution.x != Screen.width || resolution.y != Screen.height)
            {
                resolution.x = Screen.width;
                resolution.y = Screen.height;
                UIManager.ChangeResolution();
            }
#endif
        }
    }

}
