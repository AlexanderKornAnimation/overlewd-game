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
            
            private Button summonManyButton;
            private TextMeshProUGUI priceForMany;
            
            private Image item;

            protected override void Awake()
            {
                base.Awake();
                
                summonOneButton = canvas.Find("SummonOneButton").GetComponent<Button>();
                summonOneButton.onClick.AddListener(SummonOneButtonClick);
                priceForOne = summonOneButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                summonManyButton = canvas.Find("SummonManyButton").GetComponent<Button>();
                summonManyButton.onClick.AddListener(SummonManyButtonClick);
                priceForMany = summonManyButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                discount = summonManyButton.transform.Find("DiscountBack").Find("Discount").GetComponent<TextMeshProUGUI>();
                item = canvas.Find("Item").GetComponent<Image>();

            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {

            }

            private async void SummonOneButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                var summonData = await GameData.gacha.Buy(gachaId);
                UIManager.MakeScreen<SummoningScreen>().
                    SetData(new SummoningScreenInData
                    {
                        prevScreenInData = UIManager.prevScreenInData,
                        tabType = gachaData.tabType,
                        summonData = summonData
                    }).RunShowScreenProcess();
            }
            
            private async void SummonManyButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                var summonData = await GameData.gacha.BuyMany(gachaId);
                UIManager.MakeScreen<SummoningScreen>().
                    SetData(new SummoningScreenInData
                {
                    prevScreenInData = UIManager.prevScreenInData,
                    tabType = gachaData.tabType,
                    isMany = true,
                    summonData = summonData
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
