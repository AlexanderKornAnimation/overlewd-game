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

        private GameObject chapterMap;

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
            if (GameData.ftue.mapChapter == null)
            {
                GameData.ftue.mapChapter = GameData.devMode ?
                    GameData.ftue.info.chapter1 : GameData.ftue.activeChapter;
            }
            
            //backbutton.gameObject.SetActive(false);
            chapterSelectorButtonName.text = GameData.ftue.mapChapter.name;

            if (GameData.ftue.mapChapter != null)
            {
                var mapData = GameData.chapterMaps.GetById(GameData.ftue.mapChapter.chapterMapId);
                if (mapData != null)
                {
                    chapterMap = ResourceManager.InstantiateRemoteAsset<GameObject>(mapData.chapterMapPath, mapData.assetBundleId, map);
                    background.gameObject.SetActive(false);
                }
                else
                {
                    background.sprite = ResourceManager.LoadSprite(GameData.ftue.mapChapter.mapImgUrl);
                }

                foreach (var stageId in GameData.ftue.mapChapter.stages)
                {
                    var stageData = GameData.ftue.info.GetStageById(stageId);

                    var stageMapNode = chapterMap?.transform.Find(stageData.mapNodeName ?? "") ?? map;

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
                                    var dialog = NSMapScreen.DialogButton.GetInstance(stageMapNode);
                                    dialog.stageId = stageId;
                                    dialog.transform.localPosition = (chapterMap == null) ?
                                        stageData.mapPos.pos : Vector2.zero;

                                    if (!stageData.isComplete)
                                    {
                                        newStages.Add(dialog);
                                        dialog.gameObject.SetActive(false);
                                    }
                                }
                                else if (dialogData.isTypeSex)
                                {
                                    var sex = NSMapScreen.SexSceneButton.GetInstance(stageMapNode);
                                    sex.stageId = stageId;
                                    sex.transform.localPosition = (chapterMap == null) ?
                                        stageData.mapPos.pos : Vector2.zero;

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
                                    var fight = NSMapScreen.FightButton.GetInstance(stageMapNode);
                                    fight.stageId = stageId;
                                    fight.transform.localPosition = (chapterMap == null) ?
                                        stageData.mapPos.pos : Vector2.zero;

                                    if (!stageData.isComplete)
                                    {
                                        newStages.Add(fight);
                                        fight.gameObject.SetActive(false);
                                    }
                                }
                                else if (battleData.isTypeBoss)
                                {
                                    var fight = NSMapScreen.FightButton.GetInstance(stageMapNode);
                                    fight.stageId = stageId;
                                    fight.transform.localPosition = (chapterMap == null) ?
                                        stageData.mapPos.pos : Vector2.zero;

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
            buffPanel.Hide();
            DevWidget.GetInstance(transform);
            chapterSelector = NSMapScreen.ChapterSelector.GetInstance(transform);
            chapterSelector.Hide();

            if (GameData.ftue.mapChapter.isComplete && GameData.ftue.mapChapter.nextChapterId.HasValue)
            {
                var button = NSMapScreen.ButtonNextChapter.GetInstance(map);
                button.chapterId = GameData.ftue.mapChapter.nextChapterId.Value;
            }

            await Task.CompletedTask;
        }

        private void SidebarButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<SidebarMenuOverlay>();
        }

        public override async Task BeforeShowAsync()
        {
            SoundManager.GetEventInstance(FMODEventPath.Music_MapScreen);
            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            //animate opened stages
            bool waitStagesShowAnims = false;
            foreach (var stage in newStages)
            {
                stage.gameObject.SetActive(true);
                waitStagesShowAnims = true;
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
                        }).RunShowPopupProcess();
                }
                else if (battleData.isTypeBoss)
                {
                    UIManager.MakePopup<PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
            }

            //ftue part
            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle1", "chapter1"):
                    GameData.ftue.info.chapter1.ShowNotifByKey("maptutor");
                    await UIManager.WaitHideNotifications();
                    await questsPanel.ShowAsync();
                    GameData.ftue.info.chapter1.ShowNotifByKey("qbtutor");
                    break;
                case ("sex2", "chapter1"):
                    await questsPanel.ShowAsync();
                    await buffPanel.ShowAsync();
                    GameData.ftue.info.chapter1.ShowNotifByKey("bufftutor2");
                    break;
                default:
                    var showPanelTasks = new List<Task>();
                    showPanelTasks.Add(questsPanel.ShowAsync());
                    showPanelTasks.Add(buffPanel.ShowAsync());
                    await Task.WhenAll(showPanelTasks);
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
            // if (GameData.ftue.info.chapters.Count(ch => ch.isComplete) > 1)
            // {
                chapterSelector.Show();
            // }
        }
    }

    public class MapScreenInData : BaseFullScreenInData
    {
        public int chapterId;
        public AdminBRO.FTUEChapter chapterData => GameData.ftue.info.GetChapterById(chapterId);
    }
}