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

                if (GameGlobalStates.portalCanBuilded)
                {
                    portalUnaviable.SetActive(false);
                    portalMaxLevel.SetActive(false);
                    portalButton.interactable = true;
                }
                else if (GameGlobalStates.portalBuilded)
                {
                    portalUnaviable.SetActive(false);
                    portalMaxLevel.SetActive(true);
                    portalButton.interactable = false;
                }
                else
                {
                    portalUnaviable.SetActive(true);
                    portalMaxLevel.SetActive(false);
                    portalButton.interactable = false;
                }

                if (GameGlobalStates.ulviCaveCanBuilded)
                {
                    ulviCaveUnaviable.SetActive(false);
                    ulviCaveMaxLevel.SetActive(false);
                    ulviCaveButton.interactable = true;
                }
                else if (GameGlobalStates.ulviCaveBuilded)
                {
                    ulviCaveUnaviable.SetActive(false);
                    ulviCaveMaxLevel.SetActive(true);
                    ulviCaveButton.interactable = false;
                }
                else
                {
                    ulviCaveUnaviable.SetActive(true);
                    ulviCaveMaxLevel.SetActive(false);
                    ulviCaveButton.interactable = false;
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
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                UIManager.ShowPopup<PortalPopup>();
            }

            protected override void UlviCaveButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
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
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                UIManager.ShowScreen<CastleScreen>();
            }
        }
    }
}
