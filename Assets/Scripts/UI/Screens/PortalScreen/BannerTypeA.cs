using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class BannerTypeA : BaseBanner
        {
            private Button buyOneButton;
            private TextMeshProUGUI priceForOne;
            
            private Button buyTenButton;
            private TextMeshProUGUI priceForTen;
            
            private Image item;

            protected override void Awake()
            {
                base.Awake();
                
                buyOneButton = content.Find("BuyOneButton").GetComponent<Button>();
                priceForOne = buyOneButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                
                buyTenButton = content.Find("BuyTenButton").GetComponent<Button>();
                priceForTen = buyTenButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

                discount = buyTenButton.transform.Find("DiscountIcon").Find("Discount").GetComponent<TextMeshProUGUI>();
                item = content.Find("Item").GetComponent<Image>();

            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {

            }

            public static BannerTypeA GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BannerTypeA>
                    ("Prefabs/UI/Screens/PortalScreen/BannerTypeA", parent);
            }
        }
    }
}
