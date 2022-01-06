using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBannerNotification
    {
        public class ResourceIcon : MonoBehaviour
        {

            public int count
            {
                set
                {
                    resourceCount.text = $"{value}";
                }
            }

            public string caption
            {
                set
                {
                    resourceCount.text = value;
                }
            }

            public Sprite icon
            {
                set
                {
                    resourceIcon.sprite = value;
                    resourceIcon.SetNativeSize();
                }
            }

            private Image resourceIcon;
            private TextMeshProUGUI resourceCount;

            void Awake()
            {
                resourceIcon = transform.GetComponent<Image>();
                resourceCount = transform.Find("Count").GetComponent<TextMeshProUGUI>();
            }

            public static ResourceIcon GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ResourceIcon>
                    ("Prefabs/UI/Notifications/BannerNotification/ResourceIcon", parent);
            }
        }
    }
}