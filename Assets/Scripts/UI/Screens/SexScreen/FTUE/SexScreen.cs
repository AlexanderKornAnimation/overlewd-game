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
                dialogData = inputData.ftueStageData.dialogData;
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
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "sex1":
                                UIManager.MakeScreen<DialogScreen>().
                                    SetData(new DialogScreenInData
                                    {
                                        ftueStageId = GameGlobalStates.ftueChapterData.GetStageByKey("dialogue1")?.id
                                    }).RunShowScreenProcess();
                                break;

                            default:
                                UIManager.ShowScreen<MapScreen>();
                                break;
                        }
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
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "sex4":
                                GameGlobalStates.ftueChapterData.ShowNotifByKey("memorytutor2");
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

                                GameGlobalStates.ftueChapterData.ShowNotifByKey("bufftutor2");
                                await UIManager.WaitHideNotifications();
                                GameGlobalStates.ftueChapterData.ShowNotifByKey("ulviscreentutor");
                                break;
                        }
                        break;
                }
            }
        }
    }
}