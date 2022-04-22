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

        private AdminBRO.FTUEStageItem teamEditStageData;

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

        public MapScreen SetDataFromTeamEdit(AdminBRO.FTUEStageItem stageData)
        {
            teamEditStageData = stageData;
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
                                var dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                                if (dialogData != null)
                                {
                                    if (dialogData.type == AdminBRO.Dialog.Type_Dialog)
                                    {
                                        var dialog = NSMapScreen.DialogButton.GetInstance(stageMapNode);
                                        dialog.stageData = stageData;
                                    }
                                    else if (dialogData.type == AdminBRO.Dialog.Type_Sex)
                                    {
                                        var sex = NSMapScreen.SexSceneButton.GetInstance(stageMapNode);
                                        sex.stageData = stageData;
                                    }
                                }
                            }
                            else if (stageData.battleId.HasValue)
                            {
                                var battleData = GameData.GetBattleById(stageData.battleId.Value);
                                if (battleData != null)
                                {
                                    if (battleData.type == AdminBRO.Battle.Type_Battle)
                                    {
                                        var fight = NSMapScreen.FightButton.GetInstance(stageMapNode);
                                        fight.stageData = stageData;
                                    }
                                    else if (battleData.type == AdminBRO.Battle.Type_Boss)
                                    {
                                        var fight = NSMapScreen.FightButton.GetInstance(stageMapNode);
                                        fight.stageData = stageData;
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
            if (teamEditStageData != null)
            {
                if (teamEditStageData.battleId.HasValue)
                {
                    var battleData = GameData.GetBattleById(teamEditStageData.battleId.Value);
                    if (battleData != null)
                    {
                        if (battleData.type == AdminBRO.Battle.Type_Battle)
                        {
                            UIManager.MakePopup<FTUE.PrepareBattlePopup>().
                                SetStageData(teamEditStageData).RunShowPopupProcess();
                        }
                        else if (battleData.type == AdminBRO.Battle.Type_Boss)
                        {
                            UIManager.MakePopup<FTUE.PrepareBossFightPopup>().
                                SetStageData(teamEditStageData).RunShowPopupProcess();
                        }
                    }
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
                        SetDialogData(GameGlobalStates.GetFTUENotificationByKey("maptutor")).
                        RunShowNotificationProcess();
                    await UIManager.WaitHideNotifications();

                    UIManager.MakeNotification<DialogNotification>().
                        SetDialogData(GameGlobalStates.GetFTUENotificationByKey("questbooktutor")).
                        RunShowNotificationProcess();
                    await UIManager.WaitHideNotifications();

                    UIManager.MakeNotification<DialogNotification>().
                        SetDialogData(GameGlobalStates.GetFTUENotificationByKey("qbcontenttutor")).
                        RunShowNotificationProcess();
                    await UIManager.WaitHideNotifications();

                    UIManager.MakeNotification<DialogNotification>().
                        SetDialogData(GameGlobalStates.GetFTUENotificationByKey("eventbooktutor")).
                        RunShowNotificationProcess();
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
}