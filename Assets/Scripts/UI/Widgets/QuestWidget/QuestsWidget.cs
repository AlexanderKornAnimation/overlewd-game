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

            var questSkinId = 0;
            foreach (var questData in quests)
            {
                NSQuestWidget.BaseQuestButton questButton = null;
                if (questSkinId == 0)
                {
                    questButton = NSQuestWidget.SideQuestButton1.GetInstance(content);
                }
                else if (questSkinId == 1)
                {
                    questButton = NSQuestWidget.SideQuestButton2.GetInstance(content);
                }
                else if (questSkinId == 2)
                {
                    questButton = NSQuestWidget.SideQuestButton3.GetInstance(content);
                }
                questButton.questData = questData;

                questSkinId = ++questSkinId > 2 ? 0 : questSkinId;
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