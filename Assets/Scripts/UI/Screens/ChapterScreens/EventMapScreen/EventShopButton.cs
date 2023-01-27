using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class EventShopButton : BaseButton
        {
            public int? marketId { get; set; }
            public AdminBRO.MarketItem marketData =>
                GameData.markets.GetMarketById(marketId);

            private TextMeshProUGUI description;

            protected override void Awake()
            {
                base.Awake();
                description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                title.text = marketData.name;
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeOverlay<EventMarketOverlay>().
                    SetData(new EventMarketOverlayInData
                    {
                        eventId = eventId,
                        marketId = marketId
                    }).DoShow();
            }

            public static EventShopButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventShopButton>
                    ("Prefabs/UI/Screens/ChapterScreens/EventShopButton", parent);
            }
        }
    }
}
