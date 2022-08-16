using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class Equipment : MonoBehaviour
        {
            private Image rarityBackground;
            private Image icon;
            private GameObject equipStatus;
            private Button button;

            public int? equipId { get; set; }
            public AdminBRO.Equipment equipData => GameData.equipment.GetById(equipId);

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                rarityBackground = canvas.Find("RarityBackground").GetComponent<Image>();
                icon = canvas.Find("Icon").GetComponent<Image>();
                equipStatus = canvas.Find("EquipStatus").gameObject;
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                equipStatus.SetActive(false);
            }

            private void ButtonClick()
            {
                
            }
            
            public static Equipment GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Equipment>("Prefabs/UI/Screens/ForgeScreen/Equipment",
                    parent);
            }
        }
    }
}
