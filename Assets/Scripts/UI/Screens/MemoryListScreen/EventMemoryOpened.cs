using System;
using System.Collections;
using System.Collections.Generic;
using Overlewd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMemoryOpened : MonoBehaviour
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

        public static EventMemoryOpened GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<EventMemoryOpened>(
                "Prefabs/UI/Screens/MemoryListScreen/EventMemoryOpened", parent);
        }
    }
}