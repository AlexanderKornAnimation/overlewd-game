using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class PortalPopup : Overlewd.PortalPopup
        {
            private void Build()
            {
                GameGlobalStates.PortalBuild();
                GameGlobalStates.ResetStateCastleButtons();
                GameGlobalStates.castle_SideMenuLock = true;
                GameGlobalStates.castle_CaveLock = true;
                GameGlobalStates.castle_BuildingButtonLock = true;
                UIManager.ShowScreen<CastleScreen>();
            }

            protected override void FreeBuildButtonClick()
            {
                Build();
            }

            protected override void PaidBuildingButtonClick()
            {
                Build();
            }
        }
    }
}
