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
                dialogData = GameGlobalStates.ftue_StageDialogData;

                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                switch (GameGlobalStates.ftue_StageKey)
                {
                    case "dialogue1":
                        GameGlobalStates.ftue_StageKey = "battle1";
                        UIManager.ShowScreen<BattleScreen>();
                        break;
                    default:
                        UIManager.ShowScreen<MapScreen>();
                        break;
                }
            }
        }
    }
}