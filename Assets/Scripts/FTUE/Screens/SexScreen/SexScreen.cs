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
            protected override async Task PrepareHideOperationsAsync()
            {
                await Task.CompletedTask;
            }

            protected override async Task EnterScreen()
            {
                dialogData = GameGlobalStates.sexScreen_DialogData;
                await Task.CompletedTask;

                var animBack = SpineWidget.CreateInstance(transform);
                animBack.Initialize("FTUE/UlviSexScene1/Cut_in2/back_SkeletonData", false);
                animBack.PlayAnimation("back", true);

                var anim = SpineWidget.CreateInstance(transform);
                anim.Initialize("FTUE/UlviSexScene1/Cut_in2/idle01_SkeletonData", false);
                anim.PlayAnimation("idle", true);
            }

            protected override void LeaveScreen()
            {
                GameGlobalStates.dialogScreen_DialogId = 1;
                UIManager.ShowScreen<DialogScreen>();
            }
        }
    }
}
