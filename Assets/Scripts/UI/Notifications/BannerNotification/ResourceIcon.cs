using System;
using System.Collections;
using System.Collections.Generic;
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
            private Text resourceCount;

            void Awake()
            {
                resourceIcon = transform.GetComponent<Image>();
                resourceCount = transform.Find("Count").GetComponent<Text>();
            }

            public static ResourceIcon GetInstance(Transform parent)
            {
                var newIcon = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/ResourceIcon"), parent);
                newIcon.name = nameof(ResourceIcon);
                return newIcon.AddComponent<ResourceIcon>();
            }
        }
    }
}