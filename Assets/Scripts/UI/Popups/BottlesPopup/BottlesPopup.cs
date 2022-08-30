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
        private TextMeshProUGUI staminaBuyButtonTitle;
        private Button staminaMinusButton;
        private Button staminaPlusButton;
        private TextMeshProUGUI staminaCount;
        
        private Button scrollBuyButton;
        private TextMeshProUGUI scrollBuyButtonTitle;
        private Button scrollMinusButton;
        private Button scrollPlusButton;
        private TextMeshProUGUI scrollCount;

        private Button healthBuyButton;
        private TextMeshProUGUI healthBuyButtonTitle;
        private Button healthPlusButton;
        private Button healthMinusButton;
        private TextMeshProUGUI healthCount;

        private Button manaBuyButton;
        private TextMeshProUGUI manaBuyButtonTitle;
        private Button manaPlusButton;
        private Button manaMinusButton;
        private TextMeshProUGUI manaCount;

        private Transform walletWidgetPos;
        private WalletWidget walletWidget;

        private int _staminaCount => int.Parse(staminaCount.text);
        private int _scrollCount => int.Parse(scrollCount.text);
        private int _healthCount => int.Parse(healthCount.text);
        private int _manaCount => int.Parse(manaCount.text);

        private void Inc(TextMeshProUGUI value)
        {
            value.text = (int.Parse(value.text) + 1).ToString();
            CheckIncButtonsState();
            CheckBuyButtonsState();
        }
        private void Dec(TextMeshProUGUI value)
        {
            value.text = (int.Parse(value.text) - 1).ToString();
            CheckIncButtonsState();
            CheckBuyButtonsState();
        }

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
            staminaBuyButtonTitle = staminaBuyButton.GetComponentInChildren<TextMeshProUGUI>();
            staminaPlusButton = stamina.Find("ButtonPlus").GetComponent<Button>();
            staminaPlusButton.onClick.AddListener(StaminaPlusButtonClick);
            staminaMinusButton = stamina.Find("ButtonMinus").GetComponent<Button>();
            staminaMinusButton.onClick.AddListener(StaminaMinusButtonClick);
            staminaCount = stamina.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();

            scrollBuyButton = scroll.Find("BuyButton").GetComponent<Button>();
            scrollBuyButton.onClick.AddListener(ScrollBuyButtonClick);
            scrollBuyButtonTitle = scrollBuyButton.GetComponentInChildren<TextMeshProUGUI>();
            scrollMinusButton = scroll.Find("ButtonMinus").GetComponent<Button>();
            scrollMinusButton.onClick.AddListener(ScrollMinusButtonClick);
            scrollPlusButton = scroll.Find("ButtonPlus").GetComponent<Button>();
            scrollPlusButton.onClick.AddListener(ScrollPlusButtonClick);
            scrollCount = scroll.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();
            
            healthBuyButton = health.Find("BuyButton").GetComponent<Button>();
            healthBuyButton.onClick.AddListener(HealthBuyButtonClick);
            healthBuyButtonTitle = healthBuyButton.GetComponentInChildren<TextMeshProUGUI>();
            healthMinusButton = health.Find("ButtonMinus").GetComponent<Button>();
            healthMinusButton.onClick.AddListener(HealthMinusButtonClick);
            healthPlusButton = health.Find("ButtonPlus").GetComponent<Button>();
            healthPlusButton.onClick.AddListener(HealthPlusButtonClick);
            healthCount = health.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();
            
            manaBuyButton = mana.Find("BuyButton").GetComponent<Button>();
            manaBuyButton.onClick.AddListener(ManaBuyButtonClick);
            manaBuyButtonTitle = manaBuyButton.GetComponentInChildren<TextMeshProUGUI>();
            manaMinusButton = mana.Find("ButtonMinus").GetComponent<Button>();
            manaMinusButton.onClick.AddListener(ManaMinusButtonClick);
            manaPlusButton = mana.Find("ButtonPlus").GetComponent<Button>();
            manaPlusButton.onClick.AddListener(ManaPlusButtonClick);
            manaCount = mana.Find("Counter").Find("Count").GetComponent<TextMeshProUGUI>();

            walletWidgetPos = canvas.Find("WalletWidgetPos");
            
            staminaAmount = staminaCounter.Find("Stamina").Find("CounterBack").Find("Count").GetComponent<TextMeshProUGUI>();
            staminaBottleAmount = staminaCounter.Find("Bottle").Find("CounterBack").Find("Count").GetComponent<TextMeshProUGUI>();
        }

        public override async Task BeforeShowDataAsync()
        {
            await GameData.player.Get();

            await Task.CompletedTask;
        }

        public override async Task BeforeShowMakeAsync()
        {
            staminaCount.text = 1.ToString();
            scrollCount.text = 1.ToString();
            healthCount.text = 1.ToString();
            manaCount.text = 1.ToString();
            walletWidget = WalletWidget.GetInstance(walletWidgetPos);

            Refresh();
            StartCoroutine(GameData.player.UpdLocalEnergyPoints(RefreshEnergyPanel));

            await Task.CompletedTask;
        }

        private void CheckIncButtonsState()
        {
            UITools.DisableButton(staminaMinusButton, _staminaCount == 1);
            UITools.DisableButton(staminaPlusButton, _staminaCount == 10);
            UITools.DisableButton(scrollMinusButton, _scrollCount == 1);
            UITools.DisableButton(scrollPlusButton, _scrollCount == 10);
            UITools.DisableButton(healthMinusButton, _healthCount == 1);
            UITools.DisableButton(healthPlusButton, _healthCount == 10);
            UITools.DisableButton(manaMinusButton, _manaCount == 1);
            UITools.DisableButton(manaPlusButton, _manaCount == 10);
        }

        private void RefreshEnergyPanel()
        {
            refillPrice.text = $"Use bottle to get" +
                $" <size=35><sprite=\"AssetResources\" name=\"Energy\"></size>" +
                $" {GameData.potions.baseEnergyVolume}";
            staminaAmount.text = $"{GameData.player.energyPoints}/{GameData.potions.baseEnergyVolume}";
            staminaBottleAmount.text = GameData.player.energyPotionAmount.ToString();

            UITools.DisableButton(refillButton, GameData.player.energyPotionAmount < 1);
        }

        private void Refresh()
        {
            CheckIncButtonsState();
            CheckBuyButtonsState();
            RefreshEnergyPanel();
            walletWidget.Customize();
        }

        private void CheckBuyButtonsState()
        {
            var staminaPrice = UITools.PriceMul(GameData.potions.energyPrice, _staminaCount);
            staminaBuyButtonTitle.text = "Buy for " + UITools.PriceToString(staminaPrice);
            UITools.DisableButton(staminaBuyButton, !GameData.player.CanBuy(staminaPrice));

            var scrollPrice = UITools.PriceMul(GameData.potions.replayPrice, _scrollCount);
            scrollBuyButtonTitle.text = "Buy for " + UITools.PriceToString(scrollPrice);
            UITools.DisableButton(scrollBuyButton, !GameData.player.CanBuy(scrollPrice));

            var healthPrice = UITools.PriceMul(GameData.potions.hpPrice, _healthCount);
            healthBuyButtonTitle.text = "Buy for " + UITools.PriceToString(healthPrice);
            UITools.DisableButton(healthBuyButton, !GameData.player.CanBuy(healthPrice));

            var manaPrice = UITools.PriceMul(GameData.potions.manaPrice, _manaCount);
            manaBuyButtonTitle.text = "Buy for " + UITools.PriceToString(manaPrice);
            UITools.DisableButton(manaBuyButton, !GameData.player.CanBuy(manaPrice));
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
        
        private async void StaminaBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await GameData.potions.BuyEnergy(_staminaCount);
            Refresh();
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
        
        private async void ScrollBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await GameData.potions.BuyReplay(_scrollCount);
            Refresh();
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
        
        private async void HealthBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await GameData.potions.BuyHp(_healthCount);
            Refresh();
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
        
        private async void ManaBuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await GameData.potions.BuyMana(_manaCount);
            Refresh();
        }
        
        private async void RefillButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await GameData.potions.UseEnergy(1);
            RefreshEnergyPanel();
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