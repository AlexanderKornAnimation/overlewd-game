using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Overlewd
{
    public class MainCamera : MonoBehaviour
    {
        void Start()
        {
            UIManager.Initialize();

            ResourceManager.InitializeCache();

            if (NetworkHelper.HasNetworkConection())
            {
                StartCoroutine(NetworkHelper.Authorization(e =>
                {
                    UIManager.ShowScreen<DebugLoadingScreen>();
                }));
            }
            else
            {
                var localResourcesMeta = ResourceManager.GetLocalResourcesMeta();
                if (localResourcesMeta != null)
                {
                    ResourceManager.runtimeResourcesMeta = localResourcesMeta;
                    UIManager.ShowScreen<DebugContentViewer>();
                }
                else
                {
                    UIManager.ShowDialogBox("No Internet �onnection", "", () => Game.Quit());
                }
            }

        }

        void Update()
        {

        }
    }

}
