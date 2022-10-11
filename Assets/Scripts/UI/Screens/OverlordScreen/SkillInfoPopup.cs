using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSOverlordScreen
    {
        public class SkillInfoPopup : BaseInfoPopup
        {
            private Image icon;
            private TextMeshProUGUI level;
            private TextMeshProUGUI skillName;
            private TextMeshProUGUI description;
            private TextMeshProUGUI hintLevel;
            private GameObject levelBack;
            private GameObject hintLock;
            private GameObject notificationLock;
            
            public string skillType { get; set; }
            public AdminBRO.MagicGuildSkill skillData => GameData.buildings.magicGuild.GetSkillByType(skillType);
            protected override void Awake()
            {
                base.Awake();
                var background = transform.Find("Background");

                icon = background.Find("Icon").GetComponent<Image>();
                levelBack = background.Find("LevelBack").gameObject;
                level = levelBack.transform.Find("Level").GetComponent<TextMeshProUGUI>();
                skillName = background.Find("Name").GetComponent<TextMeshProUGUI>();
                description = background.Find("Description").GetComponent<TextMeshProUGUI>();
                hintLevel = background.Find("HintLevel/Title").GetComponent<TextMeshProUGUI>();
                hintLock = background.Find("HintLock").gameObject;
                notificationLock = background.Find("NotificationLock").gameObject;
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (skillData != null)
                {
                    level.text = skillData.currentSkillLevel.ToString();
                    description.text = skillData.current?.description;
                    skillName.text = skillData.current?.name;
                    hintLevel.text = $"Spell level <size=38> {skillData.currentSkillLevel}";
                    hintLock.SetActive(false);
                    notificationLock.SetActive(false);
                    // levelBack.SetActive(true);
                    
                }
            }

            public static SkillInfoPopup GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SkillInfoPopup>(
                    "Prefabs/UI/Screens/OverlordScreen/SkillPopupInfo", parent);
            }
        }
    }
}