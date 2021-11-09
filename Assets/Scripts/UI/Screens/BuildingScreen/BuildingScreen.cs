using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class BuildingScreen : BaseScreen
    {
        private Button municipalityButton;
        private Button forgeButton;
        private Button magicGuildButton;
        private Button marketButton;
        private Button portalButton;
        private Button ulviCaveButton;
        private Button fayeCaveButton;
        private Button fionaCaveButton;
        private Button jadeCaveButton;
        private Button yuiCaveButton;
        private Button backButton;

        private Image maxLevelImage;
        private Image unaviableImage;
        private Image buildingImage;

        private void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/BuildingScreen/BuildingScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            municipalityButton = canvas.Find("Grid").Find("MunicipalityButton").GetComponent<Button>();
            forgeButton = canvas.Find("Grid").Find("ForgeButton").GetComponent<Button>();
            magicGuildButton = canvas.Find("Grid").Find("MagicGuildButton").GetComponent<Button>();
            marketButton = canvas.Find("Grid").Find("MarketButton").GetComponent<Button>();
            portalButton = canvas.Find("Grid").Find("PortalButton").GetComponent<Button>();
            ulviCaveButton = canvas.Find("Grid").Find("UlviCaveButton").GetComponent<Button>();
            fayeCaveButton = canvas.Find("Grid").Find("FayeCaveButton").GetComponent<Button>();
            fionaCaveButton = canvas.Find("Grid").Find("FionaCaveButton").GetComponent<Button>();
            jadeCaveButton = canvas.Find("Grid").Find("JadeCaveButton").GetComponent<Button>();
            yuiCaveButton = canvas.Find("Grid").Find("YuiCaveButton").GetComponent<Button>();

            backButton = canvas.Find("BackButton").GetComponent<Button>();

            backButton.onClick.AddListener(BackButtonClick);
            municipalityButton.onClick.AddListener(MunicipalityButtonClick);
            forgeButton.onClick.AddListener(ForgeButtonClick);
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);
            marketButton.onClick.AddListener(MarketButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
            ulviCaveButton.onClick.AddListener(UlviCaveButtonClick);
            fayeCaveButton.onClick.AddListener(FayeCaveButtonClick);
            fionaCaveButton.onClick.AddListener(FionaCaveButtonClick);
            jadeCaveButton.onClick.AddListener(JadeCaveButtonClick);
            yuiCaveButton.onClick.AddListener(YuiCaveButtonClick);
        }

        private void MunicipalityButtonClick()
        {
        }

        private void ForgeButtonClick()
        {
            UIManager.ShowPopup<ForgePopup>();
        }

        private void MagicGuildButtonClick()
        {
        }

        private void MarketButtonClick()
        {
        }

        private void PortalButtonClick()
        {
        }

        private void UlviCaveButtonClick()
        {
            UIManager.ShowPopup<CavePopup>();
        }

        private void FayeCaveButtonClick()
        {
        }

        private void FionaCaveButtonClick()
        {
        }

        private void JadeCaveButtonClick()
        {
        }

        private void YuiCaveButtonClick()
        {
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }
    }

}
