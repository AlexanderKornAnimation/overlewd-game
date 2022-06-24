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
            crystalBuildButton.onClick.AddListener(PaidBuildButtonClick);
            
            buildButton = canvas.Find("BuildButton").GetComponent<Button>();
            buildButton.onClick.AddListener(FreeBuildButtonClick);
            
            closeButton = canvas.Find("BackButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CloseButtonClick);
            
            var grid = canvas.Find("Grid");
            
            for (int i = 1; i <= grid.childCount; i++)
            {
                var resource = grid.Find($"Resource{i}").GetComponent<Image>();
                resources.Add(resource);
                count.Add(resource.transform.Find("Count").GetComponent<TextMeshProUGUI>());
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            FireballSpell.GetInstance(spawnPoint);
            UITools.FillWallet(currencyBack);
        }

        private void PaidBuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        private void FreeBuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_FreeSpellLearnButton);
            UIManager.ShowScreen<CastleScreen>();
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

    }
}