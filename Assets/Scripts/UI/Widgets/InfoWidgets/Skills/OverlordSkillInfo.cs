using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class OverlordSkillInfo : BaseSkillInfo
    {
        private GameObject notificationLock;

        public string skillType { get; set; }
        private AdminBRO.MagicGuildSkill skillData => GameData.buildings.magicGuild.GetSkillByType(skillType);

        protected override void Awake()
        {
            base.Awake();
            notificationLock = icon.transform.Find("NotificationLock").gameObject;
        }

        protected override void Customize()
        {
            if (skillData != null)
            {
                cooldown.text = $"{skillData.current.effectCooldownDuration} turns";
                skillName.text = skillData.current.name;
                skillEffect.text = skillData.current.effectSprite;
                level.text = skillData.currentSkillLevel.ToString();
                description.text = skillData.current.description;
                notificationLock.SetActive(skillData.locked);
                levelBack.SetActive(!skillData.locked && skillType != AdminBRO.MagicGuildSkill.Type_Attack);

                icon.sprite = GetIcon();
            }
        }

        private Sprite GetIcon()
        {
            switch (skillType)
            {
                case AdminBRO.MagicGuildSkill.Type_Attack:
                    return Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/BasicAttack");
                case AdminBRO.MagicGuildSkill.Type_ActiveSkill:
                    return skillData.locked
                        ? Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/ActiveSpellStep1_Locked")
                        : Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/ActiveSpellStep1");
                case AdminBRO.MagicGuildSkill.Type_UltimateSkill:
                    return skillData.locked
                        ? Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/UltimateSpellStep1_Locked")
                        : Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/UltimateSpellStep1");
                case AdminBRO.MagicGuildSkill.Type_PassiveSkill1:
                    return skillData.locked
                        ? Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill1_Locked")
                        : Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill1");
                case AdminBRO.MagicGuildSkill.Type_PassiveSkill2:
                    return skillData.locked
                        ? Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill2_Locked")
                        : Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill2");
            }

            return null;
        }

        public static OverlordSkillInfo GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<OverlordSkillInfo>(
                "Prefabs/UI/Widgets/InfoWidgets/OverlordSkillInfo", parent);
        }
    }
}