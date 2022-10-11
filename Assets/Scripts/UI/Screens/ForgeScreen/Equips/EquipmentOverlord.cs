using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class EquipmentOverlord : EquipmentBase
        {
            public OverlordEquipContent equipCtrl { get; set; }
            public InfoBlockOverlordEquip ctrl_InfoBlock { get; set; }
            public bool IsConsume => ctrl_InfoBlock.isFilled &&
                ctrl_InfoBlock.consumeEquip.equipData.characterClass == equipData.characterClass &&
                ctrl_InfoBlock.consumeEquip.equipData.equipmentType == equipData.equipmentType &&
                ctrl_InfoBlock.consumeEquip.equipData.rarity == equipData.rarity;

            public override void RefreshState()
            {
                base.RefreshState();
                shade.gameObject.SetActive(IsConsume);
                isConsume.gameObject.SetActive(IsConsume);
            }

            protected override void ButtonClick()
            {
                equipCtrl.RefreshState();
            }
            
            public static EquipmentOverlord GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EquipmentOverlord>("Prefabs/UI/Screens/ForgeScreen/Equipment",
                    parent);
            }
        }
    }
}
