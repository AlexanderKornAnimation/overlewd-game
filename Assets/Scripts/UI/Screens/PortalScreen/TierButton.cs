using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class TierButton : MonoBehaviour
        {
            private Transform guarantedSummon;
            private Image girlBack1;
            private Image girl1;
            private Image girlBack2;
            private Image girl2;

            private Button button;
            private GameObject buttonSelected;
            private TextMeshProUGUI buttonText;
            private GameObject currentTierCheck;
            private TextMeshProUGUI tierTitle;

            void Awake()
            {
                guarantedSummon = transform.Find("GuarantedSummon");
                girlBack1 = guarantedSummon.Find("GirlBack1").GetComponent<Image>();
                girl1 = girlBack1.transform.Find("Girl").GetComponent<Image>();
                girlBack2 = guarantedSummon.Find("GirlBack2").GetComponent<Image>();
                girl2 = girlBack2.transform.Find("Girl").GetComponent<Image>();

                var button = transform.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                buttonSelected = button.transform.Find("ButtonSelected").gameObject;
                buttonText = button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                currentTierCheck = button.transform.Find("CurrentTierCheck").gameObject;
                tierTitle = button.transform.Find("TierBack").Find("Tier").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
                Customize();
            }

            private void Customize()
            {

            } 

            private void ButtonClick() 
            {
                Destroy(gameObject);
            }

            public static TierButton GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/TierButton"), parent);
                newItem.name = nameof(TierButton);
                return newItem.AddComponent<TierButton>();
            }
        }
    }
}
