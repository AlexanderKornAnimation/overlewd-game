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
            
            public string matriarchKey { get; set; }
            public AdminBRO.MatriarchItem matriarchData => GameData.matriarchs.GetMatriarchByKey(matriarchKey);

            protected virtual void Awake()
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
                button = info.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                transform.Find("Ulvi").gameObject.SetActive(matriarchData?.isUlvi ?? false);
                transform.Find("Ulvi/NoActive").gameObject.SetActive(!matriarchData?.isOpen ?? false);
                transform.Find("Adriel").gameObject.SetActive(matriarchData?.isAdriel ?? false);
                transform.Find("Adriel/NoActive").gameObject.SetActive(!matriarchData?.isOpen ?? false);
                transform.Find("Ingie").gameObject.SetActive(matriarchData?.isIngie ?? false);
                transform.Find("Ingie/NoActive").gameObject.SetActive(!matriarchData?.isOpen ?? false);
                transform.Find("Faye").gameObject.SetActive(matriarchData?.isFaye ?? false);
                transform.Find("Faye/NoActive").gameObject.SetActive(!matriarchData?.isOpen ?? false);
                transform.Find("Lili").gameObject.SetActive(matriarchData?.isLili ?? false);
                transform.Find("Lili/NoActive").gameObject.SetActive(!matriarchData?.isOpen ?? false);
                transform.Find("Info").gameObject.SetActive(matriarchData?.isOpen ?? false);
            }

            protected virtual void ButtonClick()
            {

            }
        }
    }
}