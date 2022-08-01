using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class Characters : MonoBehaviour
        {
            private Image[] items = new Image[2];

            private void Awake()
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    items[i] = transform.Find($"Item{i + 1}").GetComponent<Image>();
                }   
            }

            public static Characters GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Characters>(
                    "Prefabs/UI/Overlays/MarketOverlay/Characters", parent);
            }
        }
    }
}