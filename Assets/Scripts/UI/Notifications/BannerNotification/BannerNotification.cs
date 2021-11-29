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

        private List<ResourceIcon> resourceIcons = new List<ResourceIcon>();

        private void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/BannerNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            resourcesGrid = canvas.Find("ResourcesGrid").GetComponent<VerticalLayoutGroup>();
            vGrid = resourcesGrid.transform.Find("vGrid").GetComponent<VerticalLayoutGroup>();
            hGrid1 = vGrid.transform.Find("hGrid1").GetComponent<HorizontalLayoutGroup>();
            hGrid2 = vGrid.transform.Find("hGrid2").GetComponent<HorizontalLayoutGroup>();

            buyButton = canvas.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(BuyButtonClick);

            tradableData = GameGlobalStates.bannerNotifcation_TradableData;

            Customize();
        }

        private void Customize()
        {
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeA, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeB, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeB, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeB, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeB, null));

            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeC, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeD, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeD, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeD, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeD, null));
            AddResourceIcon(ResourceIcon.GetInstance(ResourceIcon.Type.ResourceTypeD, null));
        }

        private void AddResourceIcon(ResourceIcon resourceIcon)
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
            }
            else
            {
                resourceIcon.transform.SetParent(hGrid2.transform, false);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(resourcesGrid.GetComponent<RectTransform>());

            resourceIcons.Add(resourceIcon);
        }
        
        private async void BuyButtonClick()
        {
            var marketId = GameGlobalStates.bannerNotification_EventMarketId;
            var tradableId = GameGlobalStates.bannerNotification_TradableId;
            await GameData.BuyTradableAsync(marketId, tradableId);

            UIManager.ShowNotification<NutakuBuyingNotification>();
        }
    }
}
