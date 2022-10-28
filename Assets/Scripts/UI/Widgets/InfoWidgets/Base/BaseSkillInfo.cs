using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class BaseSkillInfo : BaseInfoWidget
    {
        protected TextMeshProUGUI skillName;
        protected Image icon;
        protected TextMeshProUGUI skillEffect;
        protected TextMeshProUGUI level;
        protected TextMeshProUGUI cooldown;
        protected TextMeshProUGUI description;

        public int? chId { get; set; }
        protected AdminBRO.Character chData => GameData.characters.GetById(chId);
        
        public string skilltype { get; set; }
        protected AdminBRO.CharacterSkill skillData => chId == GameData.characters.overlord.id
            ? GameData.buildings.magicGuild.GetSkillByType(skilltype).current : chData.GetSkillByType(skilltype);
        
        protected override void Awake()
        {
            base.Awake();
            skillName = background.Find("SkillName").GetComponent<TextMeshProUGUI>();
            icon = background.Find("Icon").GetComponent<Image>();
            skillEffect = icon.transform.Find("SkillEffect").GetComponent<TextMeshProUGUI>();
            level = icon.transform.Find("LevelBack/Level").GetComponent<TextMeshProUGUI>();
            cooldown = background.Find("CooldownBack/Cooldown").GetComponent<TextMeshProUGUI>();
            description = background.Find("Description").GetComponent<TextMeshProUGUI>();
        }

        protected virtual void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            icon.sprite = GetIconByType(skilltype);
            cooldown.text = $"{skillData?.effectCooldownDuration} turns";
            skillName.text = skillData?.name;
            skillEffect.text = skillData?.effectSprite;
            level.text = skillData?.level.ToString();
            description.text = skillData?.description;
        }

        protected Sprite GetIconByType(string type) => type switch
        {
            AdminBRO.CharacterSkill.Type_Attack => Resources.Load<Sprite>(
                "Prefabs/UI/Screens/BattleGirlScreen/Images/Skills/StandartAttackBattleGirlsButton"),
            AdminBRO.CharacterSkill.Type_Enhanced => Resources.Load<Sprite>(
                "Prefabs/UI/Screens/BattleGirlScreen/Images/Skills/MagicSkillButton2"),
            AdminBRO.CharacterSkill.Type_Passive => Resources.Load<Sprite>(
                "Prefabs/UI/Screens/BattleGirlScreen/Images/Skills/MagicSkillButton1"),
            _ => null
        };
    }
}
