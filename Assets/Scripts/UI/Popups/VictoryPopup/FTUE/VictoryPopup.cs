using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class VictoryPopup : Overlewd.VictoryPopup
        {
            public override async Task BeforeShowMakeAsync()
            {
                switch (inputData.ftueStageData.ftueState)
                {
                    case ("battle1", "chapter1"):
                        UITools.DisableButton(repeatButton);
                        break;
                }

                await Task.CompletedTask;
            }

            protected override void NextButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                switch (inputData.ftueStageData.ftueState)
                {
                    case ("battle4", "chapter1"):
                        UIManager.ShowScreen<CastleScreen>();
                        break;

                    default:
                        UIManager.ShowScreen<MapScreen>();
                        break;
                }
            }

            protected override void RepeatButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<BattleScreen>().
                    SetData(new BattleScreenInData
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowScreenProcess();
            }
        }
    }
}