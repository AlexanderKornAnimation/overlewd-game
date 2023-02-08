using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSWeaponScreen
    {
        public abstract class BaseSlot : MonoBehaviour
        {
            protected GameObject slotFull;
            protected Image icon;
            protected GameObject slotEmptyHint;
            protected TextMeshProUGUI equipName;

            protected GameObject speedBack;
            protected TextMeshProUGUI speedStat;

            protected GameObject powerBack;
            protected TextMeshProUGUI powerStat;

            protected GameObject constitutionBack;
            protected TextMeshProUGUI constitutionStat;

            protected GameObject agilityBack;
            protected TextMeshProUGUI agilityStat;

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

            public int? chId { get; set; }
            public int? equipId { get; set; }
            public AdminBRO.Equipment equipData => GameData.equipment.GetById(equipId);
            
            protected virtual void Awake()
            {
                slotFull = transform.Find("SlotFull").gameObject;
                slotEmptyHint = transform.Find("SlotEmptyHint").gameObject;
                equipName = slotFull.transform.Find("EquipName").GetComponent<TextMeshProUGUI>();
                icon = slotFull.transform.Find("WeaponIcon").GetComponent<Image>();

                var stats = slotFull.transform.Find("Stats");

                speedBack = stats.Find("Speed").gameObject;
                speedStat = speedBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
                powerBack = stats.Find("Power").gameObject;
                powerStat = powerBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
                constitutionBack = stats.Find("Constitution").gameObject;
                constitutionStat = constitutionBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
                agilityBack = stats.Find("Agility").gameObject;
                agilityStat = agilityBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
                accuracyBack = stats.Find("Accuracy").gameObject;
                accuracyStat = accuracyBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
                damageBack = stats.Find("Damage").gameObject;
                damageStat = damageBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
                dodgeBack = stats.Find("Dodge").gameObject;
                dodgeStat = dodgeBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
                healthBack = stats.Find("Health").gameObject;
                healthStat = healthBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                
                critRateBack = stats.Find("Critrate").gameObject;
                critRateStat = critRateBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
            }

            public virtual void Customize()
            {
                if (equipData != null)
                {
                    icon.sprite = ResourceManager.LoadSprite(equipData.icon);
                    equipName.text = equipData.name;

                    speedStat.text = equipData.speed.ToString();
                    speedBack.SetActive(equipData.speed > 0.0f);
                    
                    powerStat.text = equipData.power.ToString();
                    powerBack.SetActive(equipData.power > 0.0f);
                    
                    constitutionStat.text = equipData.constitution.ToString();
                    constitutionBack.SetActive(equipData.constitution > 0.0f);
                    
                    agilityStat.text = equipData.agility.ToString();
                    agilityBack.SetActive(equipData.agility > 0.0f);
                    
                    accuracyStat.text = (equipData.accuracy * 100).ToString();
                    accuracyBack.SetActive(equipData.accuracy > 0.0f);
                    
                    dodgeStat.text = (equipData.dodge * 100).ToString();
                    dodgeBack.SetActive(equipData.dodge > 0.0f);
                    
                    critRateStat.text = (equipData.critrate * 100).ToString();
                    critRateBack.SetActive(equipData.critrate > 0.0f);
                    
                    healthStat.text = equipData.health.ToString();
                    healthBack.SetActive(equipData.health > 0);
                    
                    damageStat.text = equipData.damage.ToString();
                    damageBack.SetActive(equipData.damage > 0);
                }
            }

            public virtual void Show()
            {
                slotEmptyHint.SetActive(false);
                slotFull.SetActive(true);
            }
            
            public virtual void Hide()
            {
                slotEmptyHint.SetActive(true);
                slotFull.SetActive(false);
            }
        }
    }
}