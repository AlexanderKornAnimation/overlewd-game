using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BannerNotification : BaseNotificationParent<BannerNotificationInData>
    {
        private VerticalLayoutGroup resourcesGrid;
        private VerticalLayoutGroup vGrid;
        private HorizontalLayoutGroup hGrid1;
        private HorizontalLayoutGroup hGrid2;

        private Button buyButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Notifications/BannerNotification/BannerNotification", transform);

            var canvas = screenInst.transform.Find("Canvas");

            resourcesGrid = canvas.Find("ResourcesGrid").GetComponent<VerticalLayoutGroup>();
            vGrid = resourcesGrid.transform.Find("vGrid").GetComponent<VerticalLayoutGroup>();
            hGrid1 = vGrid.transform.Find("hGrid1").GetComponent<HorizontalLayoutGroup>();
            hGrid2 = vGrid.transform.Find("hGrid2").GetComponent<HorizontalLayoutGroup>();

            buyButton = canvas.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(BuyButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        private void Customize()
        {
            for (int i = 0; i < 7; i++)
            {
                var rIcon = NSBannerNotification.ResourceIcon.GetInstance(null);
                rIcon.icon = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Backgrounds/BattleGirlLegendary");
                rIcon.caption = "_";
                AddResourceIcon(rIcon);
            }
            
        }

        private void AddResourceIcon(NSBannerNotification.ResourceIcon resourceIcon)
        {
            var iconWidth = resourceIcon.GetComponent<RectTransform>().rect.width;
            var hGrid2Width = hGrid2.GetComponent<RectTransform>().rect.width;

            if (hGrid2Width + iconWidth > 1150.0f)
            {
                Destroy(resourceIcon.gameObject);
                return;
            }

            var hGrid1Width = hGrid1.GetComponent<RectTransform>().rect.width;
            if (hGrid1Width + iconWidth < 1150.0f)
            {
                resourceIcon.transform.SetParent(hGrid1.transform, false);
                LayoutRebuilder.ForceRebuildLayoutImmediate(hGrid1.GetComponent<RectTransform>());
            }
            else
            {
                resourceIcon.transform.SetParent(hGrid2.transform, false);
                LayoutRebuilder.ForceRebuildLayoutImmediate(hGrid2.GetComponent<RectTransform>());
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(resourcesGrid.GetComponent<RectTransform>());
        }
        
        private async void BuyButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await GameData.markets.BuyTradable(inputData.marketId, inputData.tradableId);
            UIManager.ShowNotification<NutakuBuyingNotification>();
        }
    }

    public class BannerNotificationInData : BaseNotificationInData
    {
        public int? marketId { get; set; }
        public AdminBRO.MarketItem marketData =>
            GameData.markets.GetMarketById(marketId);
        public int? tradableId { get; set; }
        public AdminBRO.TradableItem tradableData =>
            GameData.markets.GetTradableById(tradableId.Value);
    }
}
