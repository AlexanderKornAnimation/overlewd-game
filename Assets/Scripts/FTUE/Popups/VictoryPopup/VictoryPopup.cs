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
        public class VictoryPopup : Overlewd.VictoryPopup
        {
            protected override void NextButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                
                if (GameGlobalStates.battleScreen_BattleId == 1)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    GameGlobalStates.map_DialogNotificationId = 3;
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 3)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 4)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    GameGlobalStates.castle_DialogNotificationId = 8;
                    GameGlobalStates.castle_HintMessage = GameData.castleScreenHints[1];
                    UIManager.ShowScreen<CastleScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 5)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    GameGlobalStates.UlviCaveCanBuilded();
                    GameGlobalStates.ResetStateCastleButtons();
                    GameGlobalStates.castle_SideMenuLock = true;
                    GameGlobalStates.castle_HintMessage = GameData.castleScreenHints[2];
                    UIManager.ShowScreen<CastleScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 6)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 7)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 8)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 9)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 10)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.battleScreen_BattleId == 11)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.battleScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
                }
            }

            protected override void RepeatButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                UIManager.ShowScreen<BattleScreen>();
            }

            public override void AfterShow()
            {
                if (GameGlobalStates.battleScreen_BattleId == 1)
                {
                    GameGlobalStates.dialogNotification_DialogId = 2;
                    UIManager.ShowNotification<DialogNotification>();
                }
            }
        }
    }
}