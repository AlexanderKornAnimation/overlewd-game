using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMarketScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMarketScreen/EventMarket"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("BackButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<EventMapScreen>();
            });

            var scrollViewContent = screenRectTransform.Find("Canvas").Find("ScrollView").Find("Viewport").Find("Content");

            NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
            NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
            NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
            NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
            NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
            NSEventMarketScreen.EventMarketItem.GetInstance(scrollViewContent);
        }

        void Update()
        {

        }
    }
}
