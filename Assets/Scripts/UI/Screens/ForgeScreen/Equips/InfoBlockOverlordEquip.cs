using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class InfoBlockOverlordEquip : BaseInfoBlock
        {
            public OverlordEquipContent equipCtrl { get; set; }
            public List<int> consumeEquips { get; private set; } = new List<int>();

            protected override void IncClick()
            {
                base.IncClick();
                AutoFilled();
                equipCtrl.RefreshState();
            }

            protected override void DecClick()
            {
                base.DecClick();
                AutoFilled();
                equipCtrl.RefreshState();
            }

            private void AutoFilled()
            {
                if (equipsCount < equipsNeeded)
                {
                    var consEquipData = consumeEquipData;
                    var canConsumeEquips = GameData.equipment.equipment.FindAll(e =>
                        e.characterClass == consEquipData.characterClass &&
                        e.equipmentType == consEquipData.equipmentType &&
                        e.rarity == consEquipData.rarity &&
                        !consumeEquips.Contains(e.id)).Select(e => e.id).ToList();
                    consumeEquips.AddRange(canConsumeEquips.GetRange(0,
                        Math.Min(canConsumeEquips.Count, equipsNeeded - equipsCount)));
                }
                else if (equipsCount > equipsNeeded)
                {
                    consumeEquips.RemoveRange(equipsCount, equipsCount - equipsNeeded);
                }
            }

            public void ActualizeConsumeEquips()
            {
                consumeEquips.RemoveAll(eId => GameData.equipment.GetById(eId) == null);
            }

            public void RefreshState()
            {
                if (!isFilled)
                {
                    msgTitle.text = "Tap to equipment wich you want to merge";
                    msgTitle.gameObject.SetActive(true);
                    arrow.gameObject.SetActive(false);
                    consumeCell.gameObject.SetActive(false);
                    targetCell.gameObject.SetActive(false);
                }
                else
                {
                    msgTitle.gameObject.SetActive(false);
                    arrow.gameObject.SetActive(true);
                    consumeCell.gameObject.SetActive(true);
                    targetCell.gameObject.SetActive(true);
                    consumeShardIcon.gameObject.SetActive(false);
                    targetShardIcon.gameObject.SetActive(false);
                    consumeSubstrate.gameObject.SetActive(false);
                    targetSubstrate.gameObject.SetActive(false);

                    var consEquipData = consumeEquipData;
                    switch (consEquipData.rarity)
                    {
                        case AdminBRO.Rarity.Basic:
                            consumeIcon.sprite = ResourceManager.LoadSprite(consEquipData.basicIcon);
                            targetIcon.sprite = ResourceManager.LoadSprite(consEquipData.advancedIcon);
                            break;
                        case AdminBRO.Rarity.Advanced:
                            consumeIcon.sprite = ResourceManager.LoadSprite(consEquipData.advancedIcon);
                            targetIcon.sprite = ResourceManager.LoadSprite(consEquipData.epicIcon);
                            break;
                        case AdminBRO.Rarity.Epic:
                            consumeIcon.sprite = ResourceManager.LoadSprite(consEquipData.epicIcon);
                            targetIcon.sprite = ResourceManager.LoadSprite(consEquipData.heroicIcon);
                            break;
                    }

                    //counter buttons
                    consumeCount.text = $"{equipsCount}/{equipsNeeded}";
                    UITools.DisableButton(decButton, targetCountValue == 1);
                    UITools.DisableButton(incButton, equipsNeeded >= maxEquipsResult);
                }
            }

            public bool isFilled => consumeEquips.Count > 0;
            public AdminBRO.Equipment consumeEquipData =>
                GameData.equipment.GetById(consumeEquips.FirstOrDefault());
            public int mergeAmount => mergeSettings.mergeAmount;
            public int maxEquipsResult => mergeSettings.maxPossibleResultAmount;
            public int equipsCount => consumeEquips.Count;
            public int equipsNeeded => targetCountValue * mergeAmount;
            public List<AdminBRO.PriceItem> mergePrice =>
                UITools.PriceMul(mergeSettings.GetPrice(consumeEquipData?.rarity),
                    targetCountValue);
            public bool canMerge =>
                GameData.player.CanBuy(mergePrice) &&
                equipsCount >= equipsNeeded;
            public AdminBRO.ForgePrice.MergeEquipmentSettings mergeSettings =>
                consumeEquipData?.equipmentType switch
                {
                    AdminBRO.Equipment.Type_OverlordBoots =>
                        GameData.buildings.forge.prices.mergeEquipmentOverlordBootsSettings,
                    AdminBRO.Equipment.Type_OverlordGloves =>
                        GameData.buildings.forge.prices.mergeEquipmentOverlordGlovesSettings,
                    AdminBRO.Equipment.Type_OverlordHarness =>
                        GameData.buildings.forge.prices.mergeEquipmentOverlordHarnessSettings,
                    AdminBRO.Equipment.Type_OverlordHelmet =>
                        GameData.buildings.forge.prices.mergeEquipmentOverlordHelmetSettings,
                    AdminBRO.Equipment.Type_OverlordThighs =>
                        GameData.buildings.forge.prices.mergeEquipmentOverlordThighsSettings,
                    AdminBRO.Equipment.Type_OverlordWeapon =>
                        GameData.buildings.forge.prices.mergeEquipmentOverlordWeaponSettings,
                    _ => null
                };
            public int[] mergeIds => consumeEquips.GetRange(0, equipsNeeded).ToArray();
            public bool IsConsume(EquipmentOverlord equip) =>
                consumeEquips.Exists(eId => eId == equip.equipId);
            public void Consume(EquipmentOverlord equip)
            {
                if (isFilled)
                {
                    var consEquipData = consumeEquipData;
                    var equipData = equip.equipData;
                    if (consEquipData.characterClass == equipData.characterClass &&
                        consEquipData.equipmentType == equipData.equipmentType &&
                        consEquipData.rarity == equipData.rarity)
                    {
                        consumeEquips.Add(equip.equipId.Value);
                    }
                    else
                    {
                        consumeEquips.Clear();
                        consumeEquips.Add(equip.equipId.Value);
                    }
                }
                else
                {
                    consumeEquips.Add(equip.equipId.Value);
                }
            }
            public void RemoveConsume(EquipmentOverlord equip)
            {
                consumeEquips.RemoveAll(eId => eId == equip.equipId);
            }
        }
    }
}
