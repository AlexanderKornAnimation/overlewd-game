using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class GirlBuildingButton : BaseButton
        {
            protected Transform markers;

            protected Transform notificationsGrid;
            protected Transform grabRewardNotification;
            protected Transform memoryAvailableNotification;

            protected override void Awake()
            {
                base.Awake();

                markers = transform.Find("Markers");

                notificationsGrid = transform.Find("NotificationGrid");
                grabRewardNotification = notificationsGrid.Find("GrabRewardNotification");
                memoryAvailableNotification = notificationsGrid.Find("MemoryAvailableNotification");
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<HaremScreen>();
            }

            public static GirlBuildingButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<GirlBuildingButton>
                    ("Prefabs/UI/Screens/CastleScreen/GirlBuildingButton", parent);
            }
        }
    }
}