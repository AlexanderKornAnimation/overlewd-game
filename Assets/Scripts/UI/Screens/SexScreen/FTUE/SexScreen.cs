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

            public void SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                await GameData.FTUEStartStage(stageData.id);
                await Task.CompletedTask;
            }


            public override async Task AfterShowAsync()
            {
                ShowStartNotifications();

                await Task.CompletedTask;
            }

            protected override async void LeaveScreen()
            {
                await GameData.FTUEEndStage(stageData.id);

                switch (stageData.key)
                {
                    case "sex1":
                        /*UIManager.ShowScreen<DialogScreen>().
                            SetStageData(GameGlobalStates.GetFTUEStageByKey("dialogue1"));*/
                        UIManager.ShowScreen<MapScreen>();
                        break;
                    case "sex4":
                        UIManager.ShowScreen<MapScreen>();
                        break;
                    default:
                        UIManager.ShowScreen<MapScreen>();
                        break;
                }
            }

            protected override void ShowLastReplica()
            {
                ShowEndNotifictaions();
            }

            private async void ShowStartNotifications()
            {
                if (stageData.key == "sex4")
                {
                    /*UIManager.ShowNotification<DialogNotification>().
                        SetDialogData(GameGlobalStates.GetFTUENotificationByKey("memorytutor2"));*/
                }

                await Task.CompletedTask;
            }

            private async void ShowEndNotifictaions()
            {
                /*if (stageData.key == "sex2")
                {
                    UIManager.AddUserInputLocker(new UserInputLocker(this));
                    await UniTask.Delay(1000);
                    UIManager.RemoveUserInputLocker(new UserInputLocker(this));

                    UIManager.ShowNotification<DialogNotification>().
                        SetDialogData(GameGlobalStates.GetFTUENotificationByKey("bufftutor2"));
                    await UIManager.WaitHideNotifications();

                    UIManager.ShowNotification<DialogNotification>().
                        SetDialogData(GameGlobalStates.GetFTUENotificationByKey("ulviscreentutor"));
                    await UIManager.WaitHideNotifications();
                }*/
                await Task.CompletedTask;
            }
        }
    }
}