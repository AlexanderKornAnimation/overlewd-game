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
        private Transform canvas;

        private NSMagicGuildScreen.Spell activeSpell;
        private NSMagicGuildScreen.Spell ultimateSpell;
        private NSMagicGuildScreen.Spell passiveSpell1;
        private NSMagicGuildScreen.Spell passiveSpell2;

        private Button backButton;

        private TextMeshProUGUI buildingLevel;
        private AdminBRO.Building buildingData => GameData.buildings.magicGuild.meta;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MagicGuildScreen/MagicGuild", transform);

            canvas = screenInst.transform.Find("Canvas");
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            buildingLevel = canvas.Find("Window").Find("BuildingLevel").GetComponent<TextMeshProUGUI>();

            backButton.onClick.AddListener(BackButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            
            await Task.CompletedTask;
        }

        private void Customize()
        {
            activeSpell = canvas.Find("ActiveSpell").gameObject.AddComponent<NSMagicGuildScreen.Spell>();
            activeSpell.skillType = AdminBRO.MagicGuildSkill.Type_ActiveSkill;

            ultimateSpell = canvas.Find("UltimateSpell").gameObject.AddComponent<NSMagicGuildScreen.Spell>();
            ultimateSpell.skillType = AdminBRO.MagicGuildSkill.Type_UltimateSkill;
            
            passiveSpell1 = canvas.Find("PassiveSpell1").gameObject.AddComponent<NSMagicGuildScreen.Spell>();
            passiveSpell1.skillType = AdminBRO.MagicGuildSkill.Type_PassiveSkill1;
            
            passiveSpell2 = canvas.Find("PassiveSpell2").gameObject.AddComponent<NSMagicGuildScreen.Spell>();
            passiveSpell2.skillType = AdminBRO.MagicGuildSkill.Type_PassiveSkill2;

            buildingLevel.text = (buildingData.currentLevel + 1).ToString(); 
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_mages_guild);
                    break;
                case (FTUE.CHAPTER_2, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_mages_guild);
                    break;
                case (FTUE.CHAPTER_3, FTUE.DIALOGUE_1):
                    GameData.ftue.chapter3.ShowNotifByKey("ch3guildtutor3");
                    break;
                case (FTUE.CHAPTER_3, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_mages_guild);
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

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_3, FTUE.DIALOGUE_1):
                    UIManager.ShowScreen<MapScreen>();
                    break;
                default:
                    UIManager.ShowScreen<CastleScreen>();
                    break;
            }
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData?.eventId)
            {
                case GameDataEvent.EventId.MagicGuildSpellLvlUp:
                    var eData = eventData.data.As<Buildings.MagicGuild.EventData>();
                    var lvlUpSkillType = eData.skillType;
                    break;
            }
        }

        public override void OnUIEvent(UIEvent eventData)
        {
            switch (eventData?.eventId)
            {
                case UIEvent.EventId.HidePopup:
                    if (eventData.SenderTypeIs<SpellPopup>())
                    {

                    }
                    break;
            }
        }
    }

    public class MagicGuildScreenInData : BaseFullScreenInData
    {

    }
}