using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class ContentByCount: BaseContent
        {
            private Button buyOneButton;
            private TextMeshProUGUI priceForOne;
            
            private Button buyFiveButton;
            private TextMeshProUGUI priceForFive;
            
            private Image item;

            protected override void Awake()
            {
                base.Awake();
                
                buyOneButton = canvas.Find("BuyOneButton").GetComponent<Button>();
                buyOneButton.onClick.AddListener(BuyOneButtonClick);
                    priceForOne = buyOneButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                
                buyFiveButton = canvas.Find("BuyTenButton").GetComponent<Button>();
                buyFiveButton.onClick.AddListener(BuyTenButtonClick);
                priceForFive = buyFiveButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

                discount = buyFiveButton.transform.Find("DiscountIcon").Find("Discount").GetComponent<TextMeshProUGUI>();
                item = canvas.Find("Item").GetComponent<Image>();

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
                    isFive = true
                }).RunShowScreenProcess();
            }

            public static ContentByCount GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ContentByCount>
                    ("Prefabs/UI/Screens/PortalScreen/ContentByCount", parent);
            }
        }
    }
}
