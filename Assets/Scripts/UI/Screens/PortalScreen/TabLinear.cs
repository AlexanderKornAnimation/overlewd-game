using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class TabLinear : BaseTab
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
                buyOneButton.onClick.AddListener(BuyOneButtonClick);
                    priceForOne = buyOneButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                
                buyTenButton = content.Find("BuyTenButton").GetComponent<Button>();
                buyTenButton.onClick.AddListener(BuyTenButtonClick);
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

            private void BuyOneButtonClick()
            {
                UIManager.MakeScreen<SummoningScreen>().
                    SetData(new SummoningScreenInData
                    {
                        prevScreenInData = UIManager.prevScreenInData,
                        tabType = gachaData.tabType,
                    }).RunShowScreenProcess();
            }
            
            private void BuyTenButtonClick()
            {
                UIManager.MakeScreen<SummoningScreen>().
                    SetData(new SummoningScreenInData
                {
                    prevScreenInData = UIManager.prevScreenInData,
                    tabType = gachaData.tabType,
                    isTen = true
                }).RunShowScreenProcess();
            }

            public static TabLinear GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<TabLinear>
                    ("Prefabs/UI/Screens/PortalScreen/TabLinear", parent);
            }
        }
    }
}
