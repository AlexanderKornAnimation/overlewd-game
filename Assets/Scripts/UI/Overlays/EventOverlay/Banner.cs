using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class Banner : MonoBehaviour
        {
            public int eventId { get; set; }
            public AdminBRO.EventItem eventData =>
                GameData.events.GetEventById(eventId);

            private Image banner;
            private Button buyButton;

            void Awake()
            {
                var canvas = transform.Find("Canvas");
                banner = canvas.Find("Banner").GetComponent<Image>();
                buyButton = canvas.Find("BuyButton").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
            }

            void Start()
            {
                var eData = eventData;

                banner.sprite = ResourceManager.LoadSprite(eData?.mapBannerImage);
            }

            private void BuyButtonClick()
            {

            }

            public static Banner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Banner>
                    ("Prefabs/UI/Overlays/EventOverlay/Banner", parent);
            }
        }
    }
}
