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

        protected List<AdminBRO.QuestItem> ftueQuests =>
            GameData.quests.quests.FindAll(q => q.isFTUE && !q.isClaimed);
        protected AdminBRO.QuestItem mainQuest =>
            ftueQuests.Find(q => q.isFTUEMain);
        protected List<AdminBRO.QuestItem> quests =>
            ftueQuests.FindAll(q => !q.isFTUEMain).
            OrderByDescending(q => q.isNew ? 100 : q.isCompleted ? 10 : 1).ToList();

        protected override void Awake()
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
            mainQuestButtonTitle.text = mainQuest?.name;

            var questSkinId = 0;
            foreach (var questData in quests)
            {
                NSQuestWidget.BaseQuestButton questButton = questSkinId switch
                {
                    0 => NSQuestWidget.SideQuestButton1.GetInstance(content),
                    1 => NSQuestWidget.SideQuestButton2.GetInstance(content),
                    _ => null
                };
                questButton.questId = questData.id;

                if (questButton.questData.isNew ||
                    questButton.questData.isCompleted)
                {
                    questButton.SetHide();
                }

                questSkinId = ++questSkinId % 2;
            }
        }

        protected virtual void MainQuestButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            
            UIManager.MakeOverlay<QuestOverlay>().
                SetData(new QuestOverlayInData
                {
                    questId = mainQuest?.id
                }).DoShow();
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

            var newQuests = content.GetComponentsInChildren<NSQuestWidget.BaseQuestButton>().
                Where(q => q.questData.isNew).
                Reverse();
            foreach (var quest in newQuests)
            {
                await quest.WaitShowAsNew();
            }

            var completeQuests = content.GetComponentsInChildren<NSQuestWidget.BaseQuestButton>().
                Where(q => q.questData.isCompleted).
                Reverse();
            foreach (var quest in completeQuests)
            {
                await quest.WaitShow();
                await quest.WaitMarkAsComplete();
            }
        }

        public async Task HideAsync()
        {
            await UITools.RightHideAsync(backRect);
        }

        public async void Refresh()
        {
            mainQuestButtonTitle.text = mainQuest?.name;

            var actualQuestsData = quests;
            var curWidgetQuests = content.GetComponentsInChildren<NSQuestWidget.BaseQuestButton>().ToList();

            var newQuests = actualQuestsData.Where(q => !curWidgetQuests.Exists(wq => wq.questData.id == q.id));
            var eraseQuests = curWidgetQuests.Where(wq => !actualQuestsData.Exists(q => q.id == wq.questData.id));
            var markCompleted = curWidgetQuests.Where(wq => wq.questData.isCompleted && !wq.questData.markCompleted);

            foreach (var q in eraseQuests)
            {
                await q.WaitErase();
            }

            var questSkinId = 0;
            foreach (var qData in newQuests)
            {
                NSQuestWidget.BaseQuestButton questButton = questSkinId switch
                {
                    0 => NSQuestWidget.SideQuestButton1.GetInstance(content),
                    1 => NSQuestWidget.SideQuestButton2.GetInstance(content),
                    _ => null
                };
                questButton.questId = qData.id;
                questButton.SetHide();
                questButton.transform.SetAsFirstSibling();
                await questButton.WaitShowAsNew();

                questSkinId = ++questSkinId % 2;
            }

            foreach (var q in markCompleted)
            {
                await q.WaitMarkAsComplete();
            }
        }

        public static QuestsWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<QuestsWidget>
                ("Prefabs/UI/Widgets/QuestsWidget/QuestWidget", parent);
        }
    }
}