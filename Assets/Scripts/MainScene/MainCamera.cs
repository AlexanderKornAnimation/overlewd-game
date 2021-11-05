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

        async void Start()
        {
            UIManager.Initialize();

            ResourceManager.InitializeCache();

            if (NetworkHelper.HasNetworkConection())
            {
                await AdminBRO.authLoginAsync();
                UIManager.ShowScreen<LoadingScreen>();
            }
            else
            {
                var localResourcesMeta = ResourceManager.GetLocalResourcesMeta();
                if (localResourcesMeta != null)
                {
                    ResourceManager.runtimeResourcesMeta = localResourcesMeta;
                    UIManager.ShowScreen<CastleScreen>();
                }
                else
                {
                    UIManager.ShowDialogBox("No Internet ñonnection", "", () => Game.Quit());
                }
            }

        }

        void Awake()
        {
#if UNITY_EDITOR
            resolution = new Vector2(Screen.width, Screen.height);
#endif
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
