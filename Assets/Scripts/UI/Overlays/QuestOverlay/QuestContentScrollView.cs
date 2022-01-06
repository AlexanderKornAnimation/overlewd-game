using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class QuestContentScrollView : MonoBehaviour
        {
            public Transform content { get; private set; }

            void Awake()
            {
                content = transform.Find("Viewport").Find("Content");
                Hide();
            }

            public void Show()
            {
                gameObject.SetActive(true);
            }

            public void Hide()
            {
                gameObject.SetActive(false);
            }

            public static QuestContentScrollView GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<QuestContentScrollView>
                    ("Prefabs/UI/Overlays/QuestOverlay/QuestContentScrollView", parent);
            }
        }
    }
}
