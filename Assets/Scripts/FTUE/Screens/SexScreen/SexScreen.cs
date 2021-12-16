using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
                await Task.CompletedTask;

                var mainSexKey = "MainSex1";
                if (GameGlobalStates.sexScreen_DialogId == 2)
                {
                    mainSexKey = "MainSex1";
                }
                else if (GameGlobalStates.sexScreen_DialogId == 8)
                {
                    mainSexKey = "MainSex1";
                }
                else if (GameGlobalStates.sexScreen_DialogId == 16)
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
                if (GameGlobalStates.sexScreen_DialogId == 2)
                {
                    GameGlobalStates.sexScreen_DialogId = 8;
                    UIManager.ShowScreen<SexScreen>();
                }
                else if (GameGlobalStates.sexScreen_DialogId == 8)
                {
                    GameGlobalStates.sexScreen_DialogId = 16;
                    UIManager.ShowScreen<SexScreen>();
                }
                else if (GameGlobalStates.sexScreen_DialogId == 16)
                {
                    GameGlobalStates.dialogScreen_DialogId = 10;
                    UIManager.ShowScreen<DialogScreen>();
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
                ShowCutIn(replica);
            }
        }
    }
}
