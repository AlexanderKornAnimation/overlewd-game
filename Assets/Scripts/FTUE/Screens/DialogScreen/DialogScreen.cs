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
            private List<SpineWidget> cutInAnimations = new List<SpineWidget>();
            private SpineWidget emotionAnimation;

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
                if (replica.cutInAnimationTemp != null)
                {
                    if (replica.cutInAnimationTemp != prevReplica?.cutInAnimationTemp)
                    {
                        foreach (var anim in cutInAnimations)
                        {
                            Destroy(anim?.gameObject);
                        }
                        cutInAnimations.Clear();

                        if (GameLocalResources.cutInAnimPath.ContainsKey(replica.cutInAnimationTemp))
                        {
                            var cutInData = GameLocalResources.cutInAnimPath[replica.cutInAnimationTemp];
                            foreach (var animData in cutInData)
                            {
                                if (animData.Value != null)
                                {
                                    var anim = SpineWidget.GetInstance(cutInAnimPos);
                                    anim.Initialize(animData.Value);
                                    anim.PlayAnimation(animData.Key, true);
                                    cutInAnimations.Add(anim);
                                }
                            }
                        }

                        cutIn.SetActive(cutInAnimations.Count > 0);
                    }
                }
                else
                {
                    cutIn.SetActive(false);

                    foreach (var anim in cutInAnimations)
                    {
                        Destroy(anim?.gameObject);
                    }
                    cutInAnimations.Clear();
                }
            }

            private void ShowPersEmotion(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
            {
                if (replica.emotionAnimationTemp != null)
                {
                    if (replica.characterSkin != prevReplica?.characterSkin ||
                        replica.emotionAnimationTemp != prevReplica?.emotionAnimationTemp)
                    {
                        Destroy(emotionAnimation?.gameObject);
                        emotionAnimation = null;

                        if (GameLocalResources.emotionsAnimPath.ContainsKey(replica.characterSkin))
                        {
                            var persEmotions = GameLocalResources.emotionsAnimPath[replica.characterSkin];
                            if (persEmotions.ContainsKey(replica.emotionAnimationTemp))
                            {
                                var headPath = persEmotions[replica.emotionAnimationTemp];
                                if (headPath != null)
                                {
                                    emotionAnimation = SpineWidget.GetInstance(emotionPos);
                                    emotionAnimation.Initialize(headPath);
                                    emotionAnimation.PlayAnimation(replica.emotionAnimationTemp, true);
                                }
                            }
                        }
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
