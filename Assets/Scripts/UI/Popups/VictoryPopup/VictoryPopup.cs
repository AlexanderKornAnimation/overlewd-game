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
        private List<Image> resources;

        private Button nextButton;
        private Button repeatButton;

        private int resourcesCount = 3;

        private void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Popups/VictoryPopup/VictoryPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            resources = new List<Image>(resourcesCount);

            nextButton = canvas.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);

            TakeResources(canvas);
            
            resources[0].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
            resources[1].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gem");
            resources[2].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gold");
        }

        private void TakeResources(Transform canvas)
        {
            for (int i = 1; i <= resources.Capacity; i++)
            {
                resources.Add(canvas.Find($"Reward{i}").Find("ResourceIcon").GetComponent<Image>());
            }
        }
        
        private void NextButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }

        private void RepeatButtonClick()
        {
            UIManager.ShowScreen<BattleScreen>();
        }
    }
}