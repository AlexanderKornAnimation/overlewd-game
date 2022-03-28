using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    namespace FTUE
    {
        public class DialogScreen : Overlewd.DialogScreen
        {
            private AdminBRO.FTUEStageItem stageData;

            public void SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                await GameData.FTUEStartStage(stageData.id);
                await Task.CompletedTask;
            }

            protected override async void LeaveScreen()
            {
                await GameData.FTUEEndStage(stageData.id);

                switch (stageData.key)
                {
                    case "dialogue1":
                        UIManager.ShowScreen<MapScreen>();
                        /*UIManager.ShowScreen<BattleScreen>().
                            SetStageData(GameGlobalStates.GetFTUEStageByKey("battle1"));*/
                        break;
                    default:
                        UIManager.ShowScreen<MapScreen>();
                        break;
                }
            }
        }
    }
}