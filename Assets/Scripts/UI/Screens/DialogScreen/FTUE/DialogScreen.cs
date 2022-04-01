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

            public DialogScreen SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
                dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                return this;
            }

            public override async Task BeforeShowDataAsync()
            {
                await GameData.FTUEStartStage(stageData.id);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(stageData.id);
            }

            protected override void LeaveScreen()
            {
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