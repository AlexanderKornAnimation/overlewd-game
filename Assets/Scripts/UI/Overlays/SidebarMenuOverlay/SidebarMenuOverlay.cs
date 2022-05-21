using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        protected Button overlordButton;
        protected TextMeshProUGUI overlordButton_Markers;
        protected TextMeshProUGUI overlordButton_Title;
        protected Image overlordButton_Icon;
        
        protected Button haremButton;
        protected TextMeshProUGUI haremButton_Markers;
        protected TextMeshProUGUI haremButton_Title;
        protected Image haremButton_Icon;

        protected Button municipalityButton;
        protected TextMeshProUGUI municipalityButton_Markers;
        protected TextMeshProUGUI municipalityButton_Title;
        protected Image municipalityButton_Icon;

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

        private SidebarMenuOverayInData inputData;

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

            overlordButton = canvas.Find("OverlordButton").GetComponent<Button>();
            overlordButton.onClick.AddListener(OverlordButtonClick);
            overlordButton_Markers = overlordButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            overlordButton_Title = overlordButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            overlordButton_Icon = overlordButton.transform.Find("Icon").GetComponent<Image>();
            
            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);
            haremButton_Markers = haremButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            haremButton_Title = haremButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            haremButton_Icon = haremButton.transform.Find("Icon").GetComponent<Image>();

            municipalityButton = canvas.Find("MunicipalityButton").GetComponent<Button>();
            municipalityButton.onClick.AddListener(MunicipalityButtonClick);
            municipalityButton_Markers = municipalityButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            municipalityButton_Title = municipalityButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            municipalityButton_Icon = municipalityButton.transform.Find("Icon").GetComponent<Image>();

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

        public SidebarMenuOverlay SetData(SidebarMenuOverayInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            switch (GameData.ftue.stats.lastEndedState)
            {
                case ("battle4", "chapter1"):
                    if (GameData.buildings.castle.isBuilt)
                    {
                        UITools.DisableButton(castleButton);
                        UITools.DisableButton(portalButton);
                        //UITools.DisableButton(globalMapButton);
                        UITools.DisableButton(overlordButton);
                        UITools.DisableButton(haremButton);
                        UITools.DisableButton(municipalityButton);
                        UITools.DisableButton(magicGuildButton);
                        UITools.DisableButton(marketButton);
                        UITools.DisableButton(forgeButton);
                    }
                    break;
            }

            await Task.CompletedTask;
        }

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_SidebarOverlayShow);
        }

        public override void StartHide()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_SidebarOverlayHide);
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

        protected virtual void OverlordButtonClick()
        {
            
        }
        
        protected virtual void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<HaremScreen>();
        }

        protected virtual void MunicipalityButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MunicipalityScreen>();
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

    public class SidebarMenuOverayInData : BaseScreenInData
    {

    }
}
