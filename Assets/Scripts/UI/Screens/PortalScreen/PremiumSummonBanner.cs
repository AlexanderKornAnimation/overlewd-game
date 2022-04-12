using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class PremiumSummonBanner : BaseBanner
        {
            protected Button button;
            protected GameObject buttonSelected;
            protected Image image;
            protected TextMeshProUGUI title;

            protected override void Awake()
            {
                base.Awake();

                button = transform.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(BannerClick);

                buttonSelected = button.transform.Find("ButtonSelected").gameObject;
                image = button.transform.Find("Image").GetComponent<Image>();
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                buttonSelected.SetActive(false);
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                TierButton.GetInstance(tierButtonsScroll?.content);
                TierButton.GetInstance(tierButtonsScroll?.content);
                TierButton.GetInstance(tierButtonsScroll?.content);
            }

            public override void Select()
            {
                base.Select();
                buttonSelected.SetActive(true);
            }

            public override void Deselect()
            {
                base.Deselect();
                buttonSelected.SetActive(false);
            }

            public static PremiumSummonBanner GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<PremiumSummonBanner>
                    ("Prefabs/UI/Screens/PortalScreen/PremiumSummonBanner", parent);
            }
        }
    }
}
