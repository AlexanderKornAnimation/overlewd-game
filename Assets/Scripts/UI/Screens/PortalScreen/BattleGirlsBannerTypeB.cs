using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class BattleGirlsBannerTypeB : BaseBanner
        {
            private Button button;
            private GameObject buttonSelected;
            private Image image;
            private TextMeshProUGUI title;

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

            void Start()
            {
                Customize();
            }

            private void Customize()
            {
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

            public static BattleGirlsBannerTypeB GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/BattleGirlsBannerTypeB"), parent);
                newItem.name = nameof(BattleGirlsBannerTypeB);
                return newItem.AddComponent<BattleGirlsBannerTypeB>();
            }
        }
    }
}
