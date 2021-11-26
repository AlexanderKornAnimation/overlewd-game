using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBannerNotification
    {
        public class ResourceTypeA : BaseResource
        {
            public static ResourceTypeA GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/ResourceTypeA"), parent);
                newItem.name = nameof(ResourceTypeA);

                return newItem.AddComponent<ResourceTypeA>();
            }
        }
    }
}