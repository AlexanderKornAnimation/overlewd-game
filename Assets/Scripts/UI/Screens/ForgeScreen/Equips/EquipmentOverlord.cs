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
            public override void RefreshState()
            {

            }

            protected override void ButtonClick()
            {
                
            }
            
            public static EquipmentOverlord GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EquipmentOverlord>("Prefabs/UI/Screens/ForgeScreen/Equipment",
                    parent);
            }
        }
    }
}
