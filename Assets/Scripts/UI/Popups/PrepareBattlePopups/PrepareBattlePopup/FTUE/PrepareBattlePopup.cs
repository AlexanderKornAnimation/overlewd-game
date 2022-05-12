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
                battleData = inputData.ftueStageData.battleData;
                Customize();

                switch (inputData.ftueStageData.ftueState)
                {
                    case (_, "chapter1"):
                        UITools.DisableButton(editTeamButton);
                        break;
                }

                await Task.CompletedTask;
            }

            protected override void EditTeamButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<TeamEditScreen>().
                    SetData(new TeamEditScreenInData 
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowScreenProcess();
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
                UIManager.MakeScreen<BattleScreen>().
                    SetData(new BattleScreenInData
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowScreenProcess();
            }
        }
    }
}