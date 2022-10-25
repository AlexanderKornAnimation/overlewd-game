using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSWeaponScreen
    {
        public class SelectedSlot : BaseSlot
        {
            private Button equipButton;

            protected override void Awake()
            {
                base.Awake();
                equipButton = slotFull.transform.Find("EquipButton").GetComponent<Button>();
                equipButton.onClick.AddListener(EquipButtonClick);
            }

            private async void EquipButtonClick()
            {
                ButtonClick();
                await GameData.equipment.Equip(chId.Value, equipId.Value);
            }
        }
    }
}