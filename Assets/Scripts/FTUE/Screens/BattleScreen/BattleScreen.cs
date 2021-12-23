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

                await Task.CompletedTask;
            }

            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override void EndBattleVideo(VideoPlayer vp)
            {
                startBattleButton.gameObject.SetActive(true);
                skipButton.gameObject.SetActive(false);
                battleVideo.gameObject.SetActive(false);

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
                startBattleButton.gameObject.SetActive(false);
                if (GameGlobalStates.battleScreen_BattleId >= 3)
                {
                    skipButton.gameObject.SetActive(true);
                }

                battleVideo.gameObject.SetActive(true);

                battleVideo.Play();
            }
        }
    }
}