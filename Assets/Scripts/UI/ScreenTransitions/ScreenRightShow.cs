using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{

    public class ScreenRightShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            UITools.SetStretch(screenRectTransform);
            UITools.RightHide(screenRectTransform);
        }

        public override async Task ProgressAsync()
        {
            screen.StartShow();
            await UITools.RightShowAsync(screenRectTransform);
            UITools.SetStretch(screenRectTransform);
            await screen.AfterShowAsync();
            Destroy(this);
        }
    }
}
