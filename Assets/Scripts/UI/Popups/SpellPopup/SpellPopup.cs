using System;
using System.Collections;
using System.Collections.Generic;
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
        private Transform currencyBack;

        private TextMeshProUGUI spellName;
        private TextMeshProUGUI description;

        private Button crystalBuildButton;
        private Button buildButton;
        private Button closeButton;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/SpellPopup/SpellPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            spawnPoint = canvas.Find("Background").Find("ImageSpawnPoint");
            currencyBack = canvas.Find("CurrencyBack");

            spellName = canvas.Find("SpellName").GetComponent<TextMeshProUGUI>();
            description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

            crystalBuildButton = canvas.Find("CrystalBuildButton").GetComponent<Button>();
            crystalBuildButton.onClick.AddListener(CrystalBuildButtonClick);
            
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
            spellName.text = spellData?.current.name;
            description.text = spellData?.current.description;
            for (int i = 0; i < spellData?.current.levelUpPrice.Count; i++)
            {
                resources[i].gameObject.SetActive(true);
                var currency = GameData.currencies.GetById(spellData?.current.levelUpPrice[i].currencyId);
                resources[i].sprite = ResourceManager.LoadSprite(currency.icon356Url);
                count[i].text = spellData.current.levelUpPrice[i].amount.ToString();
                count[i].color = spellData.canlvlUp ? Color.white : Color.red;
            }
            
            FireballSpell.GetInstance(spawnPoint);
            UITools.FillWallet(currencyBack);
        }

        private async void CrystalBuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            var spellData = inputData?.spellData;
            if (spellData != null && spellData.canlvlUp)
            {
                await GameData.buildings.MagicGuildSkillLvlUp(spellData.type);
            }
            UIManager.HidePopup();
        }

        private async void BuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_FreeSpellLearnButton);
            var spellData = inputData?.spellData;
            if (spellData != null && spellData.canlvlUp)
            {
                await GameData.buildings.MagicGuildSkillLvlUp(spellData.type);
            }
            UIManager.HidePopup();
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
        public AdminBRO.MagicGuildSkill spellData => GameData.buildings.GetMagicGuildSkillById(spellId);
    }
}