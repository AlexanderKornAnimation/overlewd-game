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
            protected Button selectButton;
            protected Button confirmButton;
            protected GameObject notActive;
            protected GameObject isTarget;
            protected GameObject isConsume;

            protected TextMeshProUGUI basicShardAmount;
            protected TextMeshProUGUI advancedShardAmount;
            protected TextMeshProUGUI epicShardAmount;
            protected TextMeshProUGUI heroicShardAmount;
            
            public string matriarchKey { get; set; }
            public AdminBRO.MatriarchItem matriarchData => GameData.matriarchs.GetMatriarchByKey(matriarchKey);

            protected virtual void Awake()
            {
                selectButton = transform.Find("ButtonActive").GetComponent<Button>();
                selectButton.onClick.AddListener(ButtonClick);
                notActive = transform.Find("NotActive").gameObject;
                isTarget = selectButton.transform.Find("IsTarget").gameObject;
                isConsume = selectButton.transform.Find("IsConsume").gameObject;
                confirmButton = transform.Find("ConfirmButton").GetComponent<Button>();
                confirmButton.onClick.AddListener(ConfirmButtonClick);

                basicShardAmount = selectButton.transform.Find("BasicShard").Find("Count").GetComponent<TextMeshProUGUI>();
                advancedShardAmount =
                    selectButton.transform.Find("AdvancedShard").Find("Count").GetComponent<TextMeshProUGUI>();
                epicShardAmount = selectButton.transform.Find("EpicShard").Find("Count").GetComponent<TextMeshProUGUI>();
                heroicShardAmount = selectButton.transform.Find("HeroicShard").Find("Count").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void ConfirmButtonClick()
            {
                
            }

            protected virtual void ButtonClick()
            {
                
            }

            protected virtual void Customize()
            {
                isTarget.SetActive(false);
                isConsume.SetActive(false);
                selectButton.gameObject.SetActive(matriarchData.isOpen);
                notActive?.SetActive(!matriarchData.isOpen);
            }
        }
    }
}