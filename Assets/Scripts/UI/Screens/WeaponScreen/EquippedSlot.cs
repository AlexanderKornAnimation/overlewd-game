using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSWeaponScreen
    {
        public class EquippedSlot : BaseSlot
        {
            private Button unequipButton;

            protected override void Awake()
            {
                base.Awake();
                unequipButton = slotFull.transform.Find("UnequipButton").GetComponent<Button>();
                unequipButton.onClick.AddListener(UnequipButtonClick);
            }

            private async void UnequipButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.SFX_UI_Equip_OFF);
                await GameData.equipment.Unequip(chId.Value, equipId.Value);
            }
        }
    }
}