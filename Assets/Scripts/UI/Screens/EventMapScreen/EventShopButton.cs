using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class EventShopButton : MonoBehaviour
        {
            public int eventMarketId;

            private Button button;
            private TextMeshProUGUI title;
            private TextMeshProUGUI description;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var eventMarketData = GameData.GetEventMarketById(eventMarketId);

                title.text = eventMarketData.name;
            }

            private void ButtonClick()
            {
                GameGlobalStates.eventShop_MarketId = eventMarketId;
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                UIManager.ShowScreen<EventMarketScreen>();
            }

            public static EventShopButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventShopButton>
                    ("Prefabs/UI/Screens/EventMapScreen/EventShopButton", parent);
            }
        }
    }
}
