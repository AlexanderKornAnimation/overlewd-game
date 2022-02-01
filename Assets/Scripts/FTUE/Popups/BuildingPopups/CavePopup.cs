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
            private void Build()
            {
                GameGlobalStates.UlviCaveBuild();
                GameGlobalStates.ResetStateCastleButtons();
                GameGlobalStates.castle_SideMenuLock = true;
                GameGlobalStates.castle_BuildingButtonLock = true;
                GameGlobalStates.castle_DialogNotificationId = 9;
                GameGlobalStates.castle_HintMessage = GameData.castleScreenHints[3];
                UIManager.ShowScreen<CastleScreen>();
            }
            protected override void FreeBuildButtonClick()
            {
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                Build();
            }

            protected override void PaidBuildingButtonClick()
            {
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                Build();
            }
        }
    }
}
