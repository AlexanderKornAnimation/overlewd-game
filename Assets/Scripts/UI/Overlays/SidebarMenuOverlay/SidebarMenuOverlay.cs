using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SidebarMenuOverlay : BaseOverlayParent<SidebarMenuOverayInData>
    {
        private Button castleButton;
        private TextMeshProUGUI castleButton_Markers;
        private TextMeshProUGUI castleButton_Title;
        private Image castleButton_Icon;

        private Button portalButton;
        private TextMeshProUGUI portalButton_Markers;
        private TextMeshProUGUI portalButton_Title;
        private Image portalButton_Icon;

        private Button globalMapButton;
        private TextMeshProUGUI globalMapButton_Markers;
        private TextMeshProUGUI globalMapButton_Title;
        private Image globalMapButton_Icon;

        private Button overlordButton;
        private TextMeshProUGUI overlordButton_Markers;
        private TextMeshProUGUI overlordButton_Title;
        private Image overlordButton_Icon;

        private Button haremButton;
        private TextMeshProUGUI haremButton_Markers;
        private TextMeshProUGUI haremButton_Title;
        private Image haremButton_Icon;

        private Button municipalityButton;
        private TextMeshProUGUI municipalityButton_Markers;
        private TextMeshProUGUI municipalityButton_Title;
        private Image municipalityButton_Icon;

        private Button magicGuildButton;
        private TextMeshProUGUI magicGuildButton_Markers;
        private TextMeshProUGUI magicGuildButton_Title;
        private Image magicGuildButton_Icon;

        private Button marketButton;
        private TextMeshProUGUI marketButton_Markers;
        private TextMeshProUGUI marketButton_Title;
        private Image marketButton_Icon;

        private Button laboratoryButton;
        private TextMeshProUGUI laboratory_Markers;
        private TextMeshProUGUI laboratory_Title;
        private Image laboratoryButton_Icon;

        private Button forgeButton;
        private TextMeshProUGUI forgeButton_Markers;
        private TextMeshProUGUI forgeButton_Title;
        private Image forgeButton_Icon;

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

            laboratoryButton = canvas.Find("LaboratoryButton").GetComponent<Button>();
            laboratoryButton.onClick.AddListener(LaboratoryButtonClick);
            laboratory_Markers = laboratoryButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            laboratory_Title = laboratoryButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            laboratoryButton_Icon = laboratoryButton.transform.Find("Icon").GetComponent<Image>();
            
            forgeButton = canvas.Find("ForgeButton").GetComponent<Button>();
            forgeButton.onClick.AddListener(ForgeButtonClick);
            forgeButton_Markers = forgeButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            forgeButton_Title = forgeButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            forgeButton_Icon = forgeButton.transform.Find("Icon").GetComponent<Image>();
        }

        public override async Task BeforeShowMakeAsync()
        {
            UITools.DisableButton(castleButton, UIManager.HasScreen<CastleScreen>());
            UITools.DisableButton(globalMapButton, UIManager.HasScreen<MapScreen>());

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

                default:
                    UITools.DisableButton(portalButton, !GameData.buildings.portal.isBuilt);
                    UITools.DisableButton(haremButton, !GameData.buildings.harem.isBuilt);
                    UITools.DisableButton(magicGuildButton, !GameData.buildings.magicGuild.isBuilt);
                    UITools.DisableButton(laboratoryButton, !GameData.buildings.laboratory.isBuilt);
                    UITools.DisableButton(forgeButton, !GameData.buildings.forge.isBuilt);
                    UITools.DisableButton(overlordButton);
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

        private void CastleButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }

        private void GlobalMapButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MapScreen>();
        }

        private void OverlordButtonClick()
        {
            
        }
        
        private void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<HaremScreen>().
                SetData(new HaremScreenInData
            {
                prevScreenInData = UIManager.prevScreenInData
            }).RunShowScreenProcess();
        }

        private void MunicipalityButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MunicipalityScreen>();
        }

        private void MagicGuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        private void MarketButtonClick()
        {
            UIManager.ShowOverlay<MarketPopup>();
        }

        private void LaboratoryButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<LaboratoryScreen>();
        }
        
        private void ForgeButtonClick()
        {
            // UIManager.ShowScreen<ForgeScreen>();
        }
    }

    public class SidebarMenuOverayInData : BaseOverlayInData
    {

    }
}
