using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class BattleGirlsBannerTypeA : BaseBanner
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
                TierButton.GetInstance(tierButtonsScroll?.content);
            }

            public virtual void Select()
            {
                base.Select();
                buttonSelected.SetActive(true);
            }

            public virtual void Deselect()
            {
                base.Deselect();
                buttonSelected.SetActive(false);
            }

            public static BattleGirlsBannerTypeA GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BattleGirlsBannerTypeA>
                    ("Prefabs/UI/Screens/PortalScreen/BattleGirlsBannerTypeA", parent);
            }
        }
    }
}
