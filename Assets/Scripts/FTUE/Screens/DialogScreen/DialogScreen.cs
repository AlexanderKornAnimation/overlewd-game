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
            private Dictionary<string, Dictionary<string, string>> emotionsAnim = new Dictionary<string, Dictionary<string, string>>
            {
                [AdminBRO.DialogCharacterKey.Overlord] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = null,
                    [AdminBRO.DialogCharacterAnimation.Happy] = null,
                    [AdminBRO.DialogCharacterAnimation.Idle] = null,
                    [AdminBRO.DialogCharacterAnimation.Love] = null,
                    [AdminBRO.DialogCharacterAnimation.Surprised] = null,
                },
                [AdminBRO.DialogCharacterKey.Ulvi] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = null,
                    [AdminBRO.DialogCharacterAnimation.Happy] = null,
                    [AdminBRO.DialogCharacterAnimation.Idle] = null,
                    [AdminBRO.DialogCharacterAnimation.Love] = null,
                    [AdminBRO.DialogCharacterAnimation.Surprised] = null,
                },
                [AdminBRO.DialogCharacterKey.UlviWolf] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = "FTUE/Emotions/UlviFurry/angry_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Happy] = "FTUE/Emotions/UlviFurry/happy_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Idle] = "FTUE/Emotions/UlviFurry/idle_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Love] = "FTUE/Emotions/UlviFurry/love_SkeletonData",
                    [AdminBRO.DialogCharacterAnimation.Surprised] = "FTUE/Emotions/UlviFurry/surprised_SkeletonData",
                },
                [AdminBRO.DialogCharacterKey.Faye] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = null,
                    [AdminBRO.DialogCharacterAnimation.Happy] = null,
                    [AdminBRO.DialogCharacterAnimation.Idle] = null,
                    [AdminBRO.DialogCharacterAnimation.Love] = null,
                    [AdminBRO.DialogCharacterAnimation.Surprised] = null,
                },
                [AdminBRO.DialogCharacterKey.Adriel] = new Dictionary<string, string>
                {
                    [AdminBRO.DialogCharacterAnimation.Angry] = null,
                    [AdminBRO.DialogCharacterAnimation.Happy] = null,
                    [AdminBRO.DialogCharacterAnimation.Idle] = null,
                    [AdminBRO.DialogCharacterAnimation.Love] = null,
                    [AdminBRO.DialogCharacterAnimation.Surprised] = null,
                },
            };

            private Dictionary<string, Dictionary<string, string>> cutInAnim = new Dictionary<string, Dictionary<string, string>>
            {
                ["dialogCutIn1"] = new Dictionary<string, string>
                {
                    ["back"] = "FTUE/UlviSexScene1/Cut_in2/back_SkeletonData",
                    ["idle"] = "FTUE/UlviSexScene1/Cut_in2/idle01_SkeletonData"
                },

            };

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

                    foreach (var animData in cutInAnim["dialogCutIn1"])
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
                    Destroy(emotionAnimation?.gameObject);
                    emotionAnimation = null;

                    if (emotionsAnim.ContainsKey(replica.characterKey))
                    {
                        var persEmotions = emotionsAnim[replica.characterKey];
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
