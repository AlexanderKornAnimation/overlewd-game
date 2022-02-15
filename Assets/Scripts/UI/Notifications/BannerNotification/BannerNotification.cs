using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BannerNotification : BaseNotification
    {
        private VerticalLayoutGroup resourcesGrid;
        private VerticalLayoutGroup vGrid;
        private HorizontalLayoutGroup hGrid1;
        private HorizontalLayoutGroup hGrid2;

        private Button buyButton;

        private AdminBRO.TradableItem tradableData;

        private List<NSBannerNotification.ResourceIcon> resourceIcons = 
            new List<NSBannerNotification.ResourceIcon>();

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

            tradableData = GameGlobalStates.bannerNotifcation_TradableData;
        }

        void Start()
        {
            Customize();
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

            resourceIcons.Add(resourceIcon);
        }
        
        private async void BuyButtonClick()
        {
            var marketId = GameGlobalStates.bannerNotification_EventMarketId;
            var tradableId = GameGlobalStates.bannerNotification_TradableId;
            await GameData.BuyTradableAsync(marketId, tradableId);

            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowNotification<NutakuBuyingNotification>();
        }
    }
}
