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
        protected Button magicGuildButton;
        protected Button inventoryButton;
        protected Button haremButton;

        void Awake()
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

        protected override void ShowMissclick()
        {
            var missClick = UIManager.ShowPopupMissclick<PopupMissclickColored>();
            missClick.missClickEnabled = false;
        }

        protected virtual void MagicGuildButtonClick()
        {
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        protected virtual void InventoryButtonClick()
        {
            UIManager.ShowScreen<InventoryAndUserScreen>();
        }

        protected virtual void HaremButtonClick()
        {
            UIManager.ShowScreen<HaremScreen>();
        }
    }
}