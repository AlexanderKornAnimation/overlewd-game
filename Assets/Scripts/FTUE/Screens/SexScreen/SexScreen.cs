using System;
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
        public class SexScreen : Overlewd.SexScreen
        {
            private SpineWidgetGroup mainAnimation;
            private SpineWidgetGroup cutInAnimation;
            private string mainSoundKey;
            private string cutInSoundKey;

            protected override async Task EnterScreen()
            {
                backImage.gameObject.SetActive(false);

                dialogData = GameGlobalStates.sexScreen_DialogData;


                if (GameGlobalStates.sexScreen_DialogId == 1)
                {
                    blackScreenTop.gameObject.SetActive(true);
                    blackScreenBot.gameObject.SetActive(true);
                }

                await Task.CompletedTask;
            }

            protected override void LeaveScreen()
            {
                SoundManager.StopAllInstances(true);

                if (GameGlobalStates.sexScreen_DialogId == 1)
                {
                    GameGlobalStates.battleScreen_StageId = 1;
                    GameGlobalStates.battleScreen_BattleId = 1;
                    UIManager.ShowScreen<BattleScreen>();
                }
                else if (GameGlobalStates.sexScreen_DialogId == 2)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.sexScreen_StageId);
                    GameGlobalStates.map_DialogNotificationId = 6;
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.sexScreen_DialogId == 3)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.sexScreen_StageId);
                    GameGlobalStates.map_DialogNotificationId = 11;
                    UIManager.ShowScreen<MapScreen>();
                }
            }

            private void ShowMain(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
            {
                if (replica.mainAnimationId.HasValue)
                {
                    if (replica.mainAnimationId.Value != mainAnimation?.animationData.id)
                    {
                        Destroy(mainAnimation?.gameObject);
                        mainAnimation = null;

                        var animation = GameData.GetAnimationById(replica.mainAnimationId.Value);
                        if (animation != null)
                        {
                            mainAnimation = SpineWidgetGroup.GetInstance(mainAnimPos);
                            mainAnimation.Initialize(animation);
                        }
                    }
                }
                else
                {
                    Destroy(mainAnimation?.gameObject);
                    mainAnimation = null;
                }
            }

            private void ShowCutIn(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
            {
                if (replica.cutInAnimationId.HasValue)
                {
                    if (replica.cutInAnimationId != cutInAnimation?.animationData.id)
                    {
                        Destroy(cutInAnimation?.gameObject);
                        cutInAnimation = null;

                        var animation = GameData.GetAnimationById(replica.cutInAnimationId.Value);
                        if (animation != null)
                        {
                            cutInAnimation = SpineWidgetGroup.GetInstance(cutInAnimPos);
                            cutInAnimation.Initialize(animation);
                        }
                    }
                }
                else
                {
                    Destroy(cutInAnimation?.gameObject);
                    cutInAnimation = null;
                }

                if (cutInAnimation != null)
                {
                    cutIn.SetActive(true);
                    mainAnimation?.Pause();
                }
                else
                {
                    cutIn.SetActive(false);
                    mainAnimation?.Play();
                }
            }

            private void PlaySound(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
            {
                //main sound
                if (!String.IsNullOrEmpty(replica.mainSoundPath))
                {
                    if (replica.mainSoundPath != mainSoundKey)
                    {
                        if (!String.IsNullOrEmpty(mainSoundKey))
                        {
                            SoundManager.Stop(mainSoundKey);
                        }

                        SoundManager.CreateSoundInstance(replica.mainSoundPath);
                        mainSoundKey = replica.mainSoundPath;
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(mainSoundKey))
                    {
                        SoundManager.Stop(mainSoundKey);
                    }
                    mainSoundKey = null;
                }

                //cutIn sound
                if (!String.IsNullOrEmpty(replica.cutInSoundPath))
                {
                    if (replica.cutInSoundPath != cutInSoundKey)
                    {
                        if (!String.IsNullOrEmpty(cutInSoundKey))
                        {
                            SoundManager.Stop(cutInSoundKey);
                        }

                        SoundManager.CreateSoundInstance(replica.cutInSoundPath);
                        cutInSoundKey = replica.cutInSoundPath;
                    }

                    if (!String.IsNullOrEmpty(mainSoundKey))
                    {
                        SoundManager.SetPause(mainSoundKey, true);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(cutInSoundKey))
                    {
                        SoundManager.Stop(cutInSoundKey);
                    }
                    cutInSoundKey = null;

                    if (!String.IsNullOrEmpty(mainSoundKey))
                    {
                        SoundManager.SetPause(mainSoundKey, false);
                    }
                }
            }

            protected override void ShowCurrentReplica()
            {
                base.ShowCurrentReplica();

                var prevReplica = currentReplicaId > 0 ? dialogData.replicas[currentReplicaId - 1] : null;
                var replica = dialogData.replicas[currentReplicaId];

                if (GameGlobalStates.sexScreen_DialogId == 1)
                {
                    if (currentReplicaId == 2)
                    {
                        StartCoroutine(FadeOut());
                    }
                }

                ShowMain(replica, prevReplica);
                ShowCutIn(replica, prevReplica);
                PlaySound(replica, prevReplica);
            }

            private IEnumerator FadeIn()
            {
                blackScreenTop.fillMethod = Image.FillMethod.Horizontal;
                blackScreenBot.fillMethod = Image.FillMethod.Horizontal;

                blackScreenBot.fillOrigin = 0;
                blackScreenTop.fillOrigin = 0;

                while (blackScreenTop.fillAmount != 1)
                {
                    yield return new WaitForSeconds(0.0005f);
                    blackScreenTop.fillAmount += 0.07f;
                    blackScreenBot.fillAmount += 0.07f;
                }
            }

            private IEnumerator FadeOut()
            {
                while (blackScreenTop.fillAmount != 0)
                {
                    yield return new WaitForSeconds(0.0005f);
                    blackScreenTop.fillAmount -= 0.07f;
                    blackScreenBot.fillAmount -= 0.07f;
                }
            }
        }
    }
}