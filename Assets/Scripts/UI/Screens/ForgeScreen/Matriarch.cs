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
        public class Matriarch : MonoBehaviour
        {
            private Button selectButton;
            private Button confirmButton;
            private GameObject notActive;
            private GameObject isTarget;
            private GameObject isConsume;

            private TextMeshProUGUI basicShardAmount;
            private TextMeshProUGUI advancedShardAmount;
            private TextMeshProUGUI epicShardAmount;
            private TextMeshProUGUI heroicShardAmount;
            
            public string matriarchKey { get; set; }
            public AdminBRO.MatriarchItem matriarchData => GameData.matriarchs.GetMatriarchByKey(matriarchKey);

            private void Awake()
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

            private void Start()
            {
                Customize();
            }

            private void ConfirmButtonClick()
            {
                
            }
            
            private void ButtonClick()
            {
                
            }
                
            private void Customize()
            {
                isTarget.SetActive(false);
                isConsume.SetActive(false);
                selectButton.gameObject.SetActive(matriarchData.isOpen);
                notActive?.SetActive(!matriarchData.isOpen);
            }
        }
    }
}