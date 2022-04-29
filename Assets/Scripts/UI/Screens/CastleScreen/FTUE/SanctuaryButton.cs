using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class SanctuaryButton : BaseButton
        {
            protected Transform notification;

            protected override void Awake()
            {
                base.Awake();

                notification = transform.Find("MergingAvailableNotification");
            }

            protected override void ButtonClick()
            {
                
            }

            public static SanctuaryButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SanctuaryButton>
                    ("Prefabs/UI/Screens/CastleScreen/SanctuaryButton", parent);
            }
        }
    }
}