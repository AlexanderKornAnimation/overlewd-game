using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Overlewd
{
    public class MagicGuildButton : BaseButton
    {
        private Transform learnInProgressNotification;
        private Transform learnAvailableNotification;

        private void Awake()
        {
            base.Awake();

            var notificationsGrid = transform.Find("NotificationGrid");
            
            learnInProgressNotification = notificationsGrid.Find("LearnInProgressNotification");
            learnAvailableNotification = notificationsGrid.Find("LearnAvailableNotification");
        }
        
        private void Start()
        {
            Customize();
        }
        
        protected override void ButtonClick()
        {
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        protected override void Customize()
        {
            title.name = "Magic guild";
        }

        public static MagicGuildButton GetInstance(Transform parent)
        {
            var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/MagicGuildButton"), parent);
            newItem.name = nameof(MagicGuildButton);

            return newItem.AddComponent<MagicGuildButton>();
        }
    }
}
