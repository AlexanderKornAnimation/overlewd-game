using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class ContentByTier : BaseContent
        {
            private Button summonManyButton;
            private TextMeshProUGUI summonButtonText;
            private List<Transform> steps = new List<Transform>();

            protected override void Awake()
            {
                base.Awake();

                summonManyButton = canvas.Find("SummonManyButton").GetComponent<Button>();
                summonManyButton.onClick.AddListener(SummonManyButtonClick);
                summonButtonText = summonManyButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                for (int i = 1; i <= 10; i++)
                {
                    steps.Add(canvas.Find("Steps").Find($"Step{i}"));
                }
            }

            public override void Customize()
            {
                
            }

            public override void OnGameDataEvent(GameDataEvent eventData)
            {

            }

            public async void SummonManyButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                var summonData = await GameData.gacha.BuyMany(gachaId);
                UIManager.MakeScreen<SummoningScreen>().
                    SetData(new SummoningScreenInData
                {
                    tabType = gachaData?.tabType,
                    prevScreenInData = UIManager.prevScreenInData,
                    isMany = true,
                    summonData = summonData
                }).RunShowScreenProcess();
            }
            
            public static ContentByTier GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ContentByTier>
                    ("Prefabs/UI/Screens/PortalScreen/ContentByTier", parent);
            }
        }
    }
}
