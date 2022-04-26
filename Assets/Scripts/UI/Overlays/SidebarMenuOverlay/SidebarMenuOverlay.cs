using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SidebarMenuOverlay : BaseOverlay
    {
        protected bool isTransitionStarted = false;
        
        protected Button castleButton;
        protected TextMeshProUGUI castleButton_Markers;
        protected TextMeshProUGUI castleButton_Title;
        protected Image castleButton_Icon;

        protected Button portalButton;
        protected TextMeshProUGUI portalButton_Markers;
        protected TextMeshProUGUI portalButton_Title;
        protected Image portalButton_Icon;

        protected Button globalMapButton;
        protected TextMeshProUGUI globalMapButton_Markers;
        protected TextMeshProUGUI globalMapButton_Title;
        protected Image globalMapButton_Icon;

        protected Button haremButton;
        protected TextMeshProUGUI haremButton_Markers;
        protected TextMeshProUGUI haremButton_Title;
        protected Image haremButton_Icon;

        protected Button castleBuildingButton;
        protected TextMeshProUGUI castleBuildingButton_Markers;
        protected TextMeshProUGUI castleBuildingButton_Title;
        protected Image castleBuildingButton_Icon;

        protected Button magicGuildButton;
        protected TextMeshProUGUI magicGuildButton_Markers;
        protected TextMeshProUGUI magicGuildButton_Title;
        protected Image magicGuildButton_Icon;

        protected Button marketButton;
        protected TextMeshProUGUI marketButton_Markers;
        protected TextMeshProUGUI marketButton_Title;
        protected Image marketButton_Icon;

        protected Button forgeButton;
        protected TextMeshProUGUI forgeButton_Markers;
        protected TextMeshProUGUI forgeButton_Title;
        protected Image forgeButton_Icon;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/SidebarMenuOverlay/SidebarMenuOverlay", transform);

            var canvas = screenInst.transform.Find("Canvas");

            castleButton = canvas.Find("CastleButton").GetComponent<Button>();
            castleButton.onClick.AddListener(CastleButtonClick);
            castleButton_Markers = castleButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            castleButton_Title = castleButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            castleButton_Icon = castleButton.transform.Find("Icon").GetComponent<Image>();

            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);
            portalButton_Markers = portalButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            portalButton_Title = portalButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            portalButton_Icon = portalButton.transform.Find("Icon").GetComponent<Image>();

            globalMapButton = canvas.Find("GlobalMapButton").GetComponent<Button>();
            globalMapButton.onClick.AddListener(GlobalMapButtonClick);
            globalMapButton_Markers = globalMapButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            globalMapButton_Title = globalMapButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            globalMapButton_Icon = globalMapButton.transform.Find("Icon").GetComponent<Image>();

            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);
            haremButton_Markers = haremButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            haremButton_Title = haremButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            haremButton_Icon = haremButton.transform.Find("Icon").GetComponent<Image>();

            castleBuildingButton = canvas.Find("CastleBuildingButton").GetComponent<Button>();
            castleBuildingButton.onClick.AddListener(CastleBuildingButtonClick);
            castleBuildingButton_Markers = castleBuildingButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            castleBuildingButton_Title = castleBuildingButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            castleBuildingButton_Icon = castleBuildingButton.transform.Find("Icon").GetComponent<Image>();

            magicGuildButton = canvas.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);
            magicGuildButton_Markers = magicGuildButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            magicGuildButton_Title = magicGuildButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            magicGuildButton_Icon = magicGuildButton.transform.Find("Icon").GetComponent<Image>();

            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);
            marketButton_Markers = marketButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            marketButton_Title = marketButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            marketButton_Icon = marketButton.transform.Find("Icon").GetComponent<Image>();

            forgeButton = canvas.Find("ForgeButton").GetComponent<Button>();
            forgeButton.onClick.AddListener(ForgeButtonClick);
            forgeButton_Markers = forgeButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            forgeButton_Title = forgeButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            forgeButton_Icon = forgeButton.transform.Find("Icon").GetComponent<Image>();
        }

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_SidebarOverlayShow);
        }

        public override void StartHide()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_SidebarOverlayHide);
        }

        private void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {

        }

        protected virtual void CastleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        protected virtual void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }

        protected virtual void GlobalMapButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MapScreen>();
        }

        protected virtual void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        protected virtual void CastleBuildingButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<BuildingScreen>();
        }

        protected virtual void MagicGuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        protected virtual void MarketButtonClick()
        {
            // UIManager.ShowScreen<MarketScreen>();
        }

        protected virtual void ForgeButtonClick()
        {
            // UIManager.ShowScreen<ForgeScreen>();
        }
    }
}
