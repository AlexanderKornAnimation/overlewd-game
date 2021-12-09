using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Resharper disable All

namespace Overlewd
{
    public class VictoryPopup : BasePopup
    {
        protected Button nextButton;
        protected Button repeatButton;

        protected Image reward1;
        protected Image reward2;
        protected Image reward3;

        void Awake()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Popups/VictoryPopup/VictoryPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            nextButton = canvas.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);

            reward1 = canvas.Find("Reward1").Find("Resource").GetComponent<Image>();
            reward2 = canvas.Find("Reward2").Find("Resource").GetComponent<Image>();
            reward3 = canvas.Find("Reward3").Find("Resource").GetComponent<Image>();
            
            reward1.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
            reward2.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gem");
            reward3.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gold");
        }

        protected override void ShowMissclick()
        {
            var missClick = UIManager.ShowPopupMissclick<PopupMissclickColored>();
            missClick.missClickEnabled = false;
        }

        protected virtual void NextButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }

        protected virtual void RepeatButtonClick()
        {
            UIManager.ShowScreen<BattleScreen>();
        }
    }
}