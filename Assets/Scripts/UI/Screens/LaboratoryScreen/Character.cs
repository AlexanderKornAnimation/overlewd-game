using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    namespace NSLaboratoryScreen
    {
        public class Character : MonoBehaviour
        {
            public LaboratoryScreen labScreen { get; set; }
            public int? characterId { get; set; }
            public AdminBRO.Character chracterData =>
                GameData.characters.GetById(characterId);

            private Image girlIcon;
            private TextMeshProUGUI level;
            private TextMeshProUGUI girlClass;
            private Transform equipStatus;
            private Button equipButton;
            private GameObject notificationNew;

            private bool initialized = false;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                var girl = canvas.Find("Girl");
                
                girlIcon = girl.GetComponent<Image>();
                equipButton = canvas.Find("EquipButton").GetComponent<Button>();
                equipButton.onClick.AddListener(OnClick);
                level = canvas.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
                girlClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
                equipStatus = canvas.Find("EquipStatus");
                notificationNew = canvas.Find("NotificationNew").GetComponent<GameObject>();

                equipStatus.gameObject.SetActive(false);

                initialized = true;
            }

            private void Start()
            {
                Customize();
            }
            
            public void Customize()
            {
                if (!initialized)
                    return;

                var chData = chracterData;

                girlIcon.sprite = ResourceManager.LoadSprite(chData.teamEditPersIcon);
                level.text = chData.level.ToString();
                girlClass.text = chData.classMarker;

                if (labScreen.IsInFlask(this))
                {
                    equipStatus.gameObject.SetActive(true);
                    girlIcon.color = Color.gray;
                    equipButton.interactable = true;
                }
                else if (labScreen.CanAddToFlask(this))
                {
                    equipStatus.gameObject.SetActive(false);
                    girlIcon.color = Color.white;
                    equipButton.interactable = true;
                }
                else
                {
                    equipStatus.gameObject.SetActive(false);
                    girlIcon.color = Color.gray;
                    equipButton.interactable = false;
                }
            }

            private void OnClick()
            {
                if (labScreen.IsInFlask(this))
                {
                    labScreen.EraseFromFlask(this);
                }
                else
                {
                    labScreen.AddToFlask(this);
                }
            }

            public static Character GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Character>(
                    "Prefabs/UI/Screens/LaboratoryScreen/Character", parent);
            }
        }
    }
}