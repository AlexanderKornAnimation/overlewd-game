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
            protected override void Customize()
            {
                UITools.DisableButton(magicGuildButton);
                UITools.DisableButton(inventoryButton);
                UITools.DisableButton(editTeamButton);
            }

            protected override void MagicGuildButtonClick()
            {
                
            }

            protected override void InventoryButtonClick()
            {

            }

            protected override void HaremButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_DefeatPopupHaremButtonClick);
                //GameGlobalStates.sexScreen_StageKey = "key";
                //UIManager.ShowScreen<SexScreen>();
            }
        }
    }
}