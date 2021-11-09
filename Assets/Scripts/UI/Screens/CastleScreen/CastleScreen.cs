using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CastleScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/Castle"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("Market").GetComponent<Button>().onClick.AddListener(() => 
            {
                UIManager.ShowScreen<MarketScreen>();
            });

            screenRectTransform.Find("Canvas").Find("GlobalMap").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<NewMapScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Harem").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<HaremScreen>();
            });

            screenRectTransform.Find("Canvas").Find("UserSettings").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<UserSettingsScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Inventory").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<InventoryAndUserScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Forge").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<ForgeScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Portal").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<PortalScreen>();
            });

            screenRectTransform.Find("Canvas").Find("MagicGuild").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<MagicGuildScreen>();
            });

            screenRectTransform.Find("Canvas").Find("ContentViewer").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<DebugContentViewer>();
            });

            EventsWidget.CreateInstance(transform);
            QuestsWidget.CreateInstance(transform);
            SidebarButtonWidget.CreateInstance(transform);
        }

        void Update()
        {
            
        }
    }
}
