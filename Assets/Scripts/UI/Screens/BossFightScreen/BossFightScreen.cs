using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Overlewd
{
    public class BossFightScreen : BaseScreen
    {
        protected Button startBattleButton;
        protected Button backButton;
        protected Button skipButton;

        protected VideoPlayer battleVideo;

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

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);

            battleVideo = canvas.Find("TestVideo").GetComponent<VideoPlayer>();

            battleVideo.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }

        protected virtual async void Start()
        {
            await GameData.EventStageStartAsync(GameGlobalStates.bossFight_EventStageData);
            battleVideo.loopPointReached += EndBattleVideo;
        }

        protected virtual async void EndBattleVideo(VideoPlayer vp)
        {
            backButton.gameObject.SetActive(true);
            startBattleButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(false);
            battleVideo.gameObject.SetActive(false);

            await GameData.EventStageEndAsync(GameGlobalStates.bossFight_EventStageData);

            UIManager.ShowPopup<VictoryPopup>();
        }

        protected virtual void StartBattleButtonClick()
        {
            backButton.gameObject.SetActive(false);
            startBattleButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(true);
            battleVideo.gameObject.SetActive(true);

            battleVideo.Play();
        }

        protected virtual void SkipButtonClick()
        {
            battleVideo.Stop();
            EndBattleVideo(battleVideo);
        }
        
        protected virtual void BackButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }
    }
}
