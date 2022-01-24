using System.Collections;
using System.Collections.Generic;
using Overlewd;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class ForgeButton : BaseButton
        {
            protected Transform craftInProgressNotification;
            protected Transform craftAvailableNotification;

            protected override void Awake()
            {
                base.Awake();

                var notificationsGrid = transform.Find("NotificationGrid");

                craftInProgressNotification = notificationsGrid.Find("CraftInProgressNotification");
                craftAvailableNotification = notificationsGrid.Find("CraftAvailableNotification");
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                // UIManager.ShowScreen<ForgeScreen>();
            }

            public static ForgeButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ForgeButton>
                    ("Prefabs/UI/Screens/CastleScreen/ForgeButton", parent);
            }
        }
    }
}
