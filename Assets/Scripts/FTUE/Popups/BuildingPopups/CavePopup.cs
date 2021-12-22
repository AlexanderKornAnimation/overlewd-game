using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    namespace FTUE
    {
        public class CavePopup : Overlewd.CavePopup
        {
            protected override void FreeBuildButtonClick()
            {
                GameGlobalStates.UlviCaveBuild();
                GameGlobalStates.ResetStateCastleButtons();
                GameGlobalStates.castle_SideMenuLock = true;
                GameGlobalStates.castle_BuildingButtonLock = true;
                UIManager.ShowScreen<CastleScreen>();
            }

            protected override void PaidBuildingButtonClick()
            {
                GameGlobalStates.UlviCaveBuild();
                GameGlobalStates.ResetStateCastleButtons();
                GameGlobalStates.castle_SideMenuLock = true;
                GameGlobalStates.castle_BuildingButtonLock = true;
                UIManager.ShowScreen<CastleScreen>();
            }
        }
    }
}
