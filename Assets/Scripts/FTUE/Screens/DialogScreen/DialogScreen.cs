using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class DialogScreen : Overlewd.DialogScreen
        {
            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.dialogScreen_DialogData;
                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                if (GameGlobalStates.dialogScreen_DialogId == 10)
                {
                    GameGlobalStates.dialogScreen_DialogId = 13;
                    UIManager.ShowScreen<DialogScreen>();
                }
                else if (GameGlobalStates.dialogScreen_DialogId == 13)
                {
                    GameGlobalStates.dialogScreen_DialogId = 14;
                    UIManager.ShowScreen<DialogScreen>();
                }
                else if (GameGlobalStates.dialogScreen_DialogId == 14)
                {
                    GameGlobalStates.dialogNotification_DialogId = 3;
                    UIManager.ShowNotification<DialogNotification>();
                }
            }
        }
    }
}
