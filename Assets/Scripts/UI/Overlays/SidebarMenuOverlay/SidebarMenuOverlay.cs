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
        protected Transform castleButton_Markers;
        protected Transform castleButton_MainQuestMark;
        protected Transform castleButton_SideQuestMark;
        protected Transform castleButton_EventMark1;
        protected Transform castleButton_EventMark2;
        protected Transform castleButton_EventMark3;
        protected TextMeshProUGUI castleButton_Title;
        protected Image castleButton_Icon;

        protected Button portalButton;
        protected Transform portalButton_Markers;
        protected Transform portalButton_MainQuestMark;
        protected Transform portalButton_SideQuestMark;
        protected Transform portalButton_EventMark1;
        protected Transform portalButton_EventMark2;
        protected Transform portalButton_EventMark3;
        protected TextMeshProUGUI portalButton_Title;
        protected Image portalButton_Icon;

        protected Button globalMapButton;
        protected Transform globalMapButton_Markers;
        protected Transform globalMapButton_MainQuestMark;
        protected Transform globalMapButton_SideQuestMark;
        protected Transform globalMapButton_EventMark1;
        protected Transform globalMapButton_EventMark2;
        protected Transform globalMapButton_EventMark3;
        protected TextMeshProUGUI globalMapButton_Title;
        protected Image globalMapButton_Icon;

        protected Button haremButton;
        protected Transform haremButton_Markers;
        protected Transform haremButton_MainQuestMark;
        protected Transform haremButton_SideQuestMark;
        protected Transform haremButton_EventMark1;
        protected Transform haremButton_EventMark2;
        protected Transform haremButton_EventMark3;
        protected TextMeshProUGUI haremButton_Title;
        protected Image haremButton_Icon;

        protected Button castleBuildingButton;
        protected Transform castleBuildingButton_Markers;
        protected Transform castleBuildingButton_MainQuestMark;
        protected Transform castleBuildingButton_SideQuestMark;
        protected Transform castleBuildingButton_EventMark1;
        protected Transform castleBuildingButton_EventMark2;
        protected Transform castleBuildingButton_EventMark3;
        protected TextMeshProUGUI castleBuildingButton_Title;
        protected Image castleBuildingButton_Icon;

        protected Button magicGuildButton;
        protected Transform magicGuildButton_Markers;
        protected Transform magicGuildButton_MainQuestMark;
        protected Transform magicGuildButton_SideQuestMark;
        protected Transform magicGuildButton_EventMark1;
        protected Transform magicGuildButton_EventMark2;
        protected Transform magicGuildButton_EventMark3;
        protected TextMeshProUGUI magicGuildButton_Title;
        protected Image magicGuildButton_Icon;

        protected Button marketButton;
        protected Transform marketButton_Markers;
        protected Transform marketButton_TimeLimitMark;
        protected Transform marketButton_NewItemMark;
        protected Transform marketButton_SaleMark;
        protected TextMeshProUGUI marketButton_Title;
        protected Image marketButton_Icon;

        protected Button forgeButton;
        protected Transform forgeButton_Markers;
        protected Transform forgeButton_MainQuestMark;
        protected Transform forgeButton_SideQuestMark;
        protected Transform forgeButton_EventMark1;
        protected Transform forgeButton_EventMark2;
        protected Transform forgeButton_EventMark3;
        protected TextMeshProUGUI forgeButton_Title;
        protected Image forgeButton_Icon;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/SidebarMenuOverlay/SidebarMenuOverlay", transform);

            var canvas = screenInst.transform.Find("Canvas");

            castleButton = canvas.Find("CastleButton").GetComponent<Button>();
            castleButton.onClick.AddListener(CastleButtonClick);
            castleButton_Markers = castleButton.transform.Find("Markers");
            castleButton_MainQuestMark = castleButton_Markers.Find("MainQuestMark");
            castleButton_SideQuestMark = castleButton_Markers.Find("SideQuestMark");
            castleButton_EventMark1 = castleButton_Markers.Find("EventMark1");
            castleButton_EventMark2 = castleButton_Markers.Find("EventMark2");
            castleButton_EventMark3 = castleButton_Markers.Find("EventMark3");
            castleButton_Title = castleButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            castleButton_Icon = castleButton.transform.Find("Icon").GetComponent<Image>();

            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);
            portalButton_Markers = portalButton.transform.Find("Markers");
            portalButton_MainQuestMark = portalButton_Markers.Find("MainQuestMark");
            portalButton_SideQuestMark = portalButton_Markers.Find("SideQuestMark");
            portalButton_EventMark1 = portalButton_Markers.Find("EventMark1");
            portalButton_EventMark2 = portalButton_Markers.Find("EventMark2");
            portalButton_EventMark3 = portalButton_Markers.Find("EventMark3");
            portalButton_Title = portalButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            portalButton_Icon = portalButton.transform.Find("Icon").GetComponent<Image>();

            globalMapButton = canvas.Find("GlobalMapButton").GetComponent<Button>();
            globalMapButton.onClick.AddListener(GlobalMapButtonClick);
            globalMapButton_Markers = globalMapButton.transform.Find("Markers");
            globalMapButton_MainQuestMark = globalMapButton_Markers.Find("MainQuestMark");
            globalMapButton_SideQuestMark = globalMapButton_Markers.Find("SideQuestMark");
            globalMapButton_EventMark1 = globalMapButton_Markers.Find("EventMark1");
            globalMapButton_EventMark2 = globalMapButton_Markers.Find("EventMark2");
            globalMapButton_EventMark3 = globalMapButton_Markers.Find("EventMark3");
            globalMapButton_Title = globalMapButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            globalMapButton_Icon = globalMapButton.transform.Find("Icon").GetComponent<Image>();

            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);
            haremButton_Markers = haremButton.transform.Find("Markers");
            haremButton_MainQuestMark = haremButton_Markers.Find("MainQuestMark");
            haremButton_SideQuestMark = haremButton_Markers.Find("SideQuestMark");
            haremButton_EventMark1 = haremButton_Markers.Find("EventMark1");
            haremButton_EventMark2 = haremButton_Markers.Find("EventMark2");
            haremButton_EventMark3 = haremButton_Markers.Find("EventMark3");
            haremButton_Title = haremButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            haremButton_Icon = haremButton.transform.Find("Icon").GetComponent<Image>();

            castleBuildingButton = canvas.Find("CastleBuildingButton").GetComponent<Button>();
            castleBuildingButton.onClick.AddListener(CastleBuildingButtonClick);
            castleBuildingButton_Markers = castleBuildingButton.transform.Find("Markers");
            castleBuildingButton_MainQuestMark = castleBuildingButton_Markers.Find("MainQuestMark");
            castleBuildingButton_SideQuestMark = castleBuildingButton_Markers.Find("SideQuestMark");
            castleBuildingButton_EventMark1 = castleBuildingButton_Markers.Find("EventMark1");
            castleBuildingButton_EventMark2 = castleBuildingButton_Markers.Find("EventMark2");
            castleBuildingButton_EventMark3 = castleBuildingButton_Markers.Find("EventMark3");
            castleBuildingButton_Title = castleBuildingButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            castleBuildingButton_Icon = castleBuildingButton.transform.Find("Icon").GetComponent<Image>();

            magicGuildButton = canvas.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);
            magicGuildButton_Markers = magicGuildButton.transform.Find("Markers");
            magicGuildButton_MainQuestMark = magicGuildButton_Markers.Find("MainQuestMark");
            magicGuildButton_SideQuestMark = magicGuildButton_Markers.Find("SideQuestMark");
            magicGuildButton_EventMark1 = magicGuildButton_Markers.Find("EventMark1");
            magicGuildButton_EventMark2 = magicGuildButton_Markers.Find("EventMark2");
            magicGuildButton_EventMark3 = magicGuildButton_Markers.Find("EventMark3");
            magicGuildButton_Title = magicGuildButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            magicGuildButton_Icon = magicGuildButton.transform.Find("Icon").GetComponent<Image>();

            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);
            marketButton_Markers = marketButton.transform.Find("Markers");
            marketButton_TimeLimitMark = marketButton_Markers.Find("TimeLimitMark");
            marketButton_NewItemMark = marketButton_Markers.Find("NewItemMark");
            marketButton_SaleMark = marketButton_Markers.Find("SaleMark");
            marketButton_Title = marketButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            marketButton_Icon = marketButton.transform.Find("Icon").GetComponent<Image>();

            forgeButton = canvas.Find("ForgeButton").GetComponent<Button>();
            forgeButton.onClick.AddListener(ForgeButtonClick);
            forgeButton_Markers = forgeButton.transform.Find("Markers");
            forgeButton_MainQuestMark = forgeButton_Markers.Find("MainQuestMark");
            forgeButton_SideQuestMark = forgeButton_Markers.Find("SideQuestMark");
            forgeButton_EventMark1 = forgeButton_Markers.Find("EventMark1");
            forgeButton_EventMark2 = forgeButton_Markers.Find("EventMark2");
            forgeButton_EventMark3 = forgeButton_Markers.Find("EventMark3");
            forgeButton_Title = forgeButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            forgeButton_Icon = forgeButton.transform.Find("Icon").GetComponent<Image>();
        }

        public override void StartShow()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.SidebarOverlayOn);
        }

        public override void StartHide()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.SidebarOverlayOff);
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
            UIManager.ShowScreen<CastleScreen>();
        }

        protected virtual void PortalButtonClick()
        {
            UIManager.ShowScreen<PortalScreen>();
        }

        protected virtual void GlobalMapButtonClick()
        {
            UIManager.ShowScreen<MapScreen>();
        }

        protected virtual void HaremButtonClick()
        {
            UIManager.ShowScreen<HaremScreen>();
        }

        protected virtual void CastleBuildingButtonClick()
        {
            UIManager.ShowScreen<BuildingScreen>();
        }

        protected virtual void MagicGuildButtonClick()
        {
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
