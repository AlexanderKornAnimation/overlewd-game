using System;
using System.Collections;
using System.Collections.Generic;
using Overlewd.NSSummoningScreen;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSOverlordScreen
    {
        public class Equipment : MonoBehaviour
        {
            private Image icon;
            private GameObject equipStatus;
            private GameObject notification;
            private Button equipButton;
            
           public int equipId { get; set; }
           public AdminBRO.Equipment equipData => GameData.equipment.GetById(equipId);

           private void Awake()
           {
               var canvas = transform.Find("Canvas");
               
               icon = canvas.Find("Icon").GetComponent<Image>();
               equipStatus = canvas.Find("EquipStatus").gameObject;
               notification = canvas.Find("Notification").gameObject;
               equipButton = canvas.Find("EquipButton").GetComponent<Button>();
               equipButton.onClick.AddListener(EquipButtonClick);
           }

           private void Start()
           {
               Customize();
           }

           public void Customize()
           {
               
           }

           private void EquipButtonClick()
           {
               
           }

           public static Equipment GetInstance(Transform parent)
           {
               return ResourceManager.InstantiateWidgetPrefab<Equipment>("Prefabs/UI/Screens/OverlordScreen/Equipment",
                   parent);
           }
        }
    }
}
