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

            public int? equipId { get; set; }
            public AdminBRO.Equipment equipData => GameData.equipment.GetById(equipId);

            void Awake()
            {
                button = transform.GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                icon = transform.GetComponent<Image>();
                shade = transform.Find("Shade");
                isConsume = transform.Find("IsConsume");
            }

            void Start()
            {
                RefreshState();
            }

            public virtual void RefreshState()
            {

            }

            protected virtual void ButtonClick()
            {
                
            }
        }
    }
}
