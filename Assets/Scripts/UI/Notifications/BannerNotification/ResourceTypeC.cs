using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBannerNotification
    {
        public class ResourceTypeC : BaseResource
        {
            public static ResourceTypeC GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/ResourceTypeC"), parent);
                newItem.name = nameof(ResourceTypeC);

                return newItem.AddComponent<ResourceTypeC>();
            }
        }
    }
}