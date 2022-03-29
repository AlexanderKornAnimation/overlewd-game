using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QuestsWidget : BaseWidget
    {
        protected Button mainQuestButton;
        protected Transform content;
        protected GameObject scrollMarker;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");
            var background = canvas.Find("Background");
            var sideQuests = background.Find("SideQuests");

            mainQuestButton = background.Find("MainQuest").GetComponent<Button>();
            scrollMarker = background.Find("ScrollMarker").gameObject;
            content = sideQuests.Find("ScrollView").Find("Viewport").Find("Content");

            mainQuestButton.onClick.AddListener(MainQuestButtonClick);
        }

        private void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            var quests = GameData.eventQuests;

            for (int qId = 0; qId < quests.Count; qId++)
            {
                NSQuestWidget.BaseQuestButton questButton = (qId % 3) switch
                {
                    0 => NSQuestWidget.SideQuestButton1.GetInstance(content),
                    1 => NSQuestWidget.SideQuestButton2.GetInstance(content),
                    2 => NSQuestWidget.SideQuestButton3.GetInstance(content),
                    _ => null
                };
                questButton.questData = quests[qId];
            }
        }

        protected virtual void MainQuestButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<QuestOverlay>();
        }

        public static QuestsWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<QuestsWidget>
                ("Prefabs/UI/Widgets/QuestsWidget/QuestWidget", parent);
        }
    }
}