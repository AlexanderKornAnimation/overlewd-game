using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class CapitolButton : BaseButton
        {
            protected Transform notification;

            protected override void Awake()
            {
                base.Awake();

                notification = transform.Find("CollectCrystalsNotification");
            }

            protected override void ButtonClick()
            {
                
            }

            public static CapitolButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CapitolButton>
                    ("Prefabs/UI/Screens/CastleScreen/CapitolButton", parent);
            }
        }
    }
}