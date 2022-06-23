using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
        public class Memory : BaseMemory
        {
            private Image artClosed;
            private TextMeshProUGUI title;
            private TextMeshProUGUI notification;

            protected override void Awake()
            {
                base.Awake();
                artClosed = canvas.Find("ArtClosed").GetComponent<Image>();
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                notification = canvas.Find("Notification").GetComponent<TextMeshProUGUI>();
            }

            public static Memory GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Memory>("Prefabs/UI/Screens/MemoryListScreen/Memory",
                    parent);
            }
        }
    }
}