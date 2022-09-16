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
            private Button button;
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
                button = transform.Find("ButtonActive").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                notActive = transform.Find("NotActive")?.gameObject;
                isTarget = button.transform.Find("IsTarget").gameObject;
                isConsume = button.transform.Find("IsConsume").gameObject;

                basicShardAmount = button.transform.Find("BasicShard").Find("Count").GetComponent<TextMeshProUGUI>();
                advancedShardAmount =
                    button.transform.Find("AdvancedShard").Find("Count").GetComponent<TextMeshProUGUI>();
                epicShardAmount = button.transform.Find("EpicShard").Find("Count").GetComponent<TextMeshProUGUI>();
                heroicShardAmount = button.transform.Find("HeroicShard").Find("Count").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void ButtonClick()
            {
                
            }
                
            private void Customize()
            {
                isTarget.SetActive(false);
                isConsume.SetActive(false);
                button.gameObject.SetActive(matriarchData.isOpen);
                notActive?.SetActive(!matriarchData.isOpen);
            }
        }
    }
}