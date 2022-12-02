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
        public class BasicAttackInfo : MonoBehaviour
        {
            protected TextMeshProUGUI skillName;
            protected Image icon;
            protected TextMeshProUGUI skillEffect;
            protected TextMeshProUGUI level;
            protected TextMeshProUGUI cooldown;
            protected TextMeshProUGUI description;
            protected GameObject levelBack;
            private GameObject notificationLock;

            protected Transform canvas;
            protected Button missclickButton;
            protected Transform background;

            public string skillType { get; set; }
            private AdminBRO.CharacterSkill skillData => GameData.characters.overlord.GetSkillByType(skillType);

            private void Awake()
            {
                UITools.SetStretch(gameObject.GetComponent<RectTransform>());
                canvas = transform.Find("Canvas");
                background = canvas.Find("Background");
                missclickButton = canvas.Find("MissclickButton").GetComponent<Button>();
                missclickButton.onClick.AddListener(MissclickButtonClick);
                
                icon = background.Find("Icon").GetComponent<Image>();
                notificationLock = icon.transform.Find("NotificationLock").gameObject;
                skillName = background.Find("SkillName").GetComponent<TextMeshProUGUI>();
                skillEffect = icon.transform.Find("SkillEffect").GetComponent<TextMeshProUGUI>();
                levelBack = icon.transform.Find("LevelBack").gameObject;
                level = levelBack.transform.Find("Level").GetComponent<TextMeshProUGUI>();
                cooldown = background.Find("CooldownBack/Cooldown").GetComponent<TextMeshProUGUI>();
                description = background.Find("Description").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (skillData != null)
                {
                    cooldown.text = $"{skillData.effectCooldownDuration} turns";
                    skillName.text = skillData.name;
                    skillEffect.text = skillData.effectSprite;
                    level.text = skillData.ToString();
                    description.text = skillData.description;
                    notificationLock.SetActive(false);
                    levelBack.SetActive(false);
                    icon.sprite =
                        Resources.Load<Sprite>("Prefabs/UI/Screens/OverlordScreen/Images/Skills/BasicAttack");
                }
            }

            protected virtual void MissclickButtonClick()
            {
                Destroy(gameObject);
            }

            public static BasicAttackInfo GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BasicAttackInfo>(
                    "Prefabs/UI/Widgets/InfoWidgets/OverlordSkillInfo", parent);
            }
        }
    }
}