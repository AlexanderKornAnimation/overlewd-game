using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBannerNotification
    {
        public class ResourceTypeB : BaseResource
        {
            public static ResourceTypeB GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/ResourceTypeB"), parent);
                newItem.name = nameof(ResourceTypeB);

                return newItem.AddComponent<ResourceTypeB>();
            }
        }
    }
}