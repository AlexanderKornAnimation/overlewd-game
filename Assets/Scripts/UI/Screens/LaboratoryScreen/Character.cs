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
            public AdminBRO.Character characterData =>
                GameData.characters.GetById(characterId);

            private Image girlIcon;
            private TextMeshProUGUI level;
            private TextMeshProUGUI girlClass;
            private Button equipButton;
            private GameObject notificationNew;
            private GameObject equipStatus;
            private GameObject inTeamStatus;

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
                notificationNew = canvas.Find("NotificationNew").GetComponent<GameObject>();
                equipStatus = canvas.Find("EquipStatus").gameObject;
                inTeamStatus = canvas.Find("InTeamStatus").gameObject;

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

                var chData = characterData;

                girlIcon.sprite = ResourceManager.LoadSprite(chData.iconUrl);
                level.text = chData.level.ToString();
                girlClass.text = chData.classMarker;
                inTeamStatus.SetActive(characterData.inTeam);

                if (labScreen.IsInFlask(this))
                {
                    equipStatus.SetActive(true);
                    inTeamStatus.SetActive(false);
                    girlIcon.color = Color.gray;
                    equipButton.interactable = true;
                }
                else if (labScreen.CanAddToFlask(this))
                {
                    equipStatus.SetActive(false);
                    girlIcon.color = Color.white;
                    equipButton.interactable = true;
                }
                else
                {
                    equipStatus.SetActive(false);
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