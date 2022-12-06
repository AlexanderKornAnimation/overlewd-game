using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SpellPopup : BasePopupParent<SpellPopupInData>
    {
        private List<Image> resources = new List<Image>();
        private List<TextMeshProUGUI> count = new List<TextMeshProUGUI>();

        private Transform spawnPoint;
        private Transform walletWidgetPos;
        private WalletWidget walletWidget;

        private TextMeshProUGUI spellName;
        private TextMeshProUGUI description;

        private Button crystalBuildButton;
        private TextMeshProUGUI crystalBuildButtonText;
        
        private Button buildButton;
        private Button closeButton;

        public event Action OnLvlUp;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/SpellPopup/SpellPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            spawnPoint = canvas.Find("Background").Find("ImageSpawnPoint");
            walletWidgetPos = canvas.Find("WalletWidgetPos");

            spellName = canvas.Find("SpellName").GetComponent<TextMeshProUGUI>();
            description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

            crystalBuildButton = canvas.Find("CrystalBuildButton").GetComponent<Button>();
            crystalBuildButton.onClick.AddListener(CrystalBuildButtonClick);
            crystalBuildButtonText = crystalBuildButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            
            buildButton = canvas.Find("BuildButton").GetComponent<Button>();
            buildButton.onClick.AddListener(BuildButtonClick);
            
            closeButton = canvas.Find("BackButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CloseButtonClick);
            
            var grid = canvas.Find("Grid");
            
            for (int i = 1; i <= grid.childCount; i++)
            {
                var resource = grid.Find($"Resource{i}").GetComponent<Image>();
                resources.Add(resource);
                count.Add(resource.transform.Find("Count").GetComponent<TextMeshProUGUI>());
                resource.gameObject.SetActive(false);
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            var spellData = inputData?.spellData;
            if (spellData != null)
            {
                spellName.text = spellData.current.name;
                description.text = spellData.current.description;
                for (int i = 0; i < spellData.current.levelUpPrice.Count; i++)
                {
                    resources[i].gameObject.SetActive(true);
                    var currency = GameData.currencies.GetById(spellData?.current.levelUpPrice[i].currencyId);
                    resources[i].sprite = ResourceManager.LoadSprite(currency.iconUrl);
                    count[i].text = spellData.current.levelUpPrice[i].amount.ToString();
                    count[i].color = spellData.canlvlUp ? Color.white : Color.red;
                }

                var crystalPrice = spellData.priceCrystal?.FirstOrDefault()?.amount;
                var color = spellData.canCrystallvlUp ? "white" : "red";
                crystalBuildButtonText.text = $"Summon building\nfor <color={color}>{crystalPrice}</color> crystals";
            }
            
            FireballSpell.GetInstance(spawnPoint);
            walletWidget = WalletWidget.GetInstance(walletWidgetPos);
        }

        private async void CrystalBuildButtonClick()
        {
            var spellData = inputData?.spellData;
            if (spellData != null)
            {
                if (spellData.canCrystallvlUp)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_FreeSpellLearnButton);
                    await GameData.buildings.magicGuild.SkillLvlUpCrystal(spellData.type);
                    UIManager.HidePopup();
                    OnLvlUp?.Invoke();
                }
                else
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    UIManager.ShowPopup<DeclinePopup>();
                }
            }
        }

        private async void BuildButtonClick()
        {
            var spellData = inputData?.spellData;
            if (spellData != null)
            {
                if (spellData.canlvlUp)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_FreeSpellLearnButton);
                    await GameData.buildings.magicGuild.SkillLvlUp(spellData.type);
                    UIManager.HidePopup();
                    OnLvlUp?.Invoke();
                }
                else
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    UIManager.ShowPopup<DeclinePopup>();
                }
            }
        }

        private void CloseButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }
    }

    public class SpellPopupInData : BasePopupInData
    {
        public int spellId;
        public AdminBRO.MagicGuildSkill spellData => GameData.buildings.magicGuild.GetSkillById(spellId);
    }
}