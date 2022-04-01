using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Overlewd
{
    namespace FTUE
    {
        public class BossFightScreen : Overlewd.BossFightScreen
        {
            private AdminBRO.FTUEStageItem stageData;

            public BossFightScreen SetStageData(AdminBRO.FTUEStageItem data)
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
                backButton.gameObject.SetActive(false);
                startBattleButton.gameObject.SetActive(false);
                skipButton.gameObject.SetActive(false);
                battleVideo.loopPointReached += EndBattleVideo;
                await Task.CompletedTask;
            }

            public override async Task AfterShowAsync()
            {
                StartBattleButtonClick();
                await Task.CompletedTask;
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
