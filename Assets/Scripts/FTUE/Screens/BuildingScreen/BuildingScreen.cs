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
                municipalityUnaviable.SetActive(true);
                municipalityMaxLevel.SetActive(false);

                forgeUnaviable.SetActive(true);
                forgeMaxLevel.SetActive(false);

                magicGuildUnaviable.SetActive(true);
                magicGuildMaxLevel.SetActive(false);

                marketUnaviable.SetActive(true);
                marketMaxLevel.SetActive(false);

                if (GameGlobalStates.portalCanBuild)
                {
                    portalUnaviable.SetActive(false);
                    portalMaxLevel.SetActive(false);
                }
                else if (GameGlobalStates.portalIsBuild)
                {
                    portalUnaviable.SetActive(false);
                    portalMaxLevel.SetActive(true);
                }
                else
                {
                    portalUnaviable.SetActive(true);
                    portalMaxLevel.SetActive(false);
                }

                if (GameGlobalStates.ulviCaveCanBuild)
                {
                    ulviCaveUnaviable.SetActive(false);
                    ulviCaveMaxLevel.SetActive(false);
                }
                else if (GameGlobalStates.ulviCaveIsBuild)
                {
                    ulviCaveUnaviable.SetActive(false);
                    ulviCaveMaxLevel.SetActive(true);
                }
                else
                {
                    ulviCaveUnaviable.SetActive(true);
                    ulviCaveMaxLevel.SetActive(false);
                }

                fayeCaveUnaviable.SetActive(true);
                fayeCaveMaxLevel.SetActive(false);

                fionaCaveUnaviable.SetActive(true);
                fionaCaveMaxLevel.SetActive(false);

                jadeCaveUnaviable.SetActive(true);
                jadeCaveMaxLevel.SetActive(false);

                yuiCaveUnaviable.SetActive(true);
                yuiCaveMaxLevel.SetActive(false);
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
                if (GameGlobalStates.portalCanBuild)
                {
                    GameGlobalStates.portalCanBuild = false;
                    GameGlobalStates.portalIsBuild = true;
                    Customize();
                }
            }

            protected override void UlviCaveButtonClick()
            {
                if (GameGlobalStates.ulviCaveCanBuild)
                {
                    GameGlobalStates.ulviCaveCanBuild = false;
                    GameGlobalStates.ulviCaveIsBuild = true;
                    Customize();
                }
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
