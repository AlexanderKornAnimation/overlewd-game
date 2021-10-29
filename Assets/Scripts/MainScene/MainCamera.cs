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
                StartCoroutine(AdminBRO.auth_login(() =>
                {
                    UIManager.ShowScreen<LoadingScreen>();
                }));
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

        void Update()
        {

        }
    }

}
