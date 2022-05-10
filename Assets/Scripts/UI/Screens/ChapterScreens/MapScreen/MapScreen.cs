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
        protected Button backbutton;
        protected TextMeshProUGUI chapterButtonMarkers;

        protected GameObject chapterMap;
        protected AdminBRO.FTUEChapter nextChapterData;

        private MapScreenInData inputData;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/ChapterScreens/MapScreen/MapScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();
            chapterButtonText = chapterButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            chapterButtonMarkers = chapterButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            backbutton = canvas.Find("BackButton").GetComponent<Button>();

            chapterButton.onClick.AddListener(ChapterButtonClick);
            backbutton.onClick.AddListener(BackButtonClick);

            map = canvas.Find("Map");
        }

        public MapScreen SetData(MapScreenInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            if (GameGlobalStates.ftueChapterData == null)
            {
                GameGlobalStates.ftueChapterData = GameGlobalStates.ftueProgressMode ?
                    GameData.ftue.activeChapter : GameData.ftue.firstChapter;
            }

            //backbutton.gameObject.SetActive(false);
            chapterButton.gameObject.SetActive(true);

            //EventsWidget.GetInstance(transform);
            QuestsWidget.GetInstance(transform);
            BuffWidget.GetInstance(transform);

            if (GameGlobalStates.ftueChapterData != null)
            {
                if (GameGlobalStates.ftueChapterData.chapterMapId.HasValue)
                {
                    var mapData = GameData.GetChapterMapById(GameGlobalStates.ftueChapterData.chapterMapId.Value);
                    chapterMap = ResourceManager.InstantiateRemoteAsset<GameObject>(mapData.chapterMapPath, mapData.assetBundleId, map);

                    foreach (var stageId in GameGlobalStates.ftueChapterData.stages)
                    {
                        var stageData = GameData.GetFTUEStageById(stageId);

                        var stageMapNode = chapterMap.transform.Find(stageData.mapNodeName);
                        if (stageMapNode == null)
                        {
                            continue;
                        }

                        var instantiateStageOnMap = GameGlobalStates.ftueProgressMode ? !stageData.isClosed : true;
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

                if (GameGlobalStates.ftueChapterData.nextChapterId.HasValue)
                {
                    nextChapterData = GameGlobalStates.ftueChapterData.nextChapterData;
                    chapterButtonText.text = nextChapterData?.name;
                    var showNextChapterButton = GameGlobalStates.ftueProgressMode ?
                        GameGlobalStates.ftueChapterData.isComplete : true;
                    chapterButton.gameObject.SetActive(showNextChapterButton);
                }
                else
                {
                    chapterButton.gameObject.SetActive(false);
                }
            }
            await Task.CompletedTask;
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<StartingScreen>();
        }

        public override async Task AfterShowAsync()
        {
            var battleData = inputData?.ftueStageData?.battleData;
            if (battleData != null)
            {
                if (battleData.isTypeBattle)
                {
                    UIManager.MakePopup<FTUE.PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
                else if (battleData.isTypeBoss)
                {
                    UIManager.MakePopup<FTUE.PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
            }

            foreach (var stage in newStages)
            {
                stage.gameObject.SetActive(true);
            }

            EnterScreen();

            await Task.CompletedTask;
        }

        public override async Task AfterHideAsync()
        {
            LeaveScreen();

            await Task.CompletedTask;
        }

        private void ChapterButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            GameGlobalStates.ftueChapterData = nextChapterData;
            UIManager.ShowScreen<MapScreen>();
        }

        private async void EnterScreen()
        {
            return;
            switch (GameGlobalStates.ftueChapterData.key)
            {
                case "chapter1":
                    UIManager.MakeNotification<DialogNotification>().
                        SetData(new DialogNotificationInData
                        {
                            dialogId = GameGlobalStates.ftueChapterData.GetNotifByKey("maptutor")?.dialogId
                        }).RunShowNotificationProcess();
                    await UIManager.WaitHideNotifications();

                    UIManager.MakeNotification<DialogNotification>().
                        SetData(new DialogNotificationInData
                        {
                            dialogId = GameGlobalStates.ftueChapterData.GetNotifByKey("questbooktutor")?.dialogId
                        }).RunShowNotificationProcess();
                    await UIManager.WaitHideNotifications();
                    
                    UIManager.MakeNotification<DialogNotification>().
                        SetData(new DialogNotificationInData
                        {
                            dialogId = GameGlobalStates.ftueChapterData.GetNotifByKey("qbcontenttutor")?.dialogId
                        }).RunShowNotificationProcess();
                    await UIManager.WaitHideNotifications();

                    UIManager.MakeNotification<DialogNotification>().
                        SetData(new DialogNotificationInData
                        { 
                            dialogId = GameGlobalStates.ftueChapterData.GetNotifByKey("eventbooktutor")?.dialogId
                        }).RunShowNotificationProcess();
                    break;
            }

            await Task.CompletedTask;
        }

        private void LeaveScreen()
        {

        }
    }

    public class MapScreenInData : BaseScreenInData
    {
        
    }
}