using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DeclinePopup : BasePopup
    {
        private Button mapButton;
        private Button marketButton;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/DeclinePopup/DeclinePopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            mapButton = canvas.Find("MapButton").GetComponent<Button>();
            // mapButton.onClick.AddListener(MapButtonClick);
            
            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            // marketButton.onClick.AddListener(MarketButtonClick);
        }

        private void MapButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MapScreen>();
        }
        
        private void MarketButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MarketScreen>();
        }
    }
}