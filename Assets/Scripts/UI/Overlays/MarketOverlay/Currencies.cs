using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class Currencies : MonoBehaviour
        {
            private Image[] items = new Image[5];

            private void Awake()
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    items[i] = transform.Find($"Item{i + 1}").GetComponent<Image>();
                }
            }

            public static Currencies GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Currencies>(
                    "Prefabs/UI/Overlays/MarketOverlay/Currencies", parent);
            }
        }
    }
}
