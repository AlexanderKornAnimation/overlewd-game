using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    namespace FTUE
    {
        public class SexScreen : Overlewd.SexScreen
        {
            private AdminBRO.FTUEStageItem stageData;

            public SexScreen SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
                dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                return this;
            }

            public override async Task BeforeShowDataAsync()
            {
                await GameData.FTUEStartStage(stageData.id);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(stageData.id);
            }

            public override async Task AfterShowAsync()
            {
                ShowStartNotifications();
                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                UIManager.ShowScreen<MapScreen>();
            }

            protected override void ShowLastReplica()
            {
                ShowEndNotifictaions();
            }

            private async void ShowStartNotifications()
            {
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (stageData.key)
                        {
                            case "sex4":
                                UIManager.MakeNotification<DialogNotification>().
                                    SetDialogData(GameGlobalStates.GetFTUENotificationByKey("memorytutor2")).
                                    RunShowNotificationProcess();
                                break;
                        }
                        break;
                }

                await Task.CompletedTask;
            }

            private async void ShowEndNotifictaions()
            {
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (stageData.key)
                        {
                            case "sex2":
                                UIManager.AddUserInputLocker(new UserInputLocker(this));
                                await UniTask.Delay(1000);
                                UIManager.RemoveUserInputLocker(new UserInputLocker(this));

                                UIManager.MakeNotification<DialogNotification>().
                                    SetDialogData(GameGlobalStates.GetFTUENotificationByKey("bufftutor2")).
                                    RunShowNotificationProcess();
                                await UIManager.WaitHideNotifications();

                                UIManager.MakeNotification<DialogNotification>().
                                    SetDialogData(GameGlobalStates.GetFTUENotificationByKey("ulviscreentutor")).
                                    RunShowNotificationProcess();
                                break;
                        }
                        break;
                }
            }
        }
    }
}