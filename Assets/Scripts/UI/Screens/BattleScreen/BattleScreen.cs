using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Overlewd
{
    public class BattleScreen : BaseScreen
    {
        protected Button startBattleButton;
        protected Button backButton;
        protected Button skipButton;

        protected VideoPlayer battleVideo;
        protected RawImage renderTarget;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/BattleScreen/BattleScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            startBattleButton = canvas.Find("StartBattleButton").GetComponent<Button>();
            startBattleButton.onClick.AddListener(StartBattleButtonClick);

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);
            
            battleVideo = canvas.Find("TestVideo").GetComponent<VideoPlayer>();
            renderTarget = battleVideo.transform.Find("RenderTarget").GetComponent<RawImage>();
        }

        public override async Task BeforeShowAsync()
        {
            backButton.gameObject.SetActive(false);
            startBattleButton.gameObject.SetActive(false);

            await GameData.EventStageStartAsync(GameGlobalStates.battle_EventStageData);
            battleVideo.loopPointReached += EndBattleVideo;

            skipButton.gameObject.SetActive(false);
        }

        public override void StartShow()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.BattleScreenShow);
        }

        public override void AfterShow()
        {
            StartBattleButtonClick();
        }

        protected virtual async void EndBattleVideo(VideoPlayer vp)
        {
            skipButton.gameObject.SetActive(false);

            await GameData.EventStageEndAsync(GameGlobalStates.battle_EventStageData);

            UIManager.ShowPopup<VictoryPopup>();
        }

        protected virtual void StartBattleButtonClick()
        {
            skipButton.gameObject.SetActive(true);
            battleVideo.Play();
        }

        protected void SkipButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            battleVideo.Stop();
            EndBattleVideo(battleVideo);
        }

        protected void BackButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }
    }
}