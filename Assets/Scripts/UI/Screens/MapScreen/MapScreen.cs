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

        protected GameObject chapterMap;
        protected AdminBRO.FTUEChapter nextChapterData;


        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MapScreen/MapScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            chapterButton = canvas.Find("ChapterButton").GetComponent<Button>();
            chapterButtonText = chapterButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            backbutton = canvas.Find("BackButton").GetComponent<Button>();

            chapterButton.onClick.AddListener(ChapterButtonClick);
            backbutton.onClick.AddListener(BackButtonClick);

            map = canvas.Find("Map");
        }

        public override async Task BeforeShowAsync()
        {
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
                }

                if (GameGlobalStates.ftueChapterData.nextChapterId.HasValue)
                {
                    nextChapterData = GameData.GetFTUEChapterById(GameGlobalStates.ftueChapterData.nextChapterId.Value);
                    chapterButtonText.text = nextChapterData?.name;
                    chapterButton.gameObject.SetActive(true);
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
            /*UIManager.ShowNotification<DialogNotification>().
                SetDialogData(GameGlobalStates.GetFTUENotificationByKey("maptutor"));
            await UIManager.WaitHideNotifications();

            UIManager.ShowNotification<DialogNotification>().
                SetDialogData(GameGlobalStates.GetFTUENotificationByKey("questbooktutor"));
            await UIManager.WaitHideNotifications();

            UIManager.ShowNotification<DialogNotification>().
                SetDialogData(GameGlobalStates.GetFTUENotificationByKey("qbcontenttutor"));
            await UIManager.WaitHideNotifications();

            UIManager.ShowNotification<DialogNotification>().
                SetDialogData(GameGlobalStates.GetFTUENotificationByKey("eventbooktutor"));*/
            await Task.CompletedTask;
        }

        private void LeaveScreen()
        {

        }
    }
}