using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class Bundle : BaseOffer
        {
            private Image girlIcon;
            private Image promoIcon;
            private Transform profit;
            private TextMeshProUGUI profitTitle;
            private TextMeshProUGUI description;
            private Transform discount;
            private TextMeshProUGUI discountTitle;
            private Transform items;
            private Button buyButton;
            private TextMeshProUGUI buyButtonTitle;

            private AdminBRO.MarketItem.Tab tabData => offerButton.tabData;
            private AdminBRO.TradableItem tradableData => tabData.tradablesData.FirstOrDefault();

            protected override void Awake()
            {
                base.Awake();

                girlIcon = canvas.Find("GirlIcon").GetComponent<Image>();
                var background = canvas.Find("Background");
                promoIcon = background.Find("PromoIcon").GetComponent<Image>();
                description = background.Find("PromoIcon/Description/Title").GetComponent<TextMeshProUGUI>();
                profit = background.Find("Profit");
                profitTitle = profit.Find("Title").GetComponent<TextMeshProUGUI>();
                items = background.Find("Items");
                discount = background.Find("Discount");
                discountTitle = discount.Find("Title").GetComponent<TextMeshProUGUI>();
                buyButton = background.Find("BuyButton").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                buyButtonTitle = buyButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            }

            protected override void Customize()
            {
                var _tabData = tabData;
                var _tradableData = tradableData;

                girlIcon.sprite = ResourceManager.LoadSprite(tabData.promoGirlIcon);
                girlIcon.SetNativeSize();
                promoIcon.sprite = ResourceManager.LoadSprite(tradableData.imageUrl);
                promoIcon.SetNativeSize();
                description.text = _tradableData.description;
                profit.gameObject.SetActive(_tabData.profit.HasValue);
                profitTitle.text = $"profit <size=68>{_tabData.profit}%";
                discount.gameObject.SetActive(!string.IsNullOrEmpty(_tradableData.discount));
                discountTitle.text = _tradableData.discount;
                buyButtonTitle.text = "Buy pack for " + UITools.PriceToString(_tradableData.price);

                foreach (var trItem in _tradableData.itemPack)
                {
                    var bundleItem = BundleItem.GetInstance(items);
                    bundleItem.icon.sprite = ResourceManager.LoadSprite(trItem.tradableData.icon);
                    bundleItem.amount.text = trItem.count.ToString();
                }
            }

            public void BuyButtonClick()
            {

            }

            public static Bundle GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Bundle>("Prefabs/UI/Overlays/MarketOverlay/Bundle",
                    parent);
            }

            private class BundleItem : MonoBehaviour
            {
                public Image icon { get; set; }
                public TextMeshProUGUI amount { get; set; }

                void Awake()
                {
                    icon = GetComponent<Image>();
                    amount = GetComponentInChildren<TextMeshProUGUI>();
                }

                public static BundleItem GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<BundleItem>("Prefabs/UI/Overlays/MarketOverlay/BundleItem", parent);
                }
            }
        }
    }
}