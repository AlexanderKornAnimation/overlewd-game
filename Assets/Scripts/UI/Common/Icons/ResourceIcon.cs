using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ResourceIcon : MonoBehaviour
    {
        public enum Type 
        {
            ResourceTypeA,
            ResourceTypeB,
            ResourceTypeC,
            ResourceTypeD
        }

        public int count
        {
            set
            {
                resourceCount.text = $"{value}";
            }
        }

        public Sprite icon
        {
            set
            {
                resourceIcon.sprite = value;
            }
        }

        private RectTransform rectTransform;
        private Image resourceIcon;
        private Text resourceCount;

        void Awake()
        {
            rectTransform = transform.GetComponent<RectTransform>();
            resourceIcon = transform.Find("Resource").Find("Icon").GetComponent<Image>();
            resourceCount = transform.Find("Resource").Find("Count").GetComponent<Text>();
        }

        public static ResourceIcon GetInstance(Type iconType, Transform parent)
        {
            var prefabPath = iconType switch
            {
                Type.ResourceTypeA => "Prefabs/UI/Common/Prefabs/ResourceIcons/ResourceTypeA",
                Type.ResourceTypeB => "Prefabs/UI/Common/Prefabs/ResourceIcons/ResourceTypeB",
                Type.ResourceTypeC => "Prefabs/UI/Common/Prefabs/ResourceIcons/ResourceTypeC",
                Type.ResourceTypeD => "Prefabs/UI/Common/Prefabs/ResourceIcons/ResourceTypeD",
                _ => ""
            };

            var newIcon = (GameObject)Instantiate(Resources.Load(prefabPath), parent);
            newIcon.name = nameof(ResourceIcon);
            return newIcon.AddComponent<ResourceIcon>();
        }
    }
}