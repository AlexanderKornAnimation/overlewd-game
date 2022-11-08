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
        protected GameObject levelBack;
        
        protected override void Awake()
        {
            base.Awake();
            skillName = background.Find("SkillName").GetComponent<TextMeshProUGUI>();
            icon = background.Find("Icon").GetComponent<Image>();
            skillEffect = icon.transform.Find("SkillEffect").GetComponent<TextMeshProUGUI>();
            levelBack = icon.transform.Find("LevelBack").gameObject;
            level = levelBack.transform.Find("Level").GetComponent<TextMeshProUGUI>();
            cooldown = background.Find("CooldownBack/Cooldown").GetComponent<TextMeshProUGUI>();
            description = background.Find("Description").GetComponent<TextMeshProUGUI>();
        }

        protected virtual void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {

        }
    }
}
