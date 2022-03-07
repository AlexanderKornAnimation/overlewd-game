using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    namespace FTUE
    {
        public class DialogScreen : Overlewd.DialogScreen
        {
            private SpineWidgetGroup cutInAnimation;
            private SpineWidgetGroup emotionAnimation;

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.dialogScreen_DialogData;

                personageHead.gameObject.SetActive(false);
                emotionBack.gameObject.SetActive(true);

                if (GameGlobalStates.newFTUE)
                {

                }
                else
                {
                    if (GameGlobalStates.dialogScreen_DialogId == 1)
                    {
                    }
                    else if (GameGlobalStates.dialogScreen_DialogId == 2)
                    {
                    }
                    else if (GameGlobalStates.dialogScreen_DialogId == 3)
                    {
                    }
                }

                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                if (GameGlobalStates.newFTUE)
                {
                    if (GameGlobalStates.dialogScreen_StageKey == "dialogue1")
                    {
                        GameGlobalStates.dialogScreen_StageKey = "dialogue2";
                        UIManager.ShowScreen<DialogScreen>();
                    }
                    else if (GameGlobalStates.dialogScreen_StageKey == "dialogue2")
                    {
                        GameGlobalStates.dialogScreen_StageKey = "dialogue3";
                        UIManager.ShowScreen<DialogScreen>();
                    }
                    else if (GameGlobalStates.dialogScreen_StageKey == "dialogue3")
                    {
                        GameGlobalStates.dialogScreen_StageKey = "dialogue4";
                        UIManager.ShowScreen<DialogScreen>();
                    }
                    else if (GameGlobalStates.dialogScreen_StageKey == "dialogue4")
                    {
                        var notifications = Overlewd.GameData.ftue.chapters[GameGlobalStates.ftueChapterId].
                            dialogs.FindAll(d => GameData.GetDialogById(d.id)?.type == AdminBRO.DialogType.Notification);
                        GameGlobalStates.dialogNotification_StageKey = notifications.First().key;
                        UIManager.ShowNotification<DialogNotification>();
                    }
                }
                else
                {
                    if (GameGlobalStates.dialogScreen_DialogId == 1)
                    {
                        GameGlobalStates.CompleteStageId(GameGlobalStates.dialogScreen_StageId);
                        UIManager.ShowScreen<MapScreen>();
                    }
                    else if (GameGlobalStates.dialogScreen_DialogId == 2)
                    {
                        GameGlobalStates.CompleteStageId(GameGlobalStates.dialogScreen_StageId);
                        UIManager.ShowScreen<MapScreen>();
                    }
                    else if (GameGlobalStates.dialogScreen_DialogId == 3)
                    {
                        GameGlobalStates.CompleteStageId(GameGlobalStates.dialogScreen_StageId);
                        GameGlobalStates.PortalCanBuilded();
                        GameGlobalStates.ResetStateCastleButtons();
                        GameGlobalStates.castle_SideMenuLock = true;
                        GameGlobalStates.castle_CaveLock = true;
                        GameGlobalStates.castle_HintMessage = GameData.castleScreenHints[4];
                        UIManager.ShowScreen<CastleScreen>();
                    }
                }
            }

            protected override void ShowCurrentReplica()
            {
                base.ShowCurrentReplica();

                var prevReplica = currentReplicaId > 0 ? dialogData.replicas[currentReplicaId - 1] : null;
                var replica = dialogData.replicas[currentReplicaId];
                
                ShowCutIn(replica, prevReplica);
                ShowPersEmotion(replica, prevReplica);
            }

            protected override AdminBRO.Animation GetAnimationById(int id)
            {
                return GameData.GetAnimationById(id);
            }
        }
    }
}