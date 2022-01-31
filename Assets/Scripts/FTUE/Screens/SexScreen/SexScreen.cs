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
            private List<SpineWidget> mainAnimations = new List<SpineWidget>();
            private List<SpineWidget> cutInAnimations = new List<SpineWidget>();

            private void SetMainAnim()
            {
                foreach (var anim in mainAnimations)
                {
                    Destroy(anim?.gameObject);
                }

                mainAnimations.Clear();

                var mainSexKey = GameGlobalStates.sexScreen_DialogId switch
                {
                    1 => "MainSex1",
                    2 => "MainSex2",
                    3 => "MainSex3",
                    _ => "MainSex1"
                };

                foreach (var animData in GameLocalResources.mainSexAnimPath[mainSexKey])
                {
                    if (animData.Value != null)
                    {
                        var anim = SpineWidget.GetInstance(mainAnimPos);
                        anim.Initialize(animData.Value, false);
                        anim.PlayAnimation(animData.Key, true);
                        mainAnimations.Add(anim);
                    }
                }
            }

            private void SetFinalMainAnim()
            {
                if (GameGlobalStates.sexScreen_DialogId == 1)
                {
                    SoundManager.StopAllInstances(true);
                }

                foreach (var anim in mainAnimations)
                {
                    Destroy(anim?.gameObject);
                }

                mainAnimations.Clear();

                var mainFinalSexKey = GameGlobalStates.sexScreen_DialogId switch
                {
                    1 => "FinalSex1",
                    2 => "FinalSex2",
                    3 => "FinalSex3",
                    _ => "FinalSex1"
                };

                foreach (var animData in GameLocalResources.mainSexAnimPath[mainFinalSexKey])
                {
                    if (animData.Value != null)
                    {
                        var anim = SpineWidget.GetInstance(mainAnimPos);
                        anim.Initialize(animData.Value, false);
                        anim.PlayAnimation(animData.Key, true);
                        mainAnimations.Add(anim);
                    }
                }
            }

            protected override async Task EnterScreen()
            {
                backImage.gameObject.SetActive(false);

                dialogData = GameGlobalStates.sexScreen_DialogData;

                SetMainAnim();

                if (GameGlobalStates.sexScreen_DialogId == 1 /* || 
                    GameGlobalStates.sexScreen_DialogId == 3*/)
                {
                    blackScreenTop.gameObject.SetActive(true);
                    blackScreenBot.gameObject.SetActive(true);
                    mainAnimations[1].Pause();
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

            private void ShowCutIn(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
            {
                if (replica.cutIn != null)
                {
                    if (replica.cutIn != prevReplica?.cutIn)
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

            protected override void PlaySound(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
            {
                var mainSexKey = GameGlobalStates.sexScreen_DialogId switch
                {
                    1 => "MainSex1",
                    2 => "MainSex2",
                    3 => "MainSex3",
                    _ => "MainSex1"
                };
                
                if (replica.cutIn != null)
                {
                    if (replica.cutIn != prevReplica?.cutIn)
                    {
                        if (replica.replicaCutInSoundPath != null)
                        {
                            mainAnimations[1].Pause();
                            SoundManager.SetPause(mainSexKey, true);
                            SoundManager.CreateEventInstance(replica.cutIn, replica.replicaCutInSoundPath);
                        }
                    }
                }
                else
                {
                    if (replica.replicaMainSoundPath != null)
                    {
                        SoundManager.CreateEventInstance(mainSexKey, replica.replicaMainSoundPath);
                        mainAnimations[1].Play();
                        SoundManager.SetPause(mainSexKey, false);
                        
                        if (prevReplica?.replicaCutInSoundPath != null)
                            SoundManager.SetPause(prevReplica.cutIn, true);
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

                /*if (GameGlobalStates.sexScreen_DialogId == 3)
                {
                    blackScreenTop.fillAmount = 0;
                    blackScreenBot.fillAmount = 0;
                    
                    if (currentReplicaId == 7)
                    {
                        StartCoroutine(FadeIn());
                    }
                }*/

                if (currentReplicaId == dialogData.replicas.Count - 1) //last replica
                {
                    SetFinalMainAnim();
                }

                PlaySound(replica, prevReplica);
                ShowCutIn(replica, prevReplica);
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