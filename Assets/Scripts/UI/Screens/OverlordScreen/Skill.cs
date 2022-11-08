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
                            return Resources.Load<Sprite>(
                                "Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill1");
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

                            return Resources.Load<Sprite>(
                                "Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill2");
                        }
                        else
                        {
                            return Resources.Load<Sprite>(
                                "Prefabs/UI/Screens/OverlordScreen/Images/Skills/PassiveSkill2_Locked");
                        }
                }

                return null;
            }
        }
    }
}