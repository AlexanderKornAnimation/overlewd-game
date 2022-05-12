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
                switch (inputData.ftueStageData.ftueState)
                {
                    case ("sex1", "chapter1"):
                        UIManager.MakeScreen<DialogScreen>().
                            SetData(new DialogScreenInData
                            {
                                ftueStageId = GameData.ftue.mapChapter.GetStageByKey("dialogue1")?.id
                            }).RunShowScreenProcess();
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
                switch (inputData.ftueStageData.ftueState)
                {
                    case ("sex4", "chapter1"):
                        GameData.ftue.mapChapter.ShowNotifByKey("memorytutor2");
                        break;
                }

                await Task.CompletedTask;
            }

            private async void ShowEndNotifictaions()
            {
                switch (inputData.ftueStageData.ftueState)
                {
                    case ("sex2", "chapter1"):
                        UIManager.AddUserInputLocker(new UserInputLocker(this));
                        await UniTask.Delay(1000);
                        UIManager.RemoveUserInputLocker(new UserInputLocker(this));

                        GameData.ftue.mapChapter.ShowNotifByKey("bufftutor2");
                        await UIManager.WaitHideNotifications();
                        GameData.ftue.mapChapter.ShowNotifByKey("ulviscreentutor");
                        break;
                }
            }
        }
    }
}