using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    namespace FTUE
    {
        public class PrepareBattlePopup : Overlewd.PrepareBattlePopup
        {
            private AdminBRO.FTUEStageItem stageData;

            public override async Task BeforeShowMakeAsync()
            {
                battleData = GameData.GetBattleById(stageData.battleId.Value);
                Customize();
                
                await Task.CompletedTask;
            }

            public PrepareBattlePopup SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
                return this;
            }

            protected override void EditTeamButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<TeamEditScreen>().
                    SetData(new TeamEditScreenInData { mapStageData = stageData }).RunShowScreenProcess();
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
                UIManager.MakeScreen<BattleScreen>().SetStageData(stageData).RunShowScreenProcess();
            }
        }
    }
}