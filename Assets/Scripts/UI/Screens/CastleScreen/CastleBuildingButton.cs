using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class CastleBuildingButton : BaseButton
    {
        private Transform buildInProgressNotification;
        private Transform buildAvailableNotification;

        private void Awake()
        {
            base.Awake();

            var notificationGrid = transform.Find("NotificationGrid");

            buildInProgressNotification = notificationGrid.Find("BuildInProgressNotification");
            buildAvailableNotification = notificationGrid.Find("BuildAvailableNotification");
        }

        protected override void ButtonClick()
        {
            UIManager.ShowScreen<BuildingScreen>();
        }

        public static CastleBuildingButton GetInstance(Transform parent)
        {
            var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/CastleBuildingButton"), parent);
            newItem.name = nameof(CastleBuildingButton);

            return newItem.AddComponent<CastleBuildingButton>();
        }
    }
}