using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class DialogScreen : Overlewd.DialogScreen
        {
            private List<SpineWidgetGroup> cutInAnimations = new List<SpineWidgetGroup>();

            private SpineWidgetGroup emotionAnimation;

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.dialogScreen_DialogData;

                personageHead.gameObject.SetActive(false);
                emotionBack.gameObject.SetActive(true);

                if (GameGlobalStates.dialogScreen_DialogId == 1)
                {
                }
                else if (GameGlobalStates.dialogScreen_DialogId == 2)
                {
                }
                else if (GameGlobalStates.dialogScreen_DialogId == 3)
                {
                }

                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
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

            private void ShowCutIn(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
            {
                if (replica.cutInAnimationId.HasValue)
                {
                    if (replica.cutInAnimationId != prevReplica?.cutInAnimationId)
                    {
                        foreach (var anim in cutInAnimations)
                        {
                            Destroy(anim?.gameObject);
                        }

                        cutInAnimations.Clear();

                        var animation = GameData.GetAnimationById(replica.cutInAnimationId.Value);
                        var cutInAnimation = SpineWidgetGroup.GetInstance(cutInAnimPos);
                        cutInAnimation.Initialize(animation);
                        cutInAnimations.Add(cutInAnimation);
                    }
                }
                else
                {
                    foreach (var anim in cutInAnimations)
                    {
                        Destroy(anim?.gameObject);
                    }

                    cutInAnimations.Clear();
                }

                cutIn.SetActive(cutInAnimations.Count > 0);
            }

            private void ShowPersEmotion(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
            {
                if (replica.emotionAnimationId.HasValue)
                {
                    if (replica.emotionAnimationId != prevReplica?.emotionAnimationId)
                    {
                        Destroy(emotionAnimation?.gameObject);
                        emotionAnimation = null;

                        var animation = GameData.GetAnimationById(replica.emotionAnimationId.Value);
                        emotionAnimation = SpineWidgetGroup.GetInstance(emotionPos);
                        emotionAnimation.Initialize(animation);
                    }
                }
                else
                {
                    Destroy(emotionAnimation?.gameObject);
                    emotionAnimation = null;
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
        }
    }
}