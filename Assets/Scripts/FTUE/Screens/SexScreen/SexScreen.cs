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
            private Dictionary<string, Dictionary<string, string>> sexMainAnim = new Dictionary<string, Dictionary<string, string>>
            {
                ["sexMainScene1"] = new Dictionary<string, string>
                {
                    ["back"] = "FTUE/UlviSexScene1/MainScene/back_SkeletonData",
                    ["idle"] = "FTUE/UlviSexScene1/MainScene/idle01_SkeletonData"
                },

            };

            private Dictionary<string, Dictionary<string, string>> cutInAnim = new Dictionary<string, Dictionary<string, string>>
            {
                ["sexCutIn1"] = new Dictionary<string, string>
                {
                    ["back"] = "FTUE/UlviSexScene1/Cut_in2/back_SkeletonData",
                    ["idle"] = "FTUE/UlviSexScene1/Cut_in2/idle01_SkeletonData"
                },

            };

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

                foreach (var animData in sexMainAnim["sexMainScene1"])
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

                    foreach (var animData in cutInAnim["sexCutIn1"])
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

            protected override void ShowCurrentReplica()
            {
                base.ShowCurrentReplica();

                var replica = dialogData.replicas[currentReplicaId];
                ShowCutIn(replica);
            }
        }
    }
}
