using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class MagicGuildScreen : BaseScreen
    {
        private Button activeSpell;
        private Button ultimateSpell;
        private Button passiveSpell_1;
        private Button passiveSpell_2;
        private Button backButton;

        private Image spellUnknown;
       
        private Text mainTitle;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MagicGuildScreen/MagicGuild", transform);

            var canvas = screenInst.transform.Find("Canvas");

            activeSpell = canvas.Find("ActiveSpell").GetComponent<Button>();
            ultimateSpell = canvas.Find("UltimateSpell").GetComponent<Button>();
            passiveSpell_1 = canvas.Find("PassiveSpell1").GetComponent<Button>();
            passiveSpell_2 = canvas.Find("PassiveSpell2").GetComponent<Button>();
            backButton = canvas.Find("BackButton").GetComponent<Button>();

            mainTitle = canvas.Find("Window").Find("MainTitle").GetComponent<Text>();

            backButton.onClick.AddListener(BackButtonClick);
            activeSpell.onClick.AddListener(ActiveSpellButtonClick);
        }

        private void ActiveSpellButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowPopup<SpellPopup>();
        }        
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }
}