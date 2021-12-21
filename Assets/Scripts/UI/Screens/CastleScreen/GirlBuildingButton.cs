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
                UIManager.ShowScreen<HaremScreen>();
            }

            public static GirlBuildingButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/GirlBuildingButton"), parent);
                newItem.name = nameof(GirlBuildingButton);

                return newItem.AddComponent<GirlBuildingButton>();
            }
        }
    }
}