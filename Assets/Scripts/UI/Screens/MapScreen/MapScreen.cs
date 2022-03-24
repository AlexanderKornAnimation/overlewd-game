using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MapScreen : BaseFullScreen
    {
        protected Transform map;
        protected Button chapterButton;
        protected Button backbutton;
        protected GameObject chapterMap;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MapScreen/MapScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();
            backbutton = canvas.Find("BackButton").GetComponent<Button>();

            chapterButton.onClick.AddListener(ChapterButtonClick);
            backbutton.onClick.AddListener(BackButtonClick);

            map = canvas.Find("Map");
        }

        public override async Task BeforeShowAsync()
        {
            backbutton.gameObject.SetActive(false);
            chapterButton.gameObject.SetActive(true);

            //FTUE.EventsWidget.GetInstance(transform);
            FTUE.QuestsWidget.GetInstance(transform);
            FTUE.BuffWidget.GetInstance(transform);

            if (GameGlobalStates.ftueChapterData.chapterMapId.HasValue)
            {
                var mapData = GameData.GetChapterMapById(GameGlobalStates.ftueChapterData.chapterMapId.Value);
                chapterMap = ResourceManager.InstantiateRemoteAsset<GameObject>(mapData.chapterMapPath, mapData.assetBundleId, map);
            }

            foreach (var stageId in GameGlobalStates.ftueChapterData.stages)
            {
                var stageData = GameData.GetFTUEStageById(stageId);
                var stageMapNode = chapterMap.transform.Find(stageData.mapNodeName);
                if (stageMapNode == null)
                {
                    continue;
                }

                //if (stageData.status != AdminBRO.FTUEStageItem.Status_Closed)
                {
                    if (stageData.dialogId.HasValue)
                    {
                        var dialogData = GameData.GetDialogById(stageData.dialogId.Value);
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
                    else if (stageData.battleId.HasValue)
                    {
                        var battleData = GameData.GetBattleById(stageData.battleId.Value);
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

            await Task.CompletedTask;
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }

        public override async Task AfterShowAsync()
        {
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
        }

        private async void EnterScreen()
        {
            GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("maptutor");
            UIManager.ShowNotification<DialogNotification>();
            await UIManager.WaitHideNotifications();

            GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("questbooktutor");
            UIManager.ShowNotification<DialogNotification>();
            await UIManager.WaitHideNotifications();

            GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("qbcontenttutor");
            UIManager.ShowNotification<DialogNotification>();
            await UIManager.WaitHideNotifications();

            GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("eventbooktutor");
            UIManager.ShowNotification<DialogNotification>();
        }

        private void LeaveScreen()
        {

        }
    }
}