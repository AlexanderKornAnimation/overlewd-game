using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Resharper disable All

namespace Overlewd
{
    namespace FTUE
    {
        public class DefeatPopup : Overlewd.DefeatPopup
        {
            public override void AfterShow()
            {
                if (GameGlobalStates.battleScreen_BattleId == 2)
                {
                    GameGlobalStates.dialogNotification_DialogId = 5;
                    UIManager.ShowNotification<DialogNotification>();
                }
            }

            protected override void Customize()
            {
                magicGuildButton.interactable = false;
                foreach (var cr in magicGuildButton.GetComponentsInChildren<CanvasRenderer>())
                {
                    cr.SetColor(Color.gray);
                }

                inventoryButton.interactable = false;
                foreach (var cr in inventoryButton.GetComponentsInChildren<CanvasRenderer>())
                {
                    cr.SetColor(Color.gray);
                }
            }

            protected override void MagicGuildButtonClick()
            {
                
            }

            protected override void InventoryButtonClick()
            {

            }

            protected override void HaremButtonClick()
            {
                GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);

                GameGlobalStates.sexScreen_StageId = 3;
                GameGlobalStates.sexScreen_DialogId = 2;
                SoundManager.PlayOneShoot(SoundPath.UI_DefeatPopupHaremButtonClick);
                UIManager.ShowScreen<SexScreen>();
            }
        }
    }
}