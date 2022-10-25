using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class BaseEquipInfo : BaseInfoWidget
    {
        protected TextMeshProUGUI equipName;
        protected Image icon;
        protected TextMeshProUGUI classIcon;
        
        protected GameObject accuracyBack;
        protected TextMeshProUGUI accuracyStat;
            
        protected GameObject damageBack;
        protected TextMeshProUGUI damageStat;
            
        protected GameObject dodgeBack;
        protected TextMeshProUGUI dodgeStat;
            
        protected GameObject healthBack;
        protected TextMeshProUGUI healthStat;
            
        protected GameObject critRateBack;
        protected TextMeshProUGUI critRateStat;
        
        public int? eqId { get; set; }
        protected AdminBRO.Equipment eqData => GameData.equipment.GetById(eqId);
        
        protected override void Awake()
        {
            base.Awake();
            equipName = background.transform.Find("EquipName").GetComponent<TextMeshProUGUI>();
            icon = background.transform.Find("Icon").GetComponent<Image>();
            classIcon = icon.transform.Find("ClassIcon").GetComponent<TextMeshProUGUI>();

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

        protected void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            if (eqData != null)
            {
                icon.sprite = ResourceManager.LoadSprite(eqData.icon);
                classIcon.text = eqData.classMarker;
                equipName.text = eqData.name;
                
                accuracyStat.text = "+" + eqData.accuracy * 100;
                accuracyBack.SetActive(eqData.accuracy > 0.0f);
                    
                dodgeStat.text = "+" + eqData.dodge * 100;
                dodgeBack.SetActive(eqData.dodge > 0.0f);

                critRateStat.text = "+" + eqData.critrate * 100;
                critRateBack.SetActive(eqData.critrate > 0.0f);
                    
                healthStat.text = "+" + eqData.health;
                healthBack.SetActive(eqData.health > 0);
                    
                damageStat.text ="+" + eqData.damage;
                damageBack.SetActive(eqData.damage > 0);
            }
        }
    }
}
