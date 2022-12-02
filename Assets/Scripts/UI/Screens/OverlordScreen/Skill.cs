using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSOverlordScreen
    {
        class Skill : MonoBehaviour
        {
            private Button button;
            private Image icon;
            private GameObject notificationLock;
            private GameObject levelBack;
            private TextMeshProUGUI level;

            public Transform infoWidgetPos { get; set; }
            public string skillType { get; set; }
            public AdminBRO.MagicGuildSkill skillData => GameData.buildings.magicGuild.GetSkillByType(skillType);

            private void Awake()
            {
                button = gameObject.GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                icon = gameObject.GetComponent<Image>();
                notificationLock = transform.Find("NotificationLock")?.gameObject;
                levelBack = transform.Find("LevelBack")?.gameObject;
                level = levelBack?.transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (!skillData.locked)
                {
                    if (level != null)
                    {
                        level.text = skillData.currentSkillLevel?.ToString();
                    }

                    notificationLock?.SetActive(false);
                }
                else
                {
                    notificationLock?.SetActive(true);
                    levelBack.SetActive(false);
                }

                icon.sprite = GetIcon();
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                var popup = OverlordSkillInfo.GetInstance(infoWidgetPos);
                popup.skillType = skillType;
            }

            private Sprite GetIcon()
            {
                switch (skillType)
                {
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
        }
    }
}