using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class PrepareBossFightPopup : Overlewd.PrepareBossFightPopup
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

            public override async Task AfterShowAsync()
            {
                await base.AfterShowAsync();

                //ftue part
                switch (GameData.ftueStats.lastEndedState)
                {
                    case ("battle4", "chapter1"):
                        GameData.ftue.chapter1.ShowNotifByKey("potionstutor1");
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
                UIManager.MakeScreen<BossFightScreen>().
                    SetData(new BossFightScreenInData
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowScreenProcess();
            }
        }
    }
}