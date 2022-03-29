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

                UIManager.ShowScreen<CastleScreen>();
            }
            protected override void FreeBuildButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_FreeBuildButton);
                Build();
            }

            protected override void PaidBuildingButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                Build();
            }
        }
    }
}
