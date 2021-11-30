using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Overlewd
{
    public class BossFightScreen : BaseScreen
    {
        private Button startBattleButton;
        private Button backButton;

        private VideoPlayer battleVideo;

        void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/BossFightScreen/BossFightScreen"));
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
            await GameData.EventStageStartAsync(GameGlobalStates.bossFight_EventStageData);
        }

        private async void EndBattleVideo(VideoPlayer vp)
        {
            backButton.gameObject.SetActive(true);
            startBattleButton.gameObject.SetActive(true);
            battleVideo.gameObject.SetActive(false);

            await GameData.EventStageEndAsync(GameGlobalStates.bossFight_EventStageData);

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

        void Update()
        {

        }
    }
}
