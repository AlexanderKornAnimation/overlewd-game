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

        private Image spellImage;
        private Transform walletWidgetPos;
        private WalletWidget walletWidget;

        private TextMeshProUGUI spellName;
        private TextMeshProUGUI description;

        private Button buildButton;
        private TextMeshProUGUI buildButtonTitle;
        
        private Button closeButton;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/SpellPopup/SpellPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            spellImage = canvas.Find("Background/SpellImage").GetComponent<Image>();
            walletWidgetPos = canvas.Find("WalletWidgetPos");

            spellName = canvas.Find("SpellName").GetComponent<TextMeshProUGUI>();
            description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

            buildButton = canvas.Find("BuildButton").GetComponent<Button>();
            buildButton.onClick.AddListener(CrystalBuildButtonClick);
            buildButtonTitle = buildButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            
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
                buildButtonTitle.text = $"Learn on your own\nfor <color={color}>{crystalPrice}</color> crystals";
            }

            spellImage.sprite = GetSpellImageByType(spellData?.type);
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
                }
                else
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    UIManager.ShowPopup<DeclinePopup>();
                }
            }
        }

        private Sprite GetSpellImageByType(string spellType)
        {
            return spellType switch
            {
                AdminBRO.MagicGuildSkill.Type_ActiveSkill => ResourceManager.InstantiateAsset<Sprite>("Prefabs/UI/Popups/SpellPopup/Images/ActiveSpellStep1"),
                AdminBRO.MagicGuildSkill.Type_UltimateSkill => ResourceManager.InstantiateAsset<Sprite>("Prefabs/UI/Popups/SpellPopup/Images/UltimateSpellStep1"),
                AdminBRO.MagicGuildSkill.Type_PassiveSkill1 => ResourceManager.InstantiateAsset<Sprite>("Prefabs/UI/Popups/SpellPopup/Images/PassiveSpell1"),
                AdminBRO.MagicGuildSkill.Type_PassiveSkill2 => ResourceManager.InstantiateAsset<Sprite>("Prefabs/UI/Popups/SpellPopup/Images/PassiveSpell2"),
                _ => null
            };
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