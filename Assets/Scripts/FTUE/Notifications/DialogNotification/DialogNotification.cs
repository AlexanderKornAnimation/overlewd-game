using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class DialogNotificationMissclick : Overlewd.DialogNotificationMissclick
        {
            protected override void OnClick()
            {
                if (GameGlobalStates.newFTUE)
                {
                    var notifications = Overlewd.GameData.ftue.chapters[GameGlobalStates.ftueChapterId].
                        dialogs.FindAll(d => GameData.GetDialogById(d.id)?.type == AdminBRO.DialogType.Notification);
                    var nextNotificationIndex = notifications.FindIndex(n => n.key == GameGlobalStates.dialogNotification_StageKey);
                    if (++nextNotificationIndex < notifications.Count)
                    {
                        GameGlobalStates.dialogNotification_StageKey = notifications[nextNotificationIndex].key;
                        UIManager.ShowNotification<DialogNotification>();
                    }
                    else
                    {
                        UIManager.HideNotification();
                        UIManager.ShowScreen<StartingScreen>();
                    }
                }
                else
                {
                    UIManager.HideNotification();
                }
            }
        }

        public class DialogNotification : Overlewd.DialogNotification
        {
            public override void ShowMissclick()
            {
                var missclick = UIManager.ShowNotificationMissclick<DialogNotificationMissclick>();
                missclick.missClickEnabled = false;
            }

            protected override void EnterScreen()
            {
                button.gameObject.SetActive(false);
                dialogData = GameGlobalStates.dialogNotification_DialogData;
            }

            protected override void LeaveByTimerScreen()
            {
                if (GameGlobalStates.newFTUE)
                {
                    UIManager.ShowScreen<StartingScreen>();
                }
            }

            protected override AdminBRO.Animation GetAnimationById(int id)
            {
                return GameData.GetAnimationById(id);
            }
        }
    }
}
