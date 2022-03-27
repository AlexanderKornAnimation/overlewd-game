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
            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.ftue_StageDialogData;

                await Task.CompletedTask;
            }


            public override async Task AfterShowAsync()
            {
                ShowStartNotifications();

                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                switch (GameGlobalStates.ftue_StageKey)
                {
                    case "sex1":
                        GameGlobalStates.ftue_StageKey = "dialogue1";
                        UIManager.ShowScreen<DialogScreen>();
                        break;
                    case "sex4":
                        UIManager.ShowScreen<StartingScreen>();
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
                if (GameGlobalStates.ftue_StageKey == "sex4")
                {
                    GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("memorytutor2");
                    UIManager.ShowNotification<DialogNotification>();
                }

                await Task.CompletedTask;
            }

            private async void ShowEndNotifictaions()
            {
                if (GameGlobalStates.ftue_StageKey == "sex2")
                {
                    UIManager.AddUserInputLocker(new UserInputLocker(this));
                    await UniTask.Delay(1000);
                    UIManager.RemoveUserInputLocker(new UserInputLocker(this));

                    GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("bufftutor2");
                    UIManager.ShowNotification<DialogNotification>();
                    await UIManager.WaitHideNotifications();

                    GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("ulviscreentutor");
                    UIManager.ShowNotification<DialogNotification>();
                    await UIManager.WaitHideNotifications();
                }
            }
        }
    }
}