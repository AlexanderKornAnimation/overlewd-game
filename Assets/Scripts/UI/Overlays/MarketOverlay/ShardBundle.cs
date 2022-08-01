using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
        public class ShardBundle : BaseOffer
        {
            private Image[] shards = new Image[4];
            private TextMeshProUGUI[] shardsAmount = new TextMeshProUGUI[4];
            private Image art;
            private Image banner;
            private TextMeshProUGUI bannerTitle;
            private Button buyButton;
            private GameObject discountBack;
            private TextMeshProUGUI discount;
            private TextMeshProUGUI buyButtonTitle;

            protected override void Awake()
            {
                base.Awake();
                var background = canvas.Find("Background");
                var grid = background.Find("Grid");

                art = canvas.Find("Art").GetComponent<Image>();
                banner = background.Find("Banner").GetComponent<Image>();
                bannerTitle = banner.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                buyButton = background.Find("BuyButton").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                buyButtonTitle = buyButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                discountBack = buyButton.transform.Find("DiscountBack").gameObject;
                discount = discountBack.transform.Find("Discount").GetComponent<TextMeshProUGUI>();

                for (int i = 0; i < grid.childCount; i++)
                {
                    shards[i] = grid.Find($"Shard{i + 1}").GetComponent<Image>();
                    shardsAmount[i] = shards[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                    shards[i].gameObject.SetActive(false);
                }
                
            }

            private void BuyButtonClick()
            {
                
            }

            public static ShardBundle GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ShardBundle>(
                    "Prefabs/UI/Overlays/MarketOverlay/ShardBundle", parent);
            }
        }
    }
}