using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class CathedralButton : BaseButton
        {
            protected override void Awake()
            {
                base.Awake();
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
            }

            public static CathedralButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CathedralButton>
                    ("Prefabs/UI/Screens/CastleScreen/CathedralButton", parent);
            }
        }
    }
}