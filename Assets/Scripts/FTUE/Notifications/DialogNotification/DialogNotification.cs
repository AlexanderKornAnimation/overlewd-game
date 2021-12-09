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

                if (GameGlobalStates.dialogNotification_DialogId == 3)
                {
                    UIManager.ShowScreen<BattleScreen>();
                }
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
                var dialogData = GameGlobalStates.dialogNotification_DialogData;
                text.text = dialogData.replicas[0].message;

                await Task.CompletedTask;
            }
        }
    }
}
