using System;
using System.Collections;
using System.Collections.Generic;
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

            public override void RefreshState()
            {

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
