using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMapScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/EventMap"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("BackButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            screenRectTransform.Find("Canvas").Find("MapButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<MapScreen>();
            });

            screenRectTransform.Find("Canvas").Find("EventShopButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<EventMarketScreen>();
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
