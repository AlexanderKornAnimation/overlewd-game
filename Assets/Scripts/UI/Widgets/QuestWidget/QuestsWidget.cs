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
            
            var iterForButton1 = 0;
            var iterForButton2 = 1;
            var iterForButton3 = 2;
            
            for (int i = 0; i < quests.Count; i++)
            {
                if (iterForButton1 == i)
                {
                    QuestWidget.SideQuestButton1.GetInstance(content);
                    iterForButton1 += 4;
                }
                else if (iterForButton2 == i)
                {
                    QuestWidget.SideQuestButton2.GetInstance(content);
                    iterForButton2 += 2;
                }
                else if (iterForButton3 == i)
                {
                    QuestWidget.SideQuestButton3.GetInstance(content);

                    iterForButton3 += i % 2 == 0 ? 4 : 3;
                }
            }

            scrollMarker.SetActive(quests.Count >= 3);
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