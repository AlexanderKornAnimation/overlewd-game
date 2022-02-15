using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
         public class MainMemoryOpened : MonoBehaviour
        {
            private Image art;
            private TextMeshProUGUI title;
            private Button button;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                art = button.transform.Find("Art").GetComponent<Image>();
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {

            }

            public static MainMemoryOpened GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MainMemoryOpened>(
                    "Prefabs/UI/Screens/MemoryListScreen/MainMemoryOpened", parent);
            }
        }
    }
}