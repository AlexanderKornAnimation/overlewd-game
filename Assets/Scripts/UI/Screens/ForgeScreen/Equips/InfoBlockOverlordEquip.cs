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
        public class InfoBlockOverlordEquip : InfoBlockEquipBase
        {
            public OverlordEquipContent equipCtrl { get; set; }

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

            public override AdminBRO.ForgePrice.MergeEquipmentSettings mergeSettings =>
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
        }
    }
}
