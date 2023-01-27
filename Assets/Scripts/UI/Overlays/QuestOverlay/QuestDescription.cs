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
            public QuestContentScrollView questContentScrollView { get; set; }
            public AdminBRO.QuestItem questData =>
                questContentScrollView.questButton.questData;

            private TextMeshProUGUI text;
            private Image girlEmotion;

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
                var _questData = questData;

                text.text = _questData.description;
                girlEmotion.sprite = ResourceManager.LoadSprite(_questData.matriarchEmotionIcon);
                girlEmotion.SetNativeSize();
            }
            
            public static QuestDescription GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<QuestDescription>
                    ("Prefabs/UI/Overlays/QuestOverlay/QuestDescription", parent);
            }
        }
    }
}
