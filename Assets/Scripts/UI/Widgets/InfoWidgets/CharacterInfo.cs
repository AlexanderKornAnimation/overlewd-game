using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharacterInfo : BaseInfoWidget
    {
        private TextMeshProUGUI charName;
        private Image icon;
        private TextMeshProUGUI classIcon;
        
        private GameObject accuracyBack;
        private TextMeshProUGUI accuracyStat;
            
        private GameObject damageBack;
        private TextMeshProUGUI damageStat;
            
        private GameObject dodgeBack;
        private TextMeshProUGUI dodgeStat;
            
        private GameObject healthBack;
        private TextMeshProUGUI healthStat;
            
        private GameObject critRateBack;
        private TextMeshProUGUI critRateStat;
        
        private GameObject speedBack;
        private TextMeshProUGUI speedStat;

        private GameObject powerBack;
        private TextMeshProUGUI powerStat;

        private GameObject constitutionBack;
        private TextMeshProUGUI constitutionStat;

        private GameObject agilityBack;
        private TextMeshProUGUI agilityStat;
        
        public AdminBRO.Character chData { get; set; }

        protected override void Awake()
        {
            base.Awake();
            
            charName = background.transform.Find("CharName").GetComponent<TextMeshProUGUI>();
            icon = background.transform.Find("Icon").GetComponent<Image>();
            classIcon = icon.transform.Find("ClassIcon").GetComponent<TextMeshProUGUI>();
            
            speedBack = background.Find("Speed").gameObject;
            speedStat = speedBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
            
            powerBack = background.Find("Power").gameObject;
            powerStat = powerBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
            
            constitutionBack = background.Find("Constitution").gameObject;
            constitutionStat = constitutionBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
            
            agilityBack = background.Find("Agility").gameObject;
            agilityStat = agilityBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

            accuracyBack = background.Find("Accuracy").gameObject;
            accuracyStat = accuracyBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
            damageBack = background.Find("Damage").gameObject;
            damageStat = damageBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
            dodgeBack = background.Find("Dodge").gameObject;
            dodgeStat = dodgeBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
            healthBack = background.Find("Health").gameObject;
            healthStat = healthBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
            critRateBack = background.Find("Critrate").gameObject;
            critRateStat = critRateBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
        }

        protected virtual void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            if (chData != null)
            {
                icon.sprite = ResourceManager.LoadSprite(chData.iconUrl);
                classIcon.text = chData.classMarker;
                charName.text = chData.name;
                
                speedStat.text = "+" + chData.speed;
                speedBack.SetActive(chData.speed != 0);

                powerStat.text = "+" + chData.power;
                powerBack.SetActive(chData.power != 0);

                constitutionStat.text = "+" + chData.constitution;
                constitutionBack.SetActive(chData.constitution != 0.0f);

                agilityStat.text = "+" + chData.agility;
                agilityBack.SetActive(chData.agility != 0.0f);
                
                accuracyStat.text = "+" + chData.accuracy * 100;
                accuracyBack.SetActive(chData.accuracy > 0.0f);
                    
                dodgeStat.text = "+" + chData.dodge * 100;
                dodgeBack.SetActive(chData.dodge > 0.0f);

                critRateStat.text = "+" + chData.critrate * 100;
                critRateBack.SetActive(chData.critrate > 0.0f);
                    
                healthStat.text = "+" + chData.health;
                healthBack.SetActive(chData.health > 0);
                    
                damageStat.text ="+" + chData.damage;
                damageBack.SetActive(chData.damage > 0);
            }
        }

        public static CharacterInfo GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<CharacterInfo>(
                "Prefabs/UI/Widgets/InfoWidgets/CharacterInfo", parent);
        }
    }
}
