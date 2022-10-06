using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public abstract class MatriarchBase : MonoBehaviour
        {
            protected TextMeshProUGUI basicShardAmount;
            protected TextMeshProUGUI advancedShardAmount;
            protected TextMeshProUGUI epicShardAmount;
            protected TextMeshProUGUI heroicShardAmount;

            protected Transform shadeInfo;
            protected Transform isTarget;
            protected Transform isConsume;
            protected TextMeshProUGUI msgTitle;
            protected Button button;

            private string _matriarchKey;
            public string matriarchKey
            {
                get => _matriarchKey;
                set
                {
                    _matriarchKey = value;

                    transform.Find("Ulvi").gameObject.SetActive(matriarchData?.isUlvi ?? false);
                    transform.Find("Ulvi/NoActive").gameObject.SetActive(!IsOpen);
                    transform.Find("Adriel").gameObject.SetActive(matriarchData?.isAdriel ?? false);
                    transform.Find("Adriel/NoActive").gameObject.SetActive(!IsOpen);
                    transform.Find("Ingie").gameObject.SetActive(matriarchData?.isIngie ?? false);
                    transform.Find("Ingie/NoActive").gameObject.SetActive(!IsOpen);
                    transform.Find("Faye").gameObject.SetActive(matriarchData?.isFaye ?? false);
                    transform.Find("Faye/NoActive").gameObject.SetActive(!IsOpen);
                    transform.Find("Lili").gameObject.SetActive(matriarchData?.isLili ?? false);
                    transform.Find("Lili/NoActive").gameObject.SetActive(!IsOpen);
                    transform.Find("Info").gameObject.SetActive(IsOpen);

                    var mtrch = matriarchData?.key switch
                    {
                        AdminBRO.MatriarchItem.Key_Ulvi => transform.Find("Ulvi"),
                        AdminBRO.MatriarchItem.Key_Adriel => transform.Find("Adriel"),
                        AdminBRO.MatriarchItem.Key_Ingie => transform.Find("Ingie"),
                        AdminBRO.MatriarchItem.Key_Faye => transform.Find("Faye"),
                        AdminBRO.MatriarchItem.Key_Lili => transform.Find("Lili"),
                        _ => null
                    };
                    button = mtrch?.GetComponent<Button>();
                    button?.onClick.AddListener(ButtonClick);
                    button.interactable = IsOpen;
                }
            }
            public AdminBRO.MatriarchItem matriarchData => GameData.matriarchs.GetMatriarchByKey(matriarchKey);

            public bool IsOpen => matriarchData?.isOpen ?? false;

            void Awake()
            {
                var info = transform.Find("Info");
                basicShardAmount = info.Find("BasicShard/Count").GetComponent<TextMeshProUGUI>();
                advancedShardAmount = info.Find("AdvancedShard/Count").GetComponent<TextMeshProUGUI>();
                epicShardAmount = info.Find("EpicShard/Count").GetComponent<TextMeshProUGUI>();
                heroicShardAmount = info.Find("HeroicShard/Count").GetComponent<TextMeshProUGUI>();

                shadeInfo = info.Find("ShadeInfo");
                isTarget = info.Find("IsTarget");
                isConsume = info.Find("IsConsume");
                msgTitle = info.Find("MsgTitle").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void ButtonClick()
            {

            }

            public virtual void RefreshState()
            {
                var mData = matriarchData;
                basicShardAmount.text = mData?.basicShard?.amount.ToString();
                advancedShardAmount.text = mData?.advancedShard?.amount.ToString();
                epicShardAmount.text = mData?.epicShard?.amount.ToString();
                heroicShardAmount.text = mData?.heroicShard?.amount.ToString();
            }
        }
    }
}