using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    namespace FTUE
    {
        public class DialogScreen : Overlewd.DialogScreen
        {
            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.dialogScreen_DialogData;

                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                if (GameGlobalStates.dialogScreen_StageKey == "dialogue1")
                {
                    GameGlobalStates.dialogScreen_StageKey = "dialogue2";
                    UIManager.ShowScreen<DialogScreen>();
                }
                else if (GameGlobalStates.dialogScreen_StageKey == "dialogue2")
                {
                    GameGlobalStates.dialogScreen_StageKey = "dialogue3";
                    UIManager.ShowScreen<DialogScreen>();
                }
                else if (GameGlobalStates.dialogScreen_StageKey == "dialogue3")
                {
                    GameGlobalStates.dialogScreen_StageKey = "dialogue4";
                    UIManager.ShowScreen<DialogScreen>();
                }
                else if (GameGlobalStates.dialogScreen_StageKey == "dialogue4")
                {
                    var firstNotif = GameData.ftue.chapters[GameGlobalStates.chapterId].
                        notifications.First();
                    GameGlobalStates.dialogNotification_StageKey = firstNotif.key;
                    UIManager.ShowNotification<DialogNotification>();
                }
            }
        }
    }
}