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
                if (!skillData.locked)
                {
                    cooldown.text = $"{skillData.current.effectCooldownDuration} turns";
                    skillName.text = skillData.current.name;
                    skillEffect.text = skillData.current.effectSprite;
                    level.text = skillData.currentSkillLevel.ToString();
                    description.text = skillData.current.description;
                }

                icon.sprite = GetIcon();
                notificationLock.SetActive(skillData.locked);
                levelBack.SetActive(skillType != AdminBRO.MagicGuildSkill.Type_Attack || !skillData.locked);
            }
        }

        private Sprite GetIcon()
        {
            switch (skillType)
            {
                case AdminBRO.MagicGuildSkill.Type_Attack:
                    return Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/BasicAttack");
                case AdminBRO.MagicGuildSkill.Type_ActiveSkill:
                    if (!skillData.locked)
                    {
                        if (skillData.next == null)
                        {
                            return Resources.Load<Sprite>(
                                "Prefabs/UI/Screens/OverlordScreen/Images/Skills/ActiveSpellStep2");
                        }

                        return Resources.Load<Sprite>(
                            "Prefabs/UI/Screens/OverlordScreen/Images/Skills/ActiveSpellStep1");
                    }
                    else
                    {
                        return Resources.Load<Sprite>(
                            "Prefabs/UI/Screens/OverlordScreen/Images/Skills/ActiveSpellStep1_Locked");
                    }
                case AdminBRO.MagicGuildSkill.Type_UltimateSkill:
                    if (!skillData.locked)
                    {
                        return Resources.Load<Sprite>(
                            "Prefabs/UI/Screens/OverlordScreen/Images/Skills/UltimateSpellStep1");
                    }
                    else
                    {
                        return Resources.Load<Sprite>(
                            "Prefabs/UI/Screens/OverlordScreen/Images/Skills/UltimateSpellStep1_Locked");
                    }
                case AdminBRO.MagicGuildSkill.Type_PassiveSkill1:
                    if (!skillData.locked)
                    {
                        return Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill1");
                    }
                    else
                    {
                        return Resources.Load<Sprite>(
                            "Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill1_Locked");
                    }
                case AdminBRO.MagicGuildSkill.Type_PassiveSkill2:
                    if (!skillData.locked)
                    {
                        if (skillData.next == null)
                        {
                            return Resources.Load<Sprite>(
                                "Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill2_Upgrade");
                        }

                        return Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill2");
                    }
                    else
                    {
                        return Resources.Load<Sprite>(
                            "Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill2_Locked");
                    }
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