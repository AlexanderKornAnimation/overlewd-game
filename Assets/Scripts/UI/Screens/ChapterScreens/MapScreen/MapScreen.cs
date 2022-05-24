using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    public class MapScreen : BaseFullScreen
    {
        protected List<NSMapScreen.BaseStageButton> newStages = new List<NSMapScreen.BaseStageButton>();

        protected Transform map;
        protected Button chapterButton;
        protected TextMeshProUGUI chapterButtonText;
        protected Button sidebarButton;
        protected TextMeshProUGUI chapterButtonMarkers;

        protected GameObject chapterMap;

        private MapScreenInData inputData = new MapScreenInData();

        private EventsWidget eventsPanel;
        private QuestsWidget questsPanel;
        private BuffWidget buffPanel;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/ChapterScreens/MapScreen/MapScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();
            chapterButtonText = chapterButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            chapterButtonMarkers = chapterButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            chapterButton.onClick.AddListener(ChapterButtonClick);

            sidebarButton = canvas.Find("SidebarButton").GetComponent<Button>();
            sidebarButton.onClick.AddListener(SidebarButtonClick);

            map = canvas.Find("Map");
        }

        public MapScreen SetData(MapScreenInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            if (GameData.ftue.mapChapter == null)
            {
                GameData.ftue.mapChapter = GameData.progressMode ?
                    GameData.ftue.activeChapter : GameData.ftue.info.chapter1;
            }

            //backbutton.gameObject.SetActive(false);
            chapterButton.gameObject.SetActive(true);

            if (GameData.ftue.mapChapter != null)
            {
                if (GameData.ftue.mapChapter.chapterMapId.HasValue)
                {
                    var mapData = GameData.GetChapterMapById(GameData.ftue.mapChapter.chapterMapId.Value);
                    chapterMap = ResourceManager.InstantiateRemoteAsset<GameObject>(mapData.chapterMapPath, mapData.assetBundleId, map);

                    foreach (var stageId in GameData.ftue.mapChapter.stages)
                    {
                        var stageData = GameData.ftue.info.GetStageById(stageId);

                        var stageMapNode = chapterMap.transform.Find(stageData.mapNodeName);
                        if (stageMapNode == null)
                        {
                            continue;
                        }

                        var instantiateStageOnMap = GameData.progressMode ? !stageData.isClosed : true;
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
                                        fight.screenInData = inputData;
                                        fight.stageId = stageId;
                                        
                                        if (!stageData.isComplete)
                                        {
                                            newStages.Add(fight);
                                            fight.gameObject.SetActive(false);
                                        }
                                    }
                                    else if (battleData.isTypeBoss)
                                    {
                                        var fight = NSMapScreen.FightButton.GetInstance(stageMapNode);
                                        fight.screenInData = inputData;
                                        fight.stageId = stageId;

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

                if (GameData.ftue.mapChapter.nextChapterId.HasValue)
                {
                    chapterButtonText.text = GameData.ftue.mapChapter.nextChapterData?.name;
                    chapterButton.gameObject.SetActive(GameData.progressMode ?
                        GameData.ftue.mapChapter.isComplete : true);
                }
                else
                {
                    chapterButton.gameObject.SetActive(false);
                }
            }

            eventsPanel = EventsWidget.GetInstance(transform);
            eventsPanel.Hide();
            questsPanel = QuestsWidget.GetInstance(transform);
            questsPanel.Hide();
            buffPanel = BuffWidget.GetInstance(transform);
            buffPanel.inputData = inputData;
            buffPanel.Hide();

            await Task.CompletedTask;
        }

        private void SidebarButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<SidebarMenuOverlay>();
        }

        public override async Task AfterShowAsync()
        {
            //return after team edit screen
            var battleData = inputData?.ftueStageData?.battleData;
            if (battleData != null)
            {
                if (battleData.isTypeBattle)
                {
                    UIManager.MakePopup<PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            prevScreenInData = inputData,
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
                else if (battleData.isTypeBoss)
                {
                    UIManager.MakePopup<PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            prevScreenInData = inputData,
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
            }

            //animate opened stages
            bool waitStagesShowAnims = false;
            foreach (var stage in newStages)
            {
                stage.gameObject.SetActive(true);
                waitStagesShowAnims = true;
            }
            if (waitStagesShowAnims) await UniTask.Delay(2000);

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

        public override async Task AfterHideAsync()
        {
            await Task.CompletedTask;
        }

        private void ChapterButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            GameData.ftue.mapChapter.nextChapterData?.SetAsMapChapter();
            UIManager.ShowScreen<MapScreen>();
        }
    }

    public class MapScreenInData : BaseScreenInData
    {
        
    }
}