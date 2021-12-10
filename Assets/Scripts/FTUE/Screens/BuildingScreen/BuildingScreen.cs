using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace FTUE
    {
        public class BuildingScreen : Overlewd.BuildingScreen
        {
            protected override void Customize()
            {

            }

            protected override void MunicipalityButtonClick()
            {

            }

            protected override void ForgeButtonClick()
            {

            }

            protected override void MagicGuildButtonClick()
            {

            }

            protected override void MarketButtonClick()
            {

            }

            protected override void PortalButtonClick()
            {
                UIManager.ShowPopup<PortalPopup>();
            }

            protected override void UlviCaveButtonClick()
            {
                UIManager.ShowPopup<CavePopup>();
            }

            protected override void FayeCaveButtonClick()
            {

            }

            protected override void FionaCaveButtonClick()
            {

            }

            protected override void JadeCaveButtonClick()
            {

            }

            protected override void YuiCaveButtonClick()
            {

            }

            protected override void BackButtonClick()
            {
                UIManager.ShowScreen<CastleScreen>();
            }
        }
    }
}
