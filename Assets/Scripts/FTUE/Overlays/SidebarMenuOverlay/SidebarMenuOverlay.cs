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
                DisableButton(castleButton);
                castleButton_Markers.gameObject.SetActive(false);

                DisableButton(portalButton);
                portalButton_Markers.gameObject.SetActive(false);

                globalMapButton_Markers.gameObject.SetActive(false);

                DisableButton(haremButton);
                haremButton_Markers.gameObject.SetActive(false);

                DisableButton(castleBuildingButton);
                castleBuildingButton_Markers.gameObject.SetActive(false);

                DisableButton(magicGuildButton);
                magicGuildButton_Markers.gameObject.SetActive(false);

                DisableButton(marketButton);
                marketButton_Markers.gameObject.SetActive(false);

                DisableButton(forgeButton);
                forgeButton_Markers.gameObject.SetActive(false);
            }

            private void DisableButton(Button button)
            {
                button.interactable = false;
                foreach (var cr in button.GetComponentsInChildren<CanvasRenderer>())
                {
                    cr.SetColor(Color.gray);
                }
            }

            protected override void CastleButtonClick()
            {

            }

            protected override void PortalButtonClick()
            {

            }

            protected override void GlobalMapButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
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
