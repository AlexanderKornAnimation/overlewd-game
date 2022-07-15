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
            public int characterId { get; set; }

            private Image girlIcon;
            private TextMeshProUGUI level;
            private TextMeshProUGUI girlClass;
            private Transform equipStatus;
            private Button girlScreenButton;
            private Button equipButton;
            private GameObject notificationNew;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                var girl = canvas.Find("Girl");
                
                girlIcon = girl.GetComponent<Image>();
                equipButton = canvas.Find("EquipButton").GetComponent<Button>();
                level = canvas.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
                girlClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
                equipStatus = canvas.Find("EquipStatus");
                notificationNew = canvas.Find("NotificationNew").GetComponent<GameObject>();
            }

            private void Start()
            {
                Customize();
            }
            
            private void Customize()
            {
                var chData = GameData.characters.GetById(characterId);
                girlIcon.sprite = ResourceManager.LoadSprite(chData.teamEditPersIcon);
                level.text = chData.level.ToString();
                girlClass.text = chData.classMarker;
            }

            public static Character GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Character>(
                    "Prefabs/UI/Screens/LaboratoryScreen/Character", parent);
            }
        }
    }
}