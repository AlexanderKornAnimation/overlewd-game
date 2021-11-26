using System;
using System.Collections;
using System.Collections.Generic;
using Overlewd.NSBannerNotification;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BannerNotification : BaseNotification
    {
        private List<NSBannerNotification.ResourceTypeA> resourcesTypeA = new List<NSBannerNotification.ResourceTypeA>();
        private List<NSBannerNotification.ResourceTypeB> resourcesTypeB = new List<NSBannerNotification.ResourceTypeB>();
        private List<NSBannerNotification.ResourceTypeC> resourcesTypeC = new List<NSBannerNotification.ResourceTypeC>();
        private List<NSBannerNotification.ResourceTypeD> resourcesTypeD = new List<NSBannerNotification.ResourceTypeD>();
        
        private Transform gridForGoods;
        private Transform gridForCurrencies;

        private Button buyButton;

        private AdminBRO.TradableItem tradableData;

        private void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/BannerNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            buyButton = canvas.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(BuyButtonClick);

            gridForGoods = canvas.Find("GridForGoods");
            gridForCurrencies = canvas.Find("GridForCurrency");
            
            InstantiateResources("a", 1);
            InstantiateResources("b", 2);
            InstantiateResources("c", 1);
            InstantiateResources("d", 5);
            
            resourcesTypeA[0].SetIcon("Prefabs/UI/Common/Images/Recources/Goods/Sword");
            
            resourcesTypeB[0].SetIcon("Prefabs/UI/Common/Images/Recources/Goods/Shards");
            resourcesTypeB[1].SetIcon("Prefabs/UI/Common/Images/Recources/Crystal");
            
            resourcesTypeC[0].SetIcon("Prefabs/UI/Common/Images/Recources/EventCurrency/CatgirlMigration");
            
            resourcesTypeD[0].SetIcon("Prefabs/UI/Common/Images/Recources/Gem");
            resourcesTypeD[1].SetIcon("Prefabs/UI/Common/Images/Recources/Gold");
            resourcesTypeD[2].SetIcon("Prefabs/UI/Common/Images/Recources/Copper");
            resourcesTypeD[3].SetIcon("Prefabs/UI/Common/Images/Recources/Stone");
            resourcesTypeD[4].SetIcon("Prefabs/UI/Common/Images/Recources/Wood");

            tradableData = GameGlobalStates.bannerNotifcation_TradableData;
        }

        private void InstantiateResources(string type, int count)
        {
            switch (type.ToLower())
            {
                case "a":
                    for (int i = 0; i < count; i++)
                    {
                        resourcesTypeA.Add(NSBannerNotification.ResourceTypeA.GetInstance(gridForGoods));
                    }
                    break;
                case "b":
                    for (int i = 0; i < count; i++)
                    {
                        resourcesTypeB.Add(NSBannerNotification.ResourceTypeB.GetInstance(gridForGoods));
                    }
                    break;
                case "c":
                    for (int i = 0; i < count; i++)
                    {
                        resourcesTypeC.Add(NSBannerNotification.ResourceTypeC.GetInstance(gridForCurrencies));
                    }
                    break;
                case "d":
                    for (int i = 0; i < count; i++)
                    {
                        resourcesTypeD.Add(NSBannerNotification.ResourceTypeD.GetInstance(gridForCurrencies));
                    }
                    break;
            }
        }
        
        private async void BuyButtonClick()
        {
            await GameData.BuyTradableAsync(tradableData);

            UIManager.ShowNotification<NutakuBuyingNotification>();
        }
    }
}
