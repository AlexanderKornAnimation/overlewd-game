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

                await Task.CompletedTask;
            }

            public override void AfterShow()
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
                skipButton.gameObject.SetActive(false);

                if (GameGlobalStates.battleScreen_BattleId == 2)
                {
                    UIManager.ShowPopup<DefeatPopup>();
                }
                else
                {
                    UIManager.ShowPopup<VictoryPopup>();
                }
            }

            protected override void StartBattleButtonClick()
            {
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                
                if (GameGlobalStates.battleScreen_BattleId >= 3)
                {
                    skipButton.gameObject.SetActive(true);
                }

                battleVideo.Play();
            }
        }
    }
}