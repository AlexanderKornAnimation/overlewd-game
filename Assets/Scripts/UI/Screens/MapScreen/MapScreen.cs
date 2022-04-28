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
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MapScreen/MapScreen", transform);

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
                if (GameGlobalStates.ftueProgressMode)
                {
                    GameGlobalStates.ftueChapterData = GetActiveChapter();
                }
                else
                {
                    GameGlobalStates.ftueChapterData = GameData.GetFTUEChapterByKey("chapter1");
                }
            }

            //backbutton.gameObject.SetActive(false);
            chapterButton.gameObject.SetActive(true);

            //FTUE.EventsWidget.GetInstance(transform);
            FTUE.QuestsWidget.GetInstance(transform);
            FTUE.BuffWidget.GetInstance(transform);

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

                        var instantiateStageOnMap = GameGlobalStates.ftueProgressMode ?
                            (stageData.status != AdminBRO.FTUEStageItem.Status_Closed) : true;
                        if (instantiateStageOnMap)
                        {
                            if (stageData.dialogId.HasValue)
                            {
                                var dialogData = stageData.dialogData;
                                if (dialogData != null)
                                {
                                    if (dialogData.type == AdminBRO.Dialog.Type_Dialog)
                                    {
                                        var dialog = NSMapScreen.DialogButton.GetInstance(stageMapNode);
                                        dialog.stageId = stageId;
                                    }
                                    else if (dialogData.type == AdminBRO.Dialog.Type_Sex)
                                    {
                                        var sex = NSMapScreen.SexSceneButton.GetInstance(stageMapNode);
                                        sex.stageId = stageId;
                                    }
                                }
                            }
                            else if (stageData.battleId.HasValue)
                            {
                                var battleData = stageData.battleData;
                                if (battleData != null)
                                {
                                    if (battleData.type == AdminBRO.Battle.Type_Battle)
                                    {
                                        var fight = NSMapScreen.FightButton.GetInstance(stageMapNode);
                                        fight.stageId = stageId;
                                    }
                                    else if (battleData.type == AdminBRO.Battle.Type_Boss)
                                    {
                                        var fight = NSMapScreen.FightButton.GetInstance(stageMapNode);
                                        fight.stageId = stageId;
                                    }
                                }
                            }
                        }
                    }
                }

                if (GameGlobalStates.ftueChapterData.nextChapterId.HasValue)
                {
                    nextChapterData = GameData.GetFTUEChapterById(GameGlobalStates.ftueChapterData.nextChapterId.Value);
                    chapterButtonText.text = nextChapterData?.name;
                    var showNextChapterButton = GameGlobalStates.ftueProgressMode ?
                        ChapterComplete(GameGlobalStates.ftueChapterData) : true;
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
                if (battleData.type == AdminBRO.Battle.Type_Battle)
                {
                    UIManager.MakePopup<FTUE.PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
                else if (battleData.type == AdminBRO.Battle.Type_Boss)
                {
                    UIManager.MakePopup<FTUE.PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
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

        private bool ChapterComplete(AdminBRO.FTUEChapter chapterData)
        {
            foreach (var stageId in chapterData.stages)
            {
                var stageData = GameData.GetFTUEStageById(stageId);
                if (stageData == null)
                    return false;
                if (stageData.status != AdminBRO.FTUEStageItem.Status_Complete)
                    return false;
            }
            return true;
        }

        private bool StageIsActive(string stageKey)
        {
            foreach (var stageId in GameGlobalStates.ftueChapterData.stages)
            {
                var stageData = GameData.GetFTUEStageById(stageId);
                if (stageData == null)
                    return false;
                if (stageData.key == stageKey)
                {
                    return (stageData.status != AdminBRO.FTUEStageItem.Status_Closed) &&
                           (stageData.status != AdminBRO.FTUEStageItem.Status_Complete);
                }
            }
            return false;
        }

        private AdminBRO.FTUEChapter GetActiveChapter()
        {
            var chapterData = GameData.GetFTUEChapterByKey("chapter1");
            while (ChapterComplete(chapterData))
            {
                if (chapterData.nextChapterId.HasValue)
                {
                    chapterData = GameData.GetFTUEChapterById(chapterData.nextChapterId.Value);
                }
            }
            return chapterData;
        }
    }

    public class MapScreenInData : BaseScreenInData
    {
        
    }
}