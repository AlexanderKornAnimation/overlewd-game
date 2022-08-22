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
            private Button summonOneButton;
            private TextMeshProUGUI priceForOne;
            
            private Button summonFiveButton;
            private TextMeshProUGUI priceForFive;
            
            private Image item;

            protected override void Awake()
            {
                base.Awake();
                
                summonOneButton = canvas.Find("SummonOneButton").GetComponent<Button>();
                summonOneButton.onClick.AddListener(SummonOneButtonClick);
                priceForOne = summonOneButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                
                summonFiveButton = canvas.Find("SummonFiveButton").GetComponent<Button>();
                summonFiveButton.onClick.AddListener(SummonTenButtonClick);
                priceForFive = summonFiveButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                discount = summonFiveButton.transform.Find("DiscountBack").Find("Discount").GetComponent<TextMeshProUGUI>();
                item = canvas.Find("Item").GetComponent<Image>();

            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {

            }

            private void SummonOneButtonClick()
            {
                UIManager.MakeScreen<SummoningScreen>().
                    SetData(new SummoningScreenInData
                    {
                        prevScreenInData = UIManager.prevScreenInData,
                        tabType = gachaData.tabType,
                    }).RunShowScreenProcess();
            }
            
            private void SummonTenButtonClick()
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
