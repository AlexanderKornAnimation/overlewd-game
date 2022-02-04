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
        public class BossFightScreen : Overlewd.BossFightScreen
        {
            public override async Task BeforeShowAsync()
            {
                backButton.gameObject.SetActive(false);
                startBattleButton.gameObject.SetActive(false);

                skipButton.gameObject.SetActive(false);
                battleVideo.loopPointReached += EndBattleVideo;

                await Task.CompletedTask;
            }

            public override void AfterShow()
            {
                StartBattleButtonClick();
            }

            protected override void EndBattleVideo(VideoPlayer vp)
            {
                skipButton.gameObject.SetActive(false);

                UIManager.ShowPopup<VictoryPopup>();
            }

            protected override void StartBattleButtonClick()
            {
                skipButton.gameObject.SetActive(true);

                battleVideo.Play();
            }
        }
    }
}
