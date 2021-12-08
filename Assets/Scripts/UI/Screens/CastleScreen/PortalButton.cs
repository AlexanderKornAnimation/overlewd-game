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
                UIManager.ShowScreen<PortalScreen>();
            }

            public static PortalButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/PortalButton"), parent);
                newItem.name = nameof(PortalButton);

                return newItem.AddComponent<PortalButton>();
            }
        }
    }
}
