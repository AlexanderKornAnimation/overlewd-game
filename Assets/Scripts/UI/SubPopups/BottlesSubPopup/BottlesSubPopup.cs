using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BottlesSubPopup : BaseSubPopupParent<BottlesSubPopupInData>
    {
        private Button backButton;

        private Button staminaBuyButton;
        private Text staminaValue;
        private Button manaBuyButton;
        private Text manaValue;
        private Button healthBuyButton;
        private Text healthValue;

        private Button staminaBottleButton;
        private Button manaBottleButton;
        private Button healthBottleButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/SubPopups/BottlesSubPopup/BottlesSubPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var recourcePanel = canvas.Find("RecourcePanel");

            var stamina = recourcePanel.Find("Stamina");
            staminaBuyButton = stamina.Find("BuyButton").GetComponent<Button>();
            staminaBuyButton.onClick.AddListener(StaminaBuyButtonClick);
            staminaValue = stamina.Find("Value").GetComponent<Text>();

            var mana = recourcePanel.Find("Mana");
            manaBuyButton = mana.Find("BuyButton").GetComponent<Button>();
            manaBuyButton.onClick.AddListener(ManaBuyButtonClick);
            manaValue = mana.Find("Value").GetComponent<Text>();

            var health = recourcePanel.Find("Health");
            healthBuyButton = health.Find("BuyButton").GetComponent<Button>();
            healthBuyButton.onClick.AddListener(HealthBuyButton);
            healthValue = health.Find("Value").GetComponent<Text>();

            staminaBottleButton = canvas.Find("StaminaBottleButton").GetComponent<Button>();
            staminaBottleButton.onClick.AddListener(StaminaBottleButtonClick);

            manaBottleButton = canvas.Find("ManaBottleButton").GetComponent<Button>();
            manaBottleButton.onClick.AddListener(ManaBottleButtonClick);

            healthBottleButton = canvas.Find("HealthBottleButton").GetComponent<Button>();
            healthBottleButton.onClick.AddListener(HealthBottleButtonClick);

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
        }

        private void StaminaBuyButtonClick()
        {

        }

        private void ManaBuyButtonClick()
        {

        }

        private void HealthBuyButton()
        {

        }

        private void StaminaBottleButtonClick()
        {

        }

        private void ManaBottleButtonClick()
        {

        }

        private void HealthBottleButtonClick()
        {

        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenBottomShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenBottomHide>();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideSubPopup();
        }
    }

    public class BottlesSubPopupInData : BaseSubPopupInData
    {

    }
}
