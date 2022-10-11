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

            public Transform infoPopupPos { get; set; }
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
                if (level != null)
                {
                    level.text = skillData.currentSkillLevel.ToString();
                }

                // levelBack?.SetActive(true);
                notificationLock?.SetActive(false);
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                var popup = SkillInfoPopup.GetInstance(infoPopupPos);
                popup.skillType = skillType;
            }
        }
    }
}