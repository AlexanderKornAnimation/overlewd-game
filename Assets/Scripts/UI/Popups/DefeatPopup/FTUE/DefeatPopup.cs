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
        public class DefeatPopup : Overlewd.DefeatPopup
        {
            private AdminBRO.FTUEStageItem stageData;

            public void SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
            }
            protected override void Customize()
            {
                UITools.DisableButton(magicGuildButton);
                UITools.DisableButton(inventoryButton);
                UITools.DisableButton(editTeamButton);
            }

            protected override void HaremButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_DefeatPopupHaremButtonClick);

                UIManager.ShowScreen<MapScreen>();
            }
        }
    }
}