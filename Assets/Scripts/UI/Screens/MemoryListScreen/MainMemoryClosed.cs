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
        public class MainMemoryClosed : MonoBehaviour
        {
            protected Image art;
            protected TextMeshProUGUI title;
            protected Button button;

            protected virtual void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                art = button.transform.Find("Art").GetComponent<Image>();
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {

            }

            public static MainMemoryClosed GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MainMemoryClosed>
                ("Prefabs/UI/Screens/MemoryListScreen/MainMemoryClosed", parent);
            }
        }
    }
}