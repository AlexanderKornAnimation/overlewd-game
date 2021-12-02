using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class SexScreen : Overlewd.SexScreen
        {
            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.sexScreen_DialogData;
                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                GameGlobalStates.dialogScreen_DialogId = 1;
                UIManager.ShowScreen<DialogScreen>();
            }
        }
    }
}
