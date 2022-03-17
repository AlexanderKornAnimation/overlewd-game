using System;
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
        public class SexScreen : Overlewd.SexScreen
        {
            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.sexScreen_DialogData;

                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                if (GameGlobalStates.sexScreen_StageKey == "sex1")
                {
                    GameGlobalStates.sexScreen_StageKey = "sex2";
                    UIManager.ShowScreen<SexScreen>();
                }
                else if (GameGlobalStates.sexScreen_StageKey == "sex2")
                {
                    GameGlobalStates.sexScreen_StageKey = "sex3";
                    UIManager.ShowScreen<SexScreen>();
                }
                else if (GameGlobalStates.sexScreen_StageKey == "sex3")
                {
                    GameGlobalStates.sexScreen_StageKey = "sex4";
                    UIManager.ShowScreen<SexScreen>();
                }
                else if (GameGlobalStates.sexScreen_StageKey == "sex4")
                {
                    GameGlobalStates.dialogScreen_StageKey = "dialogue1";
                    UIManager.ShowScreen<DialogScreen>();
                }
            }
        }
    }
}