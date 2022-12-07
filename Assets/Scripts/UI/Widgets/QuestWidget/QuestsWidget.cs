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
        enum Mode
        {
            FTUE,
            Event
        }
        private Mode mode { get; set; } = Mode.FTUE;
        private bool isFTUEMode => mode == Mode.FTUE;
        private bool isEventMode => mode == Mode.Event;

        private RectTransform backRect;
        private Button mainQuestButton;
        private TextMeshProUGUI mainQuestButtonTitle;
        private Transform ftueQuestsSR;
        private Transform ftueSRContent;
        private Transform eventQuestsSR;
        private Transform eventSRContent;

        private Transform questsScrollContenet => mode switch
        {
            Mode.Event => eventSRContent,
            _ => ftueSRContent
        };

        private List<AdminBRO.QuestItem> eventQuests =>
            GameData.quests.quests.FindAll(q => q.isEvent &&
                q.eventId == GameData.events.mapEventData?.id &&
                !q.isClaimed);
        private List<AdminBRO.QuestItem> ftueQuests =>
            GameData.quests.quests.FindAll(q => q.isFTUE && !q.isClaimed);
        private AdminBRO.QuestItem mainQuest =>
            ftueQuests.Find(q => q.isFTUEMain);
        private List<AdminBRO.QuestItem> quests => isEventMode ?
            eventQuests :
            ftueQuests.FindAll(q => !q.isFTUEMain).
            OrderByDescending(q =>
                q.isNew ? 100 : 
                q.isCompleted && !q.markCompleted ? 20 :
                q.isCompleted ? 10 : 1).ToList();

        private List<NSQuestWidget.BaseQuestButton> markNewQuests = new List<NSQuestWidget.BaseQuestButton>();
        private List<NSQuestWidget.BaseQuestButton> markCompleteQuests = new List<NSQuestWidget.BaseQuestButton>();

        protected override void Awake()
        {
            var canvas = transform.Find("Canvas");
            backRect = canvas.Find("BackRect").GetComponent<RectTransform>();

            mainQuestButton = backRect.Find("FTUEMainQuest").GetComponent<Button>();
            mainQuestButtonTitle = mainQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            ftueQuestsSR = backRect.Find("FTUEQuests");
            ftueSRContent = ftueQuestsSR.Find("Viewport").Find("Content");
            eventQuestsSR = backRect.Find("EventQuests");
            eventSRContent = eventQuestsSR.Find("Viewport").Find("Content");

            mainQuestButton.onClick.AddListener(MainQuestButtonClick);

            MakeByMode();
        }

        private void Start()
        {
            Customize();
        }

        public void SwitchToEventMode()
        {
            mode = Mode.Event;
            MakeByMode();
        }

        private void MakeByMode()
        {
            mainQuestButton.gameObject.SetActive(isFTUEMode);
            ftueQuestsSR.gameObject.SetActive(isFTUEMode);
            eventQuestsSR.gameObject.SetActive(isEventMode);
        }

        private NSQuestWidget.BaseQuestButton InstQuestButton(AdminBRO.QuestItem questData, int skinId)
        {
            NSQuestWidget.BaseQuestButton questButton = skinId switch
            {
                0 => NSQuestWidget.SideQuestButton1.GetInstance(questsScrollContenet),
                1 => NSQuestWidget.SideQuestButton2.GetInstance(questsScrollContenet),
                _ => null
            };
            questButton.questId = questData.id;
            return questButton;
        }

        protected virtual void Customize()
        {
            mainQuestButtonTitle.text = mainQuest?.name;

            var questSkinId = 0;
            foreach (var questData in quests)
            {
                var questButton = InstQuestButton(questData, questSkinId);

                if (questButton.questData.isNew)
                {
                    markNewQuests.Add(questButton);
                    questButton.SetHide();
                }

                if (questButton.questData.isCompleted &&
                    !questButton.questData.markCompleted)
                {
                    markCompleteQuests.Add(questButton);
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

        private async Task FirstShowMarkedQuests()
        {
            markNewQuests.Reverse();
            foreach (var quest in markNewQuests)
            {
                await quest.WaitShowAsNew();
            }

            markCompleteQuests.Reverse();
            foreach (var quest in markCompleteQuests)
            {
                await quest.WaitShow();
                await quest.WaitMarkAsComplete();
            }
        }

        public async Task ShowAsync()
        {
            await UITools.RightShowAsync(backRect);

            await FirstShowMarkedQuests();
        }

        public async Task HideAsync()
        {
            await UITools.RightHideAsync(backRect);
        }

        public async void Refresh()
        {
            mainQuestButtonTitle.text = mainQuest?.name;

            var actualQuestsData = quests;
            var curWidgetQuests = questsScrollContenet.GetComponentsInChildren<NSQuestWidget.BaseQuestButton>().ToList();

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
                var questButton = InstQuestButton(qData, questSkinId);
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

        public override void OnUIEvent(UIEvent eventData)
        {
            switch (eventData.id)
            {
                case UIEventId.HideOverlay:
                    if (eventData.SenderTypeIs<QuestOverlay>() ||
                        eventData.SenderTypeIs<EventOverlay>())
                    {
                        Refresh();
                    }
                    break;
            }
        }

        public static QuestsWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<QuestsWidget>
                ("Prefabs/UI/Widgets/QuestsWidget/QuestWidget", parent);
        }
    }
}