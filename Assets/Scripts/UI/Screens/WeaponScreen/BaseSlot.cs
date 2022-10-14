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

            protected void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }
        }
    }
}