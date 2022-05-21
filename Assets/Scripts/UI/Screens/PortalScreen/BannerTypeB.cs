using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class BannerTypeB: BaseBanner
        {
            private Button buyButton;
            private TextMeshProUGUI buyButtonText;
            private TextMeshProUGUI timer;
            private List<GameObject> steps = new List<GameObject>(11);

            protected override void Awake()
            {
                base.Awake();
                
                buyButton = content.Find("BuyButton").GetComponent<Button>();
                buyButtonText = buyButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                timer = content.Find("Timer").Find("Time").GetComponent<TextMeshProUGUI>();

                for (int i = 1; i < steps.Count; i++)
                {
                    steps.Add(content.Find("Steps").Find($"Step{i}").gameObject);
                }
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {

            }
            
            public static BannerTypeB GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BannerTypeB>
                    ("Prefabs/UI/Screens/PortalScreen/BannerTypeB", parent);
            }
        }
    }
}
