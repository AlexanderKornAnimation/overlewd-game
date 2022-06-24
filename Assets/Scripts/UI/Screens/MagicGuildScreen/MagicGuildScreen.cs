using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MagicGuildScreen : BaseFullScreenParent<MagicGuildScreenInData>
    {
        private Transform activeSpell;
        private Button activeSpell_isOpen;
        private Button activeSpell_isMax;
        private GameObject activeSpell_isLocked;
        
        private Transform ultimateSpell;
        private Button ultimateSpell_isOpen;
        private Button ultimateSpell_isMax;
        private GameObject ultimateSpell_isLocked;
        
        private Transform passiveSpell1;
        private Button passiveSpell1_isOpen;
        private Button passiveSpell1_isMax;
        private GameObject passiveSpell1_isLocked;
        
        private Transform passiveSpell2;
        private Button passiveSpell2_isOpen;
        private Button passiveSpell2_isMax;
        private GameObject passiveSpell2_isLocked;
        
        private Button backButton;

        private TextMeshProUGUI buildingLevel;
        private AdminBRO.Building buildingData => GameData.buildings.magicGuild;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MagicGuildScreen/MagicGuild", transform);

            var canvas = screenInst.transform.Find("Canvas");

            activeSpell = canvas.Find("ActiveSpell");
            activeSpell_isOpen = activeSpell.Find("IsOpen").GetComponent<Button>();
            activeSpell_isMax = activeSpell.Find("IsMax").GetComponent<Button>();

            ultimateSpell = canvas.Find("UltimateSpell");
            ultimateSpell_isOpen = ultimateSpell.Find("IsOpen").GetComponent<Button>();
            ultimateSpell_isMax = ultimateSpell.Find("IsMax").GetComponent<Button>();
            ultimateSpell_isLocked = ultimateSpell.Find("IsLocked").gameObject;

            passiveSpell1 = canvas.Find("PassiveSpell1");
            passiveSpell1_isOpen = passiveSpell1.Find("IsOpen").GetComponent<Button>();
            passiveSpell1_isMax = passiveSpell1.Find("IsMax").GetComponent<Button>();
            passiveSpell1_isLocked = passiveSpell1.Find("IsLocked").gameObject;

            passiveSpell2 = canvas.Find("PassiveSpell2");
            passiveSpell2_isOpen = passiveSpell2.Find("IsOpen").GetComponent<Button>();
            passiveSpell2_isMax = passiveSpell2.Find("IsMax").GetComponent<Button>();
            passiveSpell2_isLocked = passiveSpell2.Find("IsLocked").gameObject;
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();

            buildingLevel = canvas.Find("Window").Find("BuildingLevel").GetComponent<TextMeshProUGUI>();

            backButton.onClick.AddListener(BackButtonClick);
            activeSpell_isOpen.onClick.AddListener(SpellButtonClick);
            ultimateSpell_isOpen.onClick.AddListener(SpellButtonClick);
            passiveSpell1_isOpen.onClick.AddListener(SpellButtonClick);
            passiveSpell2_isOpen.onClick.AddListener(SpellButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            buildingLevel.text = (buildingData.currentLevel + 1).ToString();
            activeSpell_isMax.gameObject.SetActive(false);

            ultimateSpell_isLocked.SetActive(buildingData.currentLevel < 1);
            ultimateSpell_isOpen.gameObject.SetActive(buildingData.currentLevel >= 1);
            ultimateSpell_isMax.gameObject.SetActive(false);
            
            passiveSpell1_isLocked.SetActive(buildingData.currentLevel < 2);
            passiveSpell1_isOpen.gameObject.SetActive(buildingData.currentLevel >= 2);
            passiveSpell1_isMax.gameObject.SetActive(false);

            passiveSpell2_isLocked.SetActive(buildingData.currentLevel < 3);
            passiveSpell2_isOpen.gameObject.SetActive(buildingData.currentLevel >= 3);
            passiveSpell2_isMax.gameObject.SetActive(false);

            await Task.CompletedTask;
        }

        private void SpellButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowPopup<SpellPopup>();
        }        
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }

    public class MagicGuildScreenInData : BaseFullScreenInData
    {

    }
}