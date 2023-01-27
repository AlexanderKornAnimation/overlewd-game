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
            private Button button;
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
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                icon.sprite = ResourceManager.LoadSprite(equipData?.icon);
                equipStatus.SetActive(equipData.isEquipped);
                button.gameObject.SetActive(!equipStatus.activeSelf);
            }

            public void Select()
            {
                equipStatus.SetActive(true);
                button.gameObject.SetActive(false);
                transform.SetAsFirstSibling();
            }

            public void Deselect()
            {
                equipStatus.SetActive(false);
                button.gameObject.SetActive(true);
            }

            private async void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                
                OnClick?.Invoke(this);

                await Task.CompletedTask;
            }

            public static Equipment GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Equipment>("Prefabs/UI/Screens/OverlordScreen/Equipment",
                    parent);
            }
        }
    }
}