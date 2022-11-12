using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenBottomShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            UITools.SetStretch(screenRectTransform);
            UITools.BottomHide(screenRectTransform);
        }

        public override async Task ProgressAsync()
        {
            screen.StartShow();
            await UITools.BottomShowAsync(screenRectTransform);
            UITools.SetStretch(screenRectTransform);
            await screen.AfterShowAsync();
            Destroy(this);
        }
    }
}
