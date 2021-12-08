using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Overlewd
{
    public class BattleScreen : BaseScreen
    {
        protected Button startBattleButton;
        protected Button backButton;

        protected VideoPlayer battleVideo;

        void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/BattleScreen/BattleScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            startBattleButton = canvas.Find("StartBattleButton").GetComponent<Button>();
            startBattleButton.onClick.AddListener(StartBattleButtonClick);

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleVideo = canvas.Find("TestVideo").GetComponent<VideoPlayer>();

            battleVideo.gameObject.SetActive(false);
        }

        async void Start()
        {
            await GameData.EventStageStartAsync(GameGlobalStates.battle_EventStageData);
        }

        private async void EndBattleVideo(VideoPlayer vp)
        {
            backButton.gameObject.SetActive(true);
            startBattleButton.gameObject.SetActive(true);
            battleVideo.gameObject.SetActive(false);

            await GameData.EventStageEndAsync(GameGlobalStates.battle_EventStageData);

            UIManager.ShowPopup<VictoryPopup>();
        }

        private void StartBattleButtonClick()
        {
            backButton.gameObject.SetActive(false);
            startBattleButton.gameObject.SetActive(false);
            battleVideo.gameObject.SetActive(true);

            battleVideo.Play();
            battleVideo.loopPointReached += EndBattleVideo;
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }
    }
}
