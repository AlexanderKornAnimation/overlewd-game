using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    public class MapScreen : BaseFullScreenParent<MapScreenInData>
    {
        private List<NSMapScreen.BaseStageButton> newStages = new List<NSMapScreen.BaseStageButton>();

        private Transform map;
        private Image background;

        private Button chapterSelectorButton;
        private TextMeshProUGUI chapterSelectorButtonName;
        private Button sidebarButton;
        private TextMeshProUGUI chapterSelectorButtonMarkers;

        private EventsWidget eventsPanel;
        private QuestsWidget questsPanel;
        private BuffWidget buffPanel;
        private NSMapScreen.ChapterSelector chapterSelector;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/ChapterScreens/MapScreen/MapScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            chapterSelectorButton = canvas.Find("ChapterSelectorButton").GetComponent<Button>();
            chapterSelectorButtonName = chapterSelectorButton.transform.Find("ChapterName").GetComponent<TextMeshProUGUI>();
            chapterSelectorButtonMarkers = chapterSelectorButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            chapterSelectorButton.onClick.AddListener(ChapterButtonClick);

            sidebarButton = canvas.Find("SidebarButton").GetComponent<Button>();
            sidebarButton.onClick.AddListener(SidebarButtonClick);

            map = canvas.Find("Map");
            background = map.Find("Background").GetComponent<Image>();
        }

        public override async Task BeforeShowMakeAsync()
        {
            sidebarButton.gameObject.SetActive(GameData.progressFlags.showSidebarButton);

            if (GameData.ftue.mapChapter == null)
            {
                GameData.ftue.mapChapter = GameData.devMode ?
                    GameData.ftue.chapter1 : GameData.ftue.activeChapter;
            }
            
            //backbutton.gameObject.SetActive(false);
            chapterSelectorButtonName.text = GameData.ftue.mapChapter.name;

            if (GameData.ftue.mapChapter != null)
            {
                background.sprite = ResourceManager.LoadSprite(GameData.ftue.mapChapter.mapImgUrl);

                foreach (var stageId in GameData.ftue.mapChapter.stages)
                {
                    var stageData = GameData.ftue.GetStageById(stageId);

                    var instantiateStageOnMap = GameData.devMode ? true : !stageData.isClosed;
                    if (instantiateStageOnMap)
                    {
                        if (stageData.dialogId.HasValue)
                        {
                            var dialogData = stageData.dialogData;
                            if (dialogData != null)
                            {
                                if (dialogData.isTypeDialog)
                                {
                                    var dialog = NSMapScreen.DialogButton.GetInstance(map);
                                    dialog.stageId = stageId;
                                    dialog.transform.localPosition = stageData.mapPos.pos;

                                    if (!stageData.isComplete)
                                    {
                                        newStages.Add(dialog);
                                        dialog.gameObject.SetActive(false);
                                    }
                                }
                                else if (dialogData.isTypeSex)
                                {
                                    var sex = NSMapScreen.SexSceneButton.GetInstance(map);
                                    sex.stageId = stageId;
                                    sex.transform.localPosition = stageData.mapPos.pos;

                                    if (!stageData.isComplete)
                                    {
                                        newStages.Add(sex);
                                        sex.gameObject.SetActive(false);
                                    }
                                }
                            }
                        }
                        else if (stageData.battleId.HasValue)
                        {
                            var battleData = stageData.battleData;
                            if (battleData != null)
                            {
                                if (battleData.isTypeBattle)
                                {
                                    var fight = NSMapScreen.FightButton.GetInstance(map);
                                    fight.stageId = stageId;
                                    fight.transform.localPosition = stageData.mapPos.pos;

                                    if (!stageData.isComplete)
                                    {
                                        newStages.Add(fight);
                                        fight.gameObject.SetActive(false);
                                    }
                                }
                                else if (battleData.isTypeBoss)
                                {
                                    var fight = NSMapScreen.FightButton.GetInstance(map);
                                    fight.stageId = stageId;
                                    fight.transform.localPosition = stageData.mapPos.pos;

                                    if (!stageData.isComplete)
                                    {
                                        newStages.Add(fight);
                                        fight.gameObject.SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            eventsPanel = EventsWidget.GetInstance(transform);
            eventsPanel.Hide();
            questsPanel = QuestsWidget.GetInstance(transform);
            questsPanel.Hide();
            buffPanel = BuffWidget.GetInstance(transform);
            DevWidget.GetInstance(transform);
            chapterSelector = NSMapScreen.ChapterSelector.GetInstance(transform);
            chapterSelector.Hide();

            if (GameData.ftue.mapChapter.isComplete && GameData.ftue.mapChapter.nextChapterId.HasValue)
            {
                var button = NSMapScreen.ButtonNextChapter.GetInstance(map);
                button.transform.localPosition = GameData.ftue.mapChapter.nextChapterMapPos.pos;
                button.chapterId = GameData.ftue.mapChapter.nextChapterId;
            }

            await Task.CompletedTask;
        }

        private void SidebarButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<SidebarMenuOverlay>();
        }

        public override void StartShow()
        {
            SoundManager.PlayBGMusic(FMODEventPath.Music_MapScreen);
        }

        public override void OnUIEvent(UIEvent eventData)
        {
            switch (eventData?.type)
            {
                case UIEvent.Type.AfterChangeScreen:
                    switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
                    {
                        case (FTUE.CHAPTER_2, FTUE.DIALOGUE_1):
                            UIManager.ShowScreen<CastleScreen>();
                            break;
                    }
                    break;
            }
        }

        public override async Task AfterShowAsync()
        {
            //animate opened stages
            bool waitStagesShowAnims = false;
            foreach (var stage in newStages)
            {
                stage.gameObject.SetActive(true);
                waitStagesShowAnims = true;
                stage.transform.SetAsLastSibling();
            }
            if (waitStagesShowAnims) await UniTask.Delay(2000);

            //return after team edit screen
            var battleData = inputData?.ftueStageData?.battleData;
            if (battleData != null)
            {
                if (battleData.isTypeBattle)
                {
                    UIManager.MakePopup<PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).DoShow();
                }
                else if (battleData.isTypeBoss)
                {
                    UIManager.MakePopup<PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).DoShow();
                }
            }

            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_1):
                    GameData.ftue.chapter1.ShowNotifByKey("maptutor");
                    await UIManager.WaitHideNotifications();
                    break;
            }

            var showPanelTasks = new List<Task>();
            if (GameData.ftue.chapter1_battle1.isComplete)
            {
                showPanelTasks.Add(questsPanel.ShowAsync());
            }
            await Task.WhenAll(showPanelTasks);

            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_1):
                    GameData.ftue.chapter1.ShowNotifByKey("qbtutor");
                    break;
                case (FTUE.CHAPTER_1, FTUE.SEX_2):
                    GameData.ftue.chapter1.ShowNotifByKey("bufftutor2");
                    break;
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_1):
                    GameData.ftue.chapter2.ShowNotifByKey("ch2portaltutor1");
                    await UIManager.WaitHideNotifications();
                    break;
            }

            await Task.CompletedTask;
        }
        
        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();
            await Task.CompletedTask;
        }

        public override async Task AfterHideAsync()
        {
            await Task.CompletedTask;
        }

        private void ChapterButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            chapterSelector.Show();
        }
    }

    public class MapScreenInData : BaseFullScreenInData
    {
        public int chapterId;
        public AdminBRO.FTUEChapter chapterData => GameData.ftue.GetChapterById(chapterId);
    }
}