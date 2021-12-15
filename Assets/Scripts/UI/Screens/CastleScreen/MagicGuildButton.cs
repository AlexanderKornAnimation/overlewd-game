using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class MagicGuildButton : BaseButton
        {
            protected Transform learnInProgressNotification;
            protected Transform learnAvailableNotification;

            protected override void Awake()
            {
                base.Awake();

                var notificationsGrid = transform.Find("NotificationGrid");

                learnInProgressNotification = notificationsGrid.Find("LearnInProgressNotification");
                learnAvailableNotification = notificationsGrid.Find("LearnAvailableNotification");
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
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/MagicGuildButton"), parent);
                newItem.name = nameof(MagicGuildButton);

                return newItem.AddComponent<MagicGuildButton>();
            }
        }
    }
}