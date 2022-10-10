using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class EquipmentBattleGirls : EquipmentBase
        {
            public BattleGirlsEquipContent equipCtrl { get; set; }
            public InfoBlockBattleGirlEquip ctrl_InfoBlock { get; set; }

            public override void RefreshState()
            {

            }

            protected override void ButtonClick()
            {
                equipCtrl.RefreshState();
            }
            
            public static EquipmentBattleGirls GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EquipmentBattleGirls>("Prefabs/UI/Screens/ForgeScreen/Equipment",
                    parent);
            }
        }
    }
}
