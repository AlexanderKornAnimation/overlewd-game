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
        private GameObject activeSpell_GO;
        private GameObject ultimateSpell_GO;
        private GameObject passiveSpell1_GO;
        private GameObject passiveSpell2_GO;

        private NSMagicGuildScreen.Spell activeSpell;
        private NSMagicGuildScreen.Spell ultimateSpell;
        private NSMagicGuildScreen.Spell passiveSpell1;
        private NSMagicGuildScreen.Spell passiveSpell2;

        private Button backButton;

        private TextMeshProUGUI buildingLevel;
        private AdminBRO.Building buildingData => GameData.buildings.magicGuild;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MagicGuildScreen/MagicGuild", transform);

            var canvas = screenInst.transform.Find("Canvas");

            activeSpell_GO = canvas.Find("ActiveSpell").gameObject;
            ultimateSpell_GO = canvas.Find("UltimateSpell").gameObject;
            passiveSpell1_GO = canvas.Find("PassiveSpell1").gameObject;
            passiveSpell2_GO = canvas.Find("PassiveSpell2").gameObject;
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            buildingLevel = canvas.Find("Window").Find("BuildingLevel").GetComponent<TextMeshProUGUI>();

            backButton.onClick.AddListener(BackButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            activeSpell = activeSpell_GO.AddComponent<NSMagicGuildScreen.Spell>();
            activeSpell.skillType = AdminBRO.MagicGuildSkill.Type_ActiveSkill;

            if (buildingData.currentLevel.HasValue)
            {
                if (buildingData.currentLevel.Value >= 1)
                {
                    ultimateSpell = ultimateSpell_GO.AddComponent<NSMagicGuildScreen.Spell>();
                    ultimateSpell.skillType = AdminBRO.MagicGuildSkill.Type_UltimateSkill;
                }
                if (buildingData.currentLevel.Value >= 2)
                {
                    passiveSpell1 = passiveSpell1_GO.AddComponent<NSMagicGuildScreen.Spell>();
                    passiveSpell1.skillType = AdminBRO.MagicGuildSkill.Type_PassiveSkill1;
                    passiveSpell2 = passiveSpell2_GO.AddComponent<NSMagicGuildScreen.Spell>();
                    passiveSpell2.skillType = AdminBRO.MagicGuildSkill.Type_PassiveSkill2;
                }
            }

            buildingLevel.text = (buildingData.currentLevel + 1).ToString();

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedState)
            {
                case (_, _):
                    switch (GameData.ftue.activeChapter.key)
                    {
                        case "chapter1":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_mages_guild);
                            break;
                        case "chapter2":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_mages_guild);
                            break;
                        case "chapter3":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_mages_guild);
                            break;
                    }
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task BeforeShowAsync()
        {
            SoundManager.GetEventInstance(FMODEventPath.Castle_Screen_BGM_Attn);
            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();
            await Task.CompletedTask;
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.type)
            {
                case GameDataEvent.Type.MagicGuildSpellLvlUp:
                    activeSpell?.Customize();
                    ultimateSpell?.Customize();
                    passiveSpell1?.Customize();
                    passiveSpell2?.Customize();
                    break;
            }
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