using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Resharper disable All

namespace Overlewd
{
    namespace FTUE
    {
        public class VictoryPopup : Overlewd.VictoryPopup
        {
            private AdminBRO.FTUEStageItem stageData;

            public VictoryPopup SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
                return this;
            }
            protected override void NextButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<MapScreen>();
            }

            protected override void RepeatButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<BattleScreen>().
                    SetStageData(stageData).RunShowScreenProcess();
            }
        }
    }
}