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
        public class MainQuestInfo : BaseQuestInfo
        {
            private TextMeshProUGUI title;
            private TextMeshProUGUI progress;

            protected virtual void Awake()
            {
                var canvas = transform.Find("Canvas");

                title = canvas.Find("QuestHead").Find("Title").GetComponent<TextMeshProUGUI>();
                progress = canvas.Find("QuestHead").Find("Progress").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (questData != null)
                {
                    title.text = questData?.name;
                    progress.text = questData.goalCount.HasValue
                        ? $"{questData?.progressCount} / {questData?.goalCount}"
                        : "";
                }
            }

            public static MainQuestInfo GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MainQuestInfo>
                    ("Prefabs/UI/Overlays/QuestOverlay/MainQuestInfo", parent);
            }
        }
    }
}
