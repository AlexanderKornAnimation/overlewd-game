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

            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.sexScreen_DialogData;

                if (dialogData.id == 1 || dialogData.id == 3)
                {
                    blackScreenTop.gameObject.SetActive(true);
                    blackScreenBot.gameObject.SetActive(true);
                }

                await Task.CompletedTask;

                var mainSexKey = "MainSex1";
                if (GameGlobalStates.sexScreen_DialogId == 1)
                {
                    mainSexKey = "MainSex1";
                }
                else if (GameGlobalStates.sexScreen_DialogId == 2)
                {
                    mainSexKey = "MainSex1";
                }
                else if (GameGlobalStates.sexScreen_DialogId == 3)
                {
                    mainSexKey = "MainSex1";
                }

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

            protected override void LeaveScreen()
            {
                if (GameGlobalStates.sexScreen_DialogId == 1)
                {
                    GameGlobalStates.battleScreen_StageId = 1;
                    GameGlobalStates.battleScreen_BattleId = 1;
                    UIManager.ShowScreen<BattleScreen>();
                }
                else if (GameGlobalStates.sexScreen_DialogId == 2)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.sexScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (GameGlobalStates.sexScreen_DialogId == 3)
                {
                    GameGlobalStates.CompleteStageId(GameGlobalStates.sexScreen_StageId);
                    UIManager.ShowScreen<MapScreen>();
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

            protected override void ShowCurrentReplica()
            {
                base.ShowCurrentReplica();

                var replica = dialogData.replicas[currentReplicaId];
                
                if (dialogData.id == 1 && currentReplicaId == 2)
                {
                   StartCoroutine(FadeOut());
                }

                if (dialogData.id == 3)
                {
                    blackScreenTop.fillAmount = 0;
                    blackScreenBot.fillAmount = 0;
                    
                    if (currentReplicaId == 7)
                    {
                        StartCoroutine(FadeIn());
                    }
                }

                ShowCutIn(replica);
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