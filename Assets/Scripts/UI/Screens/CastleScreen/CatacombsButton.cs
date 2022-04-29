using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
        public class CatacombsButton : Overlewd.NSCastleScreen.CatacombsButton
        {
            protected override void Awake()
            {
                base.Awake();
            }

            protected override void ButtonClick()
            {
                // base.ButtonClick();
            }

            public new static CatacombsButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CatacombsButton>
                    ("Prefabs/UI/Screens/CastleScreen/CatacombsButton", parent);
            }
        }
        }
    }
}
