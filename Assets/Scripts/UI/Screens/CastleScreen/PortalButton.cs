using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class PortalButton : BaseButton
        {
            private Transform freeSummonNotification;

            protected override void Awake()
            {
                base.Awake();

                freeSummonNotification = transform.Find("FreeSummonNotification");
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<PortalScreen>();
            }

            public static PortalButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<PortalButton>
                    ("Prefabs/UI/Screens/CastleScreen/PortalButton", parent);
            }
        }
    }
}
