using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
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
                var screenPrefab =
                    (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/PortalScreenFTUE"));
                var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
                screenRectTransform.SetParent(transform, false);
                UIManager.SetStretch(screenRectTransform);

                var canvas = screenRectTransform.Find("Canvas");

                nextButton = canvas.Find("NextButton").GetComponent<Button>();
                nextButton.onClick.AddListener(NextButtonClick);

                video = canvas.Find("Video").GetComponent<VideoPlayer>();
            }

            private void Start()
            {
                nextButton.gameObject.SetActive(false);
                video.Play();
                StartCoroutine(ShowButton());
            }
            
            private IEnumerator ShowButton()
            {
                yield return new WaitForSeconds(10.5f);
                nextButton.gameObject.SetActive(true);

                GameGlobalStates.dialogNotification_DialogId = 10;
                UIManager.ShowNotification<DialogNotification>();
            }
            
            private void NextButtonClick()
            {
                GameGlobalStates.sexScreen_StageId = 15;
                GameGlobalStates.sexScreen_DialogId = 3;
                UIManager.ShowScreen<SexScreen>();
            }
        }
    }
}