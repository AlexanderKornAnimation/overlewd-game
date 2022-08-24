using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BottlesPopup : BasePopupParent<BottlesPopupInData>
    {
        private GameObject staminaGO;
        private GameObject scrollGO;
        private GameObject healthGO;
        private GameObject manaGO;

        private NSBottlesPopup.Item stamina;
        private NSBottlesPopup.Item scroll;
        private NSBottlesPopup.Item health;
        private NSBottlesPopup.Item mana;

        private Button closeButton;
        private Button refillButton;
        private TextMeshProUGUI refillPrice;
        private TextMeshProUGUI staminaCount;
        private TextMeshProUGUI staminaBottleCount;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/BottlesPopup/BottlesPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var staminaCounter = canvas.Find("StaminaCounter");

            staminaGO = canvas.Find("Stamina").gameObject;
            scrollGO = canvas.Find("Scrolls").gameObject;
            healthGO = canvas.Find("Health").gameObject;
            manaGO = canvas.Find("Mana").gameObject;

            closeButton = canvas.Find("CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CloseButtonClick);
            refillButton = staminaCounter.Find("RefillButton").GetComponent<Button>();
            refillButton.onClick.AddListener(RefillButtonClick);
            refillPrice = refillButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

            var staminaTr = staminaCounter.Find("Stamina");
            var bottle = staminaCounter.Find("Bottle");
            staminaCount = staminaTr.Find("CounterBack").Find("Count").GetComponent<TextMeshProUGUI>();
            staminaBottleCount = bottle.Find("CounterBack").Find("Count").GetComponent<TextMeshProUGUI>();
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            stamina = staminaGO.AddComponent<NSBottlesPopup.Item>();
            scroll = scrollGO.AddComponent<NSBottlesPopup.Item>();
            health = healthGO.AddComponent<NSBottlesPopup.Item>();
            mana = manaGO.AddComponent<NSBottlesPopup.Item>();
        }
        
        private void RefillButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }

        private void CloseButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }
    }

    public class BottlesPopupInData : BasePopupInData
    {
        
    }
}