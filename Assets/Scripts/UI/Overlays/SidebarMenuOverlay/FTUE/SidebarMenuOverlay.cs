using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class SidebarMenuOverlay : Overlewd.SidebarMenuOverlay
        {
            protected override void Customize()
            {
                UITools.DisableButton(castleButton);
                castleButton_Markers.gameObject.SetActive(false);

                UITools.DisableButton(portalButton);
                portalButton_Markers.gameObject.SetActive(false);

                globalMapButton_Markers.gameObject.SetActive(false);

                UITools.DisableButton(haremButton);
                haremButton_Markers.gameObject.SetActive(false);

                UITools.DisableButton(castleBuildingButton);
                castleBuildingButton_Markers.gameObject.SetActive(false);

                UITools.DisableButton(magicGuildButton);
                magicGuildButton_Markers.gameObject.SetActive(false);

                UITools.DisableButton(marketButton);
                marketButton_Markers.gameObject.SetActive(false);

                UITools.DisableButton(forgeButton);
                forgeButton_Markers.gameObject.SetActive(false);
                
                UITools.DisableButton(overlordButton);
                overlordButton_Markers.gameObject.SetActive(false);
            }

            protected override void CastleButtonClick()
            {

            }

            protected override void PortalButtonClick()
            {

            }

            protected override void GlobalMapButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<MapScreen>();
            }

            protected override void HaremButtonClick()
            {

            }

            protected override void CastleBuildingButtonClick()
            {

            }

            protected override void MagicGuildButtonClick()
            {

            }

            protected override void MarketButtonClick()
            {

            }

            protected override void ForgeButtonClick()
            {

            }
        }
    }
}
