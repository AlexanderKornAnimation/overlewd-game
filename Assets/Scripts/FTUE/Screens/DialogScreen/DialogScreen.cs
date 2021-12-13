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
            private SpineWidget backMainAnim;
            private SpineWidget mainAnim;
            private SpineWidget backCutInAnim;
            private SpineWidget cutInAnim;

            private SpineWidget curPersEmotionAnim;

            private Dictionary<string, Dictionary<string, string>> emotionsPath = new Dictionary<string, Dictionary<string, string>>
            {
                [AdminBRO.DialogCharacterName.Overlord] = new Dictionary<string, string>
                { 
                    [AdminBRO.DialogCharacterAnimation.Angry] = null,
                    [AdminBRO.DialogCharacterAnimation.Happy] = null,
                    [AdminBRO.DialogCharacterAnimation.Idle] = null,
                    [AdminBRO.DialogCharacterAnimation.Love] = null,
                    [AdminBRO.DialogCharacterAnimation.Surprised] = null,
                },
                [AdminBRO.DialogCharacterName.Ulvi] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = "FTUE/UlviEmotions/angry_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Happy] = "FTUE/UlviEmotions/happy_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Idle] = "FTUE/UlviEmotions/idle_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Love] = "FTUE/UlviEmotions/love_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Surprised] = "FTUE/UlviEmotions/surprised_SkeletonData",
                },
                [AdminBRO.DialogCharacterName.UlviWolf] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = "FTUE/UlviEmotions/angry_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Happy] = "FTUE/UlviEmotions/happy_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Idle] = "FTUE/UlviEmotions/idle_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Love] = "FTUE/UlviEmotions/love_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Surprised] = "FTUE/UlviEmotions/surprised_SkeletonData",
                },
                [AdminBRO.DialogCharacterName.Faye] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = null,
                    [AdminBRO.DialogCharacterAnimation.Happy] = null,
                    [AdminBRO.DialogCharacterAnimation.Idle] = null,
                    [AdminBRO.DialogCharacterAnimation.Love] = null,
                    [AdminBRO.DialogCharacterAnimation.Surprised] = null,
                },
                [AdminBRO.DialogCharacterName.Adriel] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = null,
                    [AdminBRO.DialogCharacterAnimation.Happy] = null,
                    [AdminBRO.DialogCharacterAnimation.Idle] = null,
                    [AdminBRO.DialogCharacterAnimation.Love] = null,
                    [AdminBRO.DialogCharacterAnimation.Surprised] = null,
                },
            };

            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.dialogScreen_DialogData;
                personageHead.gameObject.SetActive(false);

                var backHeadAnim = SpineWidget.GetInstance(personageEmotionPos);
                backHeadAnim.Initialize("FTUE/UlviEmotions/back_SkeletonData", false);
                backHeadAnim.PlayAnimation("back", true);

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
                    Destroy(backCutInAnim?.gameObject);
                    Destroy(cutInAnim?.gameObject);
                    backCutInAnim = null;
                    cutInAnim = null;

                    backCutInAnim = SpineWidget.GetInstance(cutInAnimPos);
                    backCutInAnim.Initialize("FTUE/UlviSexScene1/Cut_in2/back_SkeletonData", false);
                    backCutInAnim.PlayAnimation("back", true);

                    cutInAnim = SpineWidget.GetInstance(cutInAnimPos);
                    cutInAnim.Initialize("FTUE/UlviSexScene1/Cut_in2/idle01_SkeletonData", false);
                    cutInAnim.PlayAnimation("idle", true);

                    cutIn.SetActive(true);
                }
                else
                {
                    cutIn.SetActive(false);

                    Destroy(backCutInAnim?.gameObject);
                    Destroy(cutInAnim?.gameObject);
                    backCutInAnim = null;
                    cutInAnim = null;
                }
            }

            private void ShowPersEmotion(AdminBRO.DialogReplica replica)
            {
                if (replica.animation != null)
                {
                    Destroy(curPersEmotionAnim?.gameObject);
                    curPersEmotionAnim = null;

                    if (emotionsPath.ContainsKey(replica.characterName))
                    {
                        var persEmotions = emotionsPath[replica.characterName];
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
