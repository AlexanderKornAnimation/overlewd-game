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
            private SpineWidget backMainAnim;
            private SpineWidget mainAnim;
            private SpineWidget backCutInAnim;
            private SpineWidget cutInAnim;

            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.sexScreen_DialogData;
                await Task.CompletedTask;

                backMainAnim = SpineWidget.GetInstance(mainAnimPos);
                backMainAnim.Initialize("FTUE/UlviSexScene1/MainScene/back_SkeletonData", false);
                backMainAnim.PlayAnimation("back", true);

                mainAnim = SpineWidget.GetInstance(mainAnimPos);
                mainAnim.Initialize("FTUE/UlviSexScene1/MainScene/idle01_SkeletonData", false);
                mainAnim.PlayAnimation("idle", true);
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

            protected override void ShowCurrentReplica()
            {
                base.ShowCurrentReplica();

                var replica = dialogData.replicas[currentReplicaId];
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
        }
    }
}
