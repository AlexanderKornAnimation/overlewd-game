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
            public int? eventMarketId { get; set; }
            public AdminBRO.EventMarketItem eventMarketData =>
                GameData.markets.GetEventMarketById(eventMarketId);

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
                var _eventMarketData = eventMarketData;

                title.text = eventMarketData.name;
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.MakeScreen<EventMarketScreen>().
                    SetData(new EventMarketScreenInData
                    {
                        eventMarketId = eventMarketId
                    }).RunShowScreenProcess();
            }

            public static EventShopButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventShopButton>
                    ("Prefabs/UI/Screens/ChapterScreens/EventShopButton", parent);
            }
        }
    }
}
