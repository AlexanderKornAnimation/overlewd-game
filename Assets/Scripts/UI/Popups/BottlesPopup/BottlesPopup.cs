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
        private Button closeButton;
        private Button refillButton;
        private TextMeshProUGUI refillPrice;
        private TextMeshProUGUI staminaAmount;
        private TextMeshProUGUI staminaBottleAmount;

        private Button staminaBuyButton;
        private Button staminaMinusButton;
        private Button staminaPlusButton;
        private TextMeshProUGUI staminaCount;
        
        private Button scrollBuyButton;
        private Button scrollMinusButton;
        private Button scrollPlusButton;
        private TextMeshProUGUI scrollCount;

        private Button healthBuyButton;
        private Button healthPlusButton;
        private Button healthMinusButton;
        private TextMeshProUGUI healthCount;

        private Button manaBuyButton;
        private Button manaPlusButton;
        private Button manaMinusButton;
        private TextMeshProUGUI manaCount;
        
        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/BottlesPopup/BottlesPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var staminaCounter = canvas.Find("StaminaCounter");
            var stamina = canvas.Find("Stamina");
            var scroll = canvas.Find("Scroll");
            var health = canvas.Find("Health");
            var mana = canvas.Find("Mana");

            closeButton = canvas.Find("CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CloseButtonClick);
            refillButton = staminaCounter.Find("RefillButton").GetComponent<Button>();
            refillButton.onClick.AddListener(RefillButtonClick);
            refillPrice = refillButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

            staminaBuyButton = stamina.Find("BuyButton").GetComponent<Button>();
            staminaBuyButton.onClick.AddListener(StaminaBuyButtonClick);
            staminaPlusButton = stamina.Find("ButtonPlus").GetComponent<Button>();
            staminaPlusButton.onClick.AddListener(StaminaPlusButtonClick);
            staminaMinusButton = stamina.Find("ButtonMinus").GetComponent<Button>();
            staminaMinusButton.onClick.AddListener(StaminaMinusButtonClick);
            staminaCount = stamina.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();

            scrollBuyButton = scroll.Find("BuyButton").GetComponent<Button>();
            scrollBuyButton.onClick.AddListener(ScrollBuyButtonClick);
            scrollMinusButton = scroll.Find("ButtonMinus").GetComponent<Button>();
            scrollMinusButton.onClick.AddListener(ScrollMinusButtonClick);
            scrollPlusButton = scroll.Find("ButtonPlus").GetComponent<Button>();
            scrollPlusButton.onClick.AddListener(ScrollPlusButtonClick);
            scrollCount = scroll.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();
            
            healthBuyButton = health.Find("BuyButton").GetComponent<Button>();
            healthBuyButton.onClick.AddListener(HealthBuyButtonClick);
            healthMinusButton = health.Find("ButtonMinus").GetComponent<Button>();
            healthMinusButton.onClick.AddListener(HealthMinusButtonClick);
            healthPlusButton = health.Find("ButtonPlus").GetComponent<Button>();
            healthPlusButton.onClick.AddListener(HealthPlusButtonClick);
            healthCount = health.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();
            
            manaBuyButton = mana.Find("BuyButton").GetComponent<Button>();
            manaBuyButton.onClick.AddListener(ManaBuyButtonClick);
            manaMinusButton = mana.Find("ButtonMinus").GetComponent<Button>();
            manaMinusButton.onClick.AddListener(ManaMinusButtonClick);
            manaPlusButton = mana.Find("ButtonPlus").GetComponent<Button>();
            manaPlusButton.onClick.AddListener(ManaPlusButtonClick);
            manaCount = mana.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();
            
            staminaAmount = staminaCounter.Find("Stamina").Find("CounterBack").Find("Count").GetComponent<TextMeshProUGUI>();
            staminaBottleAmount = staminaCounter.Find("Bottle").Find("CounterBack").Find("Count").GetComponent<TextMeshProUGUI>();
        }

        public override async Task BeforeShowMakeAsync()
        {
            staminaCount.text = 1.ToString();
            scrollCount.text = 1.ToString();
            healthCount.text = 1.ToString();
            manaCount.text = 1.ToString();

            CheckIncButtonsState();
            CheckBuyButtonsState();

            await Task.CompletedTask;
        }

        private void Inc(TextMeshProUGUI value)
        {
            value.text = (int.Parse(value.text) + 1).ToString();
            CheckIncButtonsState();
        }
        private void Dec(TextMeshProUGUI value)
        {
            value.text = (int.Parse(value.text) - 1).ToString();
            CheckIncButtonsState();
        }

        private void CheckIncButtonsState()
        {
            UITools.DisableButton(staminaMinusButton, int.Parse(staminaCount.text) == 1);
            UITools.DisableButton(staminaPlusButton, int.Parse(staminaCount.text) == 10);
            UITools.DisableButton(scrollMinusButton, int.Parse(scrollCount.text) == 1);
            UITools.DisableButton(scrollPlusButton, int.Parse(scrollCount.text) == 10);
            UITools.DisableButton(healthMinusButton, int.Parse(healthCount.text) == 1);
            UITools.DisableButton(healthPlusButton, int.Parse(healthCount.text) == 10);
            UITools.DisableButton(manaMinusButton, int.Parse(manaCount.text) == 1);
            UITools.DisableButton(manaPlusButton, int.Parse(manaCount.text) == 10);
        }

        private void CheckBuyButtonsState()
        {

        }

        private void StaminaPlusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Inc(staminaCount);
        }
        
        private void StaminaMinusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Dec(staminaCount);
        }
        
        private void StaminaBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            
        }
        
        private void ScrollPlusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Inc(scrollCount);
        }
        
        private void ScrollMinusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Dec(scrollCount);
        }
        
        private void ScrollBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);  

        }
        
        private void HealthPlusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Inc(healthCount);
        }
        
        private void HealthMinusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Dec(healthCount);
        }
        
        private void HealthBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick); 
        }
        
        private void ManaPlusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Inc(manaCount);
        }
        
        private void ManaMinusButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            Dec(manaCount);
        }
        
        private void ManaBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);  
        }
        
        private void RefillButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
        }

        private void CloseButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData.prevPopupInData.IsType<PrepareBattlePopupInData>())
            {
                UIManager.MakePopup<PrepareBattlePopup>().
                    SetData(inputData.prevPopupInData.As<PrepareBattlePopupInData>()).
                    RunShowPopupProcess();
            }
            else if (inputData.prevPopupInData.IsType<PrepareBossFightPopupInData>())
            {
                UIManager.MakePopup<PrepareBossFightPopup>().
                    SetData(inputData.prevPopupInData.As<PrepareBossFightPopupInData>()).
                    RunShowPopupProcess();
            }
        }
    }

    public class BottlesPopupInData : BasePopupInData
    {
        
    }
}