using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            public event Action<Equipment> OnClick;

            public int equipId { get; set; }
            public AdminBRO.Equipment equipData => GameData.equipment.GetById(equipId);

            public AdminBRO.Character overlordData => GameData.characters.overlord;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                icon = canvas.Find("Icon").GetComponent<Image>();
                equipStatus = canvas.Find("EquipStatus").gameObject;
                notification = canvas.Find("Notification").gameObject;
                equipButton = canvas.Find("Button").GetComponent<Button>();
                equipButton.onClick.AddListener(EquipButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                icon.sprite = ResourceManager.LoadSprite(equipData?.icon);
                equipStatus.SetActive(equipData.isEquipped);
                equipButton.gameObject.SetActive(!equipStatus.activeSelf);
            }

            public void Select()
            {
                equipStatus.SetActive(true);
                equipButton.gameObject.SetActive(false);
                transform.SetAsFirstSibling();
            }

            public void Deselect()
            {
                equipStatus.SetActive(false);
                equipButton.gameObject.SetActive(true);
            }

            private void EquipButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                
                OnClick?.Invoke(this);
            }

            public static Equipment GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Equipment>("Prefabs/UI/Screens/OverlordScreen/Equipment",
                    parent);
            }
        }
    }
}