using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class MunicipalityButton : BaseButton
        {
            protected Transform notificationGrid;
            protected Transform buildInProgressNotification;
            protected Transform buildAvailableNotification;

            protected override void Awake()
            {
                base.Awake();

                notificationGrid = transform.Find("NotificationGrid");
                buildInProgressNotification = notificationGrid.Find("BuildInProgressNotification");
                buildAvailableNotification = notificationGrid.Find("BuildAvailableNotification");
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<MunicipalityScreen>();
            }

            public static MunicipalityButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MunicipalityButton>
                    ("Prefabs/UI/Screens/CastleScreen/MunicipalityButton", parent);
            }
        }
    }
}