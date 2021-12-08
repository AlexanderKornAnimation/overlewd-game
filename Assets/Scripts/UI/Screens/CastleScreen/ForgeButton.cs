using System.Collections;
using System.Collections.Generic;
using Overlewd;
using UnityEngine;

namespace Overlewd
{
    public class ForgeButton : BaseButton
    {
        private Transform craftInProgressNotification;
        private Transform craftAvailableNotification;

        private void Awake()
        {
            base.Awake();

            var notificationsGrid = transform.Find("NotificationGrid");
            
            craftInProgressNotification = notificationsGrid.Find("CraftInProgressNotification");
            craftAvailableNotification = notificationsGrid.Find("CraftAvailableNotification");
        }
        
        protected override void ButtonClick()
        {
            UIManager.ShowScreen<ForgeScreen>();
        }
        
        public static ForgeButton GetInstance(Transform parent)
        {
            var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/ForgeButton"), parent);
            newItem.name = nameof(ForgeButton);

            return newItem.AddComponent<ForgeButton>();
        }
    }
}
