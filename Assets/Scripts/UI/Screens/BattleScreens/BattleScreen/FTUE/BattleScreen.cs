using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Overlewd
{
    namespace FTUE
    {
        public class BattleScreen : Overlewd.BattleScreen
        {
            private AdminBRO.FTUEStageItem stageData;
            public BattleScreen SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
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

            public override async Task BeforeShowAsync()
            {
                startBattleButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
                skipButton.gameObject.SetActive(false);
                battleVideo.loopPointReached += EndBattleVideo;
                await Task.CompletedTask;
            }

            public override async Task AfterShowAsync()
            {
                ShowStartNotifications();
                await Task.CompletedTask;
            }

            protected override void EndBattleVideo(VideoPlayer vp)
            {
                skipButton.gameObject.SetActive(false);

                if (stageData.key == "battle2")
                {
                    UIManager.ShowPopup<DefeatPopup>().
                        SetStageData(stageData);
                }
                else
                {
                    UIManager.ShowPopup<VictoryPopup>().
                        SetStageData(stageData);
                }

                ShowEndNotifications();
            }

            protected override void StartBattleButtonClick()
            {
                skipButton.gameObject.SetActive(true);
                battleVideo.Play();
            }

            private async void ShowStartNotifications()
            {
                if (stageData.key == "battle1")
                {
                    UIManager.ShowNotification<DialogNotification>().
                        SetDialogData(GameGlobalStates.GetFTUENotificationByKey("battletutor1"));
                    await UIManager.WaitHideNotifications();
                }

                StartBattleButtonClick();
            }

            private async void ShowEndNotifications()
            {
                var waitPopupsEndTr = true;
                while (waitPopupsEndTr)
                {
                    var victoryPopupTrState = UIManager.GetPopup<VictoryPopup>()?.IsTransitionState() ?? false;
                    var defeatPopupTrState = UIManager.GetPopup<DefeatPopup>()?.IsTransitionState() ?? false;
                    waitPopupsEndTr = victoryPopupTrState || defeatPopupTrState;
                    await UniTask.NextFrame();
                }

                /*switch (stageData.key)
                {
                    case "battle1":
                        UIManager.ShowNotification<DialogNotification>().
                            SetDialogData(GameGlobalStates.GetFTUENotificationByKey("battletutor2"));
                        break;
                    case "battle2":
                        UIManager.ShowNotification<DialogNotification>().
                            SetDialogData(GameGlobalStates.GetFTUENotificationByKey("battletutor3"));
                        await UIManager.WaitHideNotifications();

                        UIManager.ShowNotification<DialogNotification>().
                            SetDialogData(GameGlobalStates.GetFTUENotificationByKey("bufftutor1"));
                        break;
                    case "battle5":
                        UIManager.ShowNotification<DialogNotification>().
                            SetDialogData(GameGlobalStates.GetFTUENotificationByKey("castletutor"));
                        break;
                }*/

                await Task.CompletedTask;
            }
        }
    }
}