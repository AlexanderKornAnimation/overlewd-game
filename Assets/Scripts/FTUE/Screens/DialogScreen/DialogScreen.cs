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
                UIManager.ShowScreen<StartingScreen>();
            }
        }
    }
}
