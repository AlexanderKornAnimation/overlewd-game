using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BannerNotification : BaseNotification
    {
        private List<Image> goods = new List<Image>();
        private List<Image> currency = new List<Image>();

        private Button buyButton;

        private void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/BannerNotification/BannerNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            buyButton = canvas.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(BuyButtonClick);
            
            FindResources(canvas);

            goods[0].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Goods/Sword");
            goods[1].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Goods/Shards");
            goods[2].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");

            currency[0].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/EventCurrency/CatgirlMigration");
            currency[1].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gem");
            currency[2].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gold");
            currency[3].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Copper");
            currency[4].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Stone");
            currency[5].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Wood");
        }

        private void FindResources(Transform canvas)
        {
            var goodsCount = canvas.Find("GridForGoods").childCount;
            var currencyCount = canvas.Find("GridForCurrency").childCount;

            for (int i = 1; i <= goodsCount; i++)
            {
                goods.Add(canvas.Find("GridForGoods").Find($"Goods{i}").Find("Icon").GetComponent<Image>());
            }
            
            for (int i = 1; i <= currencyCount; i++)
            {
                currency.Add(canvas.Find("GridForCurrency").Find($"Currency{i}").Find("Icon").GetComponent<Image>());
            }
        }
        
        private void BuyButtonClick()
        {
            UIManager.ShowNotification<NutakuBuyingNotification>();
        }
    }
}
