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
            protected override void Start()
            {

            }

            protected override async Task PrepareShowOperationsAsync()
            {
                backButton.gameObject.SetActive(false);

                await Task.CompletedTask;
            }

            protected override void EndBattleVideo(VideoPlayer vp)
            {
                startBattleButton.gameObject.SetActive(true);
                battleVideo.gameObject.SetActive(false);

                UIManager.ShowPopup<DefeatPopup>();
            }

            protected override void StartBattleButtonClick()
            {
                startBattleButton.gameObject.SetActive(false);
                battleVideo.gameObject.SetActive(true);

                battleVideo.Play();
                battleVideo.loopPointReached += EndBattleVideo;
            }
        }
    }
}
