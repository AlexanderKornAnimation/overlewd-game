using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
        public class SanctuaryButton : Overlewd.NSCastleScreen.SanctuaryButton
        {
            protected override void Awake()
            {
                base.Awake();
            }

            protected override void ButtonClick()
            {

            }

            public new static SanctuaryButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SanctuaryButton>
                    ("Prefabs/UI/Screens/CastleScreen/SanctuaryButton", parent);
            }
        }
        }
    }
}