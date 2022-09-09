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
        public class EquipInfoPopup : MonoBehaviour
        {
            private Image equippedItemIcon;
            private TextMeshProUGUI equippedItemSpeed;
            private TextMeshProUGUI equippedItemPower;
            private TextMeshProUGUI equippedItemCritRate;
            private TextMeshProUGUI equippedItemDamage;
            private Image equippedItemEffectRarity;
            private TextMeshProUGUI equippedItemEffect;
            private TextMeshProUGUI equippedItemBuff;

            private GameObject selectedItem;
            private Image selectedItemIcon;
            private TextMeshProUGUI selectedItemSpeed;
            private TextMeshProUGUI selectedItemSpeedArrow;
            private TextMeshProUGUI selectedItemPower;
            private TextMeshProUGUI selectedItemPowerArrow;
            private TextMeshProUGUI selectedItemCritRate;
            private TextMeshProUGUI selectedItemCritRateArrow;
            private TextMeshProUGUI selectedItemDamage;
            private TextMeshProUGUI selectedItemDamageArrow;
            private Image selectedItemEffectRarity;
            private TextMeshProUGUI selectedItemEffect;
            private TextMeshProUGUI selectedItemBuff;
            private Button equipButton;

            public int equipId;
            public AdminBRO.Equipment equipData => GameData.equipment.GetById(equipId);

            public int? newEquipId;
            private AdminBRO.Equipment selectedEquipData => GameData.equipment.GetById(newEquipId);

            public event Action<int, int> OnEquip;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                var equippedItem = canvas.Find("EquippedItem");
                var equippedStats = equippedItem.Find("Stats");

                equippedItemIcon = equippedItem.Find("EquipIcon").GetComponent<Image>();
                equippedItemSpeed = equippedStats.Find("Speed").Find("Stat").GetComponent<TextMeshProUGUI>();
                equippedItemPower = equippedStats.Find("Power").Find("Stat").GetComponent<TextMeshProUGUI>();
                equippedItemCritRate = equippedStats.Find("CritRate").Find("Stat").GetComponent<TextMeshProUGUI>();
                equippedItemDamage = equippedStats.Find("Damage").Find("Stat").GetComponent<TextMeshProUGUI>();
                equippedItemEffectRarity = equippedItem.Find("EffectRarity").GetComponent<Image>();
                equippedItemEffect = equippedItemEffectRarity.transform.Find("Effect").GetComponent<TextMeshProUGUI>();
                equippedItemBuff = equippedItemEffectRarity.transform.Find("BuffIcon").GetComponent<TextMeshProUGUI>();

                selectedItem = canvas.Find("NewItem").gameObject;
                var newItemStats = selectedItem.transform.Find("Stats");
                selectedItemIcon = selectedItem.transform.Find("EquipIcon").GetComponent<Image>();
                
                selectedItemSpeed = newItemStats.Find("Speed").Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedItemSpeedArrow = selectedItemSpeed.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();
                
                selectedItemPower = newItemStats.Find("Power").Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedItemPowerArrow = selectedItemPower.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();
                
                selectedItemCritRate = newItemStats.Find("CritRate").Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedItemCritRateArrow = selectedItemCritRate.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();
                
                selectedItemDamage = newItemStats.Find("Damage").Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedItemDamageArrow = selectedItemDamage.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();
                
                selectedItemEffectRarity = selectedItem.transform.Find("EffectRarity").GetComponent<Image>();
                selectedItemEffect = selectedItemEffectRarity.transform.Find("Effect").GetComponent<TextMeshProUGUI>();
                selectedItemBuff = selectedItemEffectRarity.transform.Find("BuffIcon").GetComponent<TextMeshProUGUI>();
                
                equipButton = selectedItem.transform.Find("EquipButton").GetComponent<Button>();
                equipButton.onClick.AddListener(EquipButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                selectedItem.SetActive(newEquipId.HasValue);

                equippedItemIcon.sprite = ResourceManager.LoadSprite(equipData.icon);
                equippedItemSpeed.text = "+" + equipData.speed;
                equippedItemPower.text = "+" + equipData.power;
                equippedItemCritRate.text = "+" + equipData.critrate * 100 + "%";
                equippedItemDamage.text = "+" + equipData.damage;

                if (selectedItem.activeSelf)
                {
                    selectedItemIcon.sprite = ResourceManager.LoadSprite(selectedEquipData.icon);
                    selectedItemSpeed.text = "+" + selectedEquipData.speed;
                    selectedItemSpeedArrow.text = GetArrowByStats(equipData.speed, selectedEquipData.speed);
                    
                    selectedItemPower.text = "+" + selectedEquipData.power;
                    selectedItemPowerArrow.text = GetArrowByStats(equipData.power, selectedEquipData.power);
                    
                    selectedItemCritRate.text = "+" + selectedEquipData.critrate * 100 + "%";
                    selectedItemCritRateArrow.text = GetArrowByStats(equipData.critrate, selectedEquipData.critrate);
                    
                    selectedItemDamage.text = "+" + selectedEquipData.damage;
                    selectedItemDamageArrow.text = GetArrowByStats(equipData.damage, selectedEquipData.damage);
                }
            }

            private string GetArrowByStats(float equippedItemStat, float selectedItemStat)
            {
                if (equippedItemStat == selectedItemStat)
                {
                    return "";
                }
                
                return equippedItemStat < selectedItemStat ? TMPSprite.IconArrowUp : TMPSprite.IconArrowDown;
            }

            private async void EquipButtonClick()
            {
                var overlordData = GameData.characters.overlord;
                
                if (overlordData.id.HasValue && newEquipId.HasValue)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    await GameData.equipment.Equip(overlordData.id.Value, newEquipId.Value);
                    OnEquip?.Invoke(equipId, newEquipId.Value);
                }
            }
            
            public static EquipInfoPopup GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EquipInfoPopup>(
                    "Prefabs/UI/Screens/OverlordScreen/EquipInfoPopup", parent);
            }
        }
    }
}