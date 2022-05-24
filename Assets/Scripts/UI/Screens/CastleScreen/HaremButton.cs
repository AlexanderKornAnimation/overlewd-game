using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class HaremButton : BaseButton
        {
            private  Transform notificationsGrid;
            private Transform grabRewardNotification;
            private Transform memoryAvailableNotification;
            public CastleScreenInData screenInData { get; set; }
            

            protected override void Awake()
            {
                base.Awake();

                notificationsGrid = transform.Find("NotificationGrid");
                grabRewardNotification = notificationsGrid.Find("GrabRewardNotification");
                memoryAvailableNotification = notificationsGrid.Find("MemoryAvailableNotification");
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<HaremScreen>();
            }

            public static HaremButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<HaremButton>
                    ("Prefabs/UI/Screens/CastleScreen/HaremButton", parent);
            }
        }
    }
}