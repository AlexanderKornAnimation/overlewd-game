using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class NotificationMissclickColored : Overlewd.NotificationMissclickColored
        {
            protected override void OnClick()
            {
                base.OnClick();

                UIManager.ShowScreen<StartingScreen>();
            }
        }

        public class DialogNotification : Overlewd.DialogNotification
        {
            protected override void ShowMissclick()
            {
                UIManager.ShowNotificationMissclick<NotificationMissclickColored>();
            }

            protected override async Task PrepareShowOperationsAsync()
            {
                text.text = "TextText";

                await Task.CompletedTask;
            }
        }
    }
}
