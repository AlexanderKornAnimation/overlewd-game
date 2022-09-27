using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace Overlewd
{
    public class QuestsWidget : BaseWidget
    {
        protected RectTransform backRect;
        protected Button mainQuestButton;
        protected TextMeshProUGUI mainQuestButtonTitle;
        protected Transform content;
        protected GameObject scrollMarker;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");
            backRect = canvas.Find("BackRect").GetComponent<RectTransform>();
            var background = backRect.Find("Background");
            var sideQuests = background.Find("SideQuests");

            mainQuestButton = background.Find("MainQuest").GetComponent<Button>();
            mainQuestButtonTitle = mainQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
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
            var questNum = 0;
            var quests =
                GameData.quests.quests.Where(q => q.isFTUE && q.ftueChapterId == GameData.ftue.activeChapter.id);
            
            foreach (var questData in quests)
            {
                if (questData.isFTUEMain)
                {
                    mainQuestButtonTitle.text = questData.name;
                }
                else
                {
                    NSQuestWidget.BaseQuestButton questButton = (questNum % 2) switch
                    {
                        0 => NSQuestWidget.SideQuestButton1.GetInstance(content),
                        1 => NSQuestWidget.SideQuestButton2.GetInstance(content),
                        _ => null
                    };
                    questButton.questId = questData.id;
                }

                questNum++;
            }
        }

        protected virtual void MainQuestButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            var mainQuest = GameData.quests.ftueMainQuest;
            
            UIManager.MakeOverlay<QuestOverlay>().
                SetData(new QuestOverlayInData
                {
                    questId = mainQuest?.id
                }).RunShowOverlayProcess();
        }

        public void Show()
        {
            UITools.RightShow(backRect);
        }

        public void Hide()
        {
            UITools.RightHide(backRect);
        }

        public async Task ShowAsync()
        {
            await UITools.RightShowAsync(backRect);
        }

        public async Task HideAsync()
        {
            await UITools.RightHideAsync(backRect);
        }

        public static QuestsWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<QuestsWidget>
                ("Prefabs/UI/Widgets/QuestsWidget/QuestWidget", parent);
        }
    }
}