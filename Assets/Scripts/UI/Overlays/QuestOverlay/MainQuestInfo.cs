using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class MainQuestInfo : MonoBehaviour
        {
            protected TextMeshProUGUI title;
            protected TextMeshProUGUI progress;

            protected virtual void Awake()
            {
                var canvas = transform.Find("Canvas");

                title = canvas.Find("QuestHead").Find("Title").GetComponent<TextMeshProUGUI>();
                progress = canvas.Find("QuestHead").Find("Progress").GetComponent<TextMeshProUGUI>();
            }

            public static MainQuestInfo GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MainQuestInfo>
                    ("Prefabs/UI/Overlays/QuestOverlay/MainQuestInfo", parent);
            }
        }
    }
}
