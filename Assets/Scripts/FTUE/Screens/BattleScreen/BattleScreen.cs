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
            protected override void Start()
            {
                skipButton.gameObject.SetActive(false);
                battleVideo.loopPointReached += EndBattleVideo;
            }

            protected override async Task PrepareShowOperationsAsync()
            {
                backButton.gameObject.SetActive(false);

                startBattleButton.gameObject.SetActive(false);
                battleVideo.gameObject.SetActive(true);

                await Task.CompletedTask;
            }

            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override async Task AfterShowOperationsAsync()
            {
                if (GameGlobalStates.battleScreen_BattleId == 1)
                {
                    GameGlobalStates.dialogNotification_DialogId = 1;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 2)
                {
                    GameGlobalStates.dialogNotification_DialogId = 4;
                    UIManager.ShowNotification<DialogNotification>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 4)
                {
                    GameGlobalStates.dialogNotification_DialogId = 7;
                    UIManager.ShowNotification<DialogNotification>();
                }

                StartCoroutine(WaitDialogNotificationHide());

                await Task.CompletedTask;
            }

            private IEnumerator WaitDialogNotificationHide()
            {
                while (UIManager.HasNotification<DialogNotification>())
                {
                    yield return new WaitForSeconds(0.1f);
                }
                StartBattleButtonClick();
            }

            protected override void EndBattleVideo(VideoPlayer vp)
            {
                //startBattleButton.gameObject.SetActive(true);
                skipButton.gameObject.SetActive(false);
                //battleVideo.gameObject.SetActive(false);

                if (GameGlobalStates.battleScreen_BattleId == 2)
                {
                    UIManager.ShowPopup<DefeatPopup>();
                }
                else
                {
                    UIManager.ShowPopup<VictoryPopup>();
                }
            }

            protected override void SkipButtonClick()
            {
                battleVideo.Stop();
                EndBattleVideo(battleVideo);
            }


            protected override void StartBattleButtonClick()
            {
                //startBattleButton.gameObject.SetActive(false);
                if (GameGlobalStates.battleScreen_BattleId >= 3)
                {
                    skipButton.gameObject.SetActive(true);
                }

                //battleVideo.gameObject.SetActive(true);

                battleVideo.Play();
            }
        }
    }
}