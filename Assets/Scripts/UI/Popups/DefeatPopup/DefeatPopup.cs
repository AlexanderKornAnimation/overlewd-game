using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Resharper disable All

namespace Overlewd
{
    public class DefeatPopup : BasePopup
    {
        private Button magicGuildButton;
        private Button inventoryButton;
        private Button haremButton;

        void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Popups/DefeatPopup/DefeatPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            magicGuildButton = canvas.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);
            inventoryButton = canvas.Find("InventoryButton").GetComponent<Button>();
            inventoryButton.onClick.AddListener(InventoryButtonClick);
            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);
        }

        private void MagicGuildButtonClick()
        {
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        private void InventoryButtonClick()
        {
            UIManager.ShowScreen<InventoryAndUserScreen>();
        }

        private void HaremButtonClick()
        {
            UIManager.ShowScreen<HaremScreen>();
        }
    }
}