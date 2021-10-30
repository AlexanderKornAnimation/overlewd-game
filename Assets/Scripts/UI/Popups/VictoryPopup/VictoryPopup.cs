using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class VictoryPopup : BasePopup
    {
        private Button nextButton;
        private Button repeatButton;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/VictoryPopup/VictoryPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            nextButton = canvas.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);
        }

        private void NextButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }

        private void RepeatButtonClick()
        {

        }

        void Update()
        {

        }
    }
}
