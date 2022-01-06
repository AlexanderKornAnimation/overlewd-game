using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class CapitolButton : Overlewd.NSCastleScreen.CapitolButton
            {
                protected override void ButtonClick()
                {

                }

                protected override void Customize()
                {
                    base.Customize();

                }

                public new static CapitolButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<CapitolButton>
                        ("Prefabs/UI/Screens/CastleScreen/CapitolButton", parent);
                }
            }
        }
    }
}
