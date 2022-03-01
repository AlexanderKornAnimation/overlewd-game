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
        protected GameObject municipalityUnaviable;
        protected GameObject municipalityMaxLevel;

        protected Button forgeButton;
        protected GameObject forgeUnaviable;
        protected GameObject forgeMaxLevel;

        protected Button magicGuildButton;
        protected GameObject magicGuildUnaviable;
        protected GameObject magicGuildMaxLevel;

        protected Button marketButton;
        protected GameObject marketUnaviable;
        protected GameObject marketMaxLevel;

        protected Button portalButton;
        protected GameObject portalUnaviable;
        protected GameObject portalMaxLevel;

        protected Button ulviCaveButton;
        protected GameObject ulviCaveUnaviable;
        protected GameObject ulviCaveMaxLevel;

        protected Button fayeCaveButton;
        protected GameObject fayeCaveUnaviable;
        protected GameObject fayeCaveMaxLevel;

        protected Button fionaCaveButton;
        protected GameObject fionaCaveUnaviable;
        protected GameObject fionaCaveMaxLevel;

        protected Button jadeCaveButton;
        protected GameObject jadeCaveUnaviable;
        protected GameObject jadeCaveMaxLevel;

        protected Button yuiCaveButton;
        protected GameObject yuiCaveUnaviable;
        protected GameObject yuiCaveMaxLevel;

        protected Button backButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/BuildingScreen/BuildingScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var grid = canvas.Find("Grid");

            municipalityButton = grid.Find("MunicipalityButton").GetComponent<Button>();
            municipalityUnaviable = municipalityButton.transform.Find("Unaviable").gameObject;
            municipalityMaxLevel =  municipalityButton.transform.Find("MaxLevel").gameObject;

            forgeButton = grid.Find("ForgeButton").GetComponent<Button>();
            forgeUnaviable = forgeButton.transform.Find("Unaviable").gameObject;
            forgeMaxLevel = forgeButton.transform.Find("MaxLevel").gameObject;

            magicGuildButton = grid.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildUnaviable = magicGuildButton.transform.Find("Unaviable").gameObject;
            magicGuildMaxLevel = magicGuildButton.transform.Find("MaxLevel").gameObject;

            marketButton = grid.Find("MarketButton").GetComponent<Button>();
            marketUnaviable = marketButton.transform.Find("Unaviable").gameObject;
            marketMaxLevel = marketButton.transform.Find("MaxLevel").gameObject;

            portalButton = grid.Find("PortalButton").GetComponent<Button>();
            portalUnaviable = portalButton.transform.Find("Unaviable").gameObject;
            portalMaxLevel = portalButton.transform.Find("MaxLevel").gameObject;

            ulviCaveButton = grid.Find("UlviCaveButton").GetComponent<Button>();
            ulviCaveUnaviable = ulviCaveButton.transform.Find("Unaviable").gameObject;
            ulviCaveMaxLevel = ulviCaveButton.transform.Find("MaxLevel").gameObject;

            fayeCaveButton = grid.Find("FayeCaveButton").GetComponent<Button>();
            fayeCaveUnaviable = fayeCaveButton.transform.Find("Unaviable").gameObject;
            fayeCaveMaxLevel = fayeCaveButton.transform.Find("MaxLevel").gameObject;

            fionaCaveButton = grid.Find("FionaCaveButton").GetComponent<Button>();
            fionaCaveUnaviable = fionaCaveButton.transform.Find("Unaviable").gameObject;
            fionaCaveMaxLevel = fionaCaveButton.transform.Find("MaxLevel").gameObject;

            jadeCaveButton = grid.Find("JadeCaveButton").GetComponent<Button>();
            jadeCaveUnaviable = jadeCaveButton.transform.Find("Unaviable").gameObject;
            jadeCaveMaxLevel = jadeCaveButton.transform.Find("MaxLevel").gameObject;

            yuiCaveButton = grid.Find("YuiCaveButton").GetComponent<Button>();
            yuiCaveUnaviable = yuiCaveButton.transform.Find("Unaviable").gameObject;
            yuiCaveMaxLevel = yuiCaveButton.transform.Find("MaxLevel").gameObject;

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

        void Start()
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
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
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
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            UIManager.ShowPopup<PortalPopup>();
        }

        protected virtual void UlviCaveButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
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
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }

}
