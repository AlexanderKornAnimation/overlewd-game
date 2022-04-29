using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class AerostatButton : BaseButton
        {
            protected override void ButtonClick()
            {
                // base.ButtonClick();
            }

            public static AerostatButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<AerostatButton>
                    ("Prefabs/UI/Screens/CastleScreen/AerostatButton", parent);
            }
        }
    }
}
