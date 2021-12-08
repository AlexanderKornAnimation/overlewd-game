using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class HaremButton : BaseButton
        {
            private Transform grabRewardNotification;
            private Transform memoryAvailableNotification;

            protected override void Awake()
            {
                base.Awake();

                var notificationsGrid = transform.Find("NotificationGrid");

                grabRewardNotification = notificationsGrid.Find("GrabRewardNotification");
                memoryAvailableNotification = notificationsGrid.Find("MemoryAvailableNotification");
            }

            protected override void ButtonClick()
            {
                UIManager.ShowScreen<HaremScreen>();
            }

            public static HaremButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/HaremButton"), parent);
                newItem.name = nameof(HaremButton);

                return newItem.AddComponent<HaremButton>();
            }
        }
    }
}