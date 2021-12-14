using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Video;

namespace Overlewd
{
    namespace FTUE
    {
        public class PortalScreen : Overlewd.BaseScreen
        {
            private Button nextButton;
            private VideoPlayer video;

            void Awake()
            {
                var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/PortalScreenFTUE"));
                var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
                screenRectTransform.SetParent(transform, false);
                UIManager.SetStretch(screenRectTransform);

                var canvas = screenRectTransform.Find("Canvas");

                nextButton = canvas.Find("NextButton").GetComponent<Button>();
                nextButton.onClick.AddListener(NextButtonClick);

                video = canvas.Find("Video").GetComponent<VideoPlayer>();
            }

            void Start()
            {
                nextButton.gameObject.SetActive(false);
                video.loopPointReached += EndVideo;
                video.Play();
            }

            private void EndVideo(VideoPlayer vp)
            {
                nextButton.gameObject.SetActive(true);
            }

            private void NextButtonClick()
            {
                UIManager.ShowScreen<StartingScreen>();
            }
        }
    }
}
