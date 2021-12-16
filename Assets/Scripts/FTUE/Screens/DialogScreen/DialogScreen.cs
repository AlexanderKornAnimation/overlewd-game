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

            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.dialogScreen_DialogData;

                personageHead.gameObject.SetActive(false);
                emotionBack.gameObject.SetActive(true);

                if (GameGlobalStates.dialogScreen_DialogId == 10)
                {

                }
                else if (GameGlobalStates.dialogScreen_DialogId == 13)
                {

                }
                else if (GameGlobalStates.dialogScreen_DialogId == 14)
                {

                }

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

                    if (GameLocalResources.cutInAnimPath.ContainsKey(replica.cutIn))
                    {
                        var cutInData = GameLocalResources.cutInAnimPath[replica.cutIn];
                        foreach (var animData in cutInData)
                        {
                            if (animData.Value != null)
                            {
                                var anim = SpineWidget.GetInstance(cutInAnimPos);
                                anim.Initialize(animData.Value, false);
                                anim.PlayAnimation(animData.Key, true);
                                cutInAnimations.Add(anim);
                            }
                        }
                    }

                    cutIn.SetActive(cutInAnimations.Count > 0);
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
                    Destroy(emotionAnimation?.gameObject);
                    emotionAnimation = null;

                    if (GameLocalResources.emotionsAnimPath.ContainsKey(replica.characterKey))
                    {
                        var persEmotions = GameLocalResources.emotionsAnimPath[replica.characterKey];
                        if (persEmotions.ContainsKey(replica.animation))
                        {
                            var headPath = persEmotions[replica.animation];
                            if (headPath != null)
                            {
                                emotionAnimation = SpineWidget.GetInstance(emotionPos);
                                emotionAnimation.Initialize(headPath, false);
                                emotionAnimation.PlayAnimation(replica.animation, true);
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

                var replica = dialogData.replicas[currentReplicaId];
                ShowCutIn(replica);
                ShowPersEmotion(replica);
            }
        }
    }
}
