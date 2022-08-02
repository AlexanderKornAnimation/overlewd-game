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
            private Button buyButton;
            private TextMeshProUGUI buyButtonText;
            private TextMeshProUGUI timer;
            private List<Transform> steps = new List<Transform>();

            protected override void Awake()
            {
                base.Awake();
                
                buyButton = content.Find("BuyButton").GetComponent<Button>();
                buyButton.onClick.AddListener(BuyButtonClick);
                buyButtonText = buyButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                timer = content.Find("Timer").Find("Time").GetComponent<TextMeshProUGUI>();

                for (int i = 0; i < 10; i++)
                {
                    steps.Add(content.Find("Steps").Find($"Step{i + 1}"));
                }
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                
            }

            public void BuyButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<SummoningScreen>().
                    SetData(new SummoningScreenInData
                {
                    tabType = gachaData.tabType,
                    prevScreenInData = UIManager.prevScreenInData
                }).RunShowScreenProcess();
            }
            
            public static ContentByTier GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ContentByTier>
                    ("Prefabs/UI/Screens/PortalScreen/TabByTier", parent);
            }
        }
    }
}
