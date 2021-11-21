using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SidebarMenuOverlay : BaseOverlay
    {
        private Button castleButton;
        private Transform castleButton_MainQuestMark;
        private Transform castleButton_SideQuestMark;
        private Transform castleButton_EventMark1;
        private Transform castleButton_EventMark2;
        private Transform castleButton_EventMark3;
        private Text castleButton_Title;
        private Image castleButton_Icon;

        private Button portalButton;
        private Transform portalButton_MainQuestMark;
        private Transform portalButton_SideQuestMark;
        private Transform portalButton_EventMark1;
        private Transform portalButton_EventMark2;
        private Transform portalButton_EventMark3;
        private Text portalButton_Title;
        private Image portalButton_Icon;

        private Button globalMapButton;
        private Transform globalMapButton_MainQuestMark;
        private Transform globalMapButton_SideQuestMark;
        private Transform globalMapButton_EventMark1;
        private Transform globalMapButton_EventMark2;
        private Transform globalMapButton_EventMark3;
        private Text globalMapButton_Title;
        private Image globalMapButton_Icon;

        private Button haremButton;
        private Transform haremButton_MainQuestMark;
        private Transform haremButton_SideQuestMark;
        private Transform haremButton_EventMark1;
        private Transform haremButton_EventMark2;
        private Transform haremButton_EventMark3;
        private Text haremButton_Title;
        private Image haremButton_Icon;

        private Button castleBuildingButton;
        private Transform castleBuildingButton_MainQuestMark;
        private Transform castleBuildingButton_SideQuestMark;
        private Transform castleBuildingButton_EventMark1;
        private Transform castleBuildingButton_EventMark2;
        private Transform castleBuildingButton_EventMark3;
        private Text castleBuildingButton_Title;
        private Image castleBuildingButton_Icon;

        private Button magicGuildButton;
        private Transform magicGuildButton_MainQuestMark;
        private Transform magicGuildButton_SideQuestMark;
        private Transform magicGuildButton_EventMark1;
        private Transform magicGuildButton_EventMark2;
        private Transform magicGuildButton_EventMark3;
        private Text magicGuildButton_Title;
        private Image magicGuildButton_Icon;

        private Button marketButton;
        private Transform marketButton_TimeLimitMark;
        private Transform marketButton_NewItemMark;
        private Transform marketButton_SaleMark;
        private Text marketButton_Title;
        private Image marketButton_Icon;

        private Button forgeButton;
        private Transform forgeButton_MainQuestMark;
        private Transform forgeButton_SideQuestMark;
        private Transform forgeButton_EventMark1;
        private Transform forgeButton_EventMark2;
        private Transform forgeButton_EventMark3;
        private Text forgeButton_Title;
        private Image forgeButton_Icon;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/SidebarMenuOverlay/SidebarMenuOverlay"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            castleButton = canvas.Find("CastleButton").GetComponent<Button>();
            castleButton.onClick.AddListener(CastleButtonClick);
            var castleButton_Markers = castleButton.transform.Find("Markers");
            castleButton_MainQuestMark = castleButton_Markers.Find("MainQuestMark");
            castleButton_SideQuestMark = castleButton_Markers.Find("SideQuestMark");
            castleButton_EventMark1 = castleButton_Markers.Find("EventMark1");
            castleButton_EventMark2 = castleButton_Markers.Find("EventMark2");
            castleButton_EventMark3 = castleButton_Markers.Find("EventMark3");
            castleButton_Title = castleButton.transform.Find("Title").GetComponent<Text>();
            castleButton_Icon = castleButton.transform.Find("Icon").GetComponent<Image>();

            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);
            var portalButton_Markers = portalButton.transform.Find("Markers");
            portalButton_MainQuestMark = portalButton_Markers.Find("MainQuestMark");
            portalButton_SideQuestMark = portalButton_Markers.Find("SideQuestMark");
            portalButton_EventMark1 = portalButton_Markers.Find("EventMark1");
            portalButton_EventMark2 = portalButton_Markers.Find("EventMark2");
            portalButton_EventMark3 = portalButton_Markers.Find("EventMark3");
            portalButton_Title = portalButton.transform.Find("Title").GetComponent<Text>();
            portalButton_Icon = portalButton.transform.Find("Icon").GetComponent<Image>();

            globalMapButton = canvas.Find("GlobalMapButton").GetComponent<Button>();
            globalMapButton.onClick.AddListener(GlobalMapButtonClick);
            var globalMapButton_Markers = globalMapButton.transform.Find("Markers");
            globalMapButton_MainQuestMark = globalMapButton_Markers.Find("MainQuestMark");
            globalMapButton_SideQuestMark = globalMapButton_Markers.Find("SideQuestMark");
            globalMapButton_EventMark1 = globalMapButton_Markers.Find("EventMark1");
            globalMapButton_EventMark2 = globalMapButton_Markers.Find("EventMark2");
            globalMapButton_EventMark3 = globalMapButton_Markers.Find("EventMark3");
            globalMapButton_Title = globalMapButton.transform.Find("Title").GetComponent<Text>();
            globalMapButton_Icon = globalMapButton.transform.Find("Icon").GetComponent<Image>();

            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);
            var haremButton_Markers = haremButton.transform.Find("Markers");
            haremButton_MainQuestMark = haremButton_Markers.Find("MainQuestMark");
            haremButton_SideQuestMark = haremButton_Markers.Find("SideQuestMark");
            haremButton_EventMark1 = haremButton_Markers.Find("EventMark1");
            haremButton_EventMark2 = haremButton_Markers.Find("EventMark2");
            haremButton_EventMark3 = haremButton_Markers.Find("EventMark3");
            haremButton_Title = haremButton.transform.Find("Title").GetComponent<Text>();
            haremButton_Icon = haremButton.transform.Find("Icon").GetComponent<Image>();

            castleBuildingButton = canvas.Find("CastleBuildingButton").GetComponent<Button>();
            castleBuildingButton.onClick.AddListener(CastleBuildingButtonClick);
            var castleBuildingButton_Markers = castleBuildingButton.transform.Find("Markers");
            castleBuildingButton_MainQuestMark = castleBuildingButton_Markers.Find("MainQuestMark");
            castleBuildingButton_SideQuestMark = castleBuildingButton_Markers.Find("SideQuestMark");
            castleBuildingButton_EventMark1 = castleBuildingButton_Markers.Find("EventMark1");
            castleBuildingButton_EventMark2 = castleBuildingButton_Markers.Find("EventMark2");
            castleBuildingButton_EventMark3 = castleBuildingButton_Markers.Find("EventMark3");
            castleBuildingButton_Title = castleBuildingButton.transform.Find("Title").GetComponent<Text>();
            castleBuildingButton_Icon = castleBuildingButton.transform.Find("Icon").GetComponent<Image>();

            magicGuildButton = canvas.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);
            var magicGuildButton_Markers = magicGuildButton.transform.Find("Markers");
            magicGuildButton_MainQuestMark = magicGuildButton_Markers.Find("MainQuestMark");
            magicGuildButton_SideQuestMark = magicGuildButton_Markers.Find("SideQuestMark");
            magicGuildButton_EventMark1 = magicGuildButton_Markers.Find("EventMark1");
            magicGuildButton_EventMark2 = magicGuildButton_Markers.Find("EventMark2");
            magicGuildButton_EventMark3 = magicGuildButton_Markers.Find("EventMark3");
            magicGuildButton_Title = magicGuildButton.transform.Find("Title").GetComponent<Text>();
            magicGuildButton_Icon = magicGuildButton.transform.Find("Icon").GetComponent<Image>();

            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);
            var marketButton_Markers = marketButton.transform.Find("Markers");
            marketButton_TimeLimitMark = marketButton_Markers.Find("TimeLimitMark");
            marketButton_NewItemMark = marketButton_Markers.Find("NewItemMark");
            marketButton_SaleMark = marketButton_Markers.Find("SaleMark");
            marketButton_Title = marketButton.transform.Find("Title").GetComponent<Text>();
            marketButton_Icon = marketButton.transform.Find("Icon").GetComponent<Image>();

            forgeButton = canvas.Find("ForgeButton").GetComponent<Button>();
            forgeButton.onClick.AddListener(ForgeButtonClick);
            var forgeButton_Markers = forgeButton.transform.Find("Markers");
            forgeButton_MainQuestMark = forgeButton_Markers.Find("MainQuestMark");
            forgeButton_SideQuestMark = forgeButton_Markers.Find("SideQuestMark");
            forgeButton_EventMark1 = forgeButton_Markers.Find("EventMark1");
            forgeButton_EventMark2 = forgeButton_Markers.Find("EventMark2");
            forgeButton_EventMark3 = forgeButton_Markers.Find("EventMark3");
            forgeButton_Title = forgeButton.transform.Find("Title").GetComponent<Text>();
            forgeButton_Icon = forgeButton.transform.Find("Icon").GetComponent<Image>();

            Customize();
        }

        private void Customize()
        {

        }

        private void CastleButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }

        private void PortalButtonClick()
        {
            UIManager.ShowScreen<PortalScreen>();
        }

        private void GlobalMapButtonClick()
        {
            UIManager.ShowScreen<MapScreen>();
        }

        private void HaremButtonClick()
        {
            UIManager.ShowScreen<HaremScreen>();
        }

        private void CastleBuildingButtonClick()
        {
            UIManager.ShowScreen<BuildingScreen>();
        }

        private void MagicGuildButtonClick()
        {
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        private void MarketButtonClick()
        {
            UIManager.ShowScreen<MarketScreen>();
        }

        private void ForgeButtonClick()
        {
            UIManager.ShowScreen<ForgeScreen>();
        }
    }
}
