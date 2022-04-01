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
            protected Transform guarantedSummon;
            protected Image girlBack1;
            protected Image girl1;
            protected Image girlBack2;
            protected Image girl2;

            protected Button button;
            protected GameObject buttonSelected;
            protected TextMeshProUGUI buttonText;
            protected GameObject currentTierCheck;
            protected TextMeshProUGUI tierTitle;

            protected virtual void Awake()
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

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {

            } 

            protected virtual void ButtonClick() 
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                Destroy(gameObject);
            }

            public static TierButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<TierButton>
                    ("Prefabs/UI/Screens/PortalScreen/TierButton", parent);
            }
        }
    }
}
