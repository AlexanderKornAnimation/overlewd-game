using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public abstract class EquipmentBase : MonoBehaviour
        {
            protected Button button;
            protected Image icon;
            protected Transform shade;
            protected Transform isConsume;
            protected Transform isMaxRarity;

            public int? equipId { get; set; }
            public AdminBRO.Equipment equipData => GameData.equipment.GetById(equipId);

            void Awake()
            {
                button = transform.GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                icon = transform.GetComponent<Image>();
                shade = transform.Find("Shade");
                isConsume = transform.Find("IsConsume");
                isMaxRarity = transform.Find("IsMaxRarity");
            }

            void Start()
            {
                RefreshState();
            }

            public virtual void RefreshState()
            {
                icon.sprite = ResourceManager.LoadSprite(equipData?.icon);
                button.interactable = (equipData?.rarity != AdminBRO.Rarity.Heroic);
                isMaxRarity.gameObject.SetActive(equipData?.rarity == AdminBRO.Rarity.Heroic);
            }

            protected virtual void ButtonClick()
            {
                
            }
        }
    }
}
