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
            public override void RefreshState()
            {

            }

            protected override void ButtonClick()
            {
                
            }
            
            public static EquipmentBattleGirls GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EquipmentBattleGirls>("Prefabs/UI/Screens/ForgeScreen/Equipment",
                    parent);
            }
        }
    }
}
