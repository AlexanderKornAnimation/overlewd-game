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
            private Transform content;
            
            public int? questId { get; set; }
            public AdminBRO.QuestItem questData => GameData.quests.GetById(questId);

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
                if (questData.isMain)
                {
                    var questInfo = MainQuestInfo.GetInstance(content);
                    questInfo.questId = questId;
                }
                else
                {
                    var questInfo = SideQuestInfo.GetInstance(content);
                    questInfo.questId = questId;
                }

                if (questData.hasDescription)
                {
                    var questDescription = QuestDescription.GetInstance(content);
                    questDescription.questId = questId;
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
