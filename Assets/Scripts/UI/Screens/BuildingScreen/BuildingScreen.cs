using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class BuildingScreen : BaseScreen
    {
        protected Button municipalityButton;
        protected Button forgeButton;
        protected Button magicGuildButton;
        protected Button marketButton;
        protected Button portalButton;
        protected Button ulviCaveButton;
        protected Button fayeCaveButton;
        protected Button fionaCaveButton;
        protected Button jadeCaveButton;
        protected Button yuiCaveButton;
        protected Button backButton;

        protected Image maxLevelImage;
        protected Image unaviableImage;
        protected Image buildingImage;

        private void Awake()
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

        private void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {

        }

        protected virtual void MunicipalityButtonClick()
        {
            
        }

        protected virtual void ForgeButtonClick()
        {
            UIManager.ShowPopup<ForgePopup>();
        }

        protected virtual void MagicGuildButtonClick()
        {

        }

        protected virtual void MarketButtonClick()
        {

        }

        protected virtual void PortalButtonClick()
        {
            UIManager.ShowPopup<PortalPopup>();
        }

        protected virtual void UlviCaveButtonClick()
        {
            UIManager.ShowPopup<CavePopup>();
        }

        protected virtual void FayeCaveButtonClick()
        {

        }

        protected virtual void FionaCaveButtonClick()
        {

        }

        protected virtual void JadeCaveButtonClick()
        {

        }

        protected virtual void YuiCaveButtonClick()
        {

        }

        protected virtual void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }
    }

}
