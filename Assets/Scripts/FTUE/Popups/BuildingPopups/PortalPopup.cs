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
                GameGlobalStates.castle_HintMessage = GameData.castleScreenHints[5];
                UIManager.ShowScreen<CastleScreen>();
            }

            protected override void FreeBuildButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI.FreeBuildButton);
                Build();
            }

            protected override void PaidBuildingButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                Build();
            }
        }
    }
}
