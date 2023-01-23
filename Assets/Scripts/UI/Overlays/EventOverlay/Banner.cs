using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class Banner : BaseWidget
        {
            public int eventId { get; set; }
            public AdminBRO.EventItem eventData =>
                GameData.events.GetEventById(eventId);

            private Image banner;
            private Button marketButton;
            private Button buyButton;
            private TextMeshProUGUI timer;
            private Button moreCurrencyButton;
            private TextMeshProUGUI countTitle;

            protected override void Awake()
            {
                base.Awake();

                var canvas = transform.Find("Canvas");
                banner = canvas.Find("Banner").GetComponent<Image>();
                timer = canvas.Find("TimerBack/Timer").GetComponent<TextMeshProUGUI>();
                buyButton = canvas.Find("BuyButton").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                marketButton = canvas.Find("MarketButton").GetComponent<Button>();
                marketButton.onClick.AddListener(MarketButtonClick);

                moreCurrencyButton = canvas.Find("MoreCurrencyButton").GetComponent<Button>();
                moreCurrencyButton.onClick.AddListener(MoreCurrencyButtonClick);
                countTitle = moreCurrencyButton.transform.Find("CountTitle").GetComponent<TextMeshProUGUI>();

                Stretch();
            }

            void Start()
            {
                var eData = eventData;

                banner.sprite = ResourceManager.LoadSprite(eData?.overlayBannerImage);
                timer.text = UITools.IncNumberSizeTo(eData?.timePeriodLeft, 50f);
                SetCountTitle();
            }

            private void MarketButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowOverlay<MarketOverlay>();
            }
            
            private void BuyButtonClick()
            {

            }

            private void MoreCurrencyButtonClick()
            {

            }

            private void SetCountTitle()
            {
                var title = "You have";
                foreach (var cd in eventData.currenciesData)
                {
                    title = $"{title} {cd?.tmpSprite}{cd?.walletInfo?.amount}";
                }
                countTitle.text = title;
            }

            private void Stretch()
            {
                var rectTr = gameObject.GetComponent<RectTransform>();
                var parentSize = transform.parent.GetComponent<RectTransform>().rect.size;
                
                rectTr.sizeDelta = new Vector2(parentSize.x, rectTr.rect.size.y);
            }

            public override void OnGameDataEvent(GameDataEvent eventData)
            {
                switch (eventData.id)
                {
                    case GameDataEventId.WalletChangeState:
                        SetCountTitle();
                        break;
                }
            }

            public static Banner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Banner>
                    ("Prefabs/UI/Overlays/EventOverlay/Banner", parent);
            }
        }
    }
}
