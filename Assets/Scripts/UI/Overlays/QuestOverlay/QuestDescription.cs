using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class QuestDescription : MonoBehaviour
        {
            private TextMeshProUGUI text;
            private Image girlEmotion;
            
            public int? questId { get; set; }
            public AdminBRO.QuestItem questData => GameData.quests.GetById(questId);

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                text = canvas.Find("Text").GetComponent<TextMeshProUGUI>();
                girlEmotion = canvas.Find("GirlEmotion").GetComponent<Image>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                text.text = questData.description;
            }
            
            public static QuestDescription GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<QuestDescription>
                    ("Prefabs/UI/Overlays/QuestOverlay/QuestDescription", parent);
            }
        }
    }
}
