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

            public override async Task AfterShowAsync()
            {
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
                skipButton.gameObject.SetActive(false);

                if (true)
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
                if (true)
                {
                    skipButton.gameObject.SetActive(true);
                }

                battleVideo.Play();
            }
        }
    }
}