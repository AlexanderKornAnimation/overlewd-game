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
            public override async Task BeforeShowAsync()
            {
                startBattleButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);

                skipButton.gameObject.SetActive(false);
                battleVideo.loopPointReached += EndBattleVideo;

                await GameData.FTUEStartStage(GameGlobalStates.ftue_StageId.Value);

                await Task.CompletedTask;
            }

            public override async Task AfterShowAsync()
            {
                ShowStartNotifications();

                await Task.CompletedTask;
            }

            public override async Task BeforeHideAsync()
            {
                await GameData.FTUEStartStage(GameGlobalStates.ftue_StageId.Value);
            }

            protected override void EndBattleVideo(VideoPlayer vp)
            {
                skipButton.gameObject.SetActive(false);

                if (GameGlobalStates.ftue_StageKey == "battle2")
                {
                    UIManager.ShowPopup<DefeatPopup>();
                }
                else
                {
                    UIManager.ShowPopup<VictoryPopup>();
                }

                ShowEndNotifications();
            }

            protected override void StartBattleButtonClick()
            {
                if (true)
                {
                    skipButton.gameObject.SetActive(true);
                }

                battleVideo.Play();
            }

            private async void ShowStartNotifications()
            {
                if (GameGlobalStates.ftue_StageKey == "battle1")
                {
                    GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("battletutor1");
                    UIManager.ShowNotification<DialogNotification>();
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

                switch (GameGlobalStates.ftue_StageKey)
                {
                    case "battle1":
                        GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("battletutor2");
                        UIManager.ShowNotification<DialogNotification>();
                        break;
                    case "battle2":
                        GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("battletutor3");
                        UIManager.ShowNotification<DialogNotification>();
                        await UIManager.WaitHideNotifications();

                        GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("bufftutor1");
                        UIManager.ShowNotification<DialogNotification>();
                        break;
                    case "battle5":
                        GameGlobalStates.dialogNotificationData = GameGlobalStates.GetFTUENotificationByKey("castletutor");
                        UIManager.ShowNotification<DialogNotification>();
                        break;
                }

                await Task.CompletedTask;
            }
        }
    }
}