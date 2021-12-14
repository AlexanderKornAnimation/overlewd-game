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
            private SpineWidget persEmotionBackAnim;
            private SpineWidget curPersEmotionAnim;

            private List<SpineWidget> cutInAnimations = new List<SpineWidget>();

            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.dialogScreen_DialogData;
                personageHead.gameObject.SetActive(false);

                persEmotionBackAnim = SpineWidget.GetInstance(personageEmotionPos);
                persEmotionBackAnim.Initialize("FTUE/UlviEmotions/back_SkeletonData", false);
                persEmotionBackAnim.PlayAnimation("back", true);

                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                if (GameGlobalStates.dialogScreen_DialogId == 10)
                {
                    GameGlobalStates.dialogScreen_DialogId = 13;
                    UIManager.ShowScreen<DialogScreen>();
                }
                else if (GameGlobalStates.dialogScreen_DialogId == 13)
                {
                    GameGlobalStates.dialogScreen_DialogId = 14;
                    UIManager.ShowScreen<DialogScreen>();
                }
                else if (GameGlobalStates.dialogScreen_DialogId == 14)
                {
                    GameGlobalStates.dialogNotification_DialogId = 3;
                    UIManager.ShowNotification<DialogNotification>();
                }
            }

            private void ShowCutIn(AdminBRO.DialogReplica replica)
            {
                if (replica.cutIn != null)
                {
                    foreach (var anim in cutInAnimations)
                    {
                        Destroy(anim?.gameObject);
                    }
                    cutInAnimations.Clear();

                    foreach (var animData in GameData.cutInAnimations["cutIn1"])
                    {
                        if (animData.Value != null)
                        {
                            var anim = SpineWidget.GetInstance(cutInAnimPos);
                            anim.Initialize(animData.Value, false);
                            anim.PlayAnimation(animData.Key, true);
                            cutInAnimations.Add(anim);
                        }
                    }

                    cutIn.SetActive(true);
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

            private void ShowPersEmotion(AdminBRO.DialogReplica replica)
            {
                if (replica.animation != null)
                {
                    Destroy(curPersEmotionAnim?.gameObject);
                    curPersEmotionAnim = null;

                    if (GameData.emotionsAnim.ContainsKey(replica.characterName))
                    {
                        var persEmotions = GameData.emotionsAnim[replica.characterName];
                        if (persEmotions.ContainsKey(replica.animation))
                        {
                            var path = persEmotions[replica.animation];
                            if (path != null)
                            {
                                curPersEmotionAnim = SpineWidget.GetInstance(personageEmotionPos);
                                curPersEmotionAnim.Initialize(path, false);
                                curPersEmotionAnim.PlayAnimation(replica.animation, true);
                            }
                        }
                    }
                }
                else
                {
                    Destroy(curPersEmotionAnim?.gameObject);
                    curPersEmotionAnim = null;
                }
            }

            protected override void ShowCurrentReplica()
            {
                base.ShowCurrentReplica();

                var replica = dialogData.replicas[currentReplicaId];
                ShowCutIn(replica);
                ShowPersEmotion(replica);
            }
        }
    }
}
