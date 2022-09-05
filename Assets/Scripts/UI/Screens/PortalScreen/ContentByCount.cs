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

            protected override void Awake()
            {
                base.Awake();
                
                summonOneButton = canvas.Find("SummonOneButton").GetComponent<Button>();
                summonOneButton.onClick.AddListener(SummonOneButtonClick);
                priceForOne = summonOneButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                summonManyButton = canvas.Find("SummonManyButton").GetComponent<Button>();
                summonManyButton.onClick.AddListener(SummonManyButtonClick);
                priceForMany = summonManyButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                discountBack = summonManyButton.transform.Find("DiscountBack").gameObject;
                discount = discountBack.transform.Find("Discount").GetComponent<TextMeshProUGUI>();
            }

            public override void Customize()
            {
                var _gachaData = gachaData;

                title.text = _gachaData.imageText;

                discountBack.SetActive(_gachaData?.discount > 0);
                if (discountBack.activeSelf)
                {
                    discount.text = $"-{_gachaData?.discount}%";
                }

                MakeSummonOneButton(summonOneButton, priceForOne);
                MakeSummonManyButton(summonManyButton, priceForMany);
            }

            public override void OnGameDataEvent(GameDataEvent eventData)
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
