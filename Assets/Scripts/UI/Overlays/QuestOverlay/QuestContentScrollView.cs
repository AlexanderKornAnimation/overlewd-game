using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class QuestContentScrollView : MonoBehaviour
        {
            public QuestButton questButton { get; set; }

            private Transform content;

            private void Awake()
            {
                content = transform.Find("Viewport").Find("Content");
                Hide();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (questButton.questData.isFTUEMain)
                {
                    var questInfo = MainQuestInfo.GetInstance(content);
                    questInfo.questContentScrollView = this;
                }
                else
                {
                    var questInfo = SideQuestInfo.GetInstance(content);
                    questInfo.questContentScrollView = this;
                }

                if (questButton.questData.hasDescription)
                {
                    var questDescription = QuestDescription.GetInstance(content);
                    questDescription.questContentScrollView = this;
                }
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
