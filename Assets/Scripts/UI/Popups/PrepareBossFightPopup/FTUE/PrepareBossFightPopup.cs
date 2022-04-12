using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class PrepareBossFightPopup : Overlewd.PrepareBossFightPopup
        {
            private AdminBRO.FTUEStageItem stageData;

            public PrepareBossFightPopup SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
                return this;
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
                UIManager.MakeScreen<BossFightScreen>().
                    SetStageData(stageData).RunShowScreenProcess();
            }
        }
    }
}
