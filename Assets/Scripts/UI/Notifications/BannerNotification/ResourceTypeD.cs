using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBannerNotification
    {
        public class ResourceTypeD : BaseResource
        {
            public static ResourceTypeD GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/ResourceTypeD"), parent);
                newItem.name = nameof(ResourceTypeD);

                return newItem.AddComponent<ResourceTypeD>();
            }
        }
    }
}