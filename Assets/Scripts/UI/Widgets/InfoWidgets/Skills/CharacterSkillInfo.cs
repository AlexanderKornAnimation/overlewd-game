using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class CharacterSkillInfo : BaseSkillInfo
    {
        public int? chId { get; set; }
        private AdminBRO.Character chData => GameData.characters.GetById(chId);
        
        public string skillType { get; set; }
        private AdminBRO.CharacterSkill skillData => chData.GetSkillByType(skillType);
        
        protected override void Start()
        {
            Customize();
        }

        protected override void Customize()
        {
            icon.sprite = GetIconByType(skillType);
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
        
        public static CharacterSkillInfo GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<CharacterSkillInfo>(
                "Prefabs/UI/Widgets/InfoWidgets/CharacterSkillInfo", parent);
        }
    }
}