using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            private Button marketButton;
            private Button buyButton;
            private TextMeshProUGUI timer;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                banner = canvas.Find("Banner").GetComponent<Image>();
                timer = canvas.Find("TimerBack/Timer").GetComponent<TextMeshProUGUI>();
                buyButton = canvas.Find("BuyButton").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                marketButton = canvas.Find("MarketButton").GetComponent<Button>();
                marketButton.onClick.AddListener(MarketButtonClick);
                Stretch();
            }

            private void Start()
            {
                var eData = eventData;

                banner.sprite = ResourceManager.LoadSprite(eData?.mapBannerImage);
                timer.text = UITools.IncNumberSizeTo(eData?.timePeriodLeft, 50f);
            }

            private void MarketButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowOverlay<MarketOverlay>();
            }
            
            private void BuyButtonClick()
            {

            }

            private void Stretch()
            {
                var rectTr = gameObject.GetComponent<RectTransform>();
                var parentSize = transform.parent.GetComponent<RectTransform>().rect.size;
                
                rectTr.sizeDelta = new Vector2(parentSize.x, rectTr.rect.size.y);
            }
            
            public static Banner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Banner>
                    ("Prefabs/UI/Overlays/EventOverlay/Banner", parent);
            }
        }
    }
}
