using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class CastleBuildingButton : BaseButton
        {
            protected Transform markers;

            protected Transform notificationGrid;
            protected Transform buildInProgressNotification;
            protected Transform buildAvailableNotification;

            protected override void Awake()
            {
                base.Awake();

                markers = transform.Find("Markers");

                notificationGrid = transform.Find("NotificationGrid");
                buildInProgressNotification = notificationGrid.Find("BuildInProgressNotification");
                buildAvailableNotification = notificationGrid.Find("BuildAvailableNotification");
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<BuildingScreen>();
            }

            public static CastleBuildingButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CastleBuildingButton>
                    ("Prefabs/UI/Screens/CastleScreen/CastleBuildingButton", parent);
            }
        }
    }
}