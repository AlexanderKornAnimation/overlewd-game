using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
        public class AerostatButton : Overlewd.NSCastleScreen.AerostatButton
        {
            protected override void ButtonClick()
            {
                // base.ButtonClick();
            }

            public new static AerostatButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<AerostatButton>
                    ("Prefabs/UI/Screens/CastleScreen/AerostatButton", parent);
            }
        }
        }
    }
}
