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
            public override async Task BeforeShowMakeAsync()
            {
                battleData = GameData.GetBattleById(inputData.ftueStageData.battleId.Value);
                Customize();
                
                await Task.CompletedTask;
            }

            protected override void EditTeamButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<TeamEditScreen>().
                    SetData(new TeamEditScreenInData 
                    {
                        mapStageData = inputData.ftueStageData
                    }).RunShowScreenProcess();
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
                UIManager.MakeScreen<BattleScreen>().
                    SetData(new BattleScreenInData
                    {
                        ftueStageData = inputData.ftueStageData
                    }).RunShowScreenProcess();
            }
        }
    }
}