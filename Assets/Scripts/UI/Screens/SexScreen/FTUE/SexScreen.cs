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
            public override async Task BeforeShowDataAsync()
            {
                dialogData = GameData.GetDialogById(inputData.ftueStageData.dialogId.Value);
                await GameData.FTUEStartStage(inputData.ftueStageData.id);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(inputData.ftueStageData.id);
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
                        switch (inputData.ftueStageData.key)
                        {
                            case "sex4":
                                UIManager.MakeNotification<DialogNotification>().
                                    SetData(new DialogNotificationInData 
                                    { 
                                        dialogData = GameGlobalStates.GetFTUENotificationByKey("memorytutor2") 
                                    }).RunShowNotificationProcess();
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
                        switch (inputData.ftueStageData.key)
                        {
                            case "sex2":
                                UIManager.AddUserInputLocker(new UserInputLocker(this));
                                await UniTask.Delay(1000);
                                UIManager.RemoveUserInputLocker(new UserInputLocker(this));

                                UIManager.MakeNotification<DialogNotification>().
                                    SetData(new DialogNotificationInData
                                    {
                                        dialogData = GameGlobalStates.GetFTUENotificationByKey("bufftutor2")
                                    }).RunShowNotificationProcess();
                                await UIManager.WaitHideNotifications();

                                UIManager.MakeNotification<DialogNotification>().
                                    SetData(new DialogNotificationInData
                                    {
                                        dialogData = GameGlobalStates.GetFTUENotificationByKey("ulviscreentutor")
                                    }).RunShowNotificationProcess();
                                break;
                        }
                        break;
                }
            }
        }
    }
}